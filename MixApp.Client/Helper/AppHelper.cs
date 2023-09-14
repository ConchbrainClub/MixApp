using Microsoft.Win32;
using MixApp.Client.Model;
using MixApp.Client.Model.Params;
using PhotinoNET;
using System;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using static MixApp.Client.AppHelper;

namespace MixApp.Client;

public static class AppHelper
{
    private const string UninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
    private const string Explorer = "explorer.exe";
    private const string Control = "control.exe";
    private const string Dowdloads = "Downloads";

    public delegate bool AppMethod<T>(T param, out string msg) where T : ParamsBase;

    private static void ExecuteMethod<T>(int id, PhotinoWindow? window, string paraStr, JsonTypeInfo type, AppMethod<T> appMethod) where T : ParamsBase
    {
        if (JsonSerializer.Deserialize(paraStr, type) is not ParamsBase param)
        {
            window?.SendWebMessage(JsonSerializer.Serialize(new ReceveMsg(id, false, $"参数解析失败"), ReceveMsgJsonCtx.Default.ReceveMsg));
            return;
        }
        param.Window = window;
        if (param is T realParam)
        {
            if (appMethod(realParam, out string msg))
                window?.SendWebMessage(JsonSerializer.Serialize(new ReceveMsg(id, true, msg), ReceveMsgJsonCtx.Default.ReceveMsg));
            else
                window?.SendWebMessage(JsonSerializer.Serialize(new ReceveMsg(id, false, msg), ReceveMsgJsonCtx.Default.ReceveMsg));
        }
    }

    public static void Install(int id, PhotinoWindow? window, string paraStr)
    {
        ExecuteMethod<InstallParameter>(id, window, paraStr, InstallParameterJsonCtx.Default.InstallParameter, Install);
    }

    public static void UnInstall(int id, PhotinoWindow? window, string paraStr)
    {
        ExecuteMethod<UnInstallParameter>(id, window, paraStr, UnInstallParameterJsonCtx.Default.UnInstallParameter, UnInstall);
    }

    [SupportedOSPlatform("windows")]
    public static void GetSoftewares(int id, PhotinoWindow? window, string paraStr)
    {
        ExecuteMethod<GetSoftewaresParameter>(id, window, paraStr, GetSoftewaresParameterJsonCtx.Default.GetSoftewaresParameter, GetSoftewares);
    }

    public static void OpenDownloadFolder(int id, PhotinoWindow? window, string paraStr)
    {
        ExecuteMethod<OpenDownloadFolderParameter>(id, window, paraStr, OpenDownloadFolderParameterJsonCtx.Default.OpenDownloadFolderParameter, OpenDownloadFolder);
    }

    /// <summary>
    /// Install Application
    /// </summary>
    /// <param name="pkgtype">installation package's Type, have msi or exe</param>
    /// <param name="pkgPath">installation package Path </param>
    /// <param name="defaultPath">default installation path</param>
    /// <param name="checkHash">Verify installation</param>
    /// <param name="silent">Whether to install silently</param>
    private static bool Install(InstallParameter param, out string msg)
    {
        if (!param.IsValid(out msg))
        {
            return false;
        }

        Process process = new();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.Verb = "runas";
        switch (param.PkgType)
        {
            case PkgType.msi:
                CreateMsiProcess(process, param.PkgPath, param.DefaultPath, param.Silent);
                break;
            case PkgType.exe:
                CreateExeProcess(process, param.PkgPath, param.DefaultPath, param.Silent);
                break;
            default:
                throw new ArgumentException("Invalid package type");
        }
        process.Start();
        process.WaitForExit();

        if (process.ExitCode == 0)
        {
            return true;
        }
        else
        {
            msg = "Installation failed ";
            return false;
        }
    }

    private static bool UnInstall(UnInstallParameter _, out string msg)
    {
        msg = string.Empty;
        var fileName = Path.Combine(Environment.SystemDirectory, Control);
        var arguments = "/name Microsoft.ProgramsAndFeatures";
        Process.Start(fileName, arguments);
        return true;
    }

    private static bool OpenDownloadFolder(OpenDownloadFolderParameter _, out string msg)
    {
        msg = string.Empty;
        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), Dowdloads);
        Process.Start(Explorer, downloadsPath);
        return true;
    }

    /// <summary>
    /// Obtain a list of all installed software on this computer
    /// </summary>
    [SupportedOSPlatform("windows")]
    private static bool GetSoftewares(GetSoftewaresParameter _, out string msg)
    {
        List<SoftInfo> lst = new();

        RegistryKey? regUninstall = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(UninstallKey, false);
        if (regUninstall != null) FindSoft(regUninstall, ref lst, RegistryView.Registry32);

        regUninstall = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(UninstallKey, false);
        if (regUninstall != null) FindSoft(regUninstall, ref lst, RegistryView.Registry64);

        regUninstall = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32).OpenSubKey(UninstallKey, false);
        if (regUninstall != null) FindSoft(regUninstall, ref lst, RegistryView.Registry32);

        regUninstall = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey(UninstallKey, false);
        if (regUninstall != null) FindSoft(regUninstall, ref lst, RegistryView.Registry64);

        msg = JsonSerializer.Serialize(lst, ListSoftInfoJsonCtx.Default.ListSoftInfo) ?? string.Empty;

        return string.IsNullOrEmpty(msg);
    }

    private static void CreateMsiProcess(Process process, string pkgPath, string defaultPath, bool silent)
    {
        process.StartInfo.FileName = "msiexec.exe";
        if (silent) process.StartInfo.Arguments = "/quiet ";
        process.StartInfo.Arguments += $"/i \"{pkgPath}\" INSTALLDIR=\"{defaultPath}\" /l*v \"{defaultPath}\\install.log\"";
    }

    private static void CreateExeProcess(Process process, string pkgPath, string defaultPath, bool silent)
    {
        process.StartInfo.FileName = pkgPath;
        if (silent) process.StartInfo.Arguments = $"/S ";
        process.StartInfo.Arguments += $"/D={defaultPath}";
    }

    [SupportedOSPlatform("windows")]
    private static void FindSoft(RegistryKey regUninstall, ref List<SoftInfo> lst, RegistryView registryView)
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

            SoftInfo? softModel = lst.FirstOrDefault(item1 => item1.Name == displayName);

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

public static class PkgType
{
    public const string msi = "msi";
    public const string exe = "exe";
}

