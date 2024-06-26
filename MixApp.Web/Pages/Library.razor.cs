using Microsoft.AspNetCore.Components;
using MixApp.Web.Models;
using MixApp.Web.Services;
using System.Net.Http.Json;

namespace MixApp.Web.Pages
{
    public class LibraryBase : ComponentBase, IDisposable
    {
        [Inject]
        public HttpClient HttpClient { get; set; } = new HttpClient();

        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = default!;

        protected override void OnInitialized()
        {
            GlobalEvent.OnHistoryQueueChanged += StateHasChanged;
            GlobalEvent.OnDownloadQueueChanged += StateHasChanged;
            GlobalEvent.OnDownloadProgressChanged += StateHasChanged;
            base.OnInitialized();
        }

        public async void DownloadFromWaitQueue(WaitItem waitItem)
        {
            waitItem.IsFetchingInfo = true;
            IEnumerable<Manifest> manifests = await HttpClient
                .GetFromJsonAsync<IEnumerable<Manifest>>($"/v1/software/detail?id={waitItem.Software?.PackageIdentifier}")
                ?? Array.Empty<Manifest>();

            Manifest latest = manifests.OrderByDescending(i =>
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
            .First();

            GlobalEvent.DownloadInstaller(latest);
            GlobalEvent.WaitQueue.Remove(waitItem!);
        }

        public void Dispose()
        {
            GlobalEvent.OnHistoryQueueChanged -= StateHasChanged;
            GlobalEvent.OnDownloadQueueChanged -= StateHasChanged;
            GlobalEvent.OnDownloadProgressChanged -= StateHasChanged;
        }
    }
}