using Microsoft.Fast.Components.FluentUI;

namespace MixApp.Models;

public class RouteInfo
{
    public string Href { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public Icon IconRegular { get; set; } = new Icons.Regular.Size24.Home();

    public Icon IconFilled { get; set; } = new Icons.Filled.Size24.Home();
}