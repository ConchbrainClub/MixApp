using Microsoft.AspNetCore.Components;
using MixApp.Models;
using MixApp.Services;

namespace MixApp.Pages
{
    public partial class LibraryBase : ComponentBase
    {
        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = new();

        public record Person(int PersonId, string Name, DateOnly BirthDate);

        public List<Software> Softwares { get; set; } = new();

        protected override void OnInitialized()
        {
            Softwares = GlobalEvent.Softwares;
        }

        public string GetIcon(Software software)
        {
            Uri uri = new(software.PackageUrl ?? "https://www.conchbrain.club");
            return $"https://icon.horse/icon/{uri.Host}";
        }
    }
}