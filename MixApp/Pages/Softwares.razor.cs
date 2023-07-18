using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MixApp.Models;

namespace MixApp.Pages
{
    public partial class SoftwaresBase : ComponentBase
    {
        [Inject]
        HttpClient HttpClient { get; set; } = new HttpClient();

        [Inject]
        IJSRuntime? JSRunTime { get; set; }

        public List<Software> Softwares { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            Softwares = await HttpClient.GetFromJsonAsync<List<Software>>("/softwares") ?? new();
        }
    }
}