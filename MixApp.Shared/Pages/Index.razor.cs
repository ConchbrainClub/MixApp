using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using MixApp.Shared.Models;
using MixApp.Shared.Services;

namespace MixApp.Shared.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; } = new HttpClient();

        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = default!;
        
        public FluentHorizontalScroll HorizontalScroll { get; set; } = default!;

        public Software? SelectedSoftware { get; set; }

        public List<Software> TrendingSoftwares { get; set; } = [];

        public List<Software> RandomSoftwares { get; set; } = [];

        public List<Software> RecentlyUpdatedSoftwares { get; set; } = [];

        protected override void OnInitialized()
        {
            LoadTopData();
            LoadRandomData();
            LoadRecentlyUpdatedData();
        }

        private async void LoadTopData()
        {
            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>("/v1/software/trending") ?? [];
            
            softwares.ForEach(TrendingSoftwares.Add);
            StateHasChanged();
        }

        private async void LoadRandomData()
        {
            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>("/v1/software/recommend") 
                ?? [];
            
            softwares.ForEach(RandomSoftwares.Add);
            StateHasChanged();
        }

        private async void LoadRecentlyUpdatedData()
        {
            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>("/v1/software/latest") ?? [];
            
            softwares.ForEach(RecentlyUpdatedSoftwares.Add);
            StateHasChanged();
        }

        public void ScrollToFirst(Software software)
        {
            int index = TrendingSoftwares.IndexOf(software);
            HorizontalScroll.ScrollInView(index);
        }
    }
    
}