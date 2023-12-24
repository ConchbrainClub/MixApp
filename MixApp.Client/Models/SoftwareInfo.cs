using Microsoft.Win32;
using System.Text.Json.Serialization;

namespace MixApp.Client.Model;

public class SoftwareInfo(string name, string version, string dateTime, string uninstallString, string publisher, string estimatedSize, string installLocation, RegistryView registryView)
{
    public string Name { get; set; } = name;

    public string Version { get; set; } = version;
    public string DateTime { get; set; } = dateTime;
    public string UninstallString { get; set; } = uninstallString;
    public string Publisher { get; set; } = publisher;
    public string EstimatedSize { get; set; } = estimatedSize;
    public string InstallLocation { get; set; } = installLocation;
    public RegistryView RegistryView { get; set; } = registryView;
}

[JsonSerializable(typeof(SoftwareInfo))]
internal partial class SoftwareInfoJsonCtx : JsonSerializerContext { }

[JsonSerializable(typeof(List<SoftwareInfo>))]
internal partial class ListSoftwareInfoJsonCtx : JsonSerializerContext { }