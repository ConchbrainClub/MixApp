using Microsoft.JSInterop;
using MixApp.Models;

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

        public void DownloadApp(Manifest manifest, Installer installer)
        {
            
        }

        public void DownloadFile(string fileName, string url)
        {
            JSRuntime!.InvokeVoidAsync("downloadFile", DotNetObjectReference.Create(this), fileName, url).AsTask();
        }
    }
}