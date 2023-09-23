using MixApp.Client.Helper;
using MixApp.Client.Model;
using PhotinoNET;
using System.Drawing;
using System.Security.Principal;
using System.Text.Json;

namespace MixApp.Client;

class Program
{
    [STAThread]
    static void Main(string[] _)
    {
        //string windowTitle = "MixStore";

        //PhotinoWindow window = new PhotinoWindow()
        //    .SetTitle(windowTitle)
        //    .SetUseOsDefaultSize(false)
        //    .SetSize(new Size(1400, 930))
        //    // .SetContextMenuEnabled(false)
        //    // .SetDevToolsEnabled(false)
        //    .SetGrantBrowserPermissions(true)
        //    .Center()
        //    .RegisterWebMessageReceivedHandler((object? sender, string sendMsgStr) =>
        //    {
        //        PhotinoWindow? window = sender as PhotinoWindow;
        //        Route.Map(window, sendMsgStr);
        //    })
        //    .Load(new Uri("https://mixstore.conchbrain.club/"));

        //window.WaitForClose();

        //string type = PkgType.exe;
        //string appPath = "D:\\MyProject\\TestApp\\Vim.exe";
        //string installPath = "D:\\MyProject\\TestApp";
        //string hash = "4BEFED920EB4C6D7C2720F9D0270FF1E9DD991A0581A41B123565C46F5BCB257";
        //AppHelper.Install(type, appPath, installPath, hash);

        //string type = PkgType.msi;
        //string appPath = "D:\\MyProject\\TestApp\\7-Zip.msi";
        //string installPath = "D:\\MyProject\\TestApp";
        //string hash = "0BA639B6DACDF573D847C911BD147C6384381A54DAC082B1E8C77BC73D58958B";
        //AppHelper.Install(type, appPath, installPath, hash);

        //AppHelper.UnInstall();

        //AppHelper.OpenDownloadFolder(string.Empty);

        Route.Init();

        var param = new InstallParameter
        {
            PkgType = "msi",
            PkgPath = "D:\\MyProject\\TestApp\\7-Zip.msi",
            DefaultPath = "D:\\MyProject\\TestApp",
            Hash = "0BA639B6DACDF573D847C911BD147C6384381A54DAC082B1E8C77BC73D58958B",
            Silent = true
        };

        var paramStr = JsonSerializer.Serialize(param, InstallParameterJsonCtx.Default.InstallParameter);

        var sendMsg = new SendMsg
        {
            ID = 123,
            MethodName = "GetSoftewares",
            Parameters = paramStr
        };
        var sendMsgStr = JsonSerializer.Serialize(sendMsg, SendMsgJsonCtx.Default.SendMsg);


        Route.Map(new PhotinoWindow(), sendMsgStr);



    }
}


