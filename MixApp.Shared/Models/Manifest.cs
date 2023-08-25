namespace MixApp.Shared.Models;

public class Manifest
{
    public string? Id { get; set; }

    public string? PackageIdentifier { get; set; }

    public string? PackageVersion { get; set; }

    public string? MinimumOSVersion { get; set; }

    public string? Installers { get; set; }

    public string? InstallerType { get; set; }

    public string? NestedInstallerType { get; set; }

    public string? NestedInstallerFiles { get; set; }

    public bool RequireExplicitUpgrade { get; set; }

    public string? InstallerSuccessCodes { get; set; }

    public string? ExpectedReturnCodes { get; set; }

    public string? Commands { get; set; }

    public string? FileExtensions { get; set; }

    public string? InstallModes { get; set; }

    public string? InstallationNotes { get; set; }

    public string? eleaseNotesUrl { get; set; }

    public string? UnsupportedOSArchitectures { get; set; }

    public string? ReleaseNotes { get; set; }

    public string? ReleaseNotesUrl { get; set; }

    public string? RelaseDate { get; set; }

    public string? ReleaseDate { get; set; }

    public string? Protocols { get; set; }

    public string? Documentations { get; set; }

    public string? PurchaseUrl { get; set; }

    public string? Dependencies { get; set; }

    public string? PackageFamilyName { get; set; }

    public bool InstallLocationRequired { get; set; }

    public string? Platform { get; set; }

    public string? Scope { get; set; }

    public string? UpgradeBehavior { get; set; }

    public string? ProductCode { get; set; }

    public string? ManifestType { get; set; }

    public string? ManifestVersion { get; set; }

    public string? PackageLocale { get; set; }

    public string? Publisher { get; set; }

    public string? PublisherUrl { get; set; }

    public string? PublisherSupportUrl { get; set; }

    public string? PrivacyUrl { get; set; }

    public string? Author { get; set; }

    public string? PackageName { get; set; }

    public string? PackageUrl { get; set; }

    public string? License { get; set; }

    public string? LicenseUrl { get; set; }

    public string? Copyright { get; set; }

    public string? CopyrightUrl { get; set; }

    public string? ShortDescription { get; set; }

    public string? Description { get; set; }

    public string? Moniker { get; set; }

    public string? AppsAndFeaturesEntries { get; set; }

    public string? Tags { get; set; }

    public string? InstallerLocale { get; set; }

    public string? ElevationRequirement { get; set; }

    public string? InstallerSwitches { get; set; }

    public string? DefaultLocale { get; set; }
}