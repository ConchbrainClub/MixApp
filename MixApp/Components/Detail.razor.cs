using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using MixApp.Models;
using System.Text.Json;
using Microsoft.JSInterop;

namespace MixApp.Components
{
    public partial class DetailBase : ComponentBase
    {
        [Inject]
        HttpClient HttpClient { get; set; } = new HttpClient();

        [Inject]
        IJSRuntime? JSRunTime { get; set; }

        public FluentDialog? Dialog;

        [Parameter]
        public Software? Software { get; set; }

        public Manifest Latest { get; set; } = new();

        public List<Manifest> Manifests { get; set; } = new();

        protected async override void OnParametersSet()
        {
            if (Software == null) return;

            Dialog?.Show();

            Manifests = (await HttpClient
                .GetFromJsonAsync<IEnumerable<Manifest>>($"/softwares/{Software?.PackageIdentifier}") ?? Array.Empty<Manifest>())
                .OrderByDescending(i => i.PackageVersion).ToList();

            Latest = Manifests.FirstOrDefault() ?? new();

            StateHasChanged();
        }

        public void OnDismiss(DialogEventArgs args)
        {
            Software = null;
        }

        public void Download(string? installers)
        {
            if (string.IsNullOrEmpty(installers)) return;

            Installer? installer = JsonSerializer.Deserialize<List<Installer>>(installers)?.Find(i =>
            {
                return i.Architecture == "x86" || i.Architecture == "x64";
            });

            JSRunTime?.InvokeVoidAsync("open", installer?.InstallerUrl);
        }
    }
}