using Microsoft.AspNetCore.Components;
using MixApp.Models;

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
            Uri uri = new Uri(Software.PackageUrl ?? string.Empty);
            return $"{uri.Scheme}://{uri.Host}/favicon.ico";
        }
    }
}