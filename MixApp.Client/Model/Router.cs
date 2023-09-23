using PhotinoNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace MixApp.Client.Model
{
    public class Router
    {
        public Router(string methodName, Action<int, PhotinoWindow?, string> method, JsonTypeInfo parameterType)
        {
            MethodName = methodName;
            Method = method;
            ParameterType = parameterType;
        }
        public string MethodName { get; set; } = string.Empty;
        public Action<int, PhotinoWindow?, string> Method { get; set; }
        public JsonTypeInfo ParameterType { get; set; }
    }
}
