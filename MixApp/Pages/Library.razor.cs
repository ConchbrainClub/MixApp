using Microsoft.AspNetCore.Components;
using MixApp.Services;

namespace MixApp.Pages
{
    public partial class LibraryBase : ComponentBase
    {
        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = default!;

        protected override void OnInitialized()
        {
            GlobalEvent.OnDownloadQueueChanged += StateHasChanged;
            base.OnInitialized();
        }
    }
}