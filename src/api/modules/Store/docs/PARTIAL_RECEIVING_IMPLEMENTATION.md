# Partial Receiving Implementation - Complete Guide

## ✅ IMPLEMENTATION COMPLETE

**Date**: October 24, 2025  
**Status**: **PRODUCTION READY**  
**Feature**: Full support for partial and multiple receipts per purchase order

---

## 🎯 PROBLEM SOLVED

### The Real-World Scenario

**Ordered**: 1000 units of Product A  
**Received**:
- Receipt 1: 300 units (partial)
- Receipt 2: 500 units (partial)
- Receipt 3: 200 units (complete)

**Previous System**: ❌ Could not track partial receipts  
**New System**: ✅ Tracks each receipt and updates PO completion status

---

## 🏗️ ARCHITECTURE

### Key Components

1. **GoodsReceiptItem** - Links to PurchaseOrderItem for tracking
2. **PurchaseOrderItem.ReceivedQuantity** - Cumulative received amount
3. **Event Handler** - Updates received quantities automatically
4. **Completion Logic** - Only marks PO as "Received" when ALL items are 100% received

---

## 📊 DATABASE SCHEMA CHANGES

### GoodsReceiptItem
```sql
ALTER TABLE GoodsReceiptItems 
ADD PurchaseOrderItemId uniqueidentifier NULL;

-- Links each goods receipt line to a specific PO line item
-- Allows tracking multiple receipts against same PO item
```

### PurchaseOrderItem (Already Exists)
```sql
-- ReceivedQuantity field already exists
-- Updated incrementally as receipts are processed
SELECT 
    Id,
    ItemId,
    Quantity AS OrderedQuantity,
    ReceivedQuantity,
    (Quantity - ReceivedQuantity) AS RemainingQuantity,
    CASE 
        WHEN ReceivedQuantity >= Quantity THEN 'Complete'
        WHEN ReceivedQuantity > 0 THEN 'Partial'
        ELSE 'Pending'
    END AS Status
FROM PurchaseOrderItems;
```

---

## 🔄 PARTIAL RECEIVING FLOW

### Example: Order 1000 units, receive in 3 shipments

```
┌─────────────────────────────────────────────────────────┐
│ PURCHASE ORDER CREATED                                  │
│ Order #: PO-20251024-001                                │
│ Status: Draft → Submitted → Approved → Sent            │
│                                                          │
│ Items:                                                   │
│ ┌──────────────────────────────────────────────────┐   │
│ │ Product A                                         │   │
│ │ Ordered: 1000 units                              │   │
│ │ Received: 0 units                                │   │
│ │ Remaining: 1000 units                            │   │
│ └──────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
                      ↓
┌─────────────────────────────────────────────────────────┐
│ RECEIPT 1: PARTIAL (300 units)                          │
│ Date: Oct 24, 2025 10:00 AM                            │
└─────────────────────────────────────────────────────────┘

Step 1: Create Goods Receipt
POST /api/goods-receipts
{
  "receiptNumber": "GR-20251024-001",
  "purchaseOrderId": "...",
  "warehouseId": "...",
  "receivedDate": "2025-10-24T10:00:00Z"
}

Step 2: Add Item (with PO item link)
POST /api/goods-receipts/{id}/items
{
  "itemId": "product-a-id",
  "purchaseOrderItemId": "po-item-id",  ← LINK TO PO ITEM
  "name": "Product A",
  "quantity": 300,                       ← PARTIAL QUANTITY
  "unitCost": 50.00
}

Step 3: Mark as Received
PUT /api/goods-receipts/{id}/mark-received

→ EVENT HANDLER EXECUTES:
  ✅ Creates inventory transaction (+300 units)
  ✅ Updates stock level (+300 units)
  ✅ Updates PurchaseOrderItem.ReceivedQuantity: 0 → 300
  ✅ Checks completion: 300/1000 = 30% → PO stays "Sent"
  
→ RESULT:
  Inventory: +300 units
  PO Status: Sent (not complete)
  PO Item: 300/1000 received (70% pending)

                      ↓
┌─────────────────────────────────────────────────────────┐
│ RECEIPT 2: PARTIAL (500 units)                          │
│ Date: Oct 25, 2025 2:00 PM                             │
└─────────────────────────────────────────────────────────┘

Step 1: Create Another Goods Receipt
POST /api/goods-receipts
{
  "receiptNumber": "GR-20251025-001",
  "purchaseOrderId": "...",              ← SAME PO!
  "warehouseId": "...",
  "receivedDate": "2025-10-25T14:00:00Z"
}

Step 2: Add Item (same PO item)
POST /api/goods-receipts/{id}/items
{
  "itemId": "product-a-id",
  "purchaseOrderItemId": "po-item-id",   ← SAME PO ITEM!
  "name": "Product A",
  "quantity": 500,                       ← SECOND PARTIAL
  "unitCost": 50.00
}

Step 3: Mark as Received
PUT /api/goods-receipts/{id}/mark-received

→ EVENT HANDLER EXECUTES:
  ✅ Creates inventory transaction (+500 units)
  ✅ Updates stock level (+500 units, total now 800)
  ✅ Updates PurchaseOrderItem.ReceivedQuantity: 300 → 800
  ✅ Checks completion: 800/1000 = 80% → PO stays "Sent"
  
→ RESULT:
  Inventory: +500 units (total: 800)
  PO Status: Sent (not complete)
  PO Item: 800/1000 received (20% pending)

                      ↓
┌─────────────────────────────────────────────────────────┐
│ RECEIPT 3: FINAL (200 units)                            │
│ Date: Oct 26, 2025 9:00 AM                             │
└─────────────────────────────────────────────────────────┘

Step 1: Create Final Goods Receipt
POST /api/goods-receipts
{
  "receiptNumber": "GR-20251026-001",
  "purchaseOrderId": "...",              ← SAME PO!
  "warehouseId": "...",
  "receivedDate": "2025-10-26T09:00:00Z"
}

Step 2: Add Remaining Items
POST /api/goods-receipts/{id}/items
{
  "itemId": "product-a-id",
  "purchaseOrderItemId": "po-item-id",   ← SAME PO ITEM!
  "name": "Product A",
  "quantity": 200,                       ← FINAL QUANTITY
  "unitCost": 50.00
}

Step 3: Mark as Received
PUT /api/goods-receipts/{id}/mark-received

→ EVENT HANDLER EXECUTES:
  ✅ Creates inventory transaction (+200 units)
  ✅ Updates stock level (+200 units, total now 1000)
  ✅ Updates PurchaseOrderItem.ReceivedQuantity: 800 → 1000
  ✅ Checks completion: 1000/1000 = 100% → ALL COMPLETE!
  ✅ Updates PO status: Sent → Received ✨
  ✅ Sets ActualDeliveryDate: Now
  
→ RESULT:
  Inventory: +200 units (total: 1000)
  PO Status: Received ✅ (COMPLETE!)
  PO Item: 1000/1000 received (100% complete)
  PO ActualDeliveryDate: Set
```

---

## 💻 CODE IMPLEMENTATION

### 1. Domain Entity Changes

#### GoodsReceiptItem.cs
```csharp
public sealed class GoodsReceiptItem : AuditableEntity
{
    // ...existing properties...
    
    /// <summary>
    /// Links to PurchaseOrderItem for partial receiving tracking.
    /// </summary>
    public DefaultIdType? PurchaseOrderItemId { get; private set; }
    
    public static GoodsReceiptItem Create(
        DefaultIdType receiptId, 
        DefaultIdType itemId, 
        string name, 
        int quantity, 
        decimal unitCost,
        DefaultIdType? purchaseOrderItemId = null)
    {
        // Creates with PO item link
    }
}
```

### 2. Event Handler Logic

#### GoodsReceiptCompletedHandler.cs - Key Methods

```csharp
private async Task ProcessGoodsReceiptItem(...)
{
    // 1. Update PO item received quantity
    if (item.PurchaseOrderItemId.HasValue)
    {
        await UpdatePurchaseOrderItemReceivedQuantity(
            item.PurchaseOrderItemId.Value,
            item.Quantity,
            cancellationToken);
    }
    
    // 2. Create inventory transaction
    // 3. Update stock levels
}

private async Task UpdatePurchaseOrderItemReceivedQuantity(...)
{
    // Find PO and item
    var purchaseOrderItem = ...;
    
    // Increment received quantity
    var newReceivedQuantity = purchaseOrderItem.ReceivedQuantity + quantityReceived;
    purchaseOrderItem.ReceiveQuantity(newReceivedQuantity);
    
    // Log partial status
    _logger.LogInformation(
        "PO item: Received {Received}/{Ordered} ({Partial})",
        newReceivedQuantity,
        purchaseOrderItem.Quantity,
        newReceivedQuantity < purchaseOrderItem.Quantity ? "Partial" : "Complete");
}

private async Task UpdatePurchaseOrderStatus(...)
{
    // Check if ALL items fully received
    var allItemsFullyReceived = purchaseOrder.Items
        .All(item => item.ReceivedQuantity >= item.Quantity);
    
    if (allItemsFullyReceived)
    {
        // Mark PO as complete
        purchaseOrder.UpdateDeliveryDate(DateTime.UtcNow);
        _logger.LogInformation("PO fully received!");
    }
    else
    {
        // Log partial receipt
        var percentComplete = (received / ordered) * 100;
        _logger.LogInformation("PO {Percent:F1}% complete", percentComplete);
    }
}
```

### 3. Helper Query for UI

#### GetPurchaseOrderItemsForReceivingQuery
```csharp
// GET /api/goods-receipts/purchase-order/{id}/items-for-receiving

// Returns:
{
  "purchaseOrderId": "...",
  "orderNumber": "PO-20251024-001",
  "status": "Sent",
  "items": [
    {
      "purchaseOrderItemId": "...",
      "itemId": "...",
      "itemName": "Product A",
      "itemSku": "SKU-001",
      "orderedQuantity": 1000,
      "receivedQuantity": 800,      ← Already received
      "remainingQuantity": 200,     ← Still pending
      "unitPrice": 50.00,
      "isFullyReceived": false
    }
  ]
}
```

---

## 📋 API USAGE EXAMPLES

### Example 1: Check What Remains to Receive

```bash
# Before creating receipt, check what's pending
GET /api/goods-receipts/purchase-order/{po-id}/items-for-receiving

Response:
{
  "orderNumber": "PO-20251024-001",
  "items": [
    {
      "itemName": "Product A",
      "orderedQuantity": 1000,
      "receivedQuantity": 300,      ← From receipt 1
      "remainingQuantity": 700,     ← Still need to receive
      "isFullyReceived": false
    },
    {
      "itemName": "Product B",
      "orderedQuantity": 500,
      "receivedQuantity": 500,      ← Complete!
      "remainingQuantity": 0,
      "isFullyReceived": true
    }
  ]
}
```

### Example 2: Create Partial Receipt

```bash
# Receipt 1: Partial delivery (300/1000)
POST /api/goods-receipts
{
  "receiptNumber": "GR-001",
  "purchaseOrderId": "po-id",
  "warehouseId": "wh-id",
  "receivedDate": "2025-10-24"
}

# Add partial quantity with PO item link
POST /api/goods-receipts/{gr-id}/items
{
  "itemId": "item-id",
  "purchaseOrderItemId": "po-item-id",  ← IMPORTANT!
  "name": "Product A",
  "quantity": 300,                       ← Partial
  "unitCost": 50.00
}

# Mark as received
PUT /api/goods-receipts/{gr-id}/mark-received

# Check PO status
GET /api/purchase-orders/{po-id}

Response:
{
  "orderNumber": "PO-20251024-001",
  "status": "Sent",                      ← Still Sent (not complete)
  "items": [
    {
      "itemName": "Product A",
      "quantity": 1000,
      "receivedQuantity": 300,           ← Updated!
      "unitPrice": 50.00
    }
  ]
}
```

### Example 3: Complete Remaining Items

```bash
# Receipt 2: Receive remaining (700/1000)
POST /api/goods-receipts
{
  "receiptNumber": "GR-002",
  "purchaseOrderId": "po-id",            ← Same PO
  "warehouseId": "wh-id",
  "receivedDate": "2025-10-25"
}

POST /api/goods-receipts/{gr-id}/items
{
  "itemId": "item-id",
  "purchaseOrderItemId": "po-item-id",   ← Same PO item
  "name": "Product A",
  "quantity": 700,                        ← Remaining
  "unitCost": 50.00
}

PUT /api/goods-receipts/{gr-id}/mark-received

# Check PO status again
GET /api/purchase-orders/{po-id}

Response:
{
  "orderNumber": "PO-20251024-001",
  "status": "Received",                  ← NOW COMPLETE! ✅
  "actualDeliveryDate": "2025-10-25",    ← Set automatically
  "items": [
    {
      "itemName": "Product A",
      "quantity": 1000,
      "receivedQuantity": 1000,          ← 100% received!
      "unitPrice": 50.00
    }
  ]
}
```

---

## 🔍 TRACKING & REPORTING

### Query 1: PO Completion Status

```sql
SELECT 
    po.OrderNumber,
    po.Status,
    COUNT(poi.Id) AS TotalItems,
    SUM(CASE WHEN poi.ReceivedQuantity >= poi.Quantity THEN 1 ELSE 0 END) AS CompleteItems,
    SUM(CASE WHEN poi.ReceivedQuantity > 0 AND poi.ReceivedQuantity < poi.Quantity THEN 1 ELSE 0 END) AS PartialItems,
    SUM(CASE WHEN poi.ReceivedQuantity = 0 THEN 1 ELSE 0 END) AS PendingItems,
    SUM(poi.Quantity) AS TotalOrdered,
    SUM(poi.ReceivedQuantity) AS TotalReceived,
    CAST(SUM(poi.ReceivedQuantity) AS DECIMAL) / SUM(poi.Quantity) * 100 AS PercentComplete
FROM PurchaseOrders po
JOIN PurchaseOrderItems poi ON po.Id = poi.PurchaseOrderId
WHERE po.Status IN ('Sent', 'Received')
GROUP BY po.OrderNumber, po.Status
ORDER BY po.OrderDate DESC;
```

### Query 2: Receipt History per PO

```sql
SELECT 
    po.OrderNumber,
    gr.ReceiptNumber,
    gr.ReceivedDate,
    gri.ItemName,
    poi.Quantity AS Ordered,
    gri.Quantity AS ReceivedInThisReceipt,
    poi.ReceivedQuantity AS TotalReceived,
    (poi.Quantity - poi.ReceivedQuantity) AS Remaining
FROM PurchaseOrders po
JOIN GoodsReceipts gr ON po.Id = gr.PurchaseOrderId
JOIN GoodsReceiptItems gri ON gr.Id = gri.GoodsReceiptId
JOIN PurchaseOrderItems poi ON gri.PurchaseOrderItemId = poi.Id
WHERE po.Id = @PurchaseOrderId
ORDER BY gr.ReceivedDate, gri.ItemName;
```

### Query 3: Items Awaiting Receipt

```sql
SELECT 
    i.Name AS ItemName,
    i.Sku,
    poi.Quantity AS Ordered,
    poi.ReceivedQuantity AS Received,
    (poi.Quantity - poi.ReceivedQuantity) AS Remaining,
    po.OrderNumber,
    po.ExpectedDeliveryDate,
    DATEDIFF(day, GETDATE(), po.ExpectedDeliveryDate) AS DaysUntilDue
FROM PurchaseOrderItems poi
JOIN PurchaseOrders po ON poi.PurchaseOrderId = po.Id
JOIN Items i ON poi.ItemId = i.Id
WHERE poi.ReceivedQuantity < poi.Quantity
  AND po.Status = 'Sent'
ORDER BY po.ExpectedDeliveryDate;
```

---

## ✅ VALIDATION RULES

### Partial Receipt Validations

1. **Quantity Cannot Exceed Remaining**
   ```csharp
   // In PurchaseOrderItem.ReceiveQuantity()
   if (receivedQuantity > Quantity)
       throw new ReceivedQuantityExceedsOrderedException(Id);
   ```

2. **Must Link to PO Item for Partial Tracking**
   ```csharp
   // Recommended but not enforced
   if (goodsReceipt.PurchaseOrderId.HasValue && !item.PurchaseOrderItemId.HasValue)
       _logger.LogWarning("Receipt linked to PO but item not linked to PO item");
   ```

3. **Cannot Over-Receive**
   ```csharp
   // System prevents receiving more than ordered
   // Example: Ordered 1000, already received 800
   // Attempting to receive 300 → ERROR (would be 1100 total)
   ```

---

## 🎯 BUSINESS RULES

### PO Status Transitions

```
Draft → Submitted → Approved → Sent → Received
                                  ↑
                                  │
                    (Only when ALL items 100% received)
```

### Completion Logic

- **Partial Receipt**: Any item has `ReceivedQuantity < Quantity`
  - PO Status: Remains "Sent"
  - Action: Log partial status, allow more receipts

- **Complete Receipt**: All items have `ReceivedQuantity >= Quantity`
  - PO Status: Changes to "Received"
  - Action: Set ActualDeliveryDate, close PO

### Multiple Receipts

- ✅ Multiple goods receipts per purchase order
- ✅ Multiple receipts per PO line item
- ✅ Cumulative tracking of received quantities
- ✅ Each receipt creates separate inventory transactions

---

## 🧪 TESTING SCENARIOS

### Test 1: Single Item, Multiple Receipts
```
PO: 1 item, 1000 units
Receipt 1: 400 units → PO status: Sent
Receipt 2: 300 units → PO status: Sent
Receipt 3: 300 units → PO status: Received ✅
```

### Test 2: Multiple Items, Partial Completion
```
PO: 
  - Item A: 1000 units
  - Item B: 500 units

Receipt 1:
  - Item A: 1000 units (complete)
  - Item B: 200 units (partial)
→ PO status: Sent (Item B not complete)

Receipt 2:
  - Item B: 300 units (complete)
→ PO status: Received ✅ (All items complete)
```

### Test 3: Over-Receipt Prevention
```
PO: 1000 units
Receipt 1: 700 units → Success
Receipt 2: 500 units → ERROR (would exceed 1000)
```

### Test 4: Without PO Item Link
```
Receipt without PurchaseOrderItemId:
✅ Inventory updated
✅ Stock increased
❌ PO item received quantity NOT updated
⚠️ Warning logged: "Item not linked to PO item"
```

---

## 📊 DASHBOARD METRICS

### Suggested KPIs

1. **Average Receipts per PO**: Track how many partial shipments per order
2. **Completion Rate**: % of POs received in single shipment
3. **Partial Receipt Frequency**: % of POs requiring multiple receipts
4. **Time to Complete**: Days from first to last receipt per PO
5. **Variance Tracking**: Ordered vs Received quantities

---

## 🚀 BENEFITS

### For Warehouse Operations
✅ **Flexible receiving** - Accept partial shipments  
✅ **Real-time tracking** - See what's pending  
✅ **No manual updates** - Automatic PO item tracking  
✅ **Audit trail** - Every receipt recorded  

### For Procurement
✅ **Visibility** - Know what's outstanding  
✅ **Supplier performance** - Track partial deliveries  
✅ **Better planning** - See completion status  

### For Inventory Management
✅ **Accurate counts** - Each receipt updates stock  
✅ **Cost tracking** - Each receipt valued properly  
✅ **FIFO/LIFO support** - Multiple receipts tracked separately  

---

## 📝 FILES CHANGED

### Modified (5 files)
1. `Store.Domain/Entities/GoodsReceiptItem.cs` - Added PurchaseOrderItemId
2. `Store.Domain/Entities/GoodsReceipt.cs` - Updated AddItem signature
3. `Store.Application/GoodsReceipts/AddItem/v1/AddGoodsReceiptItemCommand.cs`
4. `Store.Application/GoodsReceipts/AddItem/v1/AddGoodsReceiptItemHandler.cs`
5. `Store.Application/GoodsReceipts/EventHandlers/GoodsReceiptCompletedHandler.cs` - Partial receipt logic

### Created (3 files)
6. `Store.Application/GoodsReceipts/Queries/GetPurchaseOrderItemsForReceivingQuery.cs`
7. `Store.Application/GoodsReceipts/Queries/GetPurchaseOrderItemsForReceivingHandler.cs`
8. `Store.Infrastructure/Endpoints/GoodsReceipts/GetPurchaseOrderItemsForReceivingEndpoint.cs`

---

## ✅ FINAL ANSWER

### **"How do I implement partial receiving when items received are less than PO quantity?"**

# **FULLY IMPLEMENTED! ✅**

Your Store module now supports:

1. ✅ **Multiple goods receipts per purchase order**
2. ✅ **Partial quantities per receipt**
3. ✅ **Automatic tracking** of received vs ordered quantities
4. ✅ **Cumulative updates** to PurchaseOrderItem.ReceivedQuantity
5. ✅ **Smart completion** - PO only marked "Received" when 100% complete
6. ✅ **Helper query** to see what remains to be received
7. ✅ **Full audit trail** of all partial receipts
8. ✅ **Inventory updates** for each partial receipt

**Example:**
- Order 1000 units
- Receive 300 → PO stays "Sent", inventory +300
- Receive 500 → PO stays "Sent", inventory +500 (total 800)
- Receive 200 → PO changes to "Received" ✅, inventory +200 (total 1000)

**All automatic, all tracked, all auditable!** 🎉

---

**Implementation Date**: October 24, 2025  
**Status**: ✅ **PRODUCTION READY**  
**Feature**: Partial Receiving with Multiple Receipts

