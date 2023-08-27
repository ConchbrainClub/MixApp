using Microsoft.JSInterop;
using MixApp.Shared.Models;
using System.Text.Json;

namespace MixApp.Shared.Services
{
    public class GlobalEvent
    {
        private IJSRuntime JSRuntime { get; set; }

        public GlobalEvent(IJSRuntime runtime)
        {
            JSRuntime = runtime;
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
        /// Add the download task info to download history queue
        /// </summary>
        /// <param name="task">download task info</param>
        public void Add2HistoryQueue(DownloadTask task)
        {
            int index = HistoryQueue.FindIndex(history => 
            {
                return history.Manifest.PackageIdentifier == task.Manifest.PackageIdentifier &&
                    history.Manifest.PackageVersion == task.Manifest.PackageVersion &&
                    history.Installer.InstallerSha256 == task.Installer.InstallerSha256;
            });

            if (index >= 0) HistoryQueue.RemoveAt(index);
            HistoryQueue.Add(task);
        }

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
                    Add2HistoryQueue(i);
                }
                OnDownloadQueueChanged?.Invoke();
            };

            DownloadQueue.Add(task);
            // Invoke javascript to fetch the installer
            JSRuntime!.InvokeVoidAsync("downloadFile", DotNetObjectReference.Create(task), fileName, url).AsTask();
        }
    }
}