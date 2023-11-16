using Append.Blazor.Notifications;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
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

builder.Services.AddSingleton<GlobalEvent>();

builder.Services.AddSingleton(localeManager);

builder.Services.AddSingleton<RemoteAssets>();

builder.Services.AddBlazoredLocalStorageAsSingleton();

builder.Services.AddNotifications();

builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();
