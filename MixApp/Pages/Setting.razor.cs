using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace MixApp.Pages
{
    public partial class SettingBase : ComponentBase
    {
        [Inject]
        ILocalStorageService LocalStorage { get; set; } = default!;
    }
}