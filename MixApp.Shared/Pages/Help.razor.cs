using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;

namespace MixApp.Shared.Pages
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
        public HttpClient HttpClient { get; set; } = new HttpClient();

        public bool SubmitLoading { get; set; } = false;

        public Feedback Feedback { get; set; } = new();

        public void SubmitFeedback()
        {
            if (!Validator.TryValidateObject(Feedback, new ValidationContext(Feedback), null)) return;

            SubmitLoading = true;
            StringContent content = new(JsonSerializer.Serialize(Feedback));
            _ = HttpClient.PostAsync("/v1/feedback", content);
            SubmitLoading = false;
        }
    }
}