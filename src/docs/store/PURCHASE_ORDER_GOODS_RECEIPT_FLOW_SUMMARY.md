# 📦 Purchase Order to Goods Receipt - Quick Reference

**Visual Transaction Flow Guide**

---

## 🎯 At a Glance

```
PURCHASE ORDER                    GOODS RECEIPT                    INVENTORY
──────────────                   ──────────────                   ─────────
                                                                   
1. Create PO (Draft)                                              
   └─ Add Items                                                    
                                                                   
2. Submit → Approve                                               
                                                                   
3. Send to Supplier                                               
   (Status: Sent) ────────┐                                       
                          │                                       
   [Supplier delivers] ───┘                                       
                          │                                       
                          └──→ 4. Create GR from PO              
                                  ├─ Select PO                    
                                  ├─ Choose Items                 
                                  └─ Set Quantities               
                                                                   
                               5. Verify & Inspect                
                                  ├─ Count actual qty             
                                  ├─ Check quality                
                                  └─ Record lot #s                
                                                                   
                               6. Mark as Received                
                                  (Status: Received)              
   (Status: PartiallyReceived                                     
    or Received) ←───────────────┘                                
                                                                   
                               7. Post to Inventory ────────────→ Stock levels
                                  (Status: Posted)                 updated ✓
                                                                   
   [All items received]                                            Available
   (Status: Received) ✓                                            for use ✓
```

---

## 📋 Two Receipt Options

### Option A: Full Receipt
**Use when:** Supplier delivered complete order

```
┌─────────────────────────────────────┐
│  Purchase Order PO-2025-11-001      │
│  ├─ Item A: 100 units              │
│  ├─ Item B: 50 units               │
│  └─ Item C: 200 units              │
└─────────────────────────────────────┘
           │
           ▼
┌─────────────────────────────────────┐
│  Goods Receipt GR-001               │
│  ✓ Receive ALL items                │
│  ├─ Item A: 100 units ✓             │
│  ├─ Item B: 50 units ✓              │
│  └─ Item C: 200 units ✓             │
└─────────────────────────────────────┘
           │
           ▼
    PO Status: RECEIVED ✓
    (Complete)
```

### Option B: Partial Receipt
**Use when:** Partial delivery, backorder, or issues

```
┌─────────────────────────────────────┐
│  Purchase Order PO-2025-11-001      │
│  ├─ Item A: 100 units              │
│  ├─ Item B: 50 units               │
│  └─ Item C: 200 units              │
└─────────────────────────────────────┘
           │
           ▼
┌─────────────────────────────────────┐
│  Goods Receipt GR-001               │
│  Partial Delivery 1                 │
│  ✓ Item A: 100 units ✓              │
│  ✗ Item B: Not received             │
│  ✓ Item C: 100 units (partial)      │
└─────────────────────────────────────┘
           │
           ▼
    PO Status: PARTIALLY RECEIVED
    (100 units of B, 100 units of C pending)
           │
           ▼
┌─────────────────────────────────────┐
│  Goods Receipt GR-002               │
│  Partial Delivery 2                 │
│  ✓ Item B: 50 units ✓               │
│  ✓ Item C: 100 units ✓              │
└─────────────────────────────────────┘
           │
           ▼
    PO Status: RECEIVED ✓
    (Complete)
```

---

## ⚡ Quick Steps

### Creating Receipt from PO

**5-Step Process:**

1. **Navigate:** Store → Goods Receipts → "Create from Purchase Order"

2. **Select PO:** Choose from list (Status: Sent or PartiallyReceived)

3. **Choose Items:**
   - ✓ Check items to receive
   - ✗ Uncheck items not delivered
   - Adjust quantities to actual received

4. **Fill Header:**
   - Receipt #: Auto-generated
   - Date: Current date/time
   - Warehouse: Select destination
   - Notes: Document delivery details

5. **Save:** Click "Create Receipt"

**Result:** Receipt created in Draft status

---

### Finalizing Receipt

**3-Step Process:**

1. **Inspect:** 
   - Count actual quantities
   - Check quality
   - Record lot/serial numbers
   - Note any issues

2. **Mark Received:**
   - Click Menu → "Mark as Received"
   - Confirms quantities
   - Locks receipt
   - Updates PO status

3. **Post to Inventory:**
   - Click Menu → "Post to Inventory"
   - Updates stock levels
   - Makes goods available
   - Creates inventory transactions

**Result:** Goods in active inventory ✓

---

## 🚦 Status Indicators

### Purchase Order Status

| Status | Meaning | Next Action |
|--------|---------|-------------|
| 🔵 Draft | Being created | Add items, submit |
| 🟡 Submitted | Awaiting approval | Manager approves |
| 🟢 Approved | Ready to send | Send to supplier |
| 📤 Sent | Awaiting delivery | **CREATE RECEIPT** |
| 📦 PartiallyReceived | Some items received | Receive remaining |
| ✅ Received | All items received | Close/archive |
| ❌ Cancelled | Order cancelled | N/A |

### Goods Receipt Status

| Status | Meaning | Next Action |
|--------|---------|-------------|
| 🔵 Draft | Being created | Inspect, verify |
| ✅ Received | Finalized | **POST TO INVENTORY** |
| 📊 Posted | Inventory updated | Complete ✓ |
| ❌ Cancelled | Receipt voided | N/A |

---

## 📊 Quantity Tracking Example

```
Purchase Order Line: Item A
┌──────────────────────────────────────────┐
│ Ordered:    100 units                    │
├──────────────────────────────────────────┤
│ Receipt 1:   +30 units (Nov 10)          │
│ Receipt 2:   +50 units (Nov 15)          │
│ Receipt 3:   +20 units (Nov 20)          │
├──────────────────────────────────────────┤
│ Total Received:  100 units ✓             │
│ Remaining:       0 units                 │
│ Status:          COMPLETE                │
└──────────────────────────────────────────┘
```

---

## 🎓 Key Rules

### ✅ DO

- ✓ Create separate receipt for each physical delivery
- ✓ Inspect goods immediately upon arrival
- ✓ Document all variances and issues
- ✓ Record lot/serial numbers for traceability
- ✓ Process receipts same day
- ✓ Communicate discrepancies to purchasing

### ❌ DON'T

- ✗ Receive against PO not in "Sent" status
- ✗ Accept goods without inspection
- ✗ Exceed ordered quantities without approval
- ✗ Leave receipts in Draft status overnight
- ✗ Post damaged goods to inventory
- ✗ Skip documentation

---

## 🔧 Common Scenarios

| Scenario | Solution |
|----------|----------|
| **Partial Delivery** | Create receipt with actual qty, note backorder |
| **Damaged Goods** | Record qty rejected, note damage, initiate claim |
| **Wrong Items** | Reject delivery, contact supplier, do not receipt |
| **Over-Delivery** | Accept or reject extra units, document decision |
| **Multiple Shipments** | Create separate receipt for each delivery |

---

## 📞 Quick Help

**Cannot create receipt?**
- Check PO status (must be "Sent" or "PartiallyReceived")
- Verify items have remaining quantity > 0

**Inventory not updating?**
- Ensure receipt is marked "Received"
- Click "Post to Inventory"
- Check stock level exists for item/warehouse

**Need to undo receipt?**
- If Draft: Edit directly
- If Received: Cancel with reason, create new receipt
- If Posted: Requires inventory adjustment

---

## 📚 Full Documentation

For complete details, see:
- **PURCHASE_ORDER_TO_GOODS_RECEIPT_FLOW.md** - Comprehensive guide
- **PURCHASE_ORDERS_USER_GUIDE.md** - PO lifecycle details
- **GOODS_RECEIPT_IMPLEMENTATION.md** - Technical API docs

---

**Last Updated:** November 10, 2025  
**Quick Ref Version:** 1.0

