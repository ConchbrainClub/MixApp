using System.Net.Http.Json;

namespace MixApp.Services
{
    public class LocaleManager
    {
        private HttpClient httpClient;

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

        public async Task<LocaleManager> Initialize(string locale = "zh-CN")
        {
            Scripts = await httpClient.GetFromJsonAsync<Dictionary<string, string>>($"/lang/{locale}.json") ?? new();
            OnLoaded?.Invoke();
            return this;
        }
    }
}