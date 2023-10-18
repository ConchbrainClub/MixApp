using Blazored.LocalStorage;
using Microsoft.Fast.Components.FluentUI;
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

        private LocaleManager LM { get; set; }

        public GlobalEvent(IServiceScopeFactory scopeFactory)
        {
            IServiceProvider serviceProvider = scopeFactory.CreateScope().ServiceProvider;
            HttpClient = serviceProvider.GetRequiredService<HttpClient>();
            JSRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
            LocalStorage = serviceProvider.GetRequiredService<ILocalStorageService>();
            LM = serviceProvider.GetRequiredService<LocaleManager>();
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
        /// When download progress changed
        /// </summary>
        public event Action? OnDownloadQueueChanged;

        /// <summary>
        /// When download progress changed
        /// </summary>
        public event Action? OnHistoryQueueChanged;

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

            Notification.ShowToast(title, new() { Details = software.PackageName });
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
                Notification.ShowToast(
                    LM.Scripts["n.global_event.download_complete"], new() 
                    { 
                        Details = task.Manifest.PackageName
                    });
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
        public void DownloadInstaller(Manifest manifest, Installer? installer = null)
        {
            List<Installer> installersObj = JsonSerializer.Deserialize<List<Installer>>(manifest.Installers!) ?? new();

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

            string fileName = (manifest?.PackageName ?? "unknow") + "." + installer?.InstallerUrl?.Split('.').Last() ?? "exe";
            string url = "https://cors.conchbrain.club?" + installer?.InstallerUrl;

            DownloadTask task = new(manifest!, installer!);

            task.OnProgressChanged += i => 
            {
                if (i.Progress < 0) DownloadFailed(i);

                if (i.Progress == 100) 
                {
                    DownloadQueue.Remove(i);
                    AddToHistoryQueue(i);
                }

                OnDownloadQueueChanged?.Invoke();
            };

            // Do not download installer repeat
            if (DownloadQueue.Find(i => 
            {
                return i.Manifest.PackageIdentifier == task.Manifest.PackageIdentifier
                    && i.Manifest.PackageVersion == task.Manifest.PackageVersion
                    && i.Installer.InstallerSha256 == task.Installer.InstallerSha256;
            }) != null) return;

            DownloadQueue.Add(task);
            
            HttpClient.GetAsync($"/meta/change?type={(int)MetaType.Download}&identifier={manifest?.PackageIdentifier}");

            // Invoke javascript to fetch the installer
            JSRuntime!.InvokeVoidAsync("downloadFile", DotNetObjectReference.Create(task), fileName, url, task.CancelId).AsTask();

            Notification.ShowToast(
                LM.Scripts["n.global_event.start_download"], new() 
                { 
                    Details = manifest?.PackageName
                });
        }

        private void DownloadFailed(DownloadTask task)
        {
            DownloadQueue.Remove(task);

            Notification.ShowToast(
                LM.Scripts["n.global_event.download_failed"], new() 
                {
                    Subtitle = task.Manifest.PackageName,
                    Details = task.Installer.InstallerUrl
                }, 20, ToastIntent.Error);
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