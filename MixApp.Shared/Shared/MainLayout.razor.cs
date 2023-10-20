using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using MixApp.Shared.Models;
using MixApp.Shared.Services;

namespace MixApp.Shared.Shared
{
    public partial class MainLayoutBase : LayoutComponentBase 
    {

        [Inject]
        ILocalStorageService LocalStorage { get; set; } = default!;

        [Inject]
        private GlobalEvent GlobalEvent { get; set; } = default!;

        [Inject]
        private IToastService ToastService { get; set; } = default!;

        public bool Expanded { get; set; } = false;

        public Software? Software { get; set; }

        public string Theme { get; set; } = "#333333";

        public string Color { get; set; } = "#82ddfd";

        protected override async Task OnInitializedAsync()
        {
            // Notification.OnToast += ToastService.ShowCommunicationToast;

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