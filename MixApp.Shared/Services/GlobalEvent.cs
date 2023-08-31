using Blazored.LocalStorage;
using Microsoft.JSInterop;
using MixApp.Shared.Models;
using System.Text.Json;

namespace MixApp.Shared.Services
{
    public class GlobalEvent
    {
        private IJSRuntime JSRuntime { get; set; }

        private ILocalStorageService LocalStorage { get; set; }

        public GlobalEvent(IJSRuntime runtime, ILocalStorageService localStorage)
        {
            JSRuntime = runtime;
            LocalStorage = localStorage;
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
        public List<Software> WaitQueue { get; set; } = new();

        /// <summary>
        /// Downloading queue
        /// </summary>
        public List<DownloadTask> DownloadQueue { get; set; } = new();

        /// <summary>
        /// Download history queue
        /// </summary>
        public List<DownloadTask> HistoryQueue { get; set; } = new();

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
            int index = WaitQueue.FindIndex(i => i.PackageIdentifier ==  software.PackageIdentifier);

            if (index < 0)
            {
                WaitQueue.Add(software);
            }
            else
            {
                WaitQueue.RemoveAt(index);
            }

            OnWaitQueueChanged?.Invoke();
        }

        /// <summary>
        /// Add the download task info to history queue
        /// </summary>
        /// <param name="task">download task info</param>
        public void AddToHistoryQueue(DownloadTask task)
        {
            int index = HistoryQueue.FindIndex(history => 
            {
                return history.Manifest.PackageIdentifier == task.Manifest.PackageIdentifier &&
                    history.Manifest.PackageVersion == task.Manifest.PackageVersion &&
                    history.Installer.InstallerSha256 == task.Installer.InstallerSha256;
            });

            if (index >= 0) HistoryQueue.RemoveAt(index);
            HistoryQueue.Insert(0, task);
            LocalStorage.SetItemAsync("history_queue", HistoryQueue).AsTask();
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

            // Invoke javascript to fetch the installer
            JSRuntime!.InvokeVoidAsync("downloadFile", DotNetObjectReference.Create(task), fileName, url, task.CancelId).AsTask();
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