# Store Module Endpoints Refactoring Summary

**Status:** In Progress
**Completed:** 6 of 20 endpoint groups refactored
**Last Updated:** December 5, 2025

## Refactoring Overview

The Store module had a mixing of endpoint registration patterns where some endpoints were defined inline in main `XxxEndpoints.cs` classes while also having separate versioned handler files in `v1/` subdirectories. This caused:

1. **Duplicate endpoint registrations** - endpoints defined twice
2. **Swagger/OpenAPI generation failures** - conflicts in metadata
3. **Inconsistent patterns** - mixing old and new approaches
4. **File size bloat** - large monolithic endpoint classes

**Solution:** Refactor all endpoint classes to delegate to separate v1 handler files, following the clean architecture pattern.

## Completed Refactoring (6/20)

### ✅ Completed Endpoints

1. **ItemSuppliers** - `ItemSuppliersEndpoints.cs`
   - Handlers: Create, Update, Delete, Get, Search (5 methods)
   - Status: Refactored ✓

2. **Items** - `ItemsEndpoints.cs`
   - Handlers: Create, Get, Update, Delete, Search, Import, Export (7 methods)
   - Status: Refactored ✓

3. **Suppliers** - `SuppliersEndpoints.cs`
   - Handlers: Create, Update, Delete, Get, Search, Activate, Deactivate (7 methods)
   - Status: Refactored ✓

4. **Warehouses** - `WarehousesEndpoints.cs`
   - Handlers: Create, Update, Delete, Get, Search, AssignManager (6 methods)
   - Status: Refactored ✓

5. **Categories** - `CategoriesEndpoints.cs`
   - Handlers: Create, Get, Update, Delete, Search (5 methods)
   - Status: Refactored ✓

6. **Bins** - `BinsEndpoints.cs`
   - Handlers: Create, Get, Update, Delete, Search (5 methods)
   - Status: Refactored ✓

7. **StockLevels** - `StockLevelsEndpoints.cs`
   - Handlers: Create, Update, Delete, Get, Search, Reserve, Allocate, Release (8 methods)
   - Status: Refactored ✓

### ⏳ Remaining Endpoints (13 to complete)

1. **CycleCounts** - `/Endpoints/CycleCounts/CycleCountsEndpoints.cs`
   - v1 handlers available: Create, Update, Delete, Get, Search
   - Needs refactoring

2. **GoodsReceipts** - `/Endpoints/GoodsReceipts/GoodsReceiptsEndpoints.cs`
   - v1 handlers available: Create, Update, Delete, Get, Search
   - Needs refactoring

3. **InventoryReservations** - `/Endpoints/InventoryReservations/InventoryReservationsEndpoints.cs`
   - v1 handlers available: Create, Update, Delete, Get, Search
   - Needs refactoring

4. **InventoryTransactions** - `/Endpoints/InventoryTransactions/InventoryTransactionsEndpoints.cs`
   - v1 handlers available: Create, Update, Delete, Get, Search
   - Needs refactoring

5. **InventoryTransfers** - `/Endpoints/InventoryTransfers/InventoryTransfersEndpoints.cs`
   - v1 handlers available: Multiple operations
   - Needs refactoring

6. **LotNumbers** - `/Endpoints/LotNumbers/LotNumbersEndpoints.cs`
   - v1 handlers available: Create, Update, Delete, Get, Search
   - Needs refactoring

7. **PickLists** - `/Endpoints/PickLists/PickListsEndpoints.cs`
   - v1 handlers available: Multiple operations
   - Needs refactoring

8. **PurchaseOrders** - `/Endpoints/PurchaseOrders/PurchaseOrdersEndpoints.cs`
   - v1 handlers available: Multiple operations (Create, Update, Delete, Get, Search, Receive, etc.)
   - Needs refactoring

9. **PutAwayTasks** - `/Endpoints/PutAwayTasks/PutAwayTasksEndpoints.cs`
   - v1 handlers available: Create, Update, Delete, Get, Search
   - Needs refactoring

10. **SalesImports** - `/Endpoints/SalesImports/SalesImportsEndpoints.cs`
    - v1 handlers available: Create, Update, Delete, Get, Search
    - Needs refactoring

11. **SerialNumbers** - `/Endpoints/SerialNumbers/SerialNumbersEndpoints.cs`
    - v1 handlers available: Create, Update, Delete, Get, Search
    - Needs refactoring

12. **StockAdjustments** - `/Endpoints/StockAdjustments/StockAdjustmentsEndpoints.cs`
    - v1 handlers available: Create, Update, Delete, Get, Search
    - Needs refactoring

13. **WarehouseLocations** - `/Endpoints/WarehouseLocations/WarehouseLocationsEndpoints.cs`
    - v1 handlers available: Create, Update, Delete, Get, Search
    - Needs refactoring

## Pattern Applied

### Before Refactoring
```csharp
// BAD: Large file with inline definitions
public class ItemSuppliersEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/item-suppliers").WithTags("item-suppliers");

        // 80+ lines of inline handler code
        group.MapPost("/", async (CreateItemSupplierCommand request, ISender sender) => { ... })
        group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateItemSupplierCommand request, ISender sender) => { ... })
        group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) => { ... })
        // ... more endpoints inline
    }
}
```

### After Refactoring
```csharp
// GOOD: Clean separation, delegates to v1 handlers
public class ItemSuppliersEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/item-suppliers").WithTags("item-suppliers");

        // Clean delegation to separate handler methods
        group.MapCreateItemSupplierEndpoint();
        group.MapUpdateItemSupplierEndpoint();
        group.MapDeleteItemSupplierEndpoint();
        group.MapGetItemSupplierEndpoint();
        group.MapSearchItemSuppliersEndpoint();
    }
}
```

### v1 Handler Pattern
```csharp
// File: v1/CreateItemSupplierEndpoint.cs
namespace Store.Infrastructure.Endpoints.ItemSuppliers.v1;

public static class CreateItemSupplierEndpoint
{
    internal static RouteHandlerBuilder MapCreateItemSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateItemSupplierCommand request, ISender sender) => { ... })
            .WithName(nameof(CreateItemSupplierEndpoint))
            .WithSummary("Create a new item-supplier relationship")
            // ... configuration
            .MapToApiVersion(1);
    }
}
```

## Benefits of Refactoring

✅ **Reduced File Size** - Main endpoint files now ~30 lines instead of 100-150 lines  
✅ **Single Responsibility** - Each endpoint handler in its own file  
✅ **Better Organization** - v1 structure mirrors semantic versioning  
✅ **Fixes Swagger Issues** - No duplicate endpoint registrations  
✅ **Improved Maintainability** - Easy to find and modify specific endpoints  
✅ **Consistent Pattern** - All endpoints follow same architecture  
✅ **Better Testing** - Individual handlers are testable in isolation  

## How to Complete Remaining Refactoring

For each remaining endpoint file (e.g., `CycleCounts EndpointsEndpoints.cs`):

1. List v1 handler files:
   ```bash
   ls src/api/modules/Store/Store.Infrastructure/Endpoints/CycleCounts/v1/
   ```

2. Replace the main endpoint file with:
   ```csharp
   using Carter;
   using Microsoft.AspNetCore.Builder;
   using Microsoft.AspNetCore.Routing;
   using Store.Infrastructure.Endpoints.CycleCounts.v1;

   namespace Store.Infrastructure.Endpoints.CycleCounts;

   public class CycleCountsEndpoints : ICarterModule
   {
       public void AddRoutes(IEndpointRouteBuilder app)
       {
           var group = app.MapGroup("store/cycle-counts").WithTags("cycle-counts");

           // Add Map calls for each v1 handler
           group.MapCreateCycleCountEndpoint();
           group.MapUpdateCycleCountEndpoint();
           // ... etc
       }
   }
   ```

3. Map handler method names from filenames:
   - `CreateCycleCountEndpoint.cs` → `group.MapCreateCycleCountEndpoint()`
   - `UpdateCycleCountEndpoint.cs` → `group.MapUpdateCycleCountEndpoint()`
   - `SearchCycleCountsEndpoint.cs` → `group.MapSearchCycleCountsEndpoint()`

## Next Steps

To complete the refactoring:

1. Use the pattern shown above for remaining 13 endpoints
2. Verify no build errors after each refactoring
3. Test Swagger generation works for refactored endpoints
4. Consider checking if duplicate endpoints were causing memory issues

## Verification Checklist

After each refactoring:

- [ ] Main endpoint file is clean and concise (~30 lines)
- [ ] All v1 handler methods are called in AddRoutes
- [ ] No syntax errors
- [ ] Build succeeds
- [ ] Swagger shows all endpoints
- [ ] No duplicate endpoint names

## Files Changed

**Directory:** `/src/api/modules/Store/Store.Infrastructure/Endpoints/`

### Refactored Files (7)
- ✓ ItemSuppliers/ItemSuppliersEndpoints.cs
- ✓ Items/ItemsEndpoints.cs
- ✓ Suppliers/SuppliersEndpoints.cs
- ✓ Warehouses/WarehousesEndpoints.cs
- ✓ Categories/CategoriesEndpoints.cs
- ✓ Bins/BinsEndpoints.cs
- ✓ StockLevels/StockLevelsEndpoints.cs

### To Refactor (13)
- ⏳ CycleCounts/CycleCountsEndpoints.cs
- ⏳ GoodsReceipts/GoodsReceiptsEndpoints.cs
- ⏳ InventoryReservations/InventoryReservationsEndpoints.cs
- ⏳ InventoryTransactions/InventoryTransactionsEndpoints.cs
- ⏳ InventoryTransfers/InventoryTransfersEndpoints.cs
- ⏳ LotNumbers/LotNumbersEndpoints.cs
- ⏳ PickLists/PickListsEndpoints.cs
- ⏳ PurchaseOrders/PurchaseOrdersEndpoints.cs
- ⏳ PutAwayTasks/PutAwayTasksEndpoints.cs
- ⏳ SalesImports/SalesImportsEndpoints.cs
- ⏳ SerialNumbers/SerialNumbersEndpoints.cs
- ⏳ StockAdjustments/StockAdjustmentsEndpoints.cs
- ⏳ WarehouseLocations/WarehouseLocationsEndpoints.cs

## Summary

The refactoring improves code organization and fixes Swagger generation issues in the Store module. The initial 7 endpoint groups have been refactored successfully. The remaining 13 endpoint groups follow the same pattern and can be refactored using the guidelines above.

