using System.CommandLine;
using delivr.Commands;

namespace JSDelivrCLI.Extensions
{
    public static class RootCommandExtension
    {
        public static RootCommand Initialize(this RootCommand rootCommand)
        {
            rootCommand.Add(new InitCommand());
            return rootCommand;
        }
    }
}
