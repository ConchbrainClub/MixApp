using System.CommandLine;

namespace MixApp.Client.Commands;

internal class InitCommand : Command
{
    public InitCommand() : base("init", "Initialize")
    {
        this.SetHandler(Execute);
    }

    private void Execute()
    {

    }
}