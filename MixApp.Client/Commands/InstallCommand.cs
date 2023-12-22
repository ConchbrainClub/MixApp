using System.CommandLine;

namespace delivr.Commands
{
    internal class InstallCommand : Command
    {
        public InstallCommand() : base("install", "Install a package")
        {
            this.SetHandler(Execute);
        }

        private void Execute()
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
                return true;
            }
            else
            {
                msg = "Installation failed ";
                return false;
            }
        }
    }
}
