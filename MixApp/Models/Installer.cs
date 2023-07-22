namespace MixApp.Models
{
    public class Installer
    {
        public string? Architecture { get; set; }

        public string? InstallerUrl { get; set; }
        
        public string? InstallerSha256 { get; set; }
    }
}