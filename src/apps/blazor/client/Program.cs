using FSH.Starter.Blazor.Client.Services.Navigation;
using FSH.Starter.Blazor.Infrastructure;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddClientServices(builder.Configuration);

// Register dynamic navigation menu service
builder.Services.AddSingleton<IMenuService, MenuService>();

await builder.Build().RunAsync().ConfigureAwait(false);
