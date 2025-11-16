# EmployeeContact DI Fix - November 16, 2025

## Issue
Application failing to start with error:
```
Unable to resolve service for type 'FSH.Framework.Core.Persistence.IRepository<EmployeeContact>' 
while attempting to activate EmployeeContact handlers
```

## Root Cause
The EmployeeContact handlers were registered with key `"hr:employeecontacts"` but the DI container only had registrations for key `"hr:contacts"`.

## Handler Expectations
All 5 EmployeeContact handlers use the key `"hr:employeecontacts"`:
- `UpdateEmployeeContactHandler`
- `SearchEmployeeContactsHandler`
- `GetEmployeeContactHandler`
- `DeleteEmployeeContactHandler`
- `CreateEmployeeContactHandler`

Example from UpdateEmployeeContactHandler:
```csharp
public sealed class UpdateEmployeeContactHandler(
    ILogger<UpdateEmployeeContactHandler> logger,
    [FromKeyedServices("hr:employeecontacts")] IRepository<EmployeeContact> repository)
```

## Solution Applied
Added alias registration in `HumanResourcesModule.cs`:

```csharp
// Primary registration (existing)
builder.Services.AddKeyedScoped<IRepository<EmployeeContact>, 
    HumanResourcesRepository<EmployeeContact>>("hr:contacts");
builder.Services.AddKeyedScoped<IReadRepository<EmployeeContact>, 
    HumanResourcesRepository<EmployeeContact>>("hr:contacts");

// Add alias for handlers that use hr:employeecontacts key (NEW)
builder.Services.AddKeyedScoped<IRepository<EmployeeContact>, 
    HumanResourcesRepository<EmployeeContact>>("hr:employeecontacts");
builder.Services.AddKeyedScoped<IReadRepository<EmployeeContact>, 
    HumanResourcesRepository<EmployeeContact>>("hr:employeecontacts");
```

## Verification
✅ No compilation errors  
✅ Build succeeded with 0 errors, 5 warnings (unrelated)  
✅ All 5 EmployeeContact handlers can now resolve dependencies  

## Status
**RESOLVED** - EmployeeContact DI registration mismatch fixed.

## Related Documentation
See `HR_DI_REGISTRATION_FIX_COMPLETE.md` for complete list of all DI fixes applied.

