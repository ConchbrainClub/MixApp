using Microsoft.AspNetCore.Components;

namespace MixApp.Shared
{
    public partial class MainLayoutBase : LayoutComponentBase 
    {
        public bool Expanded = false;

        public string keyWord = string.Empty;
    }
}