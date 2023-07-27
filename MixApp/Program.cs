using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Fast.Components.FluentUI;
using MixApp;
using MixApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

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

builder.Services.AddSingleton<GlobalEvent>();

builder.Services.AddFluentToasts();

builder.Services.AddFluentUIComponents(options =>
{
    options.HostingModel = BlazorHostingModel.WebAssembly;
});

await builder.Build().RunAsync();
