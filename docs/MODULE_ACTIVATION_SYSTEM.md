# Module Activation System Documentation

## Overview

The solution now features a modular architecture where each module can be independently enabled or disabled through configuration settings in `appsettings.json`. This allows for flexible deployment scenarios and easier feature management.

## Configuration

All module activation settings are controlled through the `ModuleOptions` section in `appsettings.json` files.

### appsettings.json (Default)

```json
"ModuleOptions": {
  "EnableCatalog": true,
  "EnableTodo": true,
  "EnableAccounting": true,
  "EnableStore": true,
  "EnableHumanResources": true,
  "EnableMessaging": true,
  "EnableMicroFinance": false
}
```

## Available Modules

| Module | Description | Default |
|--------|-------------|---------|
| **Catalog** | Product catalog and inventory management | Enabled |
| **Todo** | Task management functionality | Enabled |
| **Accounting** | Financial accounting and ledger management | Enabled |
| **Store** | Store operations, warehousing, and inventory | Enabled |
| **HumanResources** | HR management and employee data | Enabled |
| **Messaging** | Communication and messaging features | Enabled |
| **MicroFinance** | Microfinance loan and client management | Disabled |

## How It Works

### 1. Configuration Binding

When the application starts, `Extensions.cs` reads the `ModuleOptions` from `appsettings.json`:

```csharp
var moduleOptions = new ModuleOptions();
builder.Configuration.GetSection(ModuleOptions.SectionName).Bind(moduleOptions);
```

### 2. Dynamic Assembly Registration

Only enabled modules have their assemblies added to the dependency injection container:

```csharp
if (moduleOptions.EnableStore)
{
    assemblyList.Add(typeof(StoreMetadata).Assembly);
    Log.Information("Module enabled: Store");
}
```

### 3. Conditional Service Registration

Each enabled module registers its services and database contexts:

```csharp
if (moduleOptions.EnableStore)
    builder.RegisterStoreServices();
```

### 4. Conditional Endpoint Registration

Carter endpoints are registered only for enabled modules:

```csharp
if (moduleOptions.EnableCatalog)
    config.WithModule<CatalogModule.Endpoints>();
```

### 5. Middleware Configuration

Module-specific middleware is applied only if the module is enabled:

```csharp
if (moduleOptions.EnableStore)
    app.UseStoreModule();
```

## Enabling/Disabling Modules

### To Enable a Module

Edit the appropriate `appsettings.json` file and set the module flag to `true`:

```json
"ModuleOptions": {
  "EnableMicroFinance": true
}
```

### To Disable a Module

Set the module flag to `false`:

```json
"ModuleOptions": {
  "EnableStore": false
}
```

The application will skip:
- Assembly registration
- Dependency injection configuration
- Entity Framework DbContext binding
- Repository registration
- Endpoint registration
- Middleware initialization

## Environment-Specific Configuration

Each environment has its own settings file:

- **Development**: `appsettings.Development.json`
- **Production**: `appsettings.Production.json`
- **Default**: `appsettings.json` (used as fallback)

You can override module settings per environment. For example, you might enable `MicroFinance` only in production:

### appsettings.Development.json
```json
"ModuleOptions": {
  "EnableMicroFinance": false
}
```

### appsettings.Production.json
```json
"ModuleOptions": {
  "EnableMicroFinance": true
}
```

## Logging

When the application starts, you'll see log entries indicating which modules are enabled:

```
Module enabled: Catalog
Module enabled: Todo
Module enabled: Accounting
Module enabled: Store
Module enabled: HumanResources
Module enabled: Messaging
```

If a module is disabled, it will not appear in the logs.

## Adding a New Module

To add a new module to the system:

1. **Create the Module** (Domain, Application, Infrastructure layers)

2. **Create Module Metadata** (e.g., `MicroFinanceMetadata.cs`)
   ```csharp
   namespace FSH.Starter.WebApi.MicroFinance.Application;
   
   public static class MicroFinanceMetadata
   {
       public static string Name { get; set; } = "MicroFinanceApplication";
   }
   ```

3. **Create Module Extension Methods**
   - `RegisterMicroFinanceServices()` - Configure DI
   - `UseMicroFinanceModule()` - Configure middleware

4. **Update ModuleOptions** in `Configuration/ModuleOptions.cs`
   ```csharp
   public bool EnableMicroFinance { get; set; } = false;
   ```

5. **Update Extensions.cs** - Add conditional registration in both `RegisterModules()` and `UseModules()`

6. **Update appsettings files** - Add the module flag to all three appsettings files

7. **Update GlobalUsings.cs** - Add the necessary global usings

## Best Practices

1. **Default Settings**: Set sensible defaults in `appsettings.json`. Most production modules should default to `true`.

2. **Documentation**: Keep module documentation updated with setup instructions.

3. **Database Initialization**: Module-specific database initializers only run if the module is enabled. Ensure database schema exists for enabled modules.

4. **API Versioning**: Disabled modules won't expose their endpoints, improving API clarity.

5. **Testing**: Test both enabled and disabled states for modules to ensure proper isolation.

## Troubleshooting

### Module Not Loading

1. Check that the module flag is set to `true` in your `appsettings.json`
2. Verify the module's metadata class exists and is properly named
3. Check logs for error messages during startup

### Missing Endpoints

If module endpoints are missing:
1. Verify the module is enabled in configuration
2. Check that endpoint classes implement `ICarterModule` or are registered with Carter
3. Restart the application (changes require restart)

### Database Issues

If you see database errors for a disabled module:
1. The module's DbContext may not be properly excluded
2. Check that migrations are not required for disabled modules
3. Verify the module registration properly skips DbContext binding

## Configuration Storage

The current `ModuleOptions` configuration is also stored in the DI container as a singleton:

```csharp
builder.Services.AddSingleton(moduleOptions);
```

This allows any service to access the module configuration at runtime:

```csharp
public class MyService
{
    private readonly ModuleOptions _moduleOptions;
    
    public MyService(ModuleOptions moduleOptions)
    {
        _moduleOptions = moduleOptions;
    }
    
    public void CheckIfModuleEnabled()
    {
        if (_moduleOptions.EnableStore)
        {
            // Store module operations
        }
    }
}
```

