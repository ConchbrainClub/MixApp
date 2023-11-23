using Append.Blazor.Notifications;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MixApp.Shared.Components
{
    public class RequestPermissionBase : ComponentBase
    {
        [Inject]
        ILocalStorageService LocalStorage { get; set; } = default!;

        [Inject]
        INotificationService NotificationService { get; set; } = default!;

        [Inject]
        private IJSRuntime JS { get; set; } = default!;

        private bool showDialog = false;

        public bool ShowDialog
        {
            get => showDialog;
            set
            {
                showDialog = value;
                StateHasChanged();
            }
        }

        protected async override Task OnInitializedAsync()
        {
            bool isMac = await JS.InvokeAsync<bool>("getIsMacOS");
            if (!isMac)
            {
                if (!await NotificationService.IsSupportedByBrowserAsync()) return;
                if (!string.IsNullOrEmpty(await LocalStorage.GetItemAsStringAsync("NotificationPermission"))) return;

                ShowDialog = true;
            }
        }

        public async void RequestPermission()
        {
            PermissionType permission = await NotificationService.RequestPermissionAsync();
            if (permission == PermissionType.Granted)
            {
                _ = NotificationService.CreateAsync("通知测试", "成功啦", "favicon.png").AsTask();
            }

            await LocalStorage.SetItemAsStringAsync("NotificationPermission", permission.ToString());
            ShowDialog = false;
        }
    }
}
