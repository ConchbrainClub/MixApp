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

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender) return;
            JSRunTime!.InvokeVoidAsync("initHighLight", cardReference).AsTask();
        }
    }
}
