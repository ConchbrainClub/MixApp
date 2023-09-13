using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixApp.Client.Model
{
    public class ReceveMsg
    {
        public int ID { get; set; }

        public bool Result { get; set; }

        public string Msg { get; set; } = string.Empty;

        public string ErrorMsg { get; set; } = string.Empty;
    }

    [JsonSerializable(typeof(ReceveMsg))]
    internal partial class ReceveMsgJsonCtx : JsonSerializerContext { }
}
