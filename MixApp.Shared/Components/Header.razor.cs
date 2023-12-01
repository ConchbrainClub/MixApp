using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using MixApp.Shared.Models;
using MixApp.Shared.Services;

namespace MixApp.Shared.Components
{
    public class HeaderBase : ComponentBase
    {
        [Inject]
        private HttpClient HttpClient { get; set; } = new();

        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = default!;

        private System.Timers.Timer? timer = null;

        private string? keyword;

        public string? Keyword
        {
            get => keyword;
            set
            {
                keyword = value;
                DisposeTimer();
                timer = new System.Timers.Timer(400);
                timer.Elapsed += TimerElapsed_TickAsync;
                timer.Enabled = true;
                timer.Start();
            }
        }

        public List<Software> SearchResults { get; set; } = [];

        private async void TimerElapsed_TickAsync(object? sender, EventArgs e)
        {
            DisposeTimer();
            await InvokeAsync(OnSearch);
        }

        private void DisposeTimer()
        {
            if (timer != null)
            {
                timer.Enabled = false;
                timer.Elapsed -= TimerElapsed_TickAsync;
                timer.Dispose();
                timer = null;
            }
        }

        private async void OnSearch()
        {
            if (string.IsNullOrEmpty(Keyword))
            {
                SearchResults.Clear();
                return;
            }

            SearchResults = await HttpClient
                .GetFromJsonAsync<List<Software>>($"/v1/software/index?keyword={Keyword}")
                ?? [];

            StateHasChanged();
        }

        public void OpenSoftware(Software software)
        {
            GlobalEvent.OpenSoftware(software);
            Keyword = string.Empty;
            StateHasChanged();
        }
    }
}