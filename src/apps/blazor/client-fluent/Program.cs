using FSH.Starter.Blazor.FluentClient;
using FSH.Starter.Blazor.Infrastructure;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add Fluent UI services
builder.Services.AddFluentUIComponents();

builder.Services.AddClientServices(builder.Configuration);

await builder.Build().RunAsync().ConfigureAwait(false);
