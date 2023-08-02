using System.Net.Http.Json;

namespace MixApp.Services
{
    public class LocaleManager
    {
        private HttpClient httpClient;

        private string[] supportLocale = new [] { "zh-CN", "en-US" };

        public event Action? OnLoaded;

        public Dictionary<string, string> Scripts { get; set; }

        public LocaleManager(string baseAddress)
        {
            Scripts = new();

            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public async Task Initialize(string locale = "en-US")
        {
            if (!supportLocale.Contains(locale)) locale = "en-US";
            Scripts = await httpClient.GetFromJsonAsync<Dictionary<string, string>>($"/locale/{locale}.json") ?? new();
            OnLoaded?.Invoke();
        }
    }
}