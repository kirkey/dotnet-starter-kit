# Module Configuration Guide

This guide explains how to enable/disable modules in the FSH Starter Kit, using the Store and MicroFinance modules as examples.

## Quick Start

### Enable a Module

Edit `/src/api/server/appsettings.json`:

```json
{
  "ModuleOptions": {
    "EnableStore": true,
    "EnableMicroFinance": true
  }
}
```

Set to `true` to enable, `false` to disable.

### Environment-Specific Configuration

Use environment-specific appsettings files:

**appsettings.Development.json:**
```json
{
  "ModuleOptions": {
    "EnableStore": true,
    "EnableMicroFinance": true,
    "EnableAccounting": false
  }
}
```

**appsettings.Production.json:**
```json
{
  "ModuleOptions": {
    "EnableStore": true,
    "EnableMicroFinance": false,
    "EnableAccounting": true
  }
}
```

## How Module Registration Works

### 1. Module Configuration Class
**File:** `/src/api/server/Configuration/ModuleOptions.cs`

```csharp
public class ModuleOptions
{
    public const string SectionName = "ModuleOptions";
    
    public bool EnableStore { get; set; } = true;
    public bool EnableMicroFinance { get; set; } = false;
    // ... other modules
}
```

### 2. Builder Registration
**File:** `/src/api/server/Extensions.cs` → `RegisterModules()`

```csharp
public static WebApplicationBuilder RegisterModules(this WebApplicationBuilder builder)
{
    // Read configuration
    var moduleOptions = new ModuleOptions();
    builder.Configuration.GetSection(ModuleOptions.SectionName).Bind(moduleOptions);

    // Conditionally add assemblies
    if (moduleOptions.EnableStore)
        assemblyList.Add(typeof(StoreMetadata).Assembly);
    
    if (moduleOptions.EnableMicroFinance)
        assemblyList.Add(typeof(MicroFinanceMetadata).Assembly);
    
    // Register validators from enabled assemblies
    builder.Services.AddValidatorsFromAssemblies(assemblyList.ToArray());
    
    // Register MediatR handlers from enabled assemblies
    builder.Services.AddMediatR(configuration =>
        configuration.RegisterServicesFromAssemblies(assemblyList.ToArray()));
    
    // Register module services
    if (moduleOptions.EnableStore)
        builder.RegisterStoreServices();
    
    if (moduleOptions.EnableMicroFinance)
        builder.RegisterMicroFinanceServices();
    
    return builder;
}
```

### 3. Application Middleware
**File:** `/src/api/server/Extensions.cs` → `UseModules()`

```csharp
public static WebApplication UseModules(this WebApplication app)
{
    var moduleOptions = app.Services.GetRequiredService<ModuleOptions>();

    if (moduleOptions.EnableStore)
        app.UseStoreModule();
    
    if (moduleOptions.EnableMicroFinance)
        app.UseMicroFinanceModule();
    
    // Map all endpoints
    endpoints.MapCarter();
    
    return app;
}
```

## Module Services

Each module provides two extension methods:

### Register Services
```csharp
builder.RegisterStoreServices()
builder.RegisterMicroFinanceServices()
```

**What it does:**
- Binds DbContext
- Registers repositories
- Adds database initializer
- Registers services

### Use Module
```csharp
app.UseStoreModule()
app.UseMicroFinanceModule()
```

**What it does:**
- Applies module-specific middleware
- Initializes module resources

## Available Modules

| Module | Enabled By Default | File |
|--------|-------------------|------|
| Catalog | ✅ true | `src/api/modules/Catalog/` |
| Todo | ✅ true | `src/api/modules/Todo/` |
| Accounting | ✅ true | `src/api/modules/Accounting/` |
| Store | ✅ true | `src/api/modules/Store/` |
| HumanResources | ✅ true | `src/api/modules/HumanResources/` |
| Messaging | ✅ true | `src/api/modules/Messaging/` |
| MicroFinance | ❌ false | `src/api/modules/MicroFinance/` |

## Module Structure

Each module follows this standard structure:

```
ModuleName/
├── ModuleName.Domain/
│   ├── Entities/
│   ├── Events/
│   ├── Exceptions/
│   └── ModuleName.Domain.csproj
├── ModuleName.Application/
│   ├── Features/
│   │   ├── Entity1/
│   │   │   ├── Create/v1/
│   │   │   ├── Update/v1/
│   │   │   ├── Delete/v1/
│   │   │   ├── Get/v1/
│   │   │   └── Search/v1/
│   ├── ModuleMetadata.cs
│   └── ModuleName.Application.csproj
└── ModuleName.Infrastructure/
    ├── Endpoints/
    │   └── Entity1Endpoints.cs
    ├── Persistence/
    │   ├── ModuleDbContext.cs
    │   ├── ModuleDbInitializer.cs
    │   └── ModuleRepository.cs
    ├── ModuleModule.cs
    ├── GlobalUsings.cs
    └── ModuleName.Infrastructure.csproj
```

## Example: Enable/Disable Store Module

### Default (Enabled)
```json
"ModuleOptions": {
  "EnableStore": true
}
```

**Result:**
- ✅ `/api/v1/store/*` endpoints available
- ✅ Swagger shows all Store endpoints
- ✅ Database tables created in `store_*` schema
- ✅ Store services injected

### Disabled
```json
"ModuleOptions": {
  "EnableStore": false
}
```

**Result:**
- ❌ `/api/v1/store/*` endpoints not registered
- ❌ Swagger doesn't show Store endpoints
- ❌ No database tables created
- ❌ Store services not registered
- ✅ API still starts without error

## Logging Module Status

When the application starts, you'll see logs indicating which modules are enabled:

```
[INF] Module enabled: Catalog
[INF] Module enabled: Todo
[INF] Module enabled: Accounting
[INF] Module enabled: Store
[INF] Module enabled: HumanResources
[INF] Module enabled: Messaging
[INF] Module enabled: MicroFinance
```

## Adding a New Module

To add a new module:

1. **Create module structure** following the pattern above

2. **Create ModuleMetadata class**
```csharp
namespace YourCompany.Module.Application;

public static class YourModuleMetadata
{
    public static string Name { get; set; } = "YourModuleApplication";
}
```

3. **Create ModuleModule class**
```csharp
public static class YourModule
{
    public static WebApplicationBuilder RegisterYourModuleServices(this WebApplicationBuilder builder)
    {
        // Register services
        return builder;
    }

    public static WebApplication UseYourModule(this WebApplication app)
    {
        // Apply middleware
        return app;
    }
}
```

4. **Add to ModuleOptions**
```csharp
public class ModuleOptions
{
    public bool EnableYourModule { get; set; } = true;
}
```

5. **Register in Extensions.cs**
```csharp
if (moduleOptions.EnableYourModule)
{
    assemblyList.Add(typeof(YourModuleMetadata).Assembly);
    // ... also add to the service registration section
}
```

## Performance Considerations

**Benefits of Module Toggle:**
- ✅ Faster startup when modules disabled
- ✅ Lower memory footprint
- ✅ Cleaner API when unused modules hidden
- ✅ Easier to test specific modules
- ✅ Supports multi-tenancy (different features per tenant)

**Trade-offs:**
- ⚠️ Configuration management adds complexity
- ⚠️ No runtime toggling (requires app restart)
- ⚠️ Database schema still created for all enabled modules

## Verification

### Check if Module is Enabled

```csharp
// In any service with IServiceProvider
var moduleOptions = serviceProvider.GetRequiredService<ModuleOptions>();
if (moduleOptions.EnableStore)
{
    // Store module is enabled
}
```

### Test via Swagger

1. Start the API: `dotnet run`
2. Open https://localhost:7000/swagger/
3. Verify endpoints for enabled modules are listed
4. Verify endpoints for disabled modules are NOT listed

### Check Logs

Run the application and look for:
```
[INF] Module enabled: MicroFinance
[INF] RegisterModules took XXXms
```

## Troubleshooting

### Module Endpoints Not Appearing in Swagger

**Check:**
1. Module enabled in appsettings.json
2. Namespace imports added to Extensions.cs
3. Endpoints implement `ICarterModule`
4. No build errors

**Solution:**
```json
{
  "ModuleOptions": {
    "EnableYourModule": true  // Must be true
  }
}
```

### "Type or namespace not found" Error

**Problem:** Trying to register a disabled module

**Solution:** Ensure the module is enabled in appsettings.json before referencing it

### Duplicate Endpoint Errors

**Problem:** Endpoint registered multiple times

**Solution:** Ensure endpoint handler is only called once in the module

## Summary

The module system provides:
- ✅ Clean separation of concerns
- ✅ Easy enable/disable via configuration
- ✅ Consistent architecture across all modules
- ✅ Scalable multi-module support
- ✅ Dynamic assembly loading
- ✅ Clean startup logs

By following this pattern, you can easily add new modules, disable unused features, and maintain a clean, manageable codebase.

