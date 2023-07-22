using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace MixApp.Shared
{
    public partial class MainLayoutBase : LayoutComponentBase 
    {
        [Inject]
        GlobalState GlobalState { get; set; } = default!;

        public bool Expanded { get; set; } = false;

        public string KeyWord { get; set; } = string.Empty;
    }
}