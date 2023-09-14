using MixApp.Client.Model;
using MixApp.Client.Model.Params;
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
            Routers.Add(new Router(nameof(AppHelper.OpenDownloadFolder), AppHelper.OpenDownloadFolder, OpenDownloadFolderParameterJsonCtx.Default.OpenDownloadFolderParameter));
        }

        public static void Map(PhotinoWindow? window, string message)
        {
            var sendMsg = JsonSerializer.Deserialize(message, SendMsgJsonCtx.Default.SendMsg);
            if (sendMsg == null)
            {
                window?.SendWebMessage(JsonSerializer.Serialize(new ReceveMsg(-1, false, $"参数解析失败"), ReceveMsgJsonCtx.Default.ReceveMsg));
                return;
            }

            try
            {
                Routers.FirstOrDefault(x => x.MethodName == sendMsg.MethodName)?.Method?.Invoke(sendMsg.ID, window, sendMsg.Parameters);
            }
            catch (Exception e)
            {
                window?.SendWebMessage(JsonSerializer.Serialize(new ReceveMsg(sendMsg.ID, false, $"执行失败:{e}"), ReceveMsgJsonCtx.Default.ReceveMsg));
            }
            finally
            {
                window?.SendWebMessage(JsonSerializer.Serialize(new ReceveMsg(sendMsg.ID), ReceveMsgJsonCtx.Default.ReceveMsg));
            }



        }
    }
}
