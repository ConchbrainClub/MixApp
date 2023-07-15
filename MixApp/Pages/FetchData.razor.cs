using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using MixApp.Models;

namespace MixApp.Pages
{
    public partial class FetchDataBase : ComponentBase
    {
        [Inject]
        HttpClient HttpClient { get; set; } = new HttpClient();

        public IQueryable<WeatherForecast>? Forecasts { get; set; }

        protected override async Task OnInitializedAsync()
        {
            WeatherForecast[]? forecasts = await HttpClient.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
            Forecasts = forecasts?.AsQueryable();
        }
    }
}