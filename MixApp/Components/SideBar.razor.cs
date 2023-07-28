using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

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
                    Name = "Home"
                },
                new()
                {
                    Href = "/softwares",
                    IconFilled = new Icons.Filled.Size24.Apps(),
                    IconRegular = new Icons.Regular.Size24.Apps(),
                    Name = "Apps"
                },
                new()
                {
                    Href = "/counter",
                    IconFilled = new Icons.Filled.Size24.Library(),
                    IconRegular = new Icons.Regular.Size24.Library(),
                    Name = "Library"
                }
            };

            Bottom = new()
            {
                new()
                {
                    Href = "/setting",
                    IconFilled = new Icons.Filled.Size24.Settings(),
                    IconRegular = new Icons.Regular.Size24.Settings(),
                    Name = "Setting"
                },
                new()
                {
                    Href = "/help",
                    IconFilled = new Icons.Filled.Size24.ChatHelp(),
                    IconRegular = new Icons.Regular.Size24.ChatHelp(),
                    Name = "Help"
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