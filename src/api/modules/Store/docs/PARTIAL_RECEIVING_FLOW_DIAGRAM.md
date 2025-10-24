# Partial Receiving - Visual Flow Diagram

## Complete Transaction Flow with Partial Receipts

```
┌────────────────────────────────────────────────────────────────────────────┐
│                        PURCHASE ORDER CREATED                              │
│  Order #: PO-2025-001                                                      │
│  Supplier: ABC Corp                                                        │
│  Date: Oct 20, 2025                                                        │
│  Status: Draft → Submitted → Approved → Sent                              │
│                                                                             │
│  ┌──────────────────────────────────────────────────────────────────────┐ │
│  │ LINE ITEMS                                                            │ │
│  ├──────────────────────────────────────────────────────────────────────┤ │
│  │ Product A                                                             │ │
│  │ - Ordered Qty: 1000 units                                            │ │
│  │ - Received Qty: 0 units                                              │ │
│  │ - Remaining: 1000 units                                              │ │
│  │ - Unit Price: $50.00                                                 │ │
│  │ - PO Item ID: poi-001                                                │ │
│  └──────────────────────────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ Supplier ships partial delivery
                                    ↓
┌────────────────────────────────────────────────────────────────────────────┐
│                    📦 RECEIPT 1: PARTIAL DELIVERY                          │
│  Date: Oct 24, 2025 10:00 AM                                              │
│  Receipt #: GR-2025-001                                                    │
│  Warehouse: Main Warehouse                                                 │
│  Status: Open                                                              │
└────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ↓
┌────────────────────────────────────────────────────────────────────────────┐
│  USER ACTION: Add Items to Receipt                                         │
│  ┌──────────────────────────────────────────────────────────────────────┐ │
│  │ Product A                                                             │ │
│  │ - Item ID: item-a                                                    │ │
│  │ - PO Item ID: poi-001 ← LINKS TO PO ITEM                            │ │
│  │ - Quantity: 300 units ← PARTIAL (30% of order)                      │ │
│  │ - Unit Cost: $50.00                                                  │ │
│  └──────────────────────────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ↓
┌────────────────────────────────────────────────────────────────────────────┐
│  USER ACTION: Mark Receipt as Received                                     │
│  PUT /api/goods-receipts/gr-001/mark-received                             │
└────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ↓ EVENT: GoodsReceiptCompleted
┌────────────────────────────────────────────────────────────────────────────┐
│                    ⚡ EVENT HANDLER EXECUTES                               │
│  GoodsReceiptCompletedHandler.Handle()                                     │
│                                                                             │
│  STEP 1: Update PO Item Received Quantity                                 │
│  ├─ Load PO Item (poi-001)                                                │
│  ├─ Current ReceivedQuantity: 0                                           │
│  ├─ Add this receipt: +300                                                │
│  ├─ New ReceivedQuantity: 300                                             │
│  └─ Save: PurchaseOrderItem.ReceivedQuantity = 300                        │
│                                                                             │
│  STEP 2: Create Inventory Transaction                                      │
│  ├─ Transaction #: TXN-20251024-0001                                      │
│  ├─ Type: IN                                                              │
│  ├─ Quantity: +300 units                                                  │
│  ├─ Unit Cost: $50.00                                                     │
│  ├─ Total Cost: $15,000                                                   │
│  ├─ Reference: GR-2025-001                                                │
│  ├─ PO Link: PO-2025-001                                                  │
│  └─ Status: Approved (auto)                                               │
│                                                                             │
│  STEP 3: Update Stock Levels                                              │
│  ├─ Item: Product A                                                       │
│  ├─ Warehouse: Main Warehouse                                             │
│  ├─ Previous Quantity: 0                                                  │
│  ├─ Received: +300                                                        │
│  └─ New Quantity: 300                                                     │
│                                                                             │
│  STEP 4: Check PO Completion                                              │
│  ├─ PO Item: Product A                                                    │
│  ├─ Ordered: 1000                                                         │
│  ├─ Received: 300                                                         │
│  ├─ Remaining: 700                                                        │
│  ├─ Percentage: 30.0%                                                     │
│  ├─ Is Complete? NO ❌                                                    │
│  └─ PO Status: REMAINS "Sent" (waiting for more)                         │
│                                                                             │
│  📝 LOG: "PO partially received: 300/1000 (30%). Waiting for remaining."  │
└────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ Current State After Receipt 1
                                    ↓
┌────────────────────────────────────────────────────────────────────────────┐
│  SYSTEM STATE AFTER RECEIPT 1                                              │
│  ┌──────────────────────────────────────────────────────────────────────┐ │
│  │ Purchase Order: PO-2025-001                                           │ │
│  │ Status: Sent ⏳ (Not complete)                                        │ │
│  │                                                                        │ │
│  │ Product A:                                                            │ │
│  │ - Ordered: 1000                                                       │ │
│  │ - Received: 300 ✅                                                    │ │
│  │ - Remaining: 700 ⏳                                                   │ │
│  │ - Progress: [████████░░░░░░░░░░░░░░░░░░░░] 30%                      │ │
│  └──────────────────────────────────────────────────────────────────────┘ │
│                                                                             │
│  ┌──────────────────────────────────────────────────────────────────────┐ │
│  │ Inventory: Product A                                                  │ │
│  │ - Quantity on Hand: 300 units                                        │ │
│  │ - Quantity Available: 300 units                                      │ │
│  └──────────────────────────────────────────────────────────────────────┘ │
│                                                                             │
│  ┌──────────────────────────────────────────────────────────────────────┐ │
│  │ Inventory Transactions                                                │ │
│  │ 1. TXN-20251024-0001: +300 units (GR-2025-001)                       │ │
│  └──────────────────────────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ Supplier ships second delivery
                                    ↓
┌────────────────────────────────────────────────────────────────────────────┐
│                    📦 RECEIPT 2: SECOND PARTIAL                            │
│  Date: Oct 25, 2025 2:00 PM                                               │
│  Receipt #: GR-2025-002                                                    │
│  Warehouse: Main Warehouse                                                 │
│  Status: Open                                                              │
└────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ↓
┌────────────────────────────────────────────────────────────────────────────┐
│  USER ACTION: Check What's Pending                                         │
│  GET /api/goods-receipts/purchase-order/po-id/items-for-receiving        │
│  ┌──────────────────────────────────────────────────────────────────────┐ │
│  │ RESPONSE:                                                             │ │
│  │ {                                                                     │ │
│  │   "items": [{                                                         │ │
│  │     "itemName": "Product A",                                         │ │
│  │     "purchaseOrderItemId": "poi-001",                                │ │
│  │     "orderedQuantity": 1000,                                         │ │
│  │     "receivedQuantity": 300,    ← From receipt 1                    │ │
│  │     "remainingQuantity": 700,   ← Still need this!                  │ │
│  │     "isFullyReceived": false                                         │ │
│  │   }]                                                                  │ │
│  │ }                                                                     │ │
│  └──────────────────────────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ↓
┌────────────────────────────────────────────────────────────────────────────┐
│  USER ACTION: Add Items to Receipt 2                                       │
│  ┌──────────────────────────────────────────────────────────────────────┐ │
│  │ Product A                                                             │ │
│  │ - Item ID: item-a (SAME ITEM)                                        │ │
│  │ - PO Item ID: poi-001 (SAME PO ITEM!)                               │ │
│  │ - Quantity: 700 units ← REMAINING AMOUNT                             │ │
│  │ - Unit Cost: $50.00                                                  │ │
│  └──────────────────────────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ↓
┌────────────────────────────────────────────────────────────────────────────┐
│  USER ACTION: Mark Receipt 2 as Received                                   │
│  PUT /api/goods-receipts/gr-002/mark-received                             │
└────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ↓ EVENT: GoodsReceiptCompleted
┌────────────────────────────────────────────────────────────────────────────┐
│                    ⚡ EVENT HANDLER EXECUTES AGAIN                         │
│  GoodsReceiptCompletedHandler.Handle()                                     │
│                                                                             │
│  STEP 1: Update PO Item Received Quantity                                 │
│  ├─ Load PO Item (poi-001)                                                │
│  ├─ Current ReceivedQuantity: 300                                         │
│  ├─ Add this receipt: +700                                                │
│  ├─ New ReceivedQuantity: 1000                                            │
│  └─ Save: PurchaseOrderItem.ReceivedQuantity = 1000                       │
│                                                                             │
│  STEP 2: Create Inventory Transaction                                      │
│  ├─ Transaction #: TXN-20251025-0001                                      │
│  ├─ Type: IN                                                              │
│  ├─ Quantity: +700 units                                                  │
│  ├─ Unit Cost: $50.00                                                     │
│  ├─ Total Cost: $35,000                                                   │
│  ├─ Reference: GR-2025-002                                                │
│  ├─ PO Link: PO-2025-001                                                  │
│  └─ Status: Approved (auto)                                               │
│                                                                             │
│  STEP 3: Update Stock Levels                                              │
│  ├─ Item: Product A                                                       │
│  ├─ Warehouse: Main Warehouse                                             │
│  ├─ Previous Quantity: 300                                                │
│  ├─ Received: +700                                                        │
│  └─ New Quantity: 1000                                                    │
│                                                                             │
│  STEP 4: Check PO Completion                                              │
│  ├─ PO Item: Product A                                                    │
│  ├─ Ordered: 1000                                                         │
│  ├─ Received: 1000                                                        │
│  ├─ Remaining: 0                                                          │
│  ├─ Percentage: 100.0%                                                    │
│  ├─ Is Complete? YES ✅                                                   │
│  ├─ PO Status: Sent → Received ✅                                        │
│  └─ ActualDeliveryDate: Oct 25, 2025 2:00 PM                             │
│                                                                             │
│  📝 LOG: "PO fully received! All 1 items complete."                        │
└────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    │ Final State
                                    ↓
┌────────────────────────────────────────────────────────────────────────────┐
│  ✅ FINAL SYSTEM STATE - PO COMPLETE                                       │
│  ┌──────────────────────────────────────────────────────────────────────┐ │
│  │ Purchase Order: PO-2025-001                                           │ │
│  │ Status: Received ✅ (Complete!)                                       │ │
│  │ Actual Delivery Date: Oct 25, 2025                                   │ │
│  │                                                                        │ │
│  │ Product A:                                                            │ │
│  │ - Ordered: 1000                                                       │ │
│  │ - Received: 1000 ✅                                                   │ │
│  │ - Remaining: 0                                                        │ │
│  │ - Progress: [████████████████████████████████] 100% ✅              │ │
│  │ - Receipts: 2 (partial deliveries)                                   │ │
│  └──────────────────────────────────────────────────────────────────────┘ │
│                                                                             │
│  ┌──────────────────────────────────────────────────────────────────────┐ │
│  │ Inventory: Product A                                                  │ │
│  │ - Quantity on Hand: 1000 units ✅                                    │ │
│  │ - Quantity Available: 1000 units                                     │ │
│  │ - Last Movement: Oct 25, 2025                                        │ │
│  └──────────────────────────────────────────────────────────────────────┘ │
│                                                                             │
│  ┌──────────────────────────────────────────────────────────────────────┐ │
│  │ Inventory Transactions (Audit Trail)                                  │ │
│  │ 1. TXN-20251024-0001: +300 units (GR-2025-001) Oct 24               │ │
│  │ 2. TXN-20251025-0001: +700 units (GR-2025-002) Oct 25               │ │
│  │                                                                        │ │
│  │ Total Inventory Value: $50,000                                       │ │
│  │ (300 × $50 + 700 × $50)                                              │ │
│  └──────────────────────────────────────────────────────────────────────┘ │
│                                                                             │
│  ┌──────────────────────────────────────────────────────────────────────┐ │
│  │ Goods Receipts (Receipt History)                                      │ │
│  │ 1. GR-2025-001: 300 units (Oct 24) - Partial                        │ │
│  │ 2. GR-2025-002: 700 units (Oct 25) - Final                          │ │
│  └──────────────────────────────────────────────────────────────────────┘ │
└────────────────────────────────────────────────────────────────────────────┘

KEY INSIGHTS:

1. 🔗 LINKING: Each GoodsReceiptItem links to PurchaseOrderItem via ID
2. 📊 TRACKING: ReceivedQuantity updates cumulatively (0 → 300 → 1000)
3. ✅ COMPLETION: PO only "Received" when ALL items 100% complete
4. 📦 MULTIPLE RECEIPTS: Unlimited partial receipts per PO
5. 💰 INVENTORY: Each receipt creates separate inventory transaction
6. 📝 AUDIT: Complete trail of all partial deliveries
7. 🚫 VALIDATION: Cannot over-receive (exceeding ordered quantity blocked)
8. 🔍 VISIBILITY: Query endpoint shows pending quantities

RESULT: Full support for real-world partial receiving scenarios! ✅
```

