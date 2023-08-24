using MixApp.Models;

namespace MixApp;

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
}
