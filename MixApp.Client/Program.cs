﻿using PhotinoNET;
using System.Drawing;

namespace MixApp.Client;

class Program
{
    [STAThread]
    static void Main(string[] _)
    {
        string windowTitle = "MixStore";

        var window = new PhotinoWindow()
            .SetTitle(windowTitle)
            .SetUseOsDefaultSize(false)
            .SetSize(new Size(1400, 930))
            // .SetChromeless(true)
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