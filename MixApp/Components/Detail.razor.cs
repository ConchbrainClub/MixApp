using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using MixApp.Models;

namespace MixApp.Components
{
    public partial class DetailBase : ComponentBase
    {
        [Inject]
        HttpClient HttpClient { get; set; } = new HttpClient();

        public FluentDialog? Dialog;

        [Parameter]
        public Software? Software { get; set; }

        public List<Manifest> Manifests { get; set; } = new();

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender) Hide();
        }

        protected async override void OnParametersSet()
        {
            if (Software == null) return;

            Dialog?.Show();

            Manifests = await HttpClient
                .GetFromJsonAsync<List<Manifest>>($"/softwares/{Software?.PackageIdentifier}") 
                ?? new();   
        }

        public void OnDismiss(DialogEventArgs args)
        {
            if (args?.Reason == "dismiss")
            {
                Hide();
            }
        }

        public void Hide()
        {
            Manifests = new();
            Dialog?.Hide();
        }
    }
}