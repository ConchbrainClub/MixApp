using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using MixApp.Models;

namespace MixApp.Pages
{
    public partial class IndexBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; } = new HttpClient();

        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = new();
        
        public Software? SelectedSoftware { get; set; }

        public List<Software> Softwares { get; set; } = new();

        public List<Software> RandomSoftwares { get; set; } = new();

        public List<Software> RecentlyUpdatedSoftwares { get; set; } = new();

        protected override void OnInitialized()
        {
            LoadTopData();
            LoadRandomData();
            LoadRecentlyUpdatedData();
           
        }

        private async void LoadTopData()
        {
            SelectedSoftware = null;

            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>($"/top") 
                ?? new();
            softwares.ForEach(i => Softwares.Add(i));
             StateHasChanged();
        }

        private async void LoadRandomData()
        {
            SelectedSoftware = null;

            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>($"/random") 
                ?? new();
            
            softwares.ForEach(i => RandomSoftwares.Add(i));
             StateHasChanged();
        }

        private async void LoadRecentlyUpdatedData()
        {
            SelectedSoftware = null;
            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>($"/recentlyUpdated") 
                ?? new();
            
            softwares.ForEach(i => RecentlyUpdatedSoftwares.Add(i));
            StateHasChanged();
        }
    }
    
}