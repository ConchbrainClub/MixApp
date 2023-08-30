using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using MixApp.Shared.Models;

namespace MixApp.Shared.Components
{
    public partial class DetailBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; } = new HttpClient();

        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;

        [Parameter]
        public Action<DialogEventArgs> OnDismiss { get; set; }

        [Parameter]
        public Software? Software { get; set; }

        public List<Manifest> Manifests { get; set; } = new();

        public Manifest? Latest { get; set; }

        public DetailBase()
        {
            OnDismiss = (args) => Software = null;
        }

        protected async override void OnParametersSet()
        {
            if (Software == null) return;
            Latest = null;

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
            StateHasChanged();
        }

        public void ShowInLibrary()
        {
            OnDismiss.Invoke(new DialogEventArgs());
            Software = null;
            NavigationManager.NavigateTo("/Library");
        }
    }
}