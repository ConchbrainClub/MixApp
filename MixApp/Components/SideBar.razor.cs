using Microsoft.AspNetCore.Components;

namespace MixApp.Components
{
    public partial class SideBarBase : ComponentBase
    {
        public string ActivePage { get; set; } = "Home";
    }
}