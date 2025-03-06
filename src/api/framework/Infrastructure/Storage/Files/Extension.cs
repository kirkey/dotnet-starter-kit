using FSH.Framework.Core.Origin;
using FSH.Framework.Core.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace FSH.Framework.Infrastructure.Storage.Files;

internal static class Extension
{
    internal static IServiceCollection ConfigureFileStorage(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions<OriginOptions>()
            .BindConfiguration(nameof(OriginOptions))
            .ValidateDataAnnotations();

        services.AddTransient<IStorageService, LocalFileStorageService>();

        return services;
    }

    internal static IApplicationBuilder UseFileStorage(this IApplicationBuilder app)
    {
        var uri = app.ApplicationServices.GetRequiredService<IOptions<OriginOptions>>().Value;

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "files")),
            RequestPath = new PathString("/files")
        });

        return app;
    }
}
