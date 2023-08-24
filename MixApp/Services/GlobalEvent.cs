using Microsoft.JSInterop;
using MixApp.Models;
using System.Text.Json;

namespace MixApp.Services
{
    public class GlobalEvent
    {
        private IJSRuntime JSRuntime { get; set; }

        public GlobalEvent(IJSRuntime runtime)
        {
            JSRuntime = runtime;
        }

        public event Action<Software>? OnOpenSoftware;

        public event Action<string>? OnChangeTheme;

        public event Action? OnDownloadQueueChanged;

        public List<DownloadTask> DownloadQueue { get; set; } = new();

        public List<Software> WaitQueue { get; set; } = new();

        public void OpenSoftware(Software software) => OnOpenSoftware?.Invoke(software);

        public void ChangeTheme(string color) => OnChangeTheme?.Invoke(color);

        public void Add2WaitQueue(Software software) => WaitQueue.Add(software);

        public void DownloadInstaller(Manifest manifest, Installer? installer = null)
        {
            List<Installer> installersObj = JsonSerializer.Deserialize<List<Installer>>(manifest.Installers!) ?? new();

            if (installer == null)
            {
                installer = installersObj.Find(i => i.Architecture == "x86");

                if (installersObj.FindIndex(i => i.Architecture == "x64") >= 0) 
                {
                    installer = installersObj.Find(i => i.Architecture == "x64");
                }
            }

            installer ??= installersObj.First();

            string fileName = (manifest?.PackageName ?? "unknow") + "." + installer?.InstallerUrl?.Split('.').Last() ?? "exe";
            string url = "https://cors.conchbrain.club?" + installer?.InstallerUrl;

            DownloadTask task = new(manifest!, installer!);

            task.OnProgressChanged += i => 
            {
                if (i.Progress == 100) DownloadQueue.Remove(i);
                OnDownloadQueueChanged?.Invoke();
            };

            DownloadQueue.Add(task);
            JSRuntime!.InvokeVoidAsync("downloadFile", DotNetObjectReference.Create(task), fileName, url).AsTask();
        }
    }
}