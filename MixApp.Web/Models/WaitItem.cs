namespace MixApp.Web.Models;

public class WaitItem
{
    public bool IsFetchingInfo { get; set; } = false;

    public Software? Software { get; set; }
}