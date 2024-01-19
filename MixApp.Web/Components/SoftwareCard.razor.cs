using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MixApp.Web.Models;

namespace MixApp.Web.Components
{
    public class SoftwareCardBase : ComponentBase
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
