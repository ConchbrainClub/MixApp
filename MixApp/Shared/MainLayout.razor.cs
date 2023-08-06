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
        private GlobalEvent GlobalEvent { get; set; } = new();

        public bool Expanded { get; set; } = false;

        public Software? Software { get; set; }

        public string Theme { get; set; } = "#333333";

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
                Console.WriteLine(theme);
                Theme = theme;
                StateHasChanged();
            }
        }
    }
}