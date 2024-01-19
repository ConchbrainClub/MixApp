using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using MixApp.Web.Services;

namespace MixApp.Web.Pages
{
    public record Feedback
    {
        [Required]
        [JsonPropertyName("contact")]
        public string Contact { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    };
    
    public class HelpBase : ComponentBase
    {
        [Inject]
        HttpClient HttpClient { get; set; } = default!;

        [Inject]
        LocaleManager LM { get; set; } = default!;

        public bool Loading { get; set; } = false;

        public string? ShowWarning { get; set; } = string.Empty;

        public Feedback Feedback { get; set; } = new();

        public async Task SubmitFeedback()
        {
            if (!Validator.TryValidateObject(Feedback, new ValidationContext(Feedback), null))
            {
                ShowWarning = LM.Scripts["p.help.warning"];
                return;
            }

            Loading = true;

            JsonContent content = JsonContent.Create(Feedback);
            HttpResponseMessage response = await HttpClient.PostAsync("/v1/feedback", content);

            if (response.IsSuccessStatusCode)
            {
                Feedback = new();
                ShowWarning = LM.Scripts["p.help.success"];
            }
            else
            {
                ShowWarning = LM.Scripts["p.help.failed"];
            }
            
            Loading = false;
        }
    }
}