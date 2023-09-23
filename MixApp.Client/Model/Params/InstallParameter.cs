using MixApp.Client.Model.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixApp.Client.Model
{
    public class InstallParameter : ParamsBase
    {
        public string PkgType { get; set; } = string.Empty;
        public string PkgPath { get; set; } = string.Empty;
        public string DefaultPath { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public bool Silent { get; set; } = false;

        public bool IsValid(out string errMsg)
        {
            errMsg = string.Empty;
            if (string.IsNullOrEmpty(PkgType))
                errMsg += "安装包类型不能为空\n";

            if (string.IsNullOrEmpty(PkgPath))
                errMsg += "安装包路径不能为空\n";

            if (!CheckPkgPath(PkgPath))
                errMsg += "安装路径解析失败\n";

            if (Silent && !CheckInstallPath(DefaultPath))
                errMsg += "默认安装路径解析失败\n";

            if (!CheckHash(PkgPath, Hash))
                errMsg += "安装包损坏";

            return string.IsNullOrEmpty(errMsg);
        }

        public static bool CheckInstallPath(string? path) => Directory.Exists(path);

        public static bool CheckPkgPath(string? path) => File.Exists(path);

        /// <summary>
        /// 根据path获取文件hash和checkHash比较，检查文件是否损坏
        /// </summary>
        /// <param name="path"></param>
        /// <param name="checkHash"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool CheckHash(string? path, string? checkHash)
        {
            // 判断文件是否存在
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("文件不存在");
            }
            using (var hashAlgorithm = SHA256.Create())
            {
                using (var fileStream = File.OpenRead(path))
                {
                    var fileHash = hashAlgorithm.ComputeHash(fileStream);
                    var fileHashString = BitConverter.ToString(fileHash).Replace("-", "");
                    return fileHashString.Equals(checkHash, StringComparison.OrdinalIgnoreCase);
                }
            }
        }
    }
    [JsonSerializable(typeof(InstallParameter))]
    internal partial class InstallParameterJsonCtx : JsonSerializerContext { }
}
