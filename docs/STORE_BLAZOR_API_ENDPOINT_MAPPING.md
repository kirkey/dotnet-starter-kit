# Store Module - Blazor Client to API Endpoint Mapping

This document provides a comprehensive mapping between the Blazor client Store pages and their corresponding API endpoints.

**Last Updated:** October 4, 2025  
**Status:** All API endpoints now mapped to Blazor pages ✅

## Overview

This document covers **19 fully implemented Blazor pages** for the Store module, each mapped to their corresponding API endpoints. All endpoints are now accessible through the Blazor client interface.

---

## 1. Items (Items.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/Items.razor`  
**API Base Route:** `/api/v1/store/items`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Items | `POST /api/v1/store/items/search` | POST | `SearchItemsEndpoint.cs` |
| Get Item | `GET /api/v1/store/items/{id}` | GET | `GetItemEndpoint.cs` |
| Create Item | `POST /api/v1/store/items` | POST | `CreateItemEndpoint.cs` |
| Update Item | `PUT /api/v1/store/items/{id}` | PUT | `UpdateItemEndpoint.cs` |
| Delete Item | `DELETE /api/v1/store/items/{id}` | DELETE | `DeleteItemEndpoint.cs` |

**API Commands:**
- `SearchItemsCommand` → Returns `PagedList<ItemResponse>`
- `GetItemQuery` → Returns `ItemResponse`
- `CreateItemCommand` → Returns `CreateItemResponse`
- `UpdateItemCommand` → Returns success
- `DeleteItemCommand` → Returns success

---

## 2. Categories (Categories.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/Categories.razor`  
**API Base Route:** `/api/v1/store/categories`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Categories | `POST /api/v1/store/categories/search` | POST | `SearchCategoriesEndpoint.cs` |
| Get Category | `GET /api/v1/store/categories/{id}` | GET | `GetCategoryEndpoint.cs` |
| Create Category | `POST /api/v1/store/categories` | POST | `CreateCategoryEndpoint.cs` |
| Update Category | `PUT /api/v1/store/categories/{id}` | PUT | `UpdateCategoryEndpoint.cs` |
| Delete Category | `DELETE /api/v1/store/categories/{id}` | DELETE | `DeleteCategoryEndpoint.cs` |

**API Commands:**
- `SearchCategoriesCommand` → Returns `PagedList<CategoryResponse>`
- `GetCategoryQuery` → Returns `CategoryResponse`
- `CreateCategoryCommand` → Returns `CreateCategoryResponse` (includes image upload)
- `UpdateCategoryCommand` → Returns success (includes image upload)
- `DeleteCategoryCommand` → Returns success

**Special Features:**
- Supports image upload via `FileUploadCommand`

---

## 3. Suppliers (Suppliers.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/Suppliers.razor`  
**API Base Route:** `/api/v1/store/suppliers`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Suppliers | `POST /api/v1/store/suppliers/search` | POST | `SearchSuppliersEndpoint.cs` |
| Get Supplier | `GET /api/v1/store/suppliers/{id}` | GET | `GetSupplierEndpoint.cs` |
| Create Supplier | `POST /api/v1/store/suppliers` | POST | `CreateSupplierEndpoint.cs` |
| Update Supplier | `PUT /api/v1/store/suppliers/{id}` | PUT | `UpdateSupplierEndpoint.cs` |
| Delete Supplier | `DELETE /api/v1/store/suppliers/{id}` | DELETE | `DeleteSupplierEndpoint.cs` |
| Activate Supplier | `POST /api/v1/store/suppliers/{id}/activate` | POST | `ActivateSupplierEndpoint.cs` |
| Deactivate Supplier | `POST /api/v1/store/suppliers/{id}/deactivate` | POST | `DeactivateSupplierEndpoint.cs` |

**API Commands:**
- `SearchSuppliersCommand` → Returns `PagedList<SupplierResponse>`
- `GetSupplierQuery` → Returns `SupplierResponse`
- `CreateSupplierCommand` → Returns `CreateSupplierResponse`
- `UpdateSupplierCommand` → Returns success
- `DeleteSupplierCommand` → Returns success
- `ActivateSupplierCommand` → Returns success
- `DeactivateSupplierCommand` → Returns success

---

## 4. Bins (Bins.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/Bins.razor`  
**API Base Route:** `/api/v1/store/bins`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Bins | `POST /api/v1/store/bins/search` | POST | `SearchBinsEndpoint.cs` |
| Get Bin | `GET /api/v1/store/bins/{id}` | GET | `GetBinEndpoint.cs` |
| Create Bin | `POST /api/v1/store/bins` | POST | `CreateBinEndpoint.cs` |
| Update Bin | `PUT /api/v1/store/bins/{id}` | PUT | `UpdateBinEndpoint.cs` |
| Delete Bin | `DELETE /api/v1/store/bins/{id}` | DELETE | `DeleteBinEndpoint.cs` |

**API Commands:**
- `SearchBinsCommand` → Returns `PagedList<BinResponse>`
- `GetBinQuery` → Returns `BinResponse`
- `CreateBinCommand` → Returns `CreateBinResponse`
- `UpdateBinCommand` → Returns success
- `DeleteBinCommand` → Returns success

---

## 5. Purchase Orders (PurchaseOrders.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/PurchaseOrders.razor`  
**API Base Route:** `/api/v1/store/purchase-orders`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Purchase Orders | `POST /api/v1/store/purchase-orders/search` | POST | `SearchPurchaseOrdersEndpoint.cs` |
| Get Purchase Order | `GET /api/v1/store/purchase-orders/{id}` | GET | `GetPurchaseOrderEndpoint.cs` |
| Get Purchase Order Items | `GET /api/v1/store/purchase-orders/{id}/items` | GET | `GetPurchaseOrderItemsEndpoint.cs` |
| Create Purchase Order | `POST /api/v1/store/purchase-orders` | POST | `CreatePurchaseOrderEndpoint.cs` |
| Update Purchase Order | `PUT /api/v1/store/purchase-orders/{id}` | PUT | `UpdatePurchaseOrderEndpoint.cs` |
| Delete Purchase Order | `DELETE /api/v1/store/purchase-orders/{id}` | DELETE | `DeletePurchaseOrderEndpoint.cs` |
| Submit Purchase Order | `POST /api/v1/store/purchase-orders/{id}/submit` | POST | `SubmitPurchaseOrderEndpoint.cs` |
| Approve Purchase Order | `POST /api/v1/store/purchase-orders/{id}/approve` | POST | `ApprovePurchaseOrderEndpoint.cs` |
| Send Purchase Order | `POST /api/v1/store/purchase-orders/{id}/send` | POST | `SendPurchaseOrderEndpoint.cs` |
| Receive Purchase Order | `POST /api/v1/store/purchase-orders/{id}/receive` | POST | `ReceivePurchaseOrderEndpoint.cs` |
| Cancel Purchase Order | `POST /api/v1/store/purchase-orders/{id}/cancel` | POST | `CancelPurchaseOrderEndpoint.cs` |
| Add Item to PO | `POST /api/v1/store/purchase-orders/{id}/items` | POST | `AddPurchaseOrderItemEndpoint.cs` |
| Remove Item from PO | `DELETE /api/v1/store/purchase-orders/{id}/items/{itemId}` | DELETE | `RemovePurchaseOrderItemEndpoint.cs` |
| Update Item Quantity | `PUT /api/v1/store/purchase-orders/{id}/items/{itemId}/quantity` | PUT | `UpdatePurchaseOrderItemQuantityEndpoint.cs` |
| Update Item Price | `PUT /api/v1/store/purchase-orders/{id}/items/{itemId}/price` | PUT | `UpdatePurchaseOrderItemPriceEndpoint.cs` |
| Receive Item Quantity | `PUT /api/v1/store/purchase-orders/{id}/items/{itemId}/receive` | PUT | `ReceivePurchaseOrderItemQuantityEndpoint.cs` |

**API Commands:**
- `SearchPurchaseOrdersCommand` → Returns `PagedList<PurchaseOrderResponse>`
- `GetPurchaseOrderQuery` → Returns `PurchaseOrderResponse`
- `CreatePurchaseOrderCommand` → Returns `CreatePurchaseOrderResponse`
- `UpdatePurchaseOrderCommand` → Returns success
- Various workflow commands (Submit, Approve, Send, Receive, Cancel)

**Workflow States:**
- Draft → Submitted → Approved → Sent → Received
- Can be canceled at any stage before Received

---

## 6. Goods Receipts (GoodsReceipts.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/GoodsReceipts.razor`  
**API Base Route:** `/api/v1/store/goods-receipts`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Goods Receipts | `POST /api/v1/store/goods-receipts/search` | POST | `SearchGoodsReceiptsEndpoint.cs` |
| Get Goods Receipt | `GET /api/v1/store/goods-receipts/{id}` | GET | `GetGoodsReceiptEndpoint.cs` |
| Create Goods Receipt | `POST /api/v1/store/goods-receipts` | POST | `CreateGoodsReceiptEndpoint.cs` |
| Delete Goods Receipt | `DELETE /api/v1/store/goods-receipts/{id}` | DELETE | `DeleteGoodsReceiptEndpoint.cs` |
| Add Item to Receipt | `POST /api/v1/store/goods-receipts/{id}/items` | POST | `AddGoodsReceiptItemEndpoint.cs` |
| Mark as Received | `POST /api/v1/store/goods-receipts/{id}/mark-received` | POST | `MarkReceivedEndpoint.cs` |

**API Commands:**
- `SearchGoodsReceiptsCommand` → Returns `PagedList<GoodsReceiptResponse>`
- `GetGoodsReceiptQuery` → Returns `GoodsReceiptResponse`
- `CreateGoodsReceiptCommand` → Returns `CreateGoodsReceiptResponse`
- `AddGoodsReceiptItemCommand` → Returns success
- `MarkReceivedCommand` → Returns success

---

## 7. Inventory Transfers (InventoryTransfers.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/InventoryTransfers.razor`  
**API Base Route:** `/api/v1/store/inventory-transfers`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Inventory Transfers | `POST /api/v1/store/inventory-transfers/search` | POST | `SearchInventoryTransfersEndpoint.cs` |
| Get Inventory Transfer | `GET /api/v1/store/inventory-transfers/{id}` | GET | `GetInventoryTransferEndpoint.cs` |
| Create Inventory Transfer | `POST /api/v1/store/inventory-transfers` | POST | `CreateInventoryTransferEndpoint.cs` |
| Update Inventory Transfer | `PUT /api/v1/store/inventory-transfers/{id}` | PUT | `UpdateInventoryTransferEndpoint.cs` |
| Delete Inventory Transfer | `DELETE /api/v1/store/inventory-transfers/{id}` | DELETE | `DeleteInventoryTransferEndpoint.cs` |
| Approve Transfer | `POST /api/v1/store/inventory-transfers/{id}/approve` | POST | `ApproveInventoryTransferEndpoint.cs` |
| Mark In Transit | `POST /api/v1/store/inventory-transfers/{id}/in-transit` | POST | `MarkInTransitInventoryTransferEndpoint.cs` |
| Complete Transfer | `POST /api/v1/store/inventory-transfers/{id}/complete` | POST | `CompleteInventoryTransferEndpoint.cs` |
| Cancel Transfer | `POST /api/v1/store/inventory-transfers/{id}/cancel` | POST | `CancelInventoryTransferEndpoint.cs` |
| Add Transfer Item | `POST /api/v1/store/inventory-transfers/{id}/items` | POST | `AddInventoryTransferItemEndpoint.cs` |
| Update Transfer Item | `PUT /api/v1/store/inventory-transfers/{id}/items/{itemId}` | PUT | `UpdateInventoryTransferItemEndpoint.cs` |
| Remove Transfer Item | `DELETE /api/v1/store/inventory-transfers/{id}/items/{itemId}` | DELETE | `RemoveInventoryTransferItemEndpoint.cs` |

**API Commands:**
- `SearchInventoryTransfersCommand` → Returns `PagedList<GetInventoryTransferListResponse>`
- `GetInventoryTransferQuery` → Returns `GetInventoryTransferResponse`
- `CreateInventoryTransferCommand` → Returns `CreateInventoryTransferResponse`
- Various workflow commands (Approve, MarkInTransit, Complete, Cancel)

**Workflow States:**
- Draft → Approved → InTransit → Completed
- Can be canceled at any stage before Completed

---

## 8. Inventory Transactions (InventoryTransactions.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/InventoryTransactions.razor`  
**API Base Route:** `/api/v1/store/inventory-transactions`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Transactions | `POST /api/v1/store/inventory-transactions/search` | POST | `SearchInventoryTransactionsEndpoint.cs` |
| Get Transaction | `GET /api/v1/store/inventory-transactions/{id}` | GET | `GetInventoryTransactionEndpoint.cs` |
| Create Transaction | `POST /api/v1/store/inventory-transactions` | POST | `CreateInventoryTransactionEndpoint.cs` |
| Delete Transaction | `DELETE /api/v1/store/inventory-transactions/{id}` | DELETE | `DeleteInventoryTransactionEndpoint.cs` |
| Approve Transaction | `POST /api/v1/store/inventory-transactions/{id}/approve` | POST | `ApproveInventoryTransactionEndpoint.cs` |

**API Commands:**
- `SearchInventoryTransactionsCommand` → Returns `PagedList<InventoryTransactionResponse>`
- `GetInventoryTransactionQuery` → Returns `InventoryTransactionResponse`
- `CreateInventoryTransactionCommand` → Returns `CreateInventoryTransactionResponse`
- `ApproveInventoryTransactionCommand` → Returns success

---

## 9. Inventory Reservations (InventoryReservations.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/InventoryReservations.razor`  
**API Base Route:** `/api/v1/store/inventory-reservations`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Reservations | `POST /api/v1/store/inventory-reservations/search` | POST | `SearchInventoryReservationsEndpoint.cs` |
| Get Reservation | `GET /api/v1/store/inventory-reservations/{id}` | GET | `GetInventoryReservationEndpoint.cs` |
| Create Reservation | `POST /api/v1/store/inventory-reservations` | POST | `CreateInventoryReservationEndpoint.cs` |
| Delete Reservation | `DELETE /api/v1/store/inventory-reservations/{id}` | DELETE | `DeleteInventoryReservationEndpoint.cs` |
| Release Reservation | `POST /api/v1/store/inventory-reservations/{id}/release` | POST | `ReleaseInventoryReservationEndpoint.cs` |

**API Commands:**
- `SearchInventoryReservationsCommand` → Returns `PagedList<InventoryReservationResponse>`
- `GetInventoryReservationQuery` → Returns `InventoryReservationResponse`
- `CreateInventoryReservationCommand` → Returns `CreateInventoryReservationResponse`
- `ReleaseInventoryReservationCommand` → Returns success

---

## 10. Stock Levels (StockLevels.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/StockLevels.razor`  
**API Base Route:** `/api/v1/store/stock-levels`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Stock Levels | `POST /api/v1/store/stock-levels/search` | POST | `SearchStockLevelsEndpoint.cs` |
| Get Stock Level | `GET /api/v1/store/stock-levels/{id}` | GET | `GetStockLevelEndpoint.cs` |
| Create Stock Level | `POST /api/v1/store/stock-levels` | POST | `CreateStockLevelEndpoint.cs` |
| Update Stock Level | `PUT /api/v1/store/stock-levels/{id}` | PUT | `UpdateStockLevelEndpoint.cs` |
| Delete Stock Level | `DELETE /api/v1/store/stock-levels/{id}` | DELETE | `DeleteStockLevelEndpoint.cs` |
| Reserve Stock | `POST /api/v1/store/stock-levels/{id}/reserve` | POST | `ReserveStockEndpoint.cs` |
| Release Stock | `POST /api/v1/store/stock-levels/{id}/release` | POST | `ReleaseStockEndpoint.cs` |
| Allocate Stock | `POST /api/v1/store/stock-levels/{id}/allocate` | POST | `AllocateStockEndpoint.cs` |

**API Commands:**
- `SearchStockLevelsCommand` → Returns `PagedList<StockLevelResponse>`
- `GetStockLevelQuery` → Returns `StockLevelResponse`
- `CreateStockLevelCommand` → Returns `CreateStockLevelResponse`
- `UpdateStockLevelCommand` → Returns success
- `ReserveStockCommand` → Returns success
- `ReleaseStockCommand` → Returns success
- `AllocateStockCommand` → Returns success

---

## 11. Stock Adjustments (StockAdjustments.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/StockAdjustments.razor`  
**API Base Route:** `/api/v1/store/stock-adjustments`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Stock Adjustments | `POST /api/v1/store/stock-adjustments/search` | POST | `SearchStockAdjustmentsEndpoint.cs` |
| Get Stock Adjustment | `GET /api/v1/store/stock-adjustments/{id}` | GET | `GetStockAdjustmentEndpoint.cs` |
| Create Stock Adjustment | `POST /api/v1/store/stock-adjustments` | POST | `CreateStockAdjustmentEndpoint.cs` |
| Update Stock Adjustment | `PUT /api/v1/store/stock-adjustments/{id}` | PUT | `UpdateStockAdjustmentEndpoint.cs` |
| Delete Stock Adjustment | `DELETE /api/v1/store/stock-adjustments/{id}` | DELETE | `DeleteStockAdjustmentEndpoint.cs` |
| Approve Adjustment | `POST /api/v1/store/stock-adjustments/{id}/approve` | POST | `ApproveStockAdjustmentEndpoint.cs` |

**API Commands:**
- `SearchStockAdjustmentsCommand` → Returns `PagedList<StockAdjustmentResponse>`
- `GetStockAdjustmentQuery` → Returns `StockAdjustmentResponse`
- `CreateStockAdjustmentCommand` → Returns `CreateStockAdjustmentResponse`
- `UpdateStockAdjustmentCommand` → Returns success
- `ApproveStockAdjustmentCommand` → Returns success

---

## 12. Pick Lists (PickLists.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/PickLists.razor`  
**API Base Route:** `/api/v1/store/pick-lists`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Pick Lists | `POST /api/v1/store/pick-lists/search` | POST | `SearchPickListsEndpoint.cs` |
| Get Pick List | `GET /api/v1/store/pick-lists/{id}` | GET | `GetPickListEndpoint.cs` |
| Create Pick List | `POST /api/v1/store/pick-lists` | POST | `CreatePickListEndpoint.cs` |
| Delete Pick List | `DELETE /api/v1/store/pick-lists/{id}` | DELETE | `DeletePickListEndpoint.cs` |
| Assign Pick List | `POST /api/v1/store/pick-lists/{id}/assign` | POST | `AssignPickListEndpoint.cs` |
| Start Picking | `POST /api/v1/store/pick-lists/{id}/start` | POST | `StartPickingEndpoint.cs` |
| Complete Picking | `POST /api/v1/store/pick-lists/{id}/complete` | POST | `CompletePickingEndpoint.cs` |
| Add Pick List Item | `POST /api/v1/store/pick-lists/{id}/items` | POST | `AddPickListItemEndpoint.cs` |

**API Commands:**
- `SearchPickListsCommand` → Returns `PagedList<PickListResponse>`
- `GetPickListQuery` → Returns `PickListResponse`
- `CreatePickListCommand` → Returns `CreatePickListResponse`
- `AssignPickListCommand` → Returns success
- `StartPickingCommand` → Returns success
- `CompletePickingCommand` → Returns success

**Workflow States:**
- Created → Assigned → InProgress → Completed

---

## 13. Put Away Tasks (PutAwayTasks.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/PutAwayTasks.razor`  
**API Base Route:** `/api/v1/store/put-away-tasks`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Put Away Tasks | `POST /api/v1/store/put-away-tasks/search` | POST | `SearchPutAwayTasksEndpoint.cs` |
| Get Put Away Task | `GET /api/v1/store/put-away-tasks/{id}` | GET | `GetPutAwayTaskEndpoint.cs` |
| Create Put Away Task | `POST /api/v1/store/put-away-tasks` | POST | `CreatePutAwayTaskEndpoint.cs` |
| Delete Put Away Task | `DELETE /api/v1/store/put-away-tasks/{id}` | DELETE | `DeletePutAwayTaskEndpoint.cs` |
| Assign Task | `POST /api/v1/store/put-away-tasks/{id}/assign` | POST | `AssignPutAwayTaskEndpoint.cs` |
| Start Put Away | `POST /api/v1/store/put-away-tasks/{id}/start` | POST | `StartPutAwayEndpoint.cs` |
| Complete Put Away | `POST /api/v1/store/put-away-tasks/{id}/complete` | POST | `CompletePutAwayEndpoint.cs` |
| Add Task Item | `POST /api/v1/store/put-away-tasks/{id}/items` | POST | `AddPutAwayTaskItemEndpoint.cs` |

**API Commands:**
- `SearchPutAwayTasksCommand` → Returns `PagedList<PutAwayTaskResponse>`
- `GetPutAwayTaskQuery` → Returns `PutAwayTaskResponse`
- `CreatePutAwayTaskCommand` → Returns `CreatePutAwayTaskResponse`
- `AssignPutAwayTaskCommand` → Returns success
- `StartPutAwayCommand` → Returns success
- `CompletePutAwayCommand` → Returns success

**Workflow States:**
- Created → Assigned → InProgress → Completed

---

## 14. Lot Numbers (LotNumbers.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/LotNumbers.razor`  
**API Base Route:** `/api/v1/store/lot-numbers`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Lot Numbers | `POST /api/v1/store/lot-numbers/search` | POST | `SearchLotNumbersEndpoint.cs` |
| Get Lot Number | `GET /api/v1/store/lot-numbers/{id}` | GET | `GetLotNumberEndpoint.cs` |
| Create Lot Number | `POST /api/v1/store/lot-numbers` | POST | `CreateLotNumberEndpoint.cs` |
| Update Lot Number | `PUT /api/v1/store/lot-numbers/{id}` | PUT | `UpdateLotNumberEndpoint.cs` |
| Delete Lot Number | `DELETE /api/v1/store/lot-numbers/{id}` | DELETE | `DeleteLotNumberEndpoint.cs` |

**API Commands:**
- `SearchLotNumbersCommand` → Returns `PagedList<LotNumberResponse>`
- `GetLotNumberQuery` → Returns `LotNumberResponse`
- `CreateLotNumberCommand` → Returns `CreateLotNumberResponse`
- `UpdateLotNumberCommand` → Returns success

---

## 15. Serial Numbers (SerialNumbers.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/SerialNumbers.razor`  
**API Base Route:** `/api/v1/store/serial-numbers`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Serial Numbers | `POST /api/v1/store/serial-numbers/search` | POST | `SearchSerialNumbersEndpoint.cs` |
| Get Serial Number | `GET /api/v1/store/serial-numbers/{id}` | GET | `GetSerialNumberEndpoint.cs` |
| Create Serial Number | `POST /api/v1/store/serial-numbers` | POST | `CreateSerialNumberEndpoint.cs` |
| Update Serial Number | `PUT /api/v1/store/serial-numbers/{id}` | PUT | `UpdateSerialNumberEndpoint.cs` |
| Delete Serial Number | `DELETE /api/v1/store/serial-numbers/{id}` | DELETE | `DeleteSerialNumberEndpoint.cs` |

**API Commands:**
- `SearchSerialNumbersCommand` → Returns `PagedList<SerialNumberResponse>`
- `GetSerialNumberQuery` → Returns `SerialNumberResponse`
- `CreateSerialNumberCommand` → Returns `CreateSerialNumberResponse`
- `UpdateSerialNumberCommand` → Returns success

---

## 16. Item Suppliers (ItemSuppliers.razor)

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/ItemSuppliers.razor`  
**API Base Route:** `/api/v1/store/item-suppliers`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Item Suppliers | `POST /api/v1/store/item-suppliers/search` | POST | `SearchItemSuppliersEndpoint.cs` |
| Get Item Supplier | `GET /api/v1/store/item-suppliers/{id}` | GET | `GetItemSupplierEndpoint.cs` |
| Create Item Supplier | `POST /api/v1/store/item-suppliers` | POST | `CreateItemSupplierEndpoint.cs` |
| Update Item Supplier | `PUT /api/v1/store/item-suppliers/{id}` | PUT | `UpdateItemSupplierEndpoint.cs` |
| Delete Item Supplier | `DELETE /api/v1/store/item-suppliers/{id}` | DELETE | `DeleteItemSupplierEndpoint.cs` |

**API Commands:**
- `SearchItemSuppliersCommand` → Returns `PagedList<ItemSupplierResponse>`
- `GetItemSupplierQuery` → Returns `ItemSupplierResponse`
- `CreateItemSupplierCommand` → Returns `CreateItemSupplierResponse`
- `UpdateItemSupplierCommand` → Returns success

---

## 17. Warehouses (Warehouses.razor) ✅ NEW

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/Warehouses.razor`  
**API Base Route:** `/api/v1/store/warehouses`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Warehouses | `POST /api/v1/store/warehouses/search` | POST | `SearchWarehousesEndpoint.cs` |
| Get Warehouse | `GET /api/v1/store/warehouses/{id}` | GET | `GetWarehouseEndpoint.cs` |
| Create Warehouse | `POST /api/v1/store/warehouses` | POST | `CreateWarehouseEndpoint.cs` |
| Update Warehouse | `PUT /api/v1/store/warehouses/{id}` | PUT | `UpdateWarehouseEndpoint.cs` |
| Delete Warehouse | `DELETE /api/v1/store/warehouses/{id}` | DELETE | `DeleteWarehouseEndpoint.cs` |

**API Commands:**
- `SearchWarehousesCommand` → Returns `PagedList<WarehouseResponse>`
- `GetWarehouseQuery` → Returns `WarehouseResponse`
- `CreateWarehouseCommand` → Returns `CreateWarehouseResponse`
- `UpdateWarehouseCommand` → Returns success

**Special Features:**
- Manages warehouse locations, addresses, and capacity
- Tracks warehouse managers and contact information
- Supports warehouse type classification (Standard, Refrigerated, Hazmat, etc.)
- Main warehouse designation flag

---

## 18. Warehouse Locations (WarehouseLocations.razor) ✅ NEW

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/WarehouseLocations.razor`  
**API Base Route:** `/api/v1/store/warehouse-locations`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Locations | `POST /api/v1/store/warehouse-locations/search` | POST | `SearchWarehouseLocationsEndpoint.cs` |
| Get Location | `GET /api/v1/store/warehouse-locations/{id}` | GET | `GetWarehouseLocationEndpoint.cs` |
| Create Location | `POST /api/v1/store/warehouse-locations` | POST | `CreateWarehouseLocationEndpoint.cs` |
| Update Location | `PUT /api/v1/store/warehouse-locations/{id}` | PUT | `UpdateWarehouseLocationEndpoint.cs` |
| Delete Location | `DELETE /api/v1/store/warehouse-locations/{id}` | DELETE | `DeleteWarehouseLocationEndpoint.cs` |

**API Commands:**
- `SearchWarehouseLocationsCommand` → Returns `PagedList<GetWarehouseLocationListResponse>`
- `GetWarehouseLocationQuery` → Returns `GetWarehouseLocationResponse`
- `CreateWarehouseLocationCommand` → Returns `CreateWarehouseLocationResponse`
- `UpdateWarehouseLocationCommand` → Returns success

**Special Features:**
- Hierarchical location structure: Aisle → Section → Shelf → Bin
- Capacity tracking (total and used capacity)
- Temperature control requirements for sensitive items
- Location type classification (Floor, Rack, Shelf, Bin)
- Integration with warehouse parent entity

---

## 19. Cycle Counts (CycleCounts.razor) ✅ NEW

**Blazor Page:** `/src/apps/blazor/client/Pages/Store/CycleCounts.razor`  
**API Base Route:** `/api/v1/store/cycle-counts`

### Endpoint Mappings:

| Blazor Operation | API Endpoint | HTTP Method | API File |
|-----------------|--------------|-------------|----------|
| Search Cycle Counts | `GET /api/v1/store/cycle-counts` | GET | `SearchCycleCountsEndpoint.cs` |
| Get Cycle Count | `GET /api/v1/store/cycle-counts/{id}` | GET | `GetCycleCountEndpoint.cs` |
| Create Cycle Count | `POST /api/v1/store/cycle-counts` | POST | `CreateCycleCountEndpoint.cs` |
| Start Cycle Count | `POST /api/v1/store/cycle-counts/{id}/start` | POST | `StartCycleCountEndpoint.cs` |
| Complete Cycle Count | `POST /api/v1/store/cycle-counts/{id}/complete` | POST | `CompleteCycleCountEndpoint.cs` |
| Reconcile Cycle Count | `POST /api/v1/store/cycle-counts/{id}/reconcile` | POST | `ReconcileCycleCountEndpoint.cs` |
| Add Cycle Count Item | `POST /api/v1/store/cycle-counts/{id}/items` | POST | `AddCycleCountItemEndpoint.cs` |

**API Commands:**
- `SearchCycleCountsCommand` → Returns `PagedList<CycleCountResponse>` (Note: Uses GET with query params)
- `GetCycleCountQuery` → Returns `CycleCountResponse`
- `CreateCycleCountCommand` → Returns `CreateCycleCountResponse`
- `StartCycleCountCommand` → Returns success
- `CompleteCycleCountCommand` → Returns success
- `ReconcileCycleCountCommand` → Returns success and adjusts inventory
- `AddCycleCountItemCommand` → Returns success

**Workflow States:**
- Created → Started → Completed → Reconciled

**Special Features:**
- Variance tracking (counted vs. expected quantities)
- Count type classification (Full, Partial, Spot)
- Scheduled date tracking
- Counter and supervisor assignment
- Progress tracking (total items vs. counted items)
- Inventory reconciliation adjusts stock levels

---

## API Version Information

All endpoints use API version **v1** (specified in the route as `/api/v1/...` or via `MapToApiVersion(1)`).

The Blazor client calls endpoints using:
```csharp
await Client.{EndpointName}Async("1", ...parameters)
```

Where `"1"` represents the API version.

---

## Permission Requirements

Most endpoints require specific permissions from the `Permissions.Store` namespace:

- **View/Search operations:** `Permissions.Store.View`
- **Create operations:** `Permissions.Store.Create`
- **Update operations:** `Permissions.Store.Update`
- **Delete operations:** `Permissions.Store.Delete`
- **Approve operations:** `Permissions.Store.Approve` (or similar workflow permissions)

---

## Common Patterns

### Search Endpoints
All search endpoints follow the pattern:
```
POST /api/v1/store/{entity}/search
```
They accept a search command with pagination parameters and return `PagedList<TResponse>`.

### CRUD Operations
Standard CRUD operations follow RESTful conventions:
- **GET** `/{id}` - Get single entity
- **POST** `/` - Create entity
- **PUT** `/{id}` - Update entity
- **DELETE** `/{id}` - Delete entity

### Workflow Operations
Workflow endpoints use POST with action names:
```
POST /api/v1/store/{entity}/{id}/{action}
```
Examples: `/approve`, `/submit`, `/send`, `/complete`, `/cancel`

### Child Entity Operations
Operations on child entities follow the pattern:
```
POST/PUT/DELETE /api/v1/store/{parent-entity}/{id}/{child-entity}/{childId?}
```

---

## Notes

1. All endpoints are located in: `/src/api/modules/Store/Store.Infrastructure/Endpoints/{Entity}/v1/`
2. Each endpoint typically has a corresponding command/query in the Application layer
3. The Blazor client uses the generated `IClient` interface from NSwag for API calls
4. ViewModels in Blazor pages use Mapster's `Adapt<T>()` to convert between DTOs and commands
5. API responses are adapted back to Blazor DTOs for display

---

## Recently Added Pages ✅

The following pages were recently added to complete the Store module:

1. **Warehouses** (`/store/warehouses`)
   - Full CRUD operations
   - Warehouse management with location and capacity tracking
   - Manager contact information

2. **Warehouse Locations** (`/store/warehouse-locations`)
   - Hierarchical location structure (Aisle/Section/Shelf/Bin)
   - Temperature control support
   - Capacity tracking

3. **Cycle Counts** (`/store/cycle-counts`)
   - Complete workflow support (Create → Start → Complete → Reconcile)
   - Variance tracking
   - Inventory reconciliation

All pages have been added to the navigation menu under **Store** > **Warehouse Management** section.

---

## Recommendations

1. **Enhance existing pages:**
   - ✅ All workflow action buttons implemented for entities that support them
   - Add detail/drill-down views for complex entities (Purchase Orders, Cycle Counts)
   - Implement bulk operations for high-volume tasks
   - Add export functionality for reports

2. **Consistency improvements:**
   - ✅ All pages follow the same `EntityServerTableContext` pattern
   - Standardize error handling and user feedback across all pages
   - Implement consistent permission checks
   - Add loading states and skeleton screens

3. **Testing & Validation:**
   - ✅ All endpoint calls mapped to correct API routes
   - Test workflow transitions in UI (PO approval, Transfer completion, etc.)
   - Validate permission requirements for each page
   - Add integration tests for critical workflows
   - Test with large datasets for performance

4. **Future Enhancements:**
   - Implement dashboard widgets showing key metrics
   - Add real-time notifications for inventory alerts
   - Implement barcode scanning integration for mobile devices
   - Add advanced filtering and saved searches
   - Implement print/PDF generation for documents

---

**Document Version:** 1.0  
**Generated:** October 4, 2025  
**Maintained by:** Development Team
