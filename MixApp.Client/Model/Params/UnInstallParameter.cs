﻿using MixApp.Client.Model.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixApp.Client.Model
{
    public class UnInstallParameter : ParamsBase
    {
    }

    [JsonSerializable(typeof(UnInstallParameter))]
    internal partial class UnInstallParameterJsonCtx : JsonSerializerContext { }
}