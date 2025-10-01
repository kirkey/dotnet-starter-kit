# PurchaseOrderResponse Renaming - Summary

## Overview
Renamed `GetPurchaseOrderResponse` to `PurchaseOrderResponse` throughout the codebase for simplicity and consistency.

## Backend Changes (✅ Completed)

### 1. Renamed File
- **Old:** `/src/api/modules/Store/Store.Application/PurchaseOrders/Get/v1/GetPurchaseOrderResponse.cs`
- **New:** `/src/api/modules/Store/Store.Application/PurchaseOrders/Get/v1/PurchaseOrderResponse.cs`

### 2. Updated Type References

#### PurchaseOrderResponse.cs
```csharp
public sealed record PurchaseOrderResponse(
    DefaultIdType Id,
    string OrderNumber,
    // ... rest of properties
);
```

#### GetPurchaseOrderQuery.cs
Changed:
```csharp
public sealed record GetPurchaseOrderQuery(DefaultIdType Id) : IRequest<PurchaseOrderResponse>;
```

#### GetPurchaseOrderHandler.cs
Changed:
```csharp
public sealed class GetPurchaseOrderHandler(...)
    : IRequestHandler<GetPurchaseOrderQuery, PurchaseOrderResponse>
{
    public async Task<PurchaseOrderResponse> Handle(...)
    {
        return po.Adapt<PurchaseOrderResponse>();
    }
}
```

#### PurchaseOrderAdaptExtensions.cs
Changed:
```csharp
/// <summary>
/// Adapts a PurchaseOrder domain entity to PurchaseOrderResponse DTO.
/// </summary>
public static PurchaseOrderResponse Adapt(this PurchaseOrder purchaseOrder)
{
    return new PurchaseOrderResponse(
        // ... mapping
    );
}
```

#### GetPurchaseOrderEndpoint.cs
Changed:
```csharp
.Produces<PurchaseOrderResponse>()
```

#### SearchPurchaseOrdersEndpoint.cs
Changed:
```csharp
.Produces<PagedList<PurchaseOrderResponse>>()
```

## Frontend Changes (⏳ Pending API Client Regeneration)

### 1. PurchaseOrders.razor.cs
Changed all type references:
```csharp
private EntityServerTableContext<PurchaseOrderResponse, DefaultIdType, PurchaseOrderViewModel> Context { get; set; } = default!;
private EntityTable<PurchaseOrderResponse, DefaultIdType, PurchaseOrderViewModel> _table = default!;

Context = new EntityServerTableContext<PurchaseOrderResponse, DefaultIdType, PurchaseOrderViewModel>(
    // ...
    fields:
    [
        new EntityField<PurchaseOrderResponse>(x => x.OrderNumber, "Order Number", "OrderNumber"),
        new EntityField<PurchaseOrderResponse>(x => x.SupplierId, "Supplier ID", "SupplierId"),
        // ... more fields
    ],
    // ...
);

return result.Adapt<PaginationResponse<PurchaseOrderResponse>>();
```

## Build Status

### Backend: ✅ SUCCESS
```
Build succeeded with 376 warning(s) in 10.5s
```
All Store.Application and Store.Infrastructure modules compiled successfully with the renamed type.

### Frontend: ❌ BLOCKED
```
error CS0246: The type or namespace name 'PurchaseOrderResponse' could not be found
```
**Reason:** API client needs to be regenerated to include the renamed type from the backend.

## Next Steps to Complete

### 1. Generate API Client
The frontend uses NSwag to auto-generate the API client from the backend's OpenAPI/Swagger spec.

**Option A - Using Makefile (Recommended):**
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src

# Make sure NSwag CLI is installed
make install-nswag

# Start the API server (in separate terminal)
cd api/server
dotnet run

# Wait for server to start, then generate client
make gen-client
```

**Option B - Manual:**
```bash
# Start API server first
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/api/server
dotnet run

# In another terminal, generate client
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/apps/blazor/infrastructure/Api
nswag run nswag.json /runtime:Net80
```

**Config Location:**
- `/src/apps/blazor/infrastructure/Api/nswag.json`
- **URL:** `https://localhost:7000/swagger/v1/swagger.json`
- **Output:** `/src/apps/blazor/infrastructure/Api/Client.gen.cs`

### 2. Rebuild Frontend
Once the API client is regenerated:
```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/apps/blazor/client
dotnet build
```

### 3. Verify
- ✅ Backend builds successfully
- ✅ API client contains `PurchaseOrderResponse` (not `GetPurchaseOrderResponse`)
- ✅ Frontend builds without errors
- ✅ Purchase Orders page loads correctly
- ✅ All CRUD operations work

## Files Modified

### Backend (7 files)
1. `/src/api/modules/Store/Store.Application/PurchaseOrders/Get/v1/PurchaseOrderResponse.cs` (renamed + content)
2. `/src/api/modules/Store/Store.Application/PurchaseOrders/Get/v1/GetPurchaseOrderQuery.cs`
3. `/src/api/modules/Store/Store.Application/PurchaseOrders/Get/v1/GetPurchaseOrderHandler.cs`
4. `/src/api/modules/Store/Store.Application/PurchaseOrders/PurchaseOrderAdaptExtensions.cs`
5. `/src/api/modules/Store/Store.Infrastructure/Endpoints/PurchaseOrders/v1/GetPurchaseOrderEndpoint.cs`
6. `/src/api/modules/Store/Store.Infrastructure/Endpoints/PurchaseOrders/v1/SearchPurchaseOrdersEndpoint.cs`

### Frontend (1 file)
1. `/src/apps/blazor/client/Pages/Store/PurchaseOrders.razor.cs`

### Generated (Will be regenerated)
1. `/src/apps/blazor/infrastructure/Api/Client.gen.cs` - Auto-generated API client

## API Contract Changes

### Before
```typescript
interface GetPurchaseOrderResponse {
    id: string;
    orderNumber: string;
    // ... properties
}
```

### After
```typescript
interface PurchaseOrderResponse {
    id: string;
    orderNumber: string;
    // ... properties (same, just renamed type)
}
```

**Note:** This is purely a naming change. The structure and properties remain identical.

## Benefits of This Change

1. **Simplicity**: `PurchaseOrderResponse` is shorter and clearer than `GetPurchaseOrderResponse`
2. **Consistency**: Matches common DTO naming patterns (e.g., `SupplierResponse`, `GroceryItemResponse`)
3. **Clarity**: The "Response" suffix already indicates it's returned from the API
4. **Convention**: REST conventions don't typically prefix response DTOs with the HTTP verb

## Testing Checklist

Once API client is regenerated and frontend builds:

- [ ] Navigate to `/store/purchase-orders`
- [ ] View list of purchase orders
- [ ] Search and filter purchase orders
- [ ] View purchase order details
- [ ] Create new purchase order
- [ ] Edit existing purchase order
- [ ] Submit/Approve/Send/Receive/Cancel workflows
- [ ] Verify no console errors
- [ ] Verify API calls use correct response type

## Rollback Plan

If issues arise, reverting is straightforward:

1. Rename file back: `PurchaseOrderResponse.cs` → `GetPurchaseOrderResponse.cs`
2. Change all occurrences back to `GetPurchaseOrderResponse`
3. Rebuild backend
4. Regenerate API client
5. Rebuild frontend

All changes are type-safe and caught at compile time, minimizing risk.
