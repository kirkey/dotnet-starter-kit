﻿namespace FSH.Starter.WebApi.Host;

public static class Extensions
{
    public static WebApplicationBuilder RegisterModules(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        //define module assemblies
        var assemblies = new[]
        {
            typeof(CatalogMetadata).Assembly,
            typeof(TodoModule).Assembly,
            typeof(AccountingMetadata).Assembly,
            typeof(StoreMetadata).Assembly,
            typeof(MessagingModule).Assembly,
            typeof(HumanResourcesMetadata).Assembly,
            typeof(MicroFinanceMetadata).Assembly,
        };

        //register validators
        builder.Services.AddValidatorsFromAssemblies(assemblies);

        //register mediatr
        builder.Services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(assemblies));

        //register module services
        builder.RegisterCatalogServices();
        builder.RegisterTodoServices();
        builder.RegisterAccountingServices();
        builder.RegisterStoreServices();
        builder.RegisterHumanResourcesServices();
        builder.RegisterMessagingServices();
        builder.RegisterMicroFinanceServices();

        //add carter endpoint modules
        builder.Services.AddCarter(configurator: config =>
        {
            config.WithModule<CatalogModule.Endpoints>();
            config.WithModule<HrModule.Endpoints>();
            config.WithModule<TodoModule.Endpoints>();
            config.WithModule<MessagingModule.Endpoints>();
            config.WithModule<MicroFinanceModule.Endpoints>();
        });

        return builder;
    }

    public static WebApplication UseModules(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        //register modules
        app.UseCatalogModule();
        app.UseTodoModule();
        app.UseHumanResourcesModule();
        app.UseAccountingModule();
        app.UseStoreModule();
        app.UseMessagingModule();
        app.UseMicroFinanceModule();

        //register api versions
        var versions = app.NewApiVersionSet()
                    .HasApiVersion(1)
                    .HasApiVersion(2)
                    .ReportApiVersions()
                    .Build();

        //map versioned endpoint
        var endpoints = app.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(versions);

        //map accounting endpoints directly (not using Carter)
        endpoints.MapAccountingEndpoints();

        //map store endpoints directly (not using Carter)
        endpoints.MapStoreEndpoints();

        //use carter for other modules
        endpoints.MapCarter();

        return app;
    }
}
