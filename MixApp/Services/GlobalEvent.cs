using MixApp.Models;

namespace MixApp.Services
{
    public class GlobalEvent
    {
        public event Action<Software>? OnOpenSoftware;

        public void OpenSoftware(Software software)
        {
            OnOpenSoftware?.Invoke(software);
        }
    }
}