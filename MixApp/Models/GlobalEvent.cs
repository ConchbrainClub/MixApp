namespace MixApp.Models
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