using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using MixApp.Shared.Models;
using MixApp.Shared.Services;

namespace MixApp.Shared.Pages
{
    public partial class IndexBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; } = new HttpClient();

        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = default!;
        
        public Software? SelectedSoftware { get; set; }

        public List<Software> Softwares { get; set; } = new();

        public List<Software> RandomSoftwares { get; set; } = new();

        public List<Software> RecentlyUpdatedSoftwares { get; set; } = new();

        protected override void OnInitialized()
        {
            GlobalEvent.OnWaitQueueChanged += StateHasChanged;
            LoadTopData();
            LoadRandomData();
            LoadRecentlyUpdatedData();
        }

        private async void LoadTopData()
        {
            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>("/top") 
                ?? new();
            
            softwares.ForEach(Softwares.Add);
            StateHasChanged();
        }

        private async void LoadRandomData()
        {
            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>("/random") 
                ?? new();
            
            softwares.ForEach(RandomSoftwares.Add);
            StateHasChanged();
        }

        private async void LoadRecentlyUpdatedData()
        {
            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>("/recentlyUpdated") 
                ?? new();
            
            softwares.ForEach(RecentlyUpdatedSoftwares.Add);
            StateHasChanged();
        }
    }
    
}