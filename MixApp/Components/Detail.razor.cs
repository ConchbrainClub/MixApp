using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using MixApp.Models;
using System.Text.Json;
using Microsoft.JSInterop;
using Semver;

namespace MixApp.Components
{
    public partial class DetailBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; } = new HttpClient();

        [Inject]
        IJSRuntime? JSRunTime { get; set; }

        [Parameter]
        public Action<DialogEventArgs> OnDismiss { get; set; }

        [Parameter]
        public Software? Software { get; set; }

        public List<Manifest> Manifests { get; set; } = new();

        public Manifest Latest { get; set; } = new();

        public List<Installer> Installers { get; set; } = new();

        public DetailBase()
        {
            OnDismiss = (args) => Software = null;
        }

        protected async override void OnParametersSet()
        {
            if (Software == null) return;

            Manifests = (await HttpClient
                .GetFromJsonAsync<IEnumerable<Manifest>>($"/softwares/{Software?.PackageIdentifier}") ?? Array.Empty<Manifest>())
                .OrderByDescending(i => new Version(i.PackageVersion ?? "0.0.1"))
                .ToList();

            Latest = Manifests.First();
            Installers = JsonSerializer.Deserialize<List<Installer>>(Latest.Installers!) ?? new();

            StateHasChanged();
        }

        public void Download(string? installers)
        {
            if (string.IsNullOrEmpty(installers)) return;

            List<Installer> installersObj = JsonSerializer.Deserialize<List<Installer>>(installers) ?? new();

            Installer? installer = installersObj.Find(i => i.Architecture == "x86");

            if (installersObj.FindIndex(i => i.Architecture == "x64") > 0) 
            {
                installer = installersObj.Find(i => i.Architecture == "x64");
            }

            JSRunTime?.InvokeVoidAsync("open", installer?.InstallerUrl);
        }
    }
}