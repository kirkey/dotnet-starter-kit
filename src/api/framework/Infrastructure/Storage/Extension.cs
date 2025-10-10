using Microsoft.Extensions.FileProviders;

namespace FSH.Framework.Infrastructure.Storage;

internal static class Extension
{
    internal static IServiceCollection ConfigureFileStorage(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddOptions<OriginOptions>()
            .BindConfiguration(nameof(OriginOptions))
            .ValidateDataAnnotations();

        services.AddTransient<IStorageService, LocalFileStorageService>();
        services.AddTransient<IDataExport, DataExport>();
        services.AddTransient<IDataImport, DataImport>();
        
        // Register generic import/export services
        services.AddTransient<IDataImportService, DataImportService>();
        services.AddTransient<IDataExportService, DataExportService>();

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
