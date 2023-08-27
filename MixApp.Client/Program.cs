﻿using PhotinoNET;
using System.Drawing;

namespace MixApp.Client;

class Program
{
    [STAThread]
    static void Main(string[] _)
    {
        string windowTitle = "MixStore";

        PhotinoWindow window = new PhotinoWindow()
            .SetTitle(windowTitle)
            .SetUseOsDefaultSize(false)
            .SetSize(new Size(1400, 930))
            // .SetContextMenuEnabled(false)
            // .SetDevToolsEnabled(false)
            .SetGrantBrowserPermissions(true)
            .Center()
            .RegisterWebMessageReceivedHandler((object? sender, string message) =>
            {
                PhotinoWindow? window = sender as PhotinoWindow;
                string response = $"Received message: \"{message}\"";
                window?.SendWebMessage(response);
            })
            .Load(new Uri("https://mixstore.conchbrain.club/"));

        window.WaitForClose();
    }
}