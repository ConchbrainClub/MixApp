using MixApp.Web.Models;

namespace MixApp.Web.Services;

public class RemoteAssets(IConfiguration configuration)
{
    private readonly string baseAddress = configuration.GetValue<string>("AssetsAddress") ?? string.Empty;

    /// <summary>
    /// Get software or manifest icon
    /// </summary>
    /// <returns>icon url</returns>
    public static string GetIcon(Software software) => GetIcon(software.PackageUrl ?? string.Empty);

    public static string GetIcon(Manifest manifest) => GetIcon(manifest.PackageUrl ?? string.Empty);

    public static string GetIcon(string packageUrl)
    {
        Uri uri = new(packageUrl ?? "https://www.conchbrain.club");
        return $"https://cors.conchbrain.club?https://icon.horse/icon/{uri.Host}";
    }

    public string GetAssets(string? PackageIdentifier)
    {
        return baseAddress + $"/{PackageIdentifier ?? string.Empty}.png";
    }
}