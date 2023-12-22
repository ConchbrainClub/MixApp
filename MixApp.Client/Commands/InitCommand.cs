using System.CommandLine;

namespace delivr.Commands
{
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
}
