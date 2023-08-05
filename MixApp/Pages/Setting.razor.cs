using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;

namespace MixApp.Pages
{
    public partial class SettingBase : ComponentBase
    {
        [Inject]
        IJSRuntime? JSRunTime { get; set; }

        [Inject]
        ILocalStorageService LocalStorage { get; set; } = default!;

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

        protected override async Task OnInitializedAsync()
        {
            LocaleOptions = new List<Option<string>>()
            {
                new Option<string> { Text = "p.setting.auto", Value = "" },
                new Option<string> { Text = "p.setting.zh_cn", Value = "zh-CN" },
                new Option<string> { Text = "p.setting.en_us", Value = "en-US" }
            };

            string locale = await LocalStorage.GetItemAsStringAsync("locale").AsTask();
            selectedLocale = LocaleOptions.SingleOrDefault(i => i.Value == locale);
        }
    }
}