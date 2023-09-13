using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixApp.Client.Model
{
    public partial class SendMsg
    {
        public int ID { get; set; }

        public string MethodName { get; set; } = string.Empty;

        public string Parameters { get; set; } = string.Empty;

    }

    [JsonSerializable(typeof(SendMsg))]
    internal partial class SendMsgJsonCtx : JsonSerializerContext { }
}
