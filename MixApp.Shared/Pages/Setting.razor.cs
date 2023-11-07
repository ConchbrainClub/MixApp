using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace MixApp.Shared.Pages
{
    public class SettingBase : ComponentBase
    {
        [Inject]
        IJSRuntime? JSRunTime { get; set; }

        [Inject]
        ILocalStorageService LocalStorage { get; set; } = default!;

        // Locale setting

        private Option<string>? selectedLocale;

        public Option<string>? SelectedLocale
        {
            get => selectedLocale;
            set
            {
                selectedLocale = value;
                LocalStorage.SetItemAsStringAsync("locale", value?.Value).AsTask();
                JSRunTime!.InvokeVoidAsync("reload").AsTask();
            }
        }

        public List<Option<string>>? LocaleOptions { get; set; }

        // Theme setting

        private Option<string>? selectedTheme;

        public Option<string>? SelectedTheme
        {
            get => selectedTheme;
            set
            {
                selectedTheme = value;
                LocalStorage.SetItemAsStringAsync("theme", value?.Value).AsTask();
                JSRunTime!.InvokeVoidAsync("reload").AsTask();
            }
        }

        public List<Option<string>>? ThemeOptions { get; set; }

        // Color setting

        private string? selectedColor;

        public string? SelectedColor
        {
            get => selectedColor;
            set
            {
                selectedColor = value;
                LocalStorage.SetItemAsStringAsync("color", value).AsTask();
                JSRunTime!.InvokeVoidAsync("reload").AsTask();
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

        private string? downloadProxy;

        public string? DownloadProxy
        {
            get => downloadProxy;
            set
            {
                // =============================================
                // Check Download proxy is correct 

                LocalStorage.SetItemAsStringAsync("download_proxy", value).AsTask();
                downloadProxy = value;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            // Init locale options

            LocaleOptions =
            [
                new Option<string> { Text = "p.setting.auto", Value = "" },
                new Option<string> { Text = "p.setting.zh_cn", Value = "zh-CN" },
                new Option<string> { Text = "p.setting.en_us", Value = "en-US" }
            ];

            string locale = await LocalStorage.GetItemAsStringAsync("locale").AsTask();
            selectedLocale = LocaleOptions.SingleOrDefault(i => i.Value == locale);

            // Init theme options

            // Application.Current!.RequestedTheme;

            ThemeOptions =
            [
                new Option<string> { Text = "p.setting.theme_auto", Value = "" },
                new Option<string> { Text = "p.setting.light", Value = "#f5f5f5" },
                new Option<string> { Text = "p.setting.dark", Value = "#333333" }
            ];

            string theme = await LocalStorage.GetItemAsStringAsync("theme").AsTask();
            
            selectedTheme = ThemeOptions.SingleOrDefault(i => i.Value == theme)
                ?? ThemeOptions.Single(i => i.Text == "p.setting.theme_auto");

            // Init base color
            
            string color = await LocalStorage.GetItemAsStringAsync("color").AsTask();

            if (string.IsNullOrEmpty(color))
            {
                color = "#82ddfd";
            }

            selectedColor = color;

            // Init download proxy

            enableProxy = string.IsNullOrEmpty(await LocalStorage.GetItemAsStringAsync("disable_proxy").AsTask());
            downloadProxy = await LocalStorage.GetItemAsStringAsync("download_proxy").AsTask();
        }

        public async void CleanUpNow()
        {
            await JSRunTime!.InvokeVoidAsync("resetSetting").AsTask();
        }
    }
}