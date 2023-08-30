using Microsoft.AspNetCore.Components;
using MixApp.Shared.Models;
using MixApp.Shared.Services;
using System.Net.Http.Json;

namespace MixApp.Shared.Pages
{
    public partial class LibraryBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; } = new HttpClient();

        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = default!;

        protected override void OnInitialized()
        {
            GlobalEvent.OnDownloadQueueChanged += StateHasChanged;
            base.OnInitialized();
        }

        public async void DownloadFromWaitQueue(Software software)
        {
            IEnumerable<Manifest> manifests = await HttpClient.GetFromJsonAsync<IEnumerable<Manifest>>($"/softwares/{software?.PackageIdentifier}")
                ?? Array.Empty<Manifest>();

            Manifest latest = manifests
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
                .First();

            GlobalEvent.DownloadInstaller(latest);
            RemoveFromWaitQueue(software!);
        }

        public void RemoveFromWaitQueue(Software software) => GlobalEvent.WaitQueue.Remove(software);

        public void CancelDownload(DownloadTask task)
        {
            // cancel fetch
            GlobalEvent.DownloadQueue.Remove(task);
        }

        public void RemoveFromHistoryQueue(DownloadTask task) => GlobalEvent.HistoryQueue.Remove(task);
    }
}