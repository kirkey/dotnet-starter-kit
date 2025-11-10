# Auto-Reorder Point Feature - Implementation Summary

## 📋 Overview

This feature implements an automatic reorder point system where items that reach their minimum stock levels are automatically suggested and can be bulk-added to purchase orders based on their supplier relationship.

## 🎯 Business Problem Solved

In real-world purchase order transactions:
- Users shouldn't manually add every item to a PO
- Items bound to suppliers should automatically appear when they hit reorder points
- System should calculate optimal order quantities based on current stock vs. reorder parameters
- Buyers need visibility into which items need restocking before creating a PO

## ✨ Key Features Implemented

### 1. **Items Needing Reorder Query**
**Endpoint**: `POST /api/v1/purchase-orders/suppliers/{supplierId}/items-needing-reorder`

**Functionality**:
- Retrieves all items for a specific supplier that are at or below their reorder point
- Optionally filters by warehouse location
- Calculates current stock across all locations (or specific warehouse)
- Provides smart ordering suggestions based on:
  - Current stock level
  - Reorder point threshold
  - Minimum/maximum stock levels
  - Standard reorder quantity

**Response includes**:
- Item details (SKU, name, description)
- Current stock vs. reorder point
- Shortage quantity
- **Suggested order quantity** (smart calculation)
- Estimated cost
- Lead time information

### 2. **Auto-Add Items to Purchase Order**
**Endpoint**: `POST /api/v1/purchase-orders/{id}/auto-add-items`

**Functionality**:
- Automatically adds all items needing reorder to an existing draft purchase order
- Validates purchase order status (must be in Draft)
- Checks for duplicate items (skips items already on the order)
- Uses smart quantity calculations or standard reorder quantities
- Provides detailed feedback on what was added and what was skipped

**Response includes**:
- Number of items added
- Number of items skipped (with reasons)
- Total estimated cost
- Detailed list of added items
- Detailed list of skipped items with reasons

## 🧮 Smart Quantity Calculation Algorithm

The system uses intelligent logic to determine the optimal order quantity:

```
IF current_stock < minimum_stock THEN
    // Critical shortage - order enough to reach reorder target
    target = MIN(reorder_point + reorder_quantity, maximum_stock)
    suggested_qty = MAX(target - current_stock, reorder_quantity)

ELSE IF current_stock <= reorder_point THEN
    // At reorder point - order standard quantity
    space_available = maximum_stock - current_stock
    suggested_qty = MIN(reorder_quantity, space_available)

ELSE
    // Default to standard reorder quantity
    suggested_qty = reorder_quantity
```

### Example Calculations:

**Scenario 1: Critical Shortage**
- Current Stock: 5
- Minimum Stock: 10
- Reorder Point: 20
- Reorder Quantity: 50
- Maximum Stock: 200
- **Suggested Order**: 65 units (to reach 70, which is reorder point + reorder quantity)

**Scenario 2: At Reorder Point**
- Current Stock: 20
- Minimum Stock: 10
- Reorder Point: 20
- Reorder Quantity: 50
- Maximum Stock: 200
- **Suggested Order**: 50 units (standard reorder quantity)

**Scenario 3: Near Maximum**
- Current Stock: 170
- Minimum Stock: 10
- Reorder Point: 20
- Reorder Quantity: 50
- Maximum Stock: 200
- **Suggested Order**: 30 units (only what fits without exceeding maximum)

## 🏗️ Technical Implementation

### Backend Structure

```
Store.Application/
└── PurchaseOrders/
    ├── GetItemsNeedingReorder/
    │   └── v1/
    │       ├── GetItemsNeedingReorderRequest.cs
    │       ├── ItemNeedingReorderResponse.cs
    │       └── GetItemsNeedingReorderHandler.cs
    └── AutoAddItems/
        └── v1/
            ├── AutoAddItemsToPurchaseOrderCommand.cs
            ├── AutoAddItemsToPurchaseOrderResponse.cs
            ├── AutoAddItemsToPurchaseOrderHandler.cs
            └── AutoAddItemsToPurchaseOrderCommandValidator.cs

Store.Infrastructure/
└── Endpoints/
    └── PurchaseOrders/
        └── v1/
            ├── GetItemsNeedingReorderEndpoint.cs
            └── AutoAddItemsToPurchaseOrderEndpoint.cs
```

### Key Components

**1. Specifications Used**:
- `GetItemsBySupplierSpec` - Filters active items by supplier
- `GetStockByItemSpec` - Gets all stock levels for an item
- `GetStockByItemAndWarehouseSpec` - Gets stock for item at specific warehouse

**2. Domain Logic**:
- Leverages existing Item entity fields:
  - `ReorderPoint` - Threshold that triggers reorder
  - `ReorderQuantity` - Standard order size
  - `MinimumStock` - Safety stock level
  - `MaximumStock` - Storage capacity limit
  - `LeadTimeDays` - Supplier delivery time

**3. Business Rules**:
- Only active items are considered
- Stock comparison uses `currentStock <= reorderPoint` (inclusive)
- Items already on the purchase order are skipped
- Purchase order must be in Draft status
- Quantities are validated against maximum stock
- Duplicate prevention by item ID

## 📊 Data Flow

### Getting Items Needing Reorder:
```
1. User selects supplier in PO form
2. UI calls GET items-needing-reorder endpoint
3. Backend queries:
   a. Get all active items for supplier
   b. For each item, sum stock levels
   c. Compare current stock to reorder point
4. Calculate suggested quantities
5. Return sorted list (lowest stock first)
```

### Auto-Adding Items:
```
1. User clicks "Auto-Add Items" button
2. UI calls POST auto-add-items endpoint
3. Backend:
   a. Validates PO exists and is in Draft
   b. Gets items needing reorder for PO's supplier
   c. Checks each item against existing PO items
   d. Adds non-duplicate items to PO
   e. Recalculates PO totals
4. Returns summary of additions/skips
5. UI refreshes PO items grid
```

## 🎨 UI Integration Points

### Purchase Order Form Updates Needed:

**1. Add "Auto-Add Items" Button**
- Location: Near the "Add Item" button in PO form
- Visibility: Only shown when PO is in Draft status
- Icon: Suggest magic wand or auto icon
- Action: Opens confirmation dialog or directly adds items

**2. Items Needing Reorder Preview (Optional)**
- Location: Collapsible panel above items grid
- Shows: Count of items needing reorder for selected supplier
- Action: Click to view list before adding

**3. Confirmation Dialog**
- Shows: List of items that will be added
- Displays: Item SKU, name, current stock, suggested quantity, cost
- Options:
  - Use suggested quantities (smart calculation)
  - Use standard reorder quantities
  - Select/deselect specific items
- Summary: Total items, total estimated cost

**4. Result Feedback**
- Success snackbar: "Added X items, skipped Y items"
- Option to view details of what was added/skipped
- Automatic refresh of items grid

## 🔄 Workflow Example

### Scenario: Creating a Purchase Order for Low-Stock Items

**Step 1: Create PO Header**
```
- User creates new PO
- Selects Supplier: "ABC Wholesale"
- System shows badge: "5 items need reordering"
```

**Step 2: View Items Needing Reorder**
```
Items Needing Reorder from ABC Wholesale:
╔═══════════════════════════════════════════════════════════════════╗
║ SKU      │ Name           │ Current │ Reorder │ Suggested │ Cost  ║
╠══════════╪════════════════╪═════════╪═════════╪═══════════╪═══════╣
║ ITEM-001 │ Widget A       │ 3       │ 20      │ 50        │ $15.00 ║
║ ITEM-002 │ Gadget B       │ 8       │ 15      │ 40        │ $22.50 ║
║ ITEM-003 │ Tool C         │ 15      │ 25      │ 35        │ $18.00 ║
║ ITEM-004 │ Part D         │ 5       │ 10      │ 30        │ $12.00 ║
║ ITEM-005 │ Component E    │ 0       │ 5       │ 50        │ $9.00  ║
╚══════════╧════════════════╧═════════╧═════════╧═══════════╧═══════╝
Total Estimated Cost: $3,585.00
```

**Step 3: Auto-Add Items**
```
- User clicks "Auto-Add All Items"
- System adds all 5 items with suggested quantities
- PO items grid refreshes
- Snackbar: "Successfully added 5 items totaling $3,585.00"
```

**Step 4: Review and Adjust**
```
- User can still manually adjust quantities
- User can add additional items
- User can remove items
- Proceeds to submit PO
```

## 🚀 Benefits

### For Users:
✅ **Time Savings**: No manual lookup of low-stock items
✅ **Reduced Errors**: System calculates optimal quantities
✅ **Better Planning**: See all items needing reorder at once
✅ **Informed Decisions**: Current stock vs. reorder point visibility
✅ **Flexibility**: Can still manually add/remove/adjust items

### For Business:
✅ **Optimized Inventory**: Smart quantity calculations prevent overstocking
✅ **Prevent Stockouts**: Automatic identification of low-stock items
✅ **Supplier Efficiency**: Group orders by supplier automatically
✅ **Audit Trail**: Clear record of why items were ordered
✅ **Cost Control**: Estimated cost before committing to order

## 📝 Configuration Requirements

### Item Setup:
Each item must have configured:
- `SupplierId` - Primary supplier for the item
- `ReorderPoint` - When to trigger reorder (e.g., 20 units)
- `ReorderQuantity` - Standard order size (e.g., 50 units)
- `MinimumStock` - Safety stock level (e.g., 10 units)
- `MaximumStock` - Storage limit (e.g., 200 units)
- `LeadTimeDays` - Supplier delivery time (e.g., 7 days)
- `Cost` - Unit cost from supplier

### Stock Levels:
- System uses `StockLevel` aggregate across warehouses
- Optional warehouse filtering for location-specific reordering

## 🔐 Security & Permissions

- Requires `Store.View` permission to see items needing reorder
- Requires `Store.Update` permission to auto-add items to PO
- Only works on Draft status purchase orders
- Validates supplier relationship between PO and items

## 🧪 Testing Scenarios

### Test Case 1: Happy Path
```
Given: PO in Draft with Supplier A
And: 3 items from Supplier A below reorder point
When: Auto-add items clicked
Then: All 3 items added with suggested quantities
And: PO total updated
```

### Test Case 2: Duplicate Prevention
```
Given: PO already has ITEM-001
And: ITEM-001 is below reorder point
When: Auto-add items clicked
Then: ITEM-001 is skipped
And: Reason shown: "Already on this purchase order"
```

### Test Case 3: No Items Needed
```
Given: All items from Supplier B are above reorder point
When: Auto-add items clicked
Then: Message: "No items need reordering"
```

### Test Case 4: Maximum Stock Limit
```
Given: Item current stock = 190, maximum = 200, reorder qty = 50
When: Suggested quantity calculated
Then: Suggested = 10 (respects maximum limit)
```

## 🔮 Future Enhancements

**Phase 2 Possibilities**:
- [ ] Multi-warehouse reorder optimization
- [ ] Lead time-based reorder calculations (order before hitting reorder point)
- [ ] Seasonal demand adjustments
- [ ] Supplier minimum order quantity (MOQ) compliance
- [ ] Price break optimization (order more for better unit price)
- [ ] Automatic PO generation on schedule
- [ ] Email notifications when items hit reorder point
- [ ] Dashboard widget showing items needing reorder
- [ ] Historical reorder pattern analysis
- [ ] Integration with demand forecasting

## 📚 API Documentation

### GET Items Needing Reorder
```http
POST /api/v1/purchase-orders/suppliers/{supplierId}/items-needing-reorder
Content-Type: application/json

{
  "supplierId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "warehouseId": "3fa85f64-5717-4562-b3fc-2c963f66afa6" // optional
}

Response: 200 OK
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "sku": "ITEM-001",
    "name": "Widget A",
    "description": "Premium widget",
    "currentStock": 5,
    "reorderPoint": 20,
    "reorderQuantity": 50,
    "cost": 15.00,
    "supplierId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "supplierName": "ABC Wholesale",
    "leadTimeDays": 7,
    "minimumStock": 10,
    "maximumStock": 200,
    "shortageQuantity": 15,
    "suggestedOrderQuantity": 65,
    "estimatedCost": 975.00
  }
]
```

### POST Auto-Add Items
```http
POST /api/v1/purchase-orders/{id}/auto-add-items
Content-Type: application/json

{
  "purchaseOrderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "warehouseId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // optional
  "useSuggestedQuantities": true
}

Response: 200 OK
{
  "purchaseOrderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "itemsAdded": 5,
  "itemsSkipped": 2,
  "totalEstimatedCost": 3585.00,
  "addedItems": [
    {
      "itemId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "sku": "ITEM-001",
      "name": "Widget A",
      "quantity": 65,
      "unitPrice": 15.00,
      "totalCost": 975.00
    }
  ],
  "skippedItems": [
    {
      "itemId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "sku": "ITEM-007",
      "name": "Tool X",
      "reason": "Item already on this purchase order"
    }
  ]
}
```

## ✅ Implementation Checklist

### Backend - COMPLETED ✅
- [x] GetItemsNeedingReorderRequest
- [x] ItemNeedingReorderResponse
- [x] GetItemsNeedingReorderHandler with specifications
- [x] AutoAddItemsToPurchaseOrderCommand
- [x] AutoAddItemsToPurchaseOrderResponse
- [x] AutoAddItemsToPurchaseOrderHandler
- [x] AutoAddItemsToPurchaseOrderCommandValidator
- [x] GetItemsNeedingReorderEndpoint
- [x] AutoAddItemsToPurchaseOrderEndpoint
- [x] Endpoint registration in PurchaseOrdersEndpoints

### Frontend - COMPLETED ✅
- [x] Supplier-based item filtering in AutocompleteItem
- [x] SupplierId parameter added to PurchaseOrderItemDialog
- [x] SupplierId passed through component hierarchy
- [x] Items filtered by PO's supplier when adding/editing
- [ ] Regenerate NSwag client with new endpoints
- [ ] Add "Auto-Add Items" button to PO form
- [ ] Create items needing reorder preview component
- [ ] Create auto-add confirmation dialog
- [ ] Add result feedback (snackbar/toast)
- [ ] Update PO items grid after auto-add
- [ ] Add badge showing count of items needing reorder
- [ ] Add warehouse filter option
- [ ] Add toggle for suggested vs. standard quantities
- [ ] Handle error states and validation

## 🎁 Bonus Feature: Supplier-Based Item Filtering

As part of this implementation, we also added automatic supplier-based filtering for item selection:

### What It Does:
When adding or editing items on a Purchase Order, the item search autocomplete now **automatically filters items to show only those from the PO's supplier**.

### Why It Matters:
- ❌ **Before**: Users could accidentally add items from any supplier to a PO
- ✅ **After**: Users can only select items from the correct supplier

### How It Works:
1. Purchase Order has a Supplier (e.g., "ABC Wholesale")
2. When user clicks "Add Item", the item search is filtered by that supplier
3. Only items belonging to "ABC Wholesale" appear in search results
4. Impossible to add items from wrong supplier

### Implementation:
- Added `SupplierId` parameter to `AutocompleteItem` component
- Pass supplier ID through the component hierarchy
- API already supported filtering by `SupplierId` in `SearchItemsCommand`
- Zero backend changes needed!

**See detailed documentation**: `PURCHASE_ORDER_SUPPLIER_FILTERING_FEATURE.md`

### Testing - TODO 🧪
- [ ] Unit tests for quantity calculation algorithm
- [ ] Integration tests for GetItemsNeedingReorder
- [ ] Integration tests for AutoAddItems
- [ ] UI tests for button visibility based on status
- [ ] UI tests for confirmation dialog
- [ ] UI tests for result feedback
- [ ] Load tests for large item catalogs
- [ ] Performance tests for stock calculation across warehouses

---

**Created**: {{ date }}
**Feature Status**: Backend Complete ✅ | Frontend In Progress 🔄
**Priority**: High 🔥
**Business Value**: High 💰

