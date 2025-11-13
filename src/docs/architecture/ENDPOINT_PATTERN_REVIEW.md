# Endpoint Pattern Review - Auto-Reorder Feature

## ✅ Changes Applied

### Fixed Endpoint Pattern Compliance

Both new endpoints have been updated to follow the existing Store module endpoint patterns:

### Before (Incorrect Pattern):
```csharp
public class GetItemsNeedingReorderEndpoint : IEndpoint<IResult, GetItemsNeedingReorderRequest, ISender>
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/suppliers/{supplierId}/items-needing-reorder", ...)
            .RequirePermission(FshResources.Store, FshActions.View)
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}

internal static class GetItemsNeedingReorderEndpointExtensions
{
    internal static IEndpointRouteBuilder MapGetItemsNeedingReorderEndpoint(this IEndpointRouteBuilder app)
    {
        new GetItemsNeedingReorderEndpoint().MapEndpoint(app);
        return app;
    }
}
```

### After (Correct Pattern):
```csharp
public static class GetItemsNeedingReorderEndpoint
{
    internal static RouteHandlerBuilder MapGetItemsNeedingReorderEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/suppliers/{supplierId:guid}/items-needing-reorder", 
            async (DefaultIdType supplierId, [FromBody] GetItemsNeedingReorderRequest request, ISender sender) =>
            {
                var query = request with { SupplierId = supplierId };
                var result = await sender.Send(query).ConfigureAwait(false);
                return Results.Ok(result);
            })
            .WithName(nameof(GetItemsNeedingReorderEndpoint))
            .WithSummary("Get items needing reorder for a supplier")
            .WithDescription("...")
            .Produces<List<ItemNeedingReorderResponse>>()
            .RequirePermission("Permissions.Store.View")
            .MapToApiVersion(1);
    }
}
```

## 🔧 Key Pattern Requirements

### 1. Class Declaration
- ✅ Must be `static class`
- ❌ Not instance class implementing `IEndpoint<>`

### 2. Extension Method
- ✅ Returns `RouteHandlerBuilder`
- ✅ Direct extension method on `IEndpointRouteBuilder`
- ❌ No separate extension class needed

### 3. Route Configuration
- ✅ Use `:guid` constraint for ID parameters
- ✅ Use `DefaultIdType` for parameter types
- ✅ Use `[FromBody]` attribute for request objects
- ✅ Use `.ConfigureAwait(false)` for async calls

### 4. Permission Configuration
- ✅ Use string: `"Permissions.Store.View"` or `"Permissions.Store.Update"`
- ❌ Not: `FshResources.Store, FshActions.View`

### 5. API Versioning
- ✅ Use `.MapToApiVersion(1)`
- ❌ Not: `.MapToApiVersion(new ApiVersion(1, 0))`

### 6. Documentation
- ✅ XML documentation on class and method
- ✅ Clear summary and description
- ✅ Produces<T> for response type

## 📋 Checklist Applied

### GetItemsNeedingReorderEndpoint
- [x] Changed to static class
- [x] Removed IEndpoint interface
- [x] Returns RouteHandlerBuilder
- [x] Added :guid constraint to supplierId route parameter
- [x] Used DefaultIdType for parameter
- [x] Added [FromBody] attribute
- [x] Used .ConfigureAwait(false)
- [x] Fixed permission to string format
- [x] Fixed API version to .MapToApiVersion(1)
- [x] Enhanced XML documentation

### AutoAddItemsToPurchaseOrderEndpoint
- [x] Changed to static class
- [x] Removed IEndpoint interface
- [x] Returns RouteHandlerBuilder
- [x] Added :guid constraint to id route parameter
- [x] Used DefaultIdType for parameter
- [x] Added [FromBody] attribute
- [x] Used .ConfigureAwait(false)
- [x] Fixed permission to string format
- [x] Fixed API version to .MapToApiVersion(1)
- [x] Enhanced XML documentation
- [x] Mentioned "Draft status" requirement in description

## ✅ Verification

- [x] No compilation errors
- [x] Follows exact pattern of existing endpoints like:
  - `GetPurchaseOrderEndpoint`
  - `SearchPurchaseOrdersEndpoint`
  - `SubmitPurchaseOrderEndpoint`
- [x] Namespaces correct (FSH.Starter.WebApi.Store.Application...)
- [x] Registered in PurchaseOrdersEndpoints.cs
- [x] Ready for NSwag client generation

## 🎯 Benefits of This Pattern

1. **Consistency**: Matches all other Store endpoints
2. **Simplicity**: Static extension method is cleaner than instance with interface
3. **Type Safety**: RouteHandlerBuilder return type provides better tooling
4. **Maintainability**: Follows established conventions
5. **Documentation**: Proper XML docs for OpenAPI/Swagger
6. **Versioning**: Consistent API versioning approach

## 📝 Next Steps

1. ✅ Endpoints are fixed and follow pattern
2. ⏭️ Generate NSwag client to include new endpoints
3. ⏭️ Implement Blazor UI components
4. ⏭️ Test end-to-end flow

---

**Status**: ✅ Complete - Endpoints now follow Store module patterns
**Date**: November 10, 2025

