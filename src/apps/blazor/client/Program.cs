using FSH.Starter.Blazor.Client.Services;
using FSH.Starter.Blazor.Client.Services.Navigation;
using FSH.Starter.Blazor.Infrastructure;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddClientServices(builder.Configuration);

// Read ApiBaseUrl from wwwroot/appsettings.json and register as a Uri singleton
var apiBase = new Uri(builder.Configuration["ApiBaseUrl"]!);
builder.Services.AddSingleton(apiBase);

// Register ImageUrlService so components can inject it
builder.Services.AddScoped<ImageUrlService>();

// Register ApiHelper for guarded API call execution
builder.Services.AddScoped<ApiHelper>();

// Register dynamic navigation menu service
builder.Services.AddSingleton<IMenuService, MenuService>();

await builder.Build().RunAsync().ConfigureAwait(false);
