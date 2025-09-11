using Accounting.Application;
using Accounting.Infrastructure;
using Asp.Versioning.Conventions;
using Carter;
using FluentValidation;
using FSH.Starter.WebApi.App;
using FSH.Starter.WebApi.Catalog.Application;
using FSH.Starter.WebApi.Catalog.Infrastructure;
using FSH.Starter.WebApi.Todo;
using FSH.Starter.WebApi.Warehouse;

namespace FSH.Starter.WebApi.Host;

public static class Extensions
{
    public static WebApplicationBuilder RegisterModules(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        //define module assemblies
        var assemblies = new[]
        {
            typeof(AppModule).Assembly,
            typeof(CatalogMetadata).Assembly,
            typeof(TodoModule).Assembly,
            typeof(AccountingMetadata).Assembly,
            typeof(WarehouseModule).Assembly,
        };

        //register validators
        builder.Services.AddValidatorsFromAssemblies(assemblies);

        //register mediatr
        builder.Services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(assemblies));

        //register module services
        builder.RegisterAppServices();
        builder.RegisterCatalogServices();
        builder.RegisterTodoServices();
        builder.RegisterAccountingServices();
        builder.RegisterWarehouseServices();

        //add carter endpoint modules
        builder.Services.AddCarter(configurator: config =>
        {
            config.WithModule<AppModule.Endpoints>();
            config.WithModule<CatalogModule.Endpoints>();
            config.WithModule<TodoModule.Endpoints>();
            config.WithModule<AccountingModule.Endpoints>();
            config.WithModule<WarehouseModule.Endpoints>();
        });

        return builder;
    }

    public static WebApplication UseModules(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        //register modules
        app.UseAppModule();
        app.UseCatalogModule();
        app.UseTodoModule();
        app.UseAccountingModule();
        app.UseWarehouseModule();

        //register api versions
        var versions = app.NewApiVersionSet()
                    .HasApiVersion(1)
                    .HasApiVersion(2)
                    .ReportApiVersions()
                    .Build();

        //map versioned endpoint
        var endpoints = app.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(versions);

        //use carter
        endpoints.MapCarter();

        return app;
    }
}
