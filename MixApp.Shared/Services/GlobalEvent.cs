using Append.Blazor.Notifications;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using MixApp.Shared.Models;
using System.Text.Json;

namespace MixApp.Shared.Services
{
    public class GlobalEvent
    {
        public HttpClient HttpClient { get; set; } = new();

        private IJSRuntime JSRuntime { get; set; }

        private ILocalStorageService LocalStorage { get; set; }

        private INotificationService NotificationService { get; set; }

        private LocaleManager LM { get; set; }

        private string DownloadProxy { get; set; }

        public GlobalEvent(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            IServiceProvider serviceProvider = scopeFactory.CreateScope().ServiceProvider;
            HttpClient = serviceProvider.GetRequiredService<HttpClient>();
            JSRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
            LocalStorage = serviceProvider.GetRequiredService<ILocalStorageService>();
            LM = serviceProvider.GetRequiredService<LocaleManager>();
            NotificationService = serviceProvider.GetRequiredService<INotificationService>();
            DownloadProxy = configuration.GetValue<string>("DownloadProxy") ?? string.Empty;
            Initialize();
        }

        /// <summary>
        /// When user want open software detail interface
        /// </summary>
        public event Action<Software>? OnOpenSoftware;

        /// <summary>
        /// When wait queue changed
        /// </summary>
        public event Action? OnWaitQueueChanged;

        /// <summary>
        /// When download queue changed
        /// </summary>
        public event Action? OnDownloadQueueChanged;

        /// <summary>
        /// When download history changed
        /// </summary>
        public event Action? OnHistoryQueueChanged;

        /// <summary>
        /// When download progress changed
        /// </summary>
        public event Action? OnDownloadProgressChanged;

        /// <summary>
        /// Wait for download queue
        /// </summary>
        public List<WaitItem> WaitQueue { get; set; } = [];

        /// <summary>
        /// Downloading queue
        /// </summary>
        public List<DownloadTask> DownloadQueue { get; set; } = [];

        /// <summary>
        /// Download history queue
        /// </summary>
        public List<DownloadTask> HistoryQueue { get; set; } = [];

        private async void Initialize()
        {
            HistoryQueue = (await LocalStorage.GetItemAsync<List<DownloadTask>>("history_queue")) ?? new();
            OnHistoryQueueChanged?.Invoke();
        }

        /// <summary>
        /// Open software detail interface
        /// </summary>
        /// <param name="software">Software info to fetch Manifests</param>
        public void OpenSoftware(Software software) => OnOpenSoftware?.Invoke(software);

        /// <summary>
        /// Add Software to wait to download queue
        /// </summary>
        /// <param name="software">software info</param>
        public void Add2WaitQueue(Software software)
        {
            int index = WaitQueue.FindIndex(i => 
            {
                return i.Software?.PackageIdentifier == software.PackageIdentifier;
            });

            string title = string.Empty;

            if (index < 0)
            {
                title = LM.Scripts["n.global_event.add_to_waitlist"];
                WaitQueue.Add(new (){ Software = software });
            }
            else
            {
                title = LM.Scripts["n.global_event.remove_from_waitlist"];
                WaitQueue.RemoveAt(index);
            }

            NotificationService.CreateAsync(title, software.PackageName, "favicon.png").AsTask();
            OnWaitQueueChanged?.Invoke();
        }

        /// <summary>
        /// Add the download task info to history queue
        /// </summary>
        /// <param name="task">download task info</param>
        public void AddToHistoryQueue(DownloadTask task)
        {
            // Do not notify same task
            if (HistoryQueue.FindIndex(i => i.CancelId == task.CancelId) < 0)
            {
                NotificationService.CreateAsync
                (
                    LM.Scripts["n.global_event.download_complete"],
                    task.Manifest.PackageName, 
                    "favicon.png"
                ).AsTask();
            }

            int index = HistoryQueue.FindIndex(history => 
            {
                return history.Manifest.PackageIdentifier == task.Manifest.PackageIdentifier &&
                    history.Manifest.PackageVersion == task.Manifest.PackageVersion &&
                    history.Installer.InstallerSha256 == task.Installer.InstallerSha256;
            });

            if (index >= 0) HistoryQueue.RemoveAt(index);
            HistoryQueue.Insert(0, task);
            LocalStorage.SetItemAsync("history_queue", HistoryQueue).AsTask();
            OnHistoryQueueChanged?.Invoke();
        }

        /// <summary>
        /// Remove download task from history queue
        /// </summary>
        /// <param name="task">download task</param>
        public void RemoveFromHistoryQueue(DownloadTask task)
        {
            HistoryQueue.Remove(task);
            LocalStorage.SetItemAsync("history_queue", HistoryQueue).AsTask();
        }

        /// <summary>
        /// Download the software by DwonloadTask
        /// </summary>
        /// <param name="task">download task</param>
        public void DownloadInstaller(DownloadTask task) => DownloadInstaller(task.Manifest, task.Installer);

        /// <summary>
        /// Download the software by manifest (which software) and installer (which arch) 
        /// </summary>
        /// <param name="manifest">software's manifest</param>
        /// <param name="installer">manifest's installer</param>
        public async void DownloadInstaller(Manifest manifest, Installer? installer = null)
        {
            List<Installer> installersObj = JsonSerializer.Deserialize<List<Installer>>(manifest.Installers!) ?? [];

            // Find the installer that the arch is x86 or x64 (default)
            if (installer == null)
            {
                installer = installersObj.Find(i => i.Architecture == "x86");

                if (installersObj.FindIndex(i => i.Architecture == "x64") >= 0) 
                {
                    installer = installersObj.Find(i => i.Architecture == "x64");
                }
            }

            // If can not find the default arch, download the first
            installer ??= installersObj.First();

            DownloadTask task = new(manifest!, installer!);
            _ = HttpClient.GetAsync($"/meta/change?type={(int)MetaType.Download}&identifier={manifest?.PackageIdentifier}");

            // Check if user disabled download proxy
            if (!string.IsNullOrEmpty(await LocalStorage.GetItemAsStringAsync("disable_proxy").AsTask()))
            {
                _ = JSRuntime!.InvokeVoidAsync("open", installer?.InstallerUrl).AsTask();
                AddToHistoryQueue(task);
                return;
            }

            task.OnProgressChanged += i => 
            {
                if (i.Progress < 0) DownloadFailed(i);

                if (i.Progress == 100) 
                {
                    DownloadQueue.Remove(i);
                    OnDownloadQueueChanged?.Invoke();
                    AddToHistoryQueue(i);
                }

                OnDownloadProgressChanged?.Invoke();
            };

            // Do not download installer repeat
            if (DownloadQueue.Find(i => 
            {
                return i.Manifest.PackageIdentifier == task.Manifest.PackageIdentifier
                    && i.Manifest.PackageVersion == task.Manifest.PackageVersion
                    && i.Installer.InstallerSha256 == task.Installer.InstallerSha256;
            }) != null) return;

            string fileName = (manifest?.PackageName ?? "unknow") + "." + installer?.InstallerUrl?.Split('.').Last() ?? "exe";
            string url = DownloadProxy + installer?.InstallerUrl;

            // Check if user set the custome download proxy
            string customeProxy = await LocalStorage.GetItemAsStringAsync("download_proxy").AsTask();
            if (!string.IsNullOrEmpty(customeProxy))
            {
                url = customeProxy + installer?.InstallerUrl;
            }

            // Invoke javascript to fetch the installer
            _ = JSRuntime!.InvokeVoidAsync("downloadFile", DotNetObjectReference.Create(task), fileName, url, task.CancelId).AsTask();
            
            DownloadQueue.Add(task);
            OnDownloadQueueChanged?.Invoke();

            _ = NotificationService.CreateAsync
            (
                LM.Scripts["n.global_event.start_download"],
                manifest?.PackageName,
                "favicon.png"
            ).AsTask();
        }

        private void DownloadFailed(DownloadTask task)
        {
            DownloadQueue.Remove(task);

            NotificationService.CreateAsync(
                LM.Scripts["n.global_event.download_failed"], 
                task.Installer.InstallerUrl, 
                "favicon.png"
            ).AsTask();
        }

        public void CancelDownloadingTask(DownloadTask task)
        {
            if (!DownloadQueue.Contains(task)) return;
            JSRuntime!.InvokeVoidAsync("cancelDownloading", task.CancelId).AsTask();
            
            DownloadQueue.Remove(task);
            OnDownloadQueueChanged?.Invoke();
        }
    }
}