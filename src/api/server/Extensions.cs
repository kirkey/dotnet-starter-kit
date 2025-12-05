﻿using System.Reflection;

namespace FSH.Starter.WebApi.Host;

public static class Extensions
{
    public static WebApplicationBuilder RegisterModules(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Get module options from configuration
        var moduleOptions = new ModuleOptions();
        builder.Configuration.GetSection(ModuleOptions.SectionName).Bind(moduleOptions);

        // Build list of enabled module assemblies
        var assemblyList = new List<Assembly>();

        if (moduleOptions.EnableCatalog)
        {
            assemblyList.Add(typeof(CatalogMetadata).Assembly);
            Log.Information("Module enabled: Catalog");
        }

        if (moduleOptions.EnableTodo)
        {
            assemblyList.Add(typeof(TodoModule).Assembly);
            Log.Information("Module enabled: Todo");
        }

        if (moduleOptions.EnableAccounting)
        {
            assemblyList.Add(typeof(AccountingMetadata).Assembly);
            Log.Information("Module enabled: Accounting");
        }

        if (moduleOptions.EnableStore)
        {
            assemblyList.Add(typeof(StoreMetadata).Assembly);
            Log.Information("Module enabled: Store");
        }

        if (moduleOptions.EnableHumanResources)
        {
            assemblyList.Add(typeof(HumanResourcesMetadata).Assembly);
            Log.Information("Module enabled: HumanResources");
        }

        if (moduleOptions.EnableMessaging)
        {
            assemblyList.Add(typeof(MessagingModule).Assembly);
            Log.Information("Module enabled: Messaging");
        }

        if (moduleOptions.EnableMicroFinance)
        {
            assemblyList.Add(typeof(MicroFinanceMetadata).Assembly);
            Log.Information("Module enabled: MicroFinance");
        }

        var assemblies = assemblyList.ToArray();

        //register validators
        builder.Services.AddValidatorsFromAssemblies(assemblies);

        //register mediatr
        builder.Services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblies(assemblies));

        //register module services
        if (moduleOptions.EnableCatalog)
            builder.RegisterCatalogServices();
        
        if (moduleOptions.EnableTodo)
            builder.RegisterTodoServices();
        
        if (moduleOptions.EnableAccounting)
            builder.RegisterAccountingServices();
        
        if (moduleOptions.EnableStore)
            builder.RegisterStoreServices();
        
        if (moduleOptions.EnableHumanResources)
            builder.RegisterHumanResourcesServices();
        
        if (moduleOptions.EnableMessaging)
            builder.RegisterMessagingServices();
        
        if (moduleOptions.EnableMicroFinance)
            builder.RegisterMicroFinanceServices();

        //add carter endpoint modules
        builder.Services.AddCarter(configurator: config =>
        {
            if (moduleOptions.EnableCatalog)
                config.WithModule<CatalogModule.Endpoints>();
            
            if (moduleOptions.EnableTodo)
                config.WithModule<TodoModule.Endpoints>();
            
            // Explicitly register all ICarterModule implementations for proper Swagger/OpenAPI discovery
            // This ensures Swashbuckle can discover all endpoints including those from Store, Accounting, HR, etc.
            var iCarterModuleType = typeof(Carter.ICarterModule);
            var assembliesToScan = new List<Assembly>();
            
            if (moduleOptions.EnableStore)
                assembliesToScan.Add(typeof(StoreModule).Assembly);
            
            if (moduleOptions.EnableAccounting)
                assembliesToScan.Add(typeof(AccountingModule).Assembly);
            
            if (moduleOptions.EnableHumanResources)
                assembliesToScan.Add(typeof(HrModule).Assembly);  // Use HrModule from Infrastructure (where endpoints are) instead of HumanResourcesMetadata from Application
            
            if (moduleOptions.EnableMessaging)
                assembliesToScan.Add(typeof(MessagingModule).Assembly);
            
            if (moduleOptions.EnableMicroFinance)
                assembliesToScan.Add(typeof(MicroFinanceModule).Assembly);
            
            foreach (var assembly in assembliesToScan)
            {
                if (assembly == null) continue;
                
                var icarterModuleTypes = assembly.GetTypes()
                    .Where(t => iCarterModuleType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                    .ToList();
                
                foreach (var moduleType in icarterModuleTypes)
                {
                    // Use reflection to call config.WithModule<T>() for each ICarterModule type
                    var withModuleMethod = typeof(Carter.CarterConfigurator)
                        .GetMethods()
                        .First(m => m.Name == "WithModule" && m.IsGenericMethodDefinition);
                    
                    var genericMethod = withModuleMethod.MakeGenericMethod(moduleType);
                    genericMethod.Invoke(config, null);
                }
            }
        });

        // Store module options in DI for potential runtime access
        builder.Services.AddSingleton(moduleOptions);

        return builder;
    }

    public static WebApplication UseModules(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // Get module options from DI
        var moduleOptions = app.Services.GetRequiredService<ModuleOptions>();

        //register modules
        if (moduleOptions.EnableCatalog)
            app.UseCatalogModule();
        
        if (moduleOptions.EnableTodo)
            app.UseTodoModule();
        
        if (moduleOptions.EnableHumanResources)
            app.UseHumanResourcesModule();
        
        if (moduleOptions.EnableAccounting)
            app.UseAccountingModule();
        
        if (moduleOptions.EnableStore)
            app.UseStoreModule();
        
        if (moduleOptions.EnableMessaging)
            app.UseMessagingModule();
        
        if (moduleOptions.EnableMicroFinance)
            app.UseMicroFinanceModule();

        //register api versions
        var versions = app.NewApiVersionSet()
                    .HasApiVersion(1)
                    .HasApiVersion(2)
                    .ReportApiVersions()
                    .Build();

        //map versioned endpoint
        var endpoints = app.MapGroup("api/v{version:apiVersion}").WithApiVersionSet(versions);

        //use carter for all module endpoints (Store, Accounting, HR, etc. are auto-discovered via ICarterModule)
        endpoints.MapCarter();

        // IMPORTANT: UseOpenApi must be called AFTER MapCarter() to ensure Swashbuckle discovers all ICarterModule endpoints
        // When UseOpenApi is called before MapCarter, the Swagger/OpenAPI generation misses the dynamically registered endpoints
        // app.UseOpenApi();

        return app;
    }
}
