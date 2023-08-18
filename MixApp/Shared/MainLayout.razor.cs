using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MixApp.Models;
using MixApp.Services;

namespace MixApp.Shared
{
    public partial class MainLayoutBase : LayoutComponentBase 
    {

        [Inject]
        ILocalStorageService LocalStorage { get; set; } = default!;

        [Inject]
        private GlobalEvent GlobalEvent { get; set; } = default!;

        public bool Expanded { get; set; } = false;

        public Software? Software { get; set; }

        public string Theme { get; set; } = "#333333";

        public string Color { get; set; } = "#82ddfd";

        protected override async Task OnInitializedAsync()
        {
            GlobalEvent.OnOpenSoftware += software => 
            {
                Software = software;
                StateHasChanged();
            };

            string theme = await LocalStorage.GetItemAsStringAsync("theme");

            if (!string.IsNullOrEmpty(theme))
            {
                Theme = theme;
            }

            string color = await LocalStorage.GetItemAsStringAsync("color");

            if (!string.IsNullOrEmpty(color))
            {
                Color = color;
            }
        }
    }
}