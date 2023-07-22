using Microsoft.AspNetCore.Components;
using MixApp.Models;

namespace MixApp.Shared
{
    public partial class MainLayoutBase : LayoutComponentBase 
    {
        [Inject]
        private GlobalEvent GlobalEvent { get; set; } = new();

        public bool Expanded { get; set; } = false;

        public string KeyWord { get; set; } = string.Empty;

        public Software? Software { get; set; }

        protected override void OnInitialized()
        {
            GlobalEvent.OnOpenSoftware += (software) => 
            {
                Software = software;
                StateHasChanged();
            };

            base.OnInitialized();
        }
    }
}