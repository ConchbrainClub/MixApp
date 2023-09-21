using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MixApp.Shared.Models;
using MixApp.Shared.Services;

namespace MixApp.Shared.Components
{
    public partial class HeaderBase : ComponentBase
    {
        [Inject]
        private HttpClient HttpClient { get; set; } = new();

        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = default!;

        public string? KeyWord { get; set; }

        public bool IsFocus { get; set; }

        public List<Software> SearchResults { get; set; } = [];

        public async void UserInput(ChangeEventArgs args)
        {
            KeyWord = args.Value?.ToString();

            if (string.IsNullOrEmpty(args.Value?.ToString())) 
            {
                SearchResults.Clear();
                return;
            }
            
            SearchResults = await HttpClient
                .GetFromJsonAsync<List<Software>>($"/softwares?keyword={args.Value}") 
                ?? new();

            StateHasChanged();
        }

        public void OnFocus(FocusEventArgs args)
        {
            if (args.Type == "focusin")
            {
                IsFocus = true;
                StateHasChanged();
                return;
            }
            
            Task.Run(() => 
            {
                Thread.Sleep(500);
                IsFocus = false;
                StateHasChanged();
            });
        }

        public void OpenSoftware(Software software)
        {
            GlobalEvent.OpenSoftware(software);
            StateHasChanged();
        }
    }
}