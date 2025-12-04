# Module Activation - Quick Reference

## How to Enable/Disable Modules

### Basic Usage

Edit `appsettings.json` in your environment:

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

**Change `true` to `false` to disable, `false` to `true` to enable.**

## Common Scenarios

### 1. Development with All Modules

```json
"ModuleOptions": {
  "EnableCatalog": true,
  "EnableTodo": true,
  "EnableAccounting": true,
  "EnableStore": true,
  "EnableHumanResources": true,
  "EnableMessaging": true,
  "EnableMicroFinance": true
}
```

### 2. Minimal Production Setup

```json
"ModuleOptions": {
  "EnableCatalog": true,
  "EnableTodo": false,
  "EnableAccounting": true,
  "EnableStore": true,
  "EnableHumanResources": false,
  "EnableMessaging": false,
  "EnableMicroFinance": false
}
```

### 3. Microfinance-Only Setup

```json
"ModuleOptions": {
  "EnableCatalog": false,
  "EnableTodo": false,
  "EnableAccounting": true,
  "EnableStore": false,
  "EnableHumanResources": false,
  "EnableMessaging": false,
  "EnableMicroFinance": true
}
```

## Configuration Files

| File | Purpose |
|------|---------|
| `appsettings.json` | Default settings (fallback) |
| `appsettings.Development.json` | Development environment |
| `appsettings.Production.json` | Production environment |

**Note**: Environment-specific files override the default settings.

## What Gets Disabled When You Turn Off a Module

When you set `EnableStore: false`, the system automatically:

✓ Skips loading Store assembly  
✓ Doesn't register Store services  
✓ Doesn't configure Store DbContext  
✓ Doesn't register Store repositories  
✓ Doesn't register Store endpoints  
✓ Doesn't initialize Store middleware  

## Startup Logs

You'll see messages like:
```
Module enabled: Catalog
Module enabled: Todo
Module enabled: Accounting
...
```

Missing modules won't appear in the logs.

## Important Notes

- **Restart Required**: Changes to `appsettings.json` require an application restart
- **Database Schema**: Ensure database tables exist for enabled modules
- **No Partial Loading**: Modules are either fully enabled or fully disabled
- **Zero Overhead**: Disabled modules have zero performance impact

## Fields Reference

```csharp
public class ModuleOptions
{
    public bool EnableCatalog { get; set; }           // Product catalog
    public bool EnableTodo { get; set; }              // Task management  
    public bool EnableAccounting { get; set; }        // Financial ledger
    public bool EnableStore { get; set; }             // Warehouse/inventory
    public bool EnableHumanResources { get; set; }    // HR management
    public bool EnableMessaging { get; set; }         // Communications
    public bool EnableMicroFinance { get; set; }      // Microfinance loans
}
```

## Accessing Configuration at Runtime

Any service can access module configuration:

```csharp
public class MyService
{
    public MyService(ModuleOptions options)
    {
        if (options.EnableStore)
        {
            // Store operations
        }
    }
}
```

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Module not loading | Check flag is `true` in appsettings |
| Endpoints missing | Verify module is enabled, restart app |
| Database errors | Enable module or ensure schema exists |
| Services not injected | Check module services are registered |

---

**Location**: `/src/api/server/appsettings.json`  
**Updated**: 2025-12-04  
**Version**: 1.0

