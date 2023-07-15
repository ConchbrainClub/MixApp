using Microsoft.AspNetCore.Components;

namespace MixApp.Shared
{
    public partial class MainLayoutBase : LayoutComponentBase 
    {
        public bool Expanded { get; set; } = false;

        public string KeyWord { get; set; } = string.Empty;
    }
}