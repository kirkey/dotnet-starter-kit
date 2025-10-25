# Store Pages Organization

## Overview
All Store pages have been organized into feature-based folders for better accessibility and maintainability. Each feature has its own folder containing all related pages, dialogs, and components.

## Organized Structure

### 📊 Dashboard
- **Location**: `Dashboard/`
- **Files**:
  - `StoreDashboard.razor` - Main warehouse & inventory management dashboard
  - `StoreDashboard.razor.cs`
- **Route**: `/store/dashboard`

### 📦 Bins Management
- **Location**: `Bins/`
- **Files**:
  - `Bins.razor` - Warehouse bin locations management
  - `Bins.razor.cs`
- **Route**: `/store/bins`

### 📁 Categories
- **Location**: `Categories/`
- **Files**:
  - `Categories.razor` - Item category management
  - `Categories.razor.cs`
- **Route**: `/store/categories`

### 🔍 Cycle Counts
- **Location**: `CycleCounts/`
- **Files**:
  - `CycleCounts.razor` - Main cycle counts list
  - `CycleCounts.razor.cs`
  - `CycleCountAddItemDialog.razor` - Add items to cycle count
  - `CycleCountDetailsDialog.razor` - View/edit cycle count details
  - `CycleCountDetailsDialog.razor.cs`
  - `CycleCountRecordDialog.razor` - Record count results
- **Route**: `/store/cycle-counts`
- **Features**: Variance tracking, workflow operations, item management

### 📥 Goods Receipts
- **Location**: `GoodsReceipts/`
- **Files**:
  - `GoodsReceipts.razor` - Main goods receipts list
  - `GoodsReceipts.razor.cs`
  - `GoodsReceiptDetailsDialog.razor` - View/edit receipt details
  - `GoodsReceiptDetailsDialog.razor.cs`
  - `GoodsReceiptItemDialog.razor` - Manage receipt line items
  - `GoodsReceiptItemDialog.razor.cs`
  - `CreateReceiptFromPODialog.razor` - Create receipt from purchase order
  - `CreateReceiptFromPODialog.razor.cs`
  - `ReceivingHistoryDialog.razor` - View receiving history
  - `ReceivingHistoryDialog.razor.cs`
- **Route**: `/store/goods-receipts`
- **Features**: Partial receiving, PO integration, receiving history

### 📦 Inventory Reservations
- **Location**: `InventoryReservations/`
- **Files**:
  - `InventoryReservations.razor` - Manage inventory reservations
  - `InventoryReservations.razor.cs`
  - `ReservationDetailsDialog.razor` - View detailed reservation information
  - `ReservationDetailsDialog.razor.cs`
  - `ReleaseReservationDialog.razor` - Release active reservations
  - `ReleaseReservationDialog.razor.cs`
- **Route**: `/store/inventory-reservations`
- **Features**: Prevent overselling, reservation tracking, release operations, advanced search filters

### 📊 Inventory Transactions
- **Location**: `InventoryTransactions/`
- **Files**:
  - `InventoryTransactions.razor` - View inventory transaction history
  - `InventoryTransactions.razor.cs`
- **Route**: `/store/inventory-transactions`

### 🔄 Inventory Transfers
- **Location**: `InventoryTransfers/`
- **Files**:
  - `InventoryTransfers.razor` - Manage inventory transfers between locations
  - `InventoryTransfers.razor.cs`
- **Route**: `/store/inventory-transfers`

### 🏷️ Items
- **Location**: `Items/`
- **Files**:
  - `Items.razor` - Main items/products management
  - `Items.razor.cs`
- **Route**: `/store/items`

### 🔗 Item Suppliers
- **Location**: `ItemSuppliers/`
- **Files**:
  - `ItemSuppliers.razor` - Manage item-supplier relationships
  - `ItemSuppliers.razor.cs`
- **Route**: `/store/item-suppliers`

### 🏷️ Lot Numbers
- **Location**: `LotNumbers/`
- **Files**:
  - `LotNumbers.razor` - Manage lot/batch numbers
  - `LotNumbers.razor.cs`
- **Route**: `/store/lot-numbers`

### 📋 Pick Lists
- **Location**: `PickLists/`
- **Files**:
  - `PickLists.razor` - Manage warehouse pick lists
  - `PickLists.razor.cs`
  - `PickListDetailsDialog.razor` - View pick list details and items
  - `PickListDetailsDialog.razor.cs`
  - `AssignPickListDialog.razor` - Assign pick lists to pickers
  - `AssignPickListDialog.razor.cs`
  - `AddPickListItemDialog.razor` - Add items to pick lists
  - `AddPickListItemDialog.razor.cs`
- **Route**: `/store/pick-lists`
- **Features**: Workflow operations (assign, start, complete), item management, priority management, progress tracking, advanced search filters

### 🛒 Purchase Orders
- **Location**: `PurchaseOrders/`
- **Files**:
  - `PurchaseOrders.razor` - Main purchase orders list
  - `PurchaseOrders.razor.cs`
  - `PurchaseOrderDetailsDialog.razor` - View/edit PO details
  - `PurchaseOrderDetailsDialog.razor.cs`
  - `PurchaseOrderItems.razor` - View PO line items
  - `PurchaseOrderItemDialog.razor` - Manage PO line items
  - `PurchaseOrderItemDialog.razor.cs`
  - `PurchaseOrderItemModel.cs` - Data model for PO items
- **Route**: `/store/purchase-orders`
- **Features**: Multi-item support, supplier integration, receiving integration

### 📍 Put Away Tasks
- **Location**: `PutAwayTasks/`
- **Files**:
  - `PutAwayTasks.razor` - Manage warehouse put-away tasks
  - `PutAwayTasks.razor.cs`
- **Route**: `/store/put-away-tasks`

### 🔢 Serial Numbers
- **Location**: `SerialNumbers/`
- **Files**:
  - `SerialNumbers.razor` - Manage serial number tracking
  - `SerialNumbers.razor.cs`
- **Route**: `/store/serial-numbers`

### ⚖️ Stock Adjustments
- **Location**: `StockAdjustments/`
- **Files**:
  - `StockAdjustments.razor` - Manage inventory adjustments
  - `StockAdjustments.razor.cs`
- **Route**: `/store/stock-adjustments`

### 📊 Stock Levels
- **Location**: `StockLevels/`
- **Files**:
  - `StockLevels.razor` - View current stock levels
  - `StockLevels.razor.cs`
- **Route**: `/store/stock-levels`

### 🏢 Suppliers
- **Location**: `Suppliers/`
- **Files**:
  - `Suppliers.razor` - Manage supplier information
  - `Suppliers.razor.cs`
- **Route**: `/store/suppliers`

## Benefits of This Organization

1. **Fast Accessibility**: Related files are grouped together, making it easy to find all components for a specific feature
2. **Better Maintainability**: Changes to a feature are localized to a single folder
3. **Clear Structure**: Each folder represents a distinct business feature
4. **Scalability**: Easy to add new dialogs or components to existing features
5. **Team Collaboration**: Multiple developers can work on different features without conflicts

## Navigation

All routes remain unchanged. The physical file location does not affect routing in Blazor, as routes are defined using the `@page` directive.

## Related Documentation

- See `Docs/` folder for feature-specific implementation documentation
- Each feature folder contains its main page and all related dialogs/components
- Follow CQRS and DRY principles as outlined in the coding instructions

---
*Last Updated: October 25, 2025*

