namespace MixApp.Shared.Services;

public class RemoteAssets
{
    private readonly string baseAddress;

    public RemoteAssets(string baseAddress)
    {
        this.baseAddress = baseAddress;
    }

    public string GetAssets(string? path)
    {
        return baseAddress + path ?? string.Empty;
    }
}