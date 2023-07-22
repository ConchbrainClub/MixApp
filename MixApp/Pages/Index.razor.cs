using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MixApp.Models;

namespace MixApp.Pages
{
    public partial class IndexBase : ComponentBase
    {
        [Inject]
        HttpClient HttpClient { get; set; } = new HttpClient();
        
        public Software? SelectedSoftware { get; set; }

        public List<Software> Softwares { get; set; } = new();

        protected override void OnInitialized()
        {
            LoadData();
        }

        private async void LoadData()
        {
            SelectedSoftware = null;

            List<Software> softwares = await HttpClient
                .GetFromJsonAsync<List<Software>>($"/top") 
                ?? new();
            
            softwares.ForEach(i => {
                i.Cover = HttpClient.BaseAddress?.ToString() + i.Cover;
                Softwares.Add(i);
            });

            StateHasChanged();
        }
    }
    
}