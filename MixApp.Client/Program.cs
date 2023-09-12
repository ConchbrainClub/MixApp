using PhotinoNET;
using System.Drawing;
using System.Security.Principal;

namespace MixApp.Client;

class Program
{
    [STAThread]
    static void Main(string[] _)
    {
        // string windowTitle = "MixStore";

        // PhotinoWindow window = new PhotinoWindow()
        //     .SetTitle(windowTitle)
        //     .SetUseOsDefaultSize(false)
        //     .SetSize(new Size(1400, 930))
        //     // .SetContextMenuEnabled(false)
        //     // .SetDevToolsEnabled(false)
        //     .SetGrantBrowserPermissions(true)
        //     .Center()
        //     .RegisterWebMessageReceivedHandler((object? sender, string message) =>
        //     {
        //         PhotinoWindow? window = sender as PhotinoWindow;
        //         string response = $"Received message: \"{message}\"";
        //         window?.SendWebMessage(response);
        //     })
        //     .Load(new Uri("https://mixstore.conchbrain.club/"));

        // window.WaitForClose();

        //// 获取当前用户的 WindowsIdentity 对象
        //WindowsIdentity identity = WindowsIdentity.GetCurrent();
        //// 创建一个 WindowsPrincipal 对象，传入 identity 参数
        //WindowsPrincipal principal = new WindowsPrincipal(identity);
        //// 调用 IsInRole 方法，传入 WindowsBuiltInRole.Administrator 参数
        //bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
        //// 根据 isAdmin 的值进行不同的操作
        //if (isAdmin)
        //{
        //    Console.WriteLine("You are an administrator");
        //}
        //else
        //{
        //    Console.WriteLine("You are not an administrator");
        //}

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

        AppHelper.GetSoftewares();

    }
}


