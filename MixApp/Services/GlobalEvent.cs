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

            JSRuntime!.InvokeVoidAsync("downloadFile", DotNetObjectReference.Create(this), fileName, url).AsTask();
        }

        [JSInvokable]
        public void OnProgressChanged(int progress)
        {
            Console.WriteLine(progress);
        }
    }
}