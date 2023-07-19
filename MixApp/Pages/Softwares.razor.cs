using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MixApp.Models;

namespace MixApp.Pages
{
    public partial class SoftwaresBase : ComponentBase
    {
        [Inject]
        HttpClient HttpClient { get; set; } = new HttpClient();

        [Inject]
        IJSRuntime? JSRunTime { get; set; }

        public int PageIndex { get; set; } = -1;

        public List<Software> Softwares { get; set; } = new();

        protected override void OnInitialized()
        {
            LoadData();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;
            await JSRunTime!.InvokeVoidAsync("InitPageSoftware", DotNetObjectReference.Create(this));
        }

        private async void LoadData()
        {
            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>($"/softwares?index={++PageIndex}") 
                ?? new();
            
            Softwares.AddRange(softwares);
            StateHasChanged();
        }

        [JSInvokable]
        public void OnScrollEnd(bool scrollEnd)
        {
            if (!scrollEnd) return;
            LoadData();
        }
    }
}