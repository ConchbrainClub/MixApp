using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixApp.Client.Model
{
    public class SoftInfo
    {
        public SoftInfo(string name, string version, string dateTime, string uninstallString, string publisher, string estimatedSize, string installLocation, RegistryView registryView)
        {
            Name = name;
            Version = version;
            DateTime = dateTime;
            UninstallString = uninstallString;
            Publisher = publisher;
            EstimatedSize = estimatedSize;
            InstallLocation = installLocation;
            RegistryView = registryView;
        }
        public string Name { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;
        public string DateTime { get; set; } = string.Empty;
        public string UninstallString { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string EstimatedSize { get; set; } = string.Empty;
        public string InstallLocation { get; set; } = string.Empty;
        public RegistryView RegistryView { get; set; }

    }
}
