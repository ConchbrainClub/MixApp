using MixApp.Client.Model;
using PhotinoNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MixApp.Client.Helper
{
    public static class Route
    {
        public static List<Router> Routers = new();


        public static void Init()
        {
            Routers.Add(new Router(nameof(AppHelper.Install), AppHelper.Install, InstallParameterJsonCtx.Default.InstallParameter));
            Routers.Add(new Router(nameof(AppHelper.UnInstall), AppHelper.UnInstall, UnInstallParameterJsonCtx.Default.UnInstallParameter));
            Routers.Add(new Router(nameof(AppHelper.GetSoftewares), AppHelper.GetSoftewares, GetSoftewaresParameterJsonCtx.Default.GetSoftewaresParameter));
        }

        public static void Map(PhotinoWindow? window, string message)
        {
            var sendMsg = JsonSerializer.Deserialize(message, SendMsgJsonCtx.Default.SendMsg);

            if(sendMsg == null)
            {
                return;
            }
            Routers.FirstOrDefault(x => x.MethodName == sendMsg.MethodName)?.Method?.Invoke(sendMsg.Parameters);

            var receveMsg = new ReceveMsg
            {
                ID = sendMsg.ID,
                Result = true
            };
            string? res = JsonSerializer.Serialize(receveMsg, ReceveMsgJsonCtx.Default.ReceveMsg);
            window?.SendWebMessage(res);

        }
    }
}
