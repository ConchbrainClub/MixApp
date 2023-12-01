using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MixApp.Shared.Models;
using MixApp.Shared.Services;

namespace MixApp.Shared.Pages
{
    public class SoftwaresBase : ComponentBase
    {
        [Inject]
        HttpClient HttpClient { get; set; } = new HttpClient();

        [Inject]
        IJSRuntime? JSRunTime { get; set; }

        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = default!;

        public bool IsLoading { get; set; } = false;

        public int PageIndex { get; set; } = -1;

        public List<Software> Softwares { get; set; } = new();

        public Software? SelectedSoftware { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender) return;
            JSRunTime!.InvokeVoidAsync("initPageSoftware", DotNetObjectReference.Create(this)).AsTask();
            LoadData();
        }

        public async void LoadData()
        {
            IsLoading = true;
            StateHasChanged();

            SelectedSoftware = null;

            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>($"/software/index?index={++PageIndex}") 
                ?? new();

            softwares.ForEach(Softwares.Add);
            IsLoading = false;
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