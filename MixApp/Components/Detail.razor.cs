using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using MixApp.Models;
using System.Text.Json;
using Microsoft.JSInterop;
using MixApp.Services;

namespace MixApp.Components
{
    public partial class DetailBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; } = new HttpClient();

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
                .OrderByDescending(i =>
                {
                    if (DateTime.TryParse(i.ReleaseDate, out DateTime releaseDate))
                    {
                        return releaseDate;
                    }
                    return DateTime.Now;
                })
                .OrderByDescending(i => 
                {
                    if (Version.TryParse(i.PackageVersion, out Version? version))
                    {
                        return version;
                    }
                    return new Version();
                })
                .ToList();

            Latest = Manifests.First();
            Installers = JsonSerializer.Deserialize<List<Installer>>(Latest.Installers!) ?? new();

            StateHasChanged();
        }
    }
}