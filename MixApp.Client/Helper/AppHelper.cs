using Microsoft.Win32;
using MixApp.Client.Model;
using System;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text.Json;

namespace MixApp.Client;

public static class AppHelper
{
    private const string UninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

    public static void Install(string paraStr)
    {
        var param = JsonSerializer.Deserialize(paraStr, InstallParameterJsonCtx.Default.InstallParameter);
        if (param != null)
            Install(param);
    }

    public static void UnInstall(string _)
    {
        UnInstall();
    }

    [SupportedOSPlatform("windows")]
    public static void GetSoftewares(string _)
    {
        GetSoftewares();
    }

    /// <summary>
    /// Install Application
    /// </summary>
    /// <param name="pkgtype">installation package's Type, have msi or exe</param>
    /// <param name="pkgPath">installation package Path </param>
    /// <param name="defaultPath">default installation path</param>
    /// <param name="checkHash">Verify installation</param>
    /// <param name="silent">Whether to install silently</param>
    public static void Install(InstallParameter param)
    {
        if (!param.IsValid(out string errorMsg))
        {
            throw new ArgumentException($"“{nameof(param)}”不合法");
        }

        Task.Run(() =>
        {
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
                Console.WriteLine("Installation completed successfully");
            }
            else
            {
                Console.WriteLine("Installation failed ");
            }
        });
    }

    public static void UnInstall()
    {
        Process process = new();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.Verb = "runas";
        process.StartInfo.FileName = Path.Combine(Environment.SystemDirectory, "control.exe");
        process.StartInfo.Arguments = "/name Microsoft.ProgramsAndFeatures";
        process.Start();
    }


    /// <summary>
    /// Obtain a list of all installed software on this computer
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static List<SoftInfo> GetSoftewares()
    {
        List<SoftInfo> lst = new();
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            RegistryKey? regUninstall = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(UninstallKey, false);
            if (regUninstall != null) FindSoft(regUninstall, ref lst, RegistryView.Registry32);

            regUninstall = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(UninstallKey, false);
            if (regUninstall != null) FindSoft(regUninstall, ref lst, RegistryView.Registry64);

            regUninstall = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32).OpenSubKey(UninstallKey, false);
            if (regUninstall != null) FindSoft(regUninstall, ref lst, RegistryView.Registry32);

            regUninstall = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64).OpenSubKey(UninstallKey, false);
            if (regUninstall != null) FindSoft(regUninstall, ref lst, RegistryView.Registry64);
        }

        return lst;
    }

    private static void CreateMsiProcess(Process process, string pkgPath, string defaultPath, bool silent)
    {
        process.StartInfo.FileName = "msiexec.exe";

        if (silent)
            process.StartInfo.Arguments = "/quiet ";

        process.StartInfo.Arguments += $"/i \"{pkgPath}\" INSTALLDIR=\"{defaultPath}\" /l*v \"{defaultPath}\\install.log\"";
    }

    private static void CreateExeProcess(Process process, string pkgPath, string defaultPath, bool silent)
    {
        process.StartInfo.FileName = pkgPath;

        if (silent)
            process.StartInfo.Arguments = $"/S ";

        process.StartInfo.Arguments += $"/D={defaultPath}";
    }

    public static bool CheckInstallPath(string? path)
    {
        bool exists = Directory.Exists(path);
        return exists;
    }

    public static bool CheckPkgPath(string? path)
    {
        bool exists = File.Exists(path);
        return exists;
    }

    /// <summary>
    /// 根据path获取文件hash和checkHash比较，检查文件是否损坏
    /// </summary>
    /// <param name="path"></param>
    /// <param name="checkHash"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static bool CheckHash(string? path, string? checkHash)
    {
        // 判断文件是否存在
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("文件不存在");
        }
        using (var hashAlgorithm = SHA256.Create())
        {
            using (var fileStream = File.OpenRead(path))
            {
                var fileHash = hashAlgorithm.ComputeHash(fileStream);
                var fileHashString = BitConverter.ToString(fileHash).Replace("-", "");
                return fileHashString.Equals(checkHash, StringComparison.OrdinalIgnoreCase);
            }
        }
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
                    installDate = fileInfo.CreationTime.ToShortDateString(); //使用文件创建时间作为安装时间
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

