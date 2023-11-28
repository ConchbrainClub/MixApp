using System.ComponentModel.DataAnnotations;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace MixApp.Shared.Pages
{
  public class HelpBase : ComponentBase
  {
    [Inject]
    ILocalStorageService LocalStorage { get; set; } = default!;
    [Inject]
    public HttpClient HttpClient { get; set; } = new HttpClient();

    public bool submitLoading { get; set; } = false;

    [Required(ErrorMessage = "请输入联系方式")]
    public string contact { get; set; } = "1790120845@qq.com";

    [Required(ErrorMessage = "请输入联系方式")]
    public string content { get; set; } = "测试提交";

    public void SubmitFeedback()
    {
      if (contact.Length == 0 || content.Length == 0)
      {
        return;
      }
      submitLoading = true;
      var postContent = new MultipartFormDataContent();
      // postContent.Headers.Add("ContentType", $"multipart/form-data");
      postContent.Headers.Add("ContentType", "application/json");
      postContent.Add(new StringContent(contact), "contact");
      postContent.Add(new StringContent(content), "content");
      string result = "";
      try
      {

        HttpResponseMessage response = HttpClient.PostAsync("/identifier", postContent).Result;
        // HttpResponseMessage response = HttpClient.PostAsync("/v1/feedback", postContent).Result;
        result = response.Content.ReadAsStringAsync().Result;
        Console.WriteLine(result);
        submitLoading = false;
      }
      catch (System.Exception)
      {

        submitLoading = false;
        throw;
      }
    }
  }
}