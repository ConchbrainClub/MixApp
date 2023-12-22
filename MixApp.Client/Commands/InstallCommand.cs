using System.CommandLine;
using System.CommandLine.Binding;
using System.Diagnostics;

namespace MixApp.Client.Commands;

internal class InstallCommand : Command
{
    const string INSTALL_DIR = "C:/Program Files/";

    public InstallCommand() : base("install", "Install a package")
    {
        IValueDescriptor<string> installer = new Argument<string>("installer", "installer path");
        AddArgument((Argument)installer);

        IValueDescriptor<bool> silent = new Option<bool>("--silent", "silent install");
        AddOption((Option)silent);

        this.SetHandler(Execute, installer, silent);
    }

    private void Execute(string installer, bool silent)
    {
        Process process = new()
        {
            StartInfo = new FileInfo(installer).Extension switch
            {
                "msi" => InstallMSI(installer, silent),
                "exe" => InstallNSIS(installer, silent),
                _ => throw new ArgumentException("Invalid package type"),
            }
        };

        process.Start();
        process.WaitForExit();
    }

    private static ProcessStartInfo InstallMSI(string installer, bool silent)
    {
        string arguments = $"/i \"{installer}\" INSTALLDIR=\"{INSTALL_DIR}\" /l*v \"{INSTALL_DIR}\\install.log\" ";
        if (silent) arguments = "/quiet ";

        return new()
        {
            FileName = "msiexec.exe",
            UseShellExecute = false,
            CreateNoWindow = true,
            Verb = "runas",
            Arguments = arguments
        };
    }

    private static ProcessStartInfo InstallNSIS(string installer, bool silent)
    {
        string arguments = $"/D={INSTALL_DIR} ";
        if (silent) arguments = $"/S";

        return new()
        {
            FileName = installer,
            UseShellExecute = false,
            CreateNoWindow = true,
            Verb = "runas",
            Arguments = arguments
        };
    }
}