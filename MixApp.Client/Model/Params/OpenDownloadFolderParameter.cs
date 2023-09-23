using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixApp.Client.Model.Params
{
    public class OpenDownloadFolderParameter : ParamsBase
    {
    }

    [JsonSerializable(typeof(OpenDownloadFolderParameter))]
    internal partial class OpenDownloadFolderParameterJsonCtx : JsonSerializerContext { }
}
