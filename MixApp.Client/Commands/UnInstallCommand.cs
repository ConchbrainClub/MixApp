using System.CommandLine;
using System.Diagnostics;

namespace MixApp.Client.Commands;

public class UnInstallCommand : Command
{
    public UnInstallCommand() : base("uninstall", "uninstall a package")
    {
        this.SetHandler(Execute);
    }

    private void Execute()
    {
        var fileName = Path.Combine(Environment.SystemDirectory, "control.exe");
        var arguments = "/name Microsoft.ProgramsAndFeatures";
        Process.Start(fileName, arguments);
    }
}
