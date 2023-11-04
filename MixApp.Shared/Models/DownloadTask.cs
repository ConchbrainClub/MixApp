using Microsoft.JSInterop;

namespace MixApp.Shared.Models;

public class DownloadTask(Manifest manifest, Installer installer)
{
    public string CancelId { get; } = Guid.NewGuid().ToString();

    public Manifest Manifest { get; set; } = manifest;

    public Installer Installer { get; set; } = installer;

    private int progress;

    public int Progress 
    { 
        get => progress;
        set
        {
            if (progress != value)
            {
                progress = value;
            }
        }
    }

    public event Action<DownloadTask>? OnProgressChanged;

    [JSInvokable]
    public void ChangedProgress(int value)
    {
        Progress = value;
        OnProgressChanged?.Invoke(this);
    }
}
