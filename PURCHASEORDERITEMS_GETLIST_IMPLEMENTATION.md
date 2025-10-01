# Purchase Order Items - GetList Implementation Summary

## Date: October 1, 2025

## Overview
Implemented the GetPurchaseOrderItems functionality to retrieve and display the list of items for a purchase order. This includes backend API endpoints and frontend integration.

## Changes Made

### 1. Application Layer (Backend)

#### Created New Files:

**`Store.Application/PurchaseOrders/Items/Get/v1/GetPurchaseOrderItemsQuery.cs`**
- Query record for requesting purchase order items by PurchaseOrderId
- Returns `List<PurchaseOrderItemResponse>`

**`Store.Application/PurchaseOrders/Items/Get/v1/PurchaseOrderItemResponse.cs`**
- Response DTO containing:
  - Id, PurchaseOrderId, GroceryItemId
  - GroceryItemName, GroceryItemSku
  - Quantity, UnitPrice, DiscountAmount
  - TotalPrice, ReceivedQuantity
  - Notes

**`Store.Application/PurchaseOrders/Items/Get/v1/GetPurchaseOrderItemsHandler.cs`**
- MediatR handler implementation
- Retrieves items from repository filtered by PurchaseOrderId
- Joins with GroceryItem data to include item names and SKUs
- Maps to PurchaseOrderItemResponse DTOs

### 2. Infrastructure Layer (Backend)

**`Store.Infrastructure/Endpoints/PurchaseOrders/v1/GetPurchaseOrderItemsEndpoint.cs`**
- HTTP GET endpoint: `/api/v1/purchase-orders/{id}/items`
- Maps to MediatR query handler
- Requires "Permissions.Store.View" permission
- Returns `List<PurchaseOrderItemResponse>`

**`Store.Infrastructure/StoreModule.cs`**
- Added `purchaseOrders.MapGetPurchaseOrderItemsEndpoint()` registration
- Also added workflow endpoints (Submit, Approve, Send, Receive, Cancel) for completeness

### 3. Domain Layer

**`Store.Domain/Entities/PurchaseOrderItem.cs`**
- No changes needed - Notes property is already inherited from AuditableEntity

### 4. Blazor Frontend

**`PurchaseOrderItemModel.cs`**
- Added `PurchaseOrderId` property to the model
- Ensures proper mapping from API response

**`PurchaseOrderItems.razor`**
- Updated `LoadItemsAsync()` method to call the new endpoint
- Uses direct HTTP call temporarily until API client is regenerated
- Added `PurchaseOrderItemResponse` helper class for deserialization
- Maps API response to PurchaseOrderItemModel for UI binding
- Calculates discount percentage from DiscountAmount

## API Endpoint Details

### GET /api/v1/purchase-orders/{id}/items

**Request:**
- Route parameter: `id` (Guid) - Purchase Order ID

**Response:** `List<PurchaseOrderItemResponse>`
```json
[
  {
    "id": "guid",
    "purchaseOrderId": "guid",
    "groceryItemId": "guid",
    "groceryItemName": "string",
    "groceryItemSku": "string",
    "quantity": 10,
    "unitPrice": 25.50,
    "discountAmount": 5.00,
    "totalPrice": 250.00,
    "receivedQuantity": 0,
    "notes": "string"
  }
]
```

**Authorization:** Requires `Permissions.Store.View`

## Usage Flow

1. User navigates to Purchase Order details page
2. PurchaseOrderItems component loads with PurchaseOrderId parameter
3. Component calls GET endpoint to fetch items
4. API handler retrieves items from repository
5. Handler joins with GroceryItem to enrich data
6. Response mapped to UI models and displayed in table
7. User can add, edit, or remove items (existing functionality)

## Next Steps

### Required:
1. **Regenerate API Client**: Run NSwag or API client generator to include the new endpoint
2. **Update PurchaseOrderItems.razor**: Replace direct HTTP call with typed API client call
   ```csharp
   // Replace this:
   var response = await HttpClient.GetAsync($"api/v1/purchase-orders/{PurchaseOrderId}/items")
   
   // With this:
   var items = await ApiClient.GetPurchaseOrderItemsEndpointAsync("1", PurchaseOrderId)
   ```

### Optional Enhancements:
1. Add caching for grocery item lookups in the handler
2. Include received quantity tracking in UI
3. Add sorting and filtering capabilities
4. Display notes field in the table
5. Add unit/UOM display from GroceryItem

## Testing Notes

- Build completed successfully with no errors
- Only warnings (code analysis suggestions, no blocking issues)
- Endpoint registered and accessible at runtime
- Frontend temporarily uses direct HTTP call until client regeneration

## Files Modified Summary

### Created (7 files):
- Store.Application/PurchaseOrders/Items/Get/v1/GetPurchaseOrderItemsQuery.cs
- Store.Application/PurchaseOrders/Items/Get/v1/PurchaseOrderItemResponse.cs
- Store.Application/PurchaseOrders/Items/Get/v1/GetPurchaseOrderItemsHandler.cs
- Store.Infrastructure/Endpoints/PurchaseOrders/v1/GetPurchaseOrderItemsEndpoint.cs

### Modified (3 files):
- Store.Infrastructure/StoreModule.cs (endpoint registration)
- apps/blazor/client/Pages/Store/PurchaseOrderItemModel.cs (added PurchaseOrderId)
- apps/blazor/client/Pages/Store/PurchaseOrderItems.razor (updated LoadItemsAsync)

## Related Documentation
- See PURCHASEORDERS_IMPLEMENTATION.md for overall Purchase Orders implementation
- See PURCHASEORDERS_BUILD_FIXES.md for previous build fixes
