using MixApp.Shared.Models;

namespace MixApp.Shared.Services;

public class RemoteAssets(string baseAddress)
{
    private readonly string baseAddress = baseAddress;

    /// <summary>
    /// Get software or manifest icon
    /// </summary>
    /// <returns>icon url</returns>
    public static string GetIcon(Software software) => GetIcon(software.PackageUrl ?? string.Empty);

    public static string GetIcon(Manifest manifest) => GetIcon(manifest.PackageUrl ?? string.Empty);

    public static string GetIcon(string packageUrl)
    {
        Uri uri = new(packageUrl ?? "https://www.conchbrain.club");
        return $"https://icon.horse/icon/{uri.Host}";
    }

    public string GetAssets(string? path)
    {
        return baseAddress + path ?? string.Empty;
    }
}