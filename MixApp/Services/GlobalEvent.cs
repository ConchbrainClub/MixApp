using MixApp.Models;

namespace MixApp.Services
{
    public class GlobalEvent
    {
        public event Action<Software>? OnOpenSoftware;

        public event Action<string>? OnChangeTheme;

        public List<Software> WaitQueue { get; set; } = new();

        public void OpenSoftware(Software software)
        {
            OnOpenSoftware?.Invoke(software);
        }

        public void ChangeTheme(string color)
        {
            OnChangeTheme?.Invoke(color);
        }

        public void Add2WaitQueue(Software software) 
        {
            WaitQueue.Add(software);
        }
    }
}