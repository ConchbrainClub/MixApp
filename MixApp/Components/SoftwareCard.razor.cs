using Microsoft.AspNetCore.Components;
using MixApp.Models;
using MixApp.Services;

namespace MixApp.Components
{
    public partial class SoftwareCardBase : ComponentBase
    {
        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = new();

        [Parameter]
        public Software Software { get; set; } = new();

        public string GetIcon()
        {
            Uri uri = new Uri(Software.PackageUrl ?? "https://www.conchbrain.club");
            return $"https://icon.horse/icon/{uri.Host}";
        }
    }
}
