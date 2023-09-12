using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace MixApp.Client;

public static class AppHelper
{
    /// <summary>
    /// Install Application
    /// </summary>
    /// <param name="pkgtype">installation package's Type, have msi or exe</param>
    /// <param name="pkgPath">installation package Path </param>
    /// <param name="defaultPath">default installation path</param>
    /// <param name="checkHash">Verify installation</param>
    /// <param name="silent">Whether to install silently</param>
    public static void Install(string pkgtype, string pkgPath, string defaultPath, string checkHash, bool silent = true)
    {
        if (string.IsNullOrEmpty(pkgtype))
            throw new ArgumentException($"“{nameof(pkgtype)}”不能为 null 或空。", nameof(pkgtype));

        if (string.IsNullOrEmpty(pkgPath))
            throw new ArgumentException($"“{nameof(pkgPath)}”不能为 null 或空。", nameof(pkgPath));

        if (!CheckPkgPath(pkgPath))
            throw new ArgumentException("Invalid installation package path");

        if (silent && !CheckInstallPath(defaultPath))
            throw new ArgumentException("Invalid default installation path");
        if (!CheckHash(pkgPath, checkHash))
            throw new ArgumentException("。。。");
        Process process = new();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.Verb = "runas";

        switch (pkgtype)
        {
            case PkgType.msi:
                CreateMsiProcess(process, pkgPath, defaultPath, silent);
                break;
            case PkgType.exe:
                CreateExeProcess(process, pkgPath, defaultPath, silent);
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
    }

    public static void UnInstall()
    {

    }

    /// <summary>
    /// Obtain a list of all installed software on this computer
    /// </summary>
    public static void GetSoftewares()
    {
        // 创建一个空的列表，用来存储软件的名称
        var softwareList = new List<string>();
        // 打开注册表中的软件安装信息的键
        var softwareKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
        // 遍历该键下的所有子键
        foreach (var subKeyName in softwareKey.GetSubKeyNames())
        {
            // 打开子键
            var subKey = softwareKey.OpenSubKey(subKeyName);
            // 获取子键中的DisplayName值，如果存在，就表示该子键对应一个软件
            var displayName = subKey.GetValue("DisplayName") as string;
            // 如果displayName不为空，就将其添加到列表中
            if (!string.IsNullOrEmpty(displayName))
            {
                softwareList.Add(displayName);
            }
        }
        // 关闭注册表键
        softwareKey.Close();
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

    private static bool CheckInstallPath(string path)
    {
        bool exists = Directory.Exists(path);
        return exists;
    }

    private static bool CheckPkgPath(string path)
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
    private static bool CheckHash(string path, string checkHash)
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



}

public static class PkgType
{
    public const string msi = "msi";
    public const string exe = "exe";
}