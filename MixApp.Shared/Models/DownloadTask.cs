using Microsoft.JSInterop;

namespace MixApp.Shared.Models;

public class DownloadTask(Manifest manifest, Installer installer)
{
    public string CancelId { get; } = Guid.NewGuid().ToString();

    public Manifest Manifest { get; set; } = manifest;

    public Installer Installer { get; set; } = installer;

    public int Progress { get; set; }

    public event Action<DownloadTask>? OnProgressChanged;

    [JSInvokable]
    public void ChangedProgress(int progress)
    {
        if (Progress == progress) return;
        Progress = progress;
        OnProgressChanged?.Invoke(this);
    }
}
