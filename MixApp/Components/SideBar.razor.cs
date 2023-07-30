using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MixApp.Components
{
    public partial class SideBarBase : ComponentBase
    {
        [Inject]
        public NavigationManager? Navigation { get; set; }

        [Inject]
        public IJSRuntime? JSRuntime { get; set; }

        public string? ActivePage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var uri = new Uri(Navigation?.Uri??"");
            string pageName = uri.Segments.LastOrDefault() ?? "";
            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("console.log", pageName);
            }
            ActivePage = pageName;
            
        }
    }
}
