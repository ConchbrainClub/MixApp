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
        public ReceveMsg()
        {

        }
        public ReceveMsg(int iD)
        {
            ID = iD;
        }
        public ReceveMsg(int iD, bool result, string msg = "")
        {
            ID = iD;
            Result = result;
            Msg = msg;
        }
        public int ID { get; set; }

        public bool Result { get; set; } = true;

        public string Msg { get; set; } = string.Empty;
    }

    [JsonSerializable(typeof(ReceveMsg))]
    internal partial class ReceveMsgJsonCtx : JsonSerializerContext { }
}
