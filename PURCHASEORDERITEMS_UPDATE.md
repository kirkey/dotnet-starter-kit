# PurchaseOrderItems Page - List Refresh Implementation

## Overview
Updated the PurchaseOrderItems page to automatically refresh the items list after any CRUD operation and on component initialization.

## Changes Made

### 1. Component Initialization
- **Added `OnInitializedAsync()`**: Ensures items load when component is first created
- **Kept `OnParametersSetAsync()`**: Handles reloading when PurchaseOrderId parameter changes
- Both methods now call `LoadItemsAsync()` to fetch fresh data from the server

### 2. Dependency Injection
Added missing service injections:
```csharp
[Inject] private ISnackbar Toast { get; set; } = default!;
[Inject] private IDialogService DialogService { get; set; } = default!;
```

### 3. CRUD Operations - Auto-Refresh

#### Add Item Dialog
- **Before**: Added item to local list only
- **After**: Reloads entire list from server after successful add
```csharp
if (result is { Canceled: false })
{
    await LoadItemsAsync();
    Toast.Add("Item added successfully", Severity.Success);
}
```

#### Edit Item Dialog
- **Before**: Updated item in local list only
- **After**: Reloads entire list from server after successful edit
```csharp
if (result is { Canceled: false })
{
    await LoadItemsAsync();
    Toast.Add("Item updated successfully", Severity.Success);
}
```

#### Remove Item
- **Before**: Removed from local list after API call
- **After**: Reloads entire list from server after successful deletion
```csharp
await ApiClient.RemovePurchaseOrderItemEndpointAsync("1", PurchaseOrderId, item.GroceryItemId);
await LoadItemsAsync();
Toast.Add("Item removed successfully", Severity.Success);
```

## Benefits

1. **Data Consistency**: Always shows the latest data from the server
2. **Calculated Fields**: Server-calculated fields (TotalPrice, etc.) are always accurate
3. **Concurrent Updates**: Handles changes made by other users or background processes
4. **Simpler Logic**: No need to manually update complex local state
5. **Better UX**: Users see immediate feedback with toast notifications

## API Endpoint Used

```
GET /api/v1/purchase-orders/{id}/items
```

Returns: `List<PurchaseOrderItemResponse>` with all item details including:
- Item ID, PurchaseOrderId, GroceryItemId
- GroceryItemName, GroceryItemSku
- Quantity, UnitPrice, DiscountAmount
- TotalPrice, ReceivedQuantity
- Notes

## Testing Checklist

- [ ] Items load when page opens
- [ ] Items load when PurchaseOrderId parameter changes
- [ ] Adding a new item refreshes the list
- [ ] Editing an item refreshes the list
- [ ] Removing an item refreshes the list
- [ ] Toast notifications appear for all operations
- [ ] Loading indicator shows during data fetch
- [ ] Error messages display if API calls fail

## Next Steps

1. **Regenerate API Client**: Update NSwag/OpenAPI client to include `GetPurchaseOrderItemsEndpointAsync`
2. **Replace HTTP Call**: Replace direct HttpClient call with typed API client method
3. **Add Received Quantity Display**: Update UI to show received quantities from API response
4. **Consider Pagination**: If items list grows large, implement pagination

## Related Files

- `/src/apps/blazor/client/Pages/Store/PurchaseOrderItems.razor` - Updated component
- `/src/api/modules/Store/Store.Application/PurchaseOrders/Items/Get/v1/` - Backend query/handler
- `/src/api/modules/Store/Store.Infrastructure/Endpoints/PurchaseOrders/v1/GetPurchaseOrderItemsEndpoint.cs` - API endpoint

---
**Date**: October 1, 2025
**Status**: ✅ Complete - No compilation errors
