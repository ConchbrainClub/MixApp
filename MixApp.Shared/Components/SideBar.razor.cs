using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using MixApp.Shared.Models;
using MixApp.Shared.Services;

namespace MixApp.Shared.Components
{
    public class SideBarBase : ComponentBase
    {
        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        [Inject]
        public GlobalEvent GlobalEvent { get; set; } = default!;

        public List<RouteInfo> Top { get; set; }

        public List<RouteInfo> Bottom { get; set; }

        public string CurrentHref { get; set; }

        public SideBarBase()
        {
            Top =
            [
                new()
                {
                    Href = "/",
                    IconFilled = new Icons.Filled.Size24.Home(),
                    IconRegular = new Icons.Regular.Size24.Home(),
                    Name = "c.side_bar.home"
                },
                new()
                {
                    Href = "/Collections",
                    IconFilled = new Icons.Filled.Size24.BoxSearch(),
                    IconRegular = new Icons.Regular.Size24.BoxSearch(),
                    Name = "c.side_bar.collections"
                },
                new()
                {
                    Href = "/Softwares",
                    IconFilled = new Icons.Filled.Size24.Apps(),
                    IconRegular = new Icons.Regular.Size24.Apps(),
                    Name = "c.side_bar.apps"
                },
                new()
                {
                    Href = "/Library",
                    IconFilled = new Icons.Filled.Size24.Library(),
                    IconRegular = new Icons.Regular.Size24.Library(),
                    Name = "c.side_bar.library"
                }
            ];

            Bottom =
            [
                new()
                {
                    Href = "/Setting",
                    IconFilled = new Icons.Filled.Size24.Settings(),
                    IconRegular = new Icons.Regular.Size24.Settings(),
                    Name = "c.side_bar.setting"
                },
                new()
                {
                    Href = "/Help",
                    IconFilled = new Icons.Filled.Size24.ChatHelp(),
                    IconRegular = new Icons.Regular.Size24.ChatHelp(),
                    Name = "c.side_bar.help"
                }
            ];

            CurrentHref = "/";
        }

        protected override void OnInitialized()
        {
            GlobalEvent.OnDownloadQueueChanged += StateHasChanged; 
            RouteInfo routeInfo = Top.Concat(Bottom)
                .SingleOrDefault(i => new Uri(Navigation!.Uri).AbsolutePath == i.Href) ?? Top.First();

            CurrentHref = routeInfo.Href;
        }
    }
}
