using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.JSInterop;
using MixApp.Shared;
using MixApp.Shared.Services;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

IJSRuntime? jsRuntime = builder.Services.BuildServiceProvider().GetService<IJSRuntime>();

LocaleManager localeManager = await new LocaleManager().Initialize
(
    new HttpClient()
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    },
    await jsRuntime!.InvokeAsync<string>("locale")
);

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => 
{
    return new HttpClient()
    {
        BaseAddress = new Uri(builder.Configuration.GetSection("BaseAddress").Value ?? string.Empty)
    };
});

builder.Services.AddSingleton(new GlobalEvent(jsRuntime!));

builder.Services.AddSingleton(localeManager);

builder.Services.AddBlazoredLocalStorageAsSingleton();

builder.Services.AddFluentUIComponents(options =>
{
    options.HostingModel = BlazorHostingModel.WebAssembly;
});

await builder.Build().RunAsync();
