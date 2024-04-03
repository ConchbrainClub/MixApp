using Append.Blazor.Notifications;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MixApp.Web.Components
{
    public class RequestPermissionBase : ComponentBase
    {
        [Inject]
        ILocalStorageService LocalStorage { get; set; } = default!;

        [Inject]
        INotificationService NotificationService { get; set; } = default!;

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

        private bool enableProxy;

        public bool EnableProxy
        {
            get => enableProxy;
            set
            {
                if (value)
                {
                    LocalStorage.RemoveItemAsync("disable_proxy").AsTask();
                }
                else
                {
                    LocalStorage.SetItemAsStringAsync("disable_proxy", "true").AsTask();
                }
                enableProxy = value;
            }
        }

        protected async override Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(await LocalStorage.GetItemAsStringAsync("NotificationPermissionDialog")))
            {
                if (!await NotificationService.IsSupportedByBrowserAsync()) return;
                if (!string.IsNullOrEmpty(await LocalStorage.GetItemAsStringAsync("NotificationPermission"))) return;
                ShowDialog = true;
            }

            enableProxy = string.IsNullOrEmpty(await LocalStorage.GetItemAsStringAsync("disable_proxy").AsTask());
        }

        public async void NoLongerShowDialog()
        {
            await LocalStorage.SetItemAsStringAsync("NotificationPermissionDialog", "false");
            ShowDialog = false;
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
