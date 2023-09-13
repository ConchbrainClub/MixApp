using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixApp.Client.Model
{
    public class InstallParameter
    {
        public string PkgType { get; internal set; } = string.Empty;
        public string PkgPath { get; internal set; } = string.Empty;
        public string DefaultPath { get; internal set; } = string.Empty;
        public string CheckHash { get; internal set; } = string.Empty;
        public bool Silent { get; internal set; } = false;

        public bool IsValid(out string errMsg)
        {
            errMsg = string.Empty;
            if (string.IsNullOrEmpty(PkgType))
                errMsg = "安装包类型不能为空";

            if (string.IsNullOrEmpty(PkgPath))
                errMsg = "安装包路径不能为空";

            if (!AppHelper.CheckPkgPath(PkgPath))
                errMsg = "安装路径不能为空";

            if (Silent && !AppHelper.CheckInstallPath(DefaultPath))
                errMsg = "默认安装路径不能为空";

            if (!AppHelper.CheckHash(PkgPath, CheckHash))
                errMsg = "安装包损坏";

            return true;
        }
    }
    [JsonSerializable(typeof(InstallParameter))]
    internal partial class InstallParameterJsonCtx : JsonSerializerContext { }
}
