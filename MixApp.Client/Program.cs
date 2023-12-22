using MixApp.Client.Extensions;
using PhotinoNET;
using System.CommandLine;
using System.Drawing;

namespace MixApp.Client;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        string windowTitle = "MixStore";

        RootCommand rootCommand = [];
        rootCommand.Initialize();

        if (args.Length != 0)
        {
            rootCommand.Invoke(args);
            return;
        }

        PhotinoWindow window = new PhotinoWindow()
            .SetTitle(windowTitle)
            .SetUseOsDefaultSize(false)
            .SetSize(new Size(1400, 930))
            // .SetContextMenuEnabled(false)
            // .SetDevToolsEnabled(false)
            .SetGrantBrowserPermissions(true)
            .Center()
            .RegisterWebMessageReceivedHandler((object? sender, string command) =>
            {
                PhotinoWindow? window = sender as PhotinoWindow;
                rootCommand.Invoke(command);
            })
            .Load(new Uri("https://mixstore.conchbrain.club/"));

        window.WaitForClose();
    }
}


