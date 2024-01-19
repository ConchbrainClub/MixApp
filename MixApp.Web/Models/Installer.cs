namespace MixApp.Web.Models;

public class AppsAndFeaturesEntry
{
    public string? DisplayName { get; set; }

    public string? Publisher { get; set; }

    public string? DisplayVersion { get; set; }

    public string? ProductCode { get; set; }
}

public class InstallerSwitches
{
    public string? InstallLocation { get; set; }
    
    public string? Custom { get; set; }
}

public class Installer
{
    public string? Architecture { get; set; }

    public string? Scope { get; set; }

    public string? InstallerUrl { get; set; }
    
    public string? InstallerSha256 { get; set; }

    public string? ProductCode { get; set; }

    public InstallerSwitches? InstallerSwitches { get; set; }

    public List<AppsAndFeaturesEntry>? AppsAndFeaturesEntries { get; set; }
}