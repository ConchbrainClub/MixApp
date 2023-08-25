using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MixApp.Shared.Models;

namespace MixApp.Shared.Components
{
    public partial class SoftwareCardBase : ComponentBase
    {
        [Inject]
        IJSRuntime JSRunTime { get; set; } = default!;

        public ElementReference cardReference;

        [Parameter]
        public Software Software { get; set; } = new();

        public string GetIcon()
        {
            Uri uri = new(Software.PackageUrl ?? "https://www.conchbrain.club");
            return $"https://icon.horse/icon/{uri.Host}";
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender) return;
            JSRunTime!.InvokeVoidAsync("initHighLight", cardReference).AsTask();
        }
    }
}
