using System.Net.Http.Json;

namespace MixApp.Services
{
    public class LocaleManager
    {
        private HttpClient httpClient;

        private string locale = string.Empty;

        public event Action? OnLoaded;

        public Dictionary<string, string> Scripts { get; set; }

        public string Locale
        {
            get => locale;
            set
            {
                locale = value;
                LoadScript();
            }
        }

        public LocaleManager(string baseAddress)
        {
            Scripts = new();

            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(baseAddress)
            };

            Locale = "zh-CN";
        }

        public async void LoadScript()
        {
            Scripts = await httpClient.GetFromJsonAsync<Dictionary<string, string>>($"/lang/{Locale}.json") ?? new();
            OnLoaded?.Invoke();
        }
    }
}