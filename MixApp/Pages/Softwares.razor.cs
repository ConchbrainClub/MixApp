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

        public Software? SelectedSoftware { get; set; }

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
            SelectedSoftware = null;

            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>($"/softwares?index={++PageIndex}") 
                ?? new();
            
            softwares.ForEach(i => {
                i.Cover = HttpClient.BaseAddress?.ToString() + i.Cover;
                Softwares.Add(i);
            });

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