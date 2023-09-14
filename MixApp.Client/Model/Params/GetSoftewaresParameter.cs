using MixApp.Client.Model.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixApp.Client.Model
{
    public class GetSoftewaresParameter : ParamsBase
    {
    }


    [JsonSerializable(typeof(GetSoftewaresParameter))]
    internal partial class GetSoftewaresParameterJsonCtx : JsonSerializerContext { }
}
