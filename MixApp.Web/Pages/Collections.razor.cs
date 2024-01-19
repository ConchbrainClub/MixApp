using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using MixApp.Web.Models;
using MixApp.Web.Services;

namespace MixApp.Web.Pages
{
    public class CollectionsBase : ComponentBase
    {
        [Inject]
        HttpClient HttpClient { get; set; } = new HttpClient();

        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = default!;

        public List<Collection> Collections { get; set; } = [];

        public Collection? SelectedCollection { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (!firstRender) return;
            LoadData();
        }

        public async void LoadData()
        {
            Collections = await HttpClient.GetFromJsonAsync<List<Collection>>("/v1/collection") ?? [];
            StateHasChanged();
        }
    }
}