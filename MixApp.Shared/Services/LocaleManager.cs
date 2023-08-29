using System.Net.Http.Json;

namespace MixApp.Shared.Services
{
    public class LocaleManager
    {
        private readonly string[] supportLocale = new [] { "zh-CN", "en-US" };

        public Dictionary<string, string> Scripts { get; set; } = new();

        public async Task<LocaleManager> Initialize(HttpClient httpClient, string locale = "en-US")
        {
            if (!supportLocale.Contains(locale)) locale = "en-US";
            Scripts = await httpClient.GetFromJsonAsync<Dictionary<string, string>>($"/locale/{locale}.json") ?? new();
            return this;
        }
    }
}