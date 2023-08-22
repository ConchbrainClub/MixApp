using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using MixApp.Models;

namespace MixApp.Components
{
    public partial class SideBarBase : ComponentBase
    {
        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        public List<RouteInfo> Top { get; set; }

        public List<RouteInfo> Bottom { get; set; }

        public string CurrentHref { get; set; }

        public SideBarBase()
        {
            Top = new()
            {
                new()
                {
                    Href = "/",
                    IconFilled = new Icons.Filled.Size24.Home(),
                    IconRegular = new Icons.Regular.Size24.Home(),
                    Name = "c.side_bar.home"
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
            };

            Bottom = new()
            {
                new()
                {
                    Href = "/Setting",
                    IconFilled = new Icons.Filled.Size24.Settings(),
                    IconRegular = new Icons.Regular.Size24.Settings(),
                    Name = "c.side_bar.setting"
                },
                new()
                {
                    Href = "javascript:void(0)",
                    IconFilled = new Icons.Filled.Size24.ChatHelp(),
                    IconRegular = new Icons.Regular.Size24.ChatHelp(),
                    Name = "c.side_bar.help"
                }
            };

            CurrentHref = "/";
        }

        protected override void OnInitialized()
        {
            RouteInfo routeInfo = Top.Concat(Bottom)
                .SingleOrDefault(i => new Uri(Navigation!.Uri).AbsolutePath == i.Href) ?? Top.First();

            CurrentHref = routeInfo.Href;
        }
    }
}
