using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MixApp.Shared.Models;
using MixApp.Shared.Services;

namespace MixApp.Shared.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
        [Inject]
        ILocalStorageService LocalStorage { get; set; } = default!;

        [Inject]
        private GlobalEvent GlobalEvent { get; set; } = default!;

        [Inject]
        private IJSRuntime JS { get; set; } = default!;


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

            GlobalEvent.OnThemeChanged += async () => 
            {
                await InitTheme();
                StateHasChanged();
            };

            await InitTheme();
        }

        private async Task InitTheme()
        {
            string theme = await LocalStorage.GetItemAsStringAsync("theme");

            if (string.IsNullOrEmpty(theme))
            {
                bool isDark = await JS.InvokeAsync<bool>("systemIsDarkTheme");
                Theme = isDark ? "#333333" : "#f5f5f5";
            }
            else
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