using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;
using MixApp;
using MixApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

IJSRuntime? jsRuntime = builder.Services.BuildServiceProvider().GetService<IJSRuntime>();
string locale = await jsRuntime!.InvokeAsync<string>("locale") ?? "en-US";

LocaleManager localeManager = new(builder.HostEnvironment.BaseAddress);
await localeManager.Initialize(locale);

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => 
{
    string? baseAddress = builder.Configuration.GetSection("BaseAddress").Value;
    
    return new HttpClient 
    {
        BaseAddress = new Uri(baseAddress ?? string.Empty) 
    };
});

builder.Services.AddSingleton(new GlobalEvent(jsRuntime!));

builder.Services.AddSingleton(localeManager);

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddFluentToasts();

builder.Services.AddFluentUIComponents(options =>
{
    options.HostingModel = BlazorHostingModel.WebAssembly;
});

await builder.Build().RunAsync();
