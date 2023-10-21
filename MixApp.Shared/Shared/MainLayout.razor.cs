using Append.Blazor.Notifications;
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
        private IDialogService DialogService { get; set; } = default!;

        [Inject]
        private INotificationService NotificationService { get; set; } = default!;

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

            CheckNotifyPermission();
        }

        private async void CheckNotifyPermission()
        {
            if(!await NotificationService.IsSupportedByBrowserAsync()) return;

            if (await LocalStorage.GetItemAsStringAsync("NotificationPermission") == "Granted") return;

            IDialogReference dialog = await DialogService.ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>()
            {
                Content = new()
                {
                    Title = "获得更好的使用体验",
                    MarkupMessage = new MarkupString
                    (
                        """
                        <i>MixStore 推荐您开启以下权限</i>
                        <br/><br/>
                        允许 <u><strong>通知</strong></u> 权限来提醒您软件的下载状态
                        """
                    )
                }
            });

            if ((await dialog.Result).Cancelled) return;

            PermissionType permission = await NotificationService.RequestPermissionAsync();
            if (permission != PermissionType.Granted) return;

            await LocalStorage.SetItemAsStringAsync("NotificationPermission", permission.ToString());
        }
    }
}