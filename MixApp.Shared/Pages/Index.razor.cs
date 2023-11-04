using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
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
            GlobalEvent.OnWaitQueueChanged += StateHasChanged;
            LoadTopData();
            LoadRandomData();
            LoadRecentlyUpdatedData();
        }

        private async void LoadTopData()
        {
            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>("/top") ?? [];
            
            softwares.ForEach(TrendingSoftwares.Add);
            StateHasChanged();
        }

        private async void LoadRandomData()
        {
            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>("/random") 
                ?? [];
            
            softwares.ForEach(RandomSoftwares.Add);
            StateHasChanged();
        }

        private async void LoadRecentlyUpdatedData()
        {
            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>("/recent") ?? [];
            
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