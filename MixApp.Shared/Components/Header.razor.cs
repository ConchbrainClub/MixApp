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

        public bool IsFocus { get; set; }

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
                .GetFromJsonAsync<List<Software>>($"/softwares?keyword={Keyword}")
                ?? [];

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
                Thread.Sleep(1000);
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