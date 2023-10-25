using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using MixApp.Shared.Models;
using MixApp.Shared.Services;

namespace MixApp.Shared.Components
{
    public partial class DetailBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; } = new HttpClient();

        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = default!;

        [Parameter]
        public Action<DialogEventArgs> OnDismiss { get; set; }

        [Parameter]
        public Software? Software { get; set; }

        public List<Manifest> Manifests { get; set; } = [];

        public Manifest? Latest { get; set; }

        public DetailBase()
        {
            OnDismiss = (args) => Software = null;
        }

        protected async override void OnParametersSet()
        {
            if (Software == null)
            {
                GlobalEvent.OnDownloadQueueChanged -= StateHasChanged;
                return;
            }
            else
            {
                GlobalEvent.OnDownloadQueueChanged += StateHasChanged;
            }

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
            _ = HttpClient.GetAsync($"/meta/change?type={(int)MetaType.Visit}&identifier={Latest?.PackageIdentifier}");
        }

        public void ShowInLibrary()
        {
            OnDismiss.Invoke(new DialogEventArgs());
            Software = null;
            NavigationManager.NavigateTo("/Library");
        }
    }
}