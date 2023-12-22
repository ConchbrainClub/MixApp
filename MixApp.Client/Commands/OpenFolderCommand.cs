using System.CommandLine;
using System.Diagnostics;

namespace MixApp.Client.Commands;

public class OpenFolderCommand : Command
{
    public OpenFolderCommand() : base("folder", "open a folder")
    {
        this.SetHandler(Execute);
    }

    private void Execute()
    {
        string downloadsPath = Path.Combine
        (
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"
        );
        Process.Start("explorer.exe", downloadsPath);
    }
}
