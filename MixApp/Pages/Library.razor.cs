using Microsoft.AspNetCore.Components;
using MixApp.Models;

namespace MixApp.Pages
{
    public partial class LibraryBase : ComponentBase
    {
        public static string GetIcon(Software software)
        {
            Uri uri = new(software.PackageUrl ?? "https://www.conchbrain.club");
            return $"https://icon.horse/icon/{uri.Host}";
        }
    }
}