using Microsoft.JSInterop;

namespace MixApp.Models;

public class DownloadTask
{
    public DownloadTask(Manifest manifest, Installer installer)
    {
        Id = Guid.NewGuid().ToString();
        Manifest = manifest;
        Installer = installer;
    }

    public string Id { get; set; }

    public Manifest Manifest { get; set; }

    public Installer Installer { get; set; }

    public int Progress { get; set; }

    public event Action<DownloadTask>? OnProgressChanged;

    [JSInvokable]
    public void ChangedProgress(int progress)
    {
        Progress = progress;
        OnProgressChanged?.Invoke(this);
    }
}
