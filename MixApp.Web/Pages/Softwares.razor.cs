using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MixApp.Web.Models;
using MixApp.Web.Services;

namespace MixApp.Web.Pages
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

        public bool IsFirstRender { get; set; } = true;

        public int PageIndex { get; set; } = -1;

        public List<Software> Softwares { get; set; } = [];

        public Software? SelectedSoftware { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) return;
            await JSRunTime!.InvokeVoidAsync("initPageSoftware", DotNetObjectReference.Create(this));
            LoadData();
            IsFirstRender = false;
        }

        public async void LoadData()
        {
            IsLoading = true;
            StateHasChanged();

            SelectedSoftware = null;

            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>($"/v1/software/index?index={++PageIndex}") 
                ?? [];

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