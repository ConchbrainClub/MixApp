using System.CommandLine;
using System.Runtime.Versioning;
using System.Text.Json;
using Microsoft.Win32;
using MixApp.Client.Model;

namespace MixApp.Client.Commands;

[SupportedOSPlatform("windows")]
public class ListCommand : Command
{
    const string UNINSTALL_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

    public ListCommand() : base("list", "list installed softwares")
    {
        this.SetHandler(Execute);
    }

    private void Execute()
    {
        List<SoftwareInfo> softwareInfos = [];

        RegistryKey? regUninstall = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(UNINSTALL_KEY, false);
        if (regUninstall != null) FindSoft(regUninstall, ref softwareInfos, RegistryView.Registry32);

        regUninstall = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(UNINSTALL_KEY, false);
        if (regUninstall != null) FindSoft(regUninstall, ref softwareInfos, RegistryView.Registry64);

        regUninstall = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32).OpenSubKey(UNINSTALL_KEY, false);
        if (regUninstall != null) FindSoft(regUninstall, ref softwareInfos, RegistryView.Registry32);

        regUninstall = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey(UNINSTALL_KEY, false);
        if (regUninstall != null) FindSoft(regUninstall, ref softwareInfos, RegistryView.Registry64);

        string result = JsonSerializer.Serialize(softwareInfos, ListSoftwareInfoJsonCtx.Default.ListSoftwareInfo) ?? string.Empty;

        Console.WriteLine(result);
    }

    private static void FindSoft(RegistryKey regUninstall, ref List<SoftwareInfo> lst, RegistryView registryView)
    {
        foreach (var item in regUninstall.GetSubKeyNames())
        {
            RegistryKey? regSub = regUninstall.OpenSubKey(item, false);
            if (regSub == null) continue;

            string displayName = regSub.GetValue("DisplayName") as string ?? string.Empty;
            string installLocation = regSub.GetValue("InstallLocation") as string ?? string.Empty;
            string uninstallString = regSub.GetValue("UninstallString") as string ?? string.Empty;
            string displayVersion = regSub.GetValue("DisplayVersion") as string ?? string.Empty;
            string installDate = regSub.GetValue("InstallDate") as string ?? string.Empty;
            string publisher = regSub.GetValue("Publisher") as string ?? string.Empty;
            string displayIcon = regSub.GetValue("DisplayIcon") as string ?? string.Empty;
            int estimatedSize = (int)regSub.GetValue("EstimatedSize", 0);
            int systemComponent = (int)regSub.GetValue("SystemComponent", 0);

            if (string.IsNullOrWhiteSpace(displayName)) continue;
            if (string.IsNullOrWhiteSpace(uninstallString)) continue;
            if (string.IsNullOrWhiteSpace(displayVersion) && string.IsNullOrWhiteSpace(displayIcon)) continue;
            if (systemComponent == 1) continue;

            if (string.IsNullOrWhiteSpace(installDate) && !string.IsNullOrWhiteSpace(displayIcon))
            {
                try
                {
                    string[] array = displayIcon.Split(',');
                    if (array.Length >= 2)
                    {
                        uninstallString = array[0];
                    }
                    FileInfo fileInfo = new(uninstallString);
                    installDate = fileInfo.CreationTime.ToShortDateString();
                }
                catch (Exception)
                {
                }
            }

            SoftwareInfo? softModel = lst.FirstOrDefault(item1 => item1.Name == displayName);

            if (softModel == null)
            {
                lst.Add(new(
                    displayName,
                    displayVersion,
                    installDate,
                    uninstallString,
                    publisher,
                    estimatedSize == 0 ? "未知" : (estimatedSize / 1024.0).ToString("0.00M"),
                    installLocation,
                    registryView));
            }
            else
            {
                if (string.IsNullOrWhiteSpace(softModel.DateTime) && !string.IsNullOrWhiteSpace(installDate))
                {
                    softModel.DateTime = installDate;
                }
            }
        }
    }
}
