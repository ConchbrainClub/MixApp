using System.CommandLine;
using MixApp.Client.Commands;

namespace MixApp.Client.Extensions
{
    public static class RootCommandExtension
    {
        public static RootCommand Initialize(this RootCommand rootCommand)
        {
            rootCommand.Add(new InitCommand());

            if(OperatingSystem.IsWindows())
            {
                rootCommand.Add(new InstallCommand());
                rootCommand.Add(new UnInstallCommand());
                rootCommand.Add(new OpenFolderCommand());
                rootCommand.Add(new ListCommand());
            }

            return rootCommand;
        }
    }
}
