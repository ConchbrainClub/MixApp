﻿using Microsoft.JSInterop;

namespace MixApp.Shared.Models;

public class DownloadTask
{
    public DownloadTask(Manifest manifest, Installer installer)
    {
        Manifest = manifest;
        Installer = installer;
        CancelId = Guid.NewGuid().ToString();
    }

    public string CancelId { get; }

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
