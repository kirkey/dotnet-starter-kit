# Purchase Order to Goods Receipt - Complete Transaction Flow

**Last Updated:** November 10, 2025  
**Module:** Store Management  
**Process:** Procurement to Inventory Receiving

---

## 📋 Executive Summary

This document describes the complete end-to-end process for managing purchase orders through to goods receipt and inventory update in the Store Management module. It covers the proper workflow, validation rules, and best practices for receiving inventory.

---

## 🔄 Complete Transaction Lifecycle

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    PURCHASE ORDER TO GOODS RECEIPT FLOW                     │
└─────────────────────────────────────────────────────────────────────────────┘

PROCUREMENT PHASE (Purchase Orders)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
1. CREATE Purchase Order (Draft)
   └─ Enter supplier, delivery date, contact info
   
2. ADD ITEMS (Draft)
   └─ Select items, quantities, unit prices
   
3. SUBMIT (Draft → Submitted)
   └─ Lock order, send for approval
   
4. APPROVE (Submitted → Approved)
   └─ Manager approves order
   
5. SEND TO SUPPLIER (Approved → Sent)
   └─ Generate PDF, email to supplier
   └─ STATUS: "Sent" (Ready for receiving)

RECEIVING PHASE (Goods Receipts)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
6. CREATE Goods Receipt from PO
   ├─ Option A: Receive All Items (Full Receipt)
   └─ Option B: Receive Selected Items (Partial Receipt)
   
7. VERIFY & INSPECT Quantities
   ├─ Check actual qty vs ordered qty
   ├─ Record lot/serial numbers
   ├─ Perform quality inspection
   └─ Note any variances or damages
   
8. MARK AS RECEIVED (Draft → Received)
   └─ Finalize receipt, update PO status
   └─ PO STATUS: "PartiallyReceived" or "Received"
   
9. POST TO INVENTORY (Received → Posted)
   └─ Inventory levels updated automatically
   └─ Stock available for use

COMPLETION
━━━━━━━━━━
10. CLOSE Purchase Order
    └─ When all items fully received
    └─ PO STATUS: "Received" (Complete)
```

---

## 📖 Detailed Transaction Steps

### Phase 1: Purchase Order Management

#### Step 1: Create Purchase Order (Status: Draft)

**Location:** Store → Purchase Orders → Add

**Required Information:**
- **Order Number**: Unique identifier (e.g., `PO-2025-11-001`)
- **Supplier**: Select the vendor supplying the goods
- **Order Date**: Date the order is placed
- **Expected Delivery Date**: When goods should arrive (optional)

**Additional Fields:**
- Delivery Address
- Contact Person & Phone
- Special Instructions in Notes
- Is Urgent flag

**Result:** Purchase Order created with status = **Draft**

---

#### Step 2: Add Items to Purchase Order (Status: Draft)

**Location:** Purchase Orders → View Details → Add Item

**For Each Item:**
1. Select **Item** from dropdown (filtered by supplier if auto-reorder)
2. Enter **Quantity** to order
3. Enter **Unit Price** from supplier quote
4. Optional: Enter discount amount
5. Add item-specific notes if needed

**Automatic Calculations:**
```
Line Total = (Quantity × Unit Price) - Discount Amount
Order Subtotal = Sum of all line totals
Tax (if applicable) = Subtotal × Tax Rate
Order Total = Subtotal + Tax + Shipping - Global Discount
```

**Validation:**
- Must add at least 1 item before submission
- Quantity must be > 0
- Unit Price must be ≥ 0

**⚠️ Important:** You can modify/delete items only while status = **Draft**

---

#### Step 3: Submit for Approval (Status: Draft → Submitted)

**Location:** Purchase Orders → Menu (⋮) → Submit for Approval

**Prerequisites:**
- ✓ At least 1 item added
- ✓ All required fields completed
- ✓ Order total within budget limits

**Action:** Click "Submit for Approval"

**What Happens:**
- Status changes to **Submitted**
- Order is locked (no editing allowed)
- Notification sent to approver
- Order number frozen

**⚠️ Cannot Undo:** Once submitted, order cannot be edited. Contact approver to cancel if needed.

---

#### Step 4: Approve Order (Status: Submitted → Approved)

**Who:** Procurement Manager / Authorized Approver

**Location:** Purchase Orders → Filter by Status: Submitted

**Review Checklist:**
- ✓ Supplier is correct and active
- ✓ Items are appropriate for business needs
- ✓ Quantities are reasonable
- ✓ Prices match supplier quotes
- ✓ Budget is available
- ✓ Delivery date is acceptable

**Actions:**
- **Approve:** Click Menu (⋮) → Approve Order → Status = **Approved**
- **Reject:** Click Menu (⋮) → Cancel Order → Provide reason

**What Happens After Approval:**
- Order authorized for sending
- Budget committed
- Ready for supplier communication

---

#### Step 5: Send to Supplier (Status: Approved → Sent)

**Who:** Purchasing Department

**Location:** Purchase Orders → Menu (⋮)

**Steps:**
1. Click **"Download PDF"** to generate purchase order document
   - PDF includes: Company logo, supplier details, line items, totals, terms
   
2. Send PDF to supplier via:
   - Email attachment
   - Supplier portal upload
   - EDI/API integration
   
3. After confirming supplier received PO, click **"Send to Supplier"**

**Result:** 
- Status changes to **Sent**
- PO is now **ready for receiving**
- Awaiting goods delivery

---

### Phase 2: Goods Receipt Management

#### Step 6: Create Goods Receipt from Purchase Order

**When:** Goods physically arrive at your warehouse

**Location:** Store → Goods Receipts → "Create from Purchase Order"

**Prerequisites:**
- ✓ Purchase Order exists with Status = **Sent** or **PartiallyReceived**
- ✓ Physical goods have arrived
- ✓ Delivery documentation available (packing slip, BOL)

---

##### Option A: Full Receipt (Receive All Items)

**Use When:** Supplier delivered complete order exactly as ordered

**Steps:**
1. Click **"Create from Purchase Order"**
2. **Select Purchase Order** from list
   - Shows only orders with Status = "Sent" or "PartiallyReceived"
   - Displays Supplier, Order #, Expected Date, Items count
   
3. **Review Items List:**
   ```
   Item Name | SKU | Ordered | Already Received | Remaining | Qty to Receive
   ──────────────────────────────────────────────────────────────────────────
   Item A    | 001 | 100     | 0                | 100       | [100]
   Item B    | 002 | 50      | 0                | 50        | [50]
   ```
   
4. **All items auto-selected** with default quantities = Remaining Quantity
5. Check **"Select All"** checkbox (pre-checked for full receipt)
6. Fill receipt header:
   - **Receipt Number:** Auto-generated (e.g., `GR-2025-11-10T14-30-00`)
   - **Received Date:** Auto-filled with current date/time
   - **Warehouse:** Select destination warehouse
   - **Notes:** Any delivery notes or issues
   
7. Click **"Create Receipt"**

**Result:**
- Goods Receipt created with all items
- Status = **Draft**
- PO Status updates based on completion:
  - If all items received → PO Status = **Received**
  - If some items remain → PO Status = **PartiallyReceived**

---

##### Option B: Partial Receipt (Receive Selected Items)

**Use When:**
- Supplier delivered partial order (backorder, split shipment)
- Some items damaged/rejected
- Quality hold on certain items
- Different delivery dates for items

**Steps:**
1. Click **"Create from Purchase Order"**
2. **Select Purchase Order** from list
3. **Review Items List** showing what's available to receive:
   ```
   Item Name | SKU | Ordered | Already Received | Remaining | Qty to Receive
   ──────────────────────────────────────────────────────────────────────────
   Item A    | 001 | 100     | 0                | 100       | [100] ← Full qty
   Item B    | 002 | 50      | 0                | 50        | [30]  ← Partial
   Item C    | 003 | 200     | 100              | 100       | [0]   ← Not received
   ```
   
4. **Select Items to Receive:**
   - ✓ Check checkbox for items physically received
   - ✗ Uncheck items not received in this shipment
   
5. **Adjust Quantities:**
   - Change "Qty to Receive" to match actual delivered quantity
   - Must be ≤ Remaining Quantity
   - Can be 0 to exclude item from this receipt
   
6. Fill receipt header:
   - **Receipt Number:** Auto-generated
   - **Received Date:** Date goods arrived
   - **Warehouse:** Where goods are stored
   - **Notes:** Document partial delivery reason:
     - "Partial shipment 1 of 2 - backorder expected Dec 1"
     - "Item C damaged in transit - vendor sending replacement"
     - "Items A & B only per supplier split delivery"
   
7. Click **"Create Receipt"**

**Result:**
- Goods Receipt created with selected items only
- Status = **Draft**
- PO Status = **PartiallyReceived** (still has outstanding items)
- Remaining items can be received in future receipts

**Example Scenario:**
```
Purchase Order PO-2025-11-001 (Status: Sent)
├─ Item A: 100 units ordered
├─ Item B: 50 units ordered  
└─ Item C: 200 units ordered

Delivery 1 (Nov 10):
├─ Receipt GR-001: Receive 100 units of Item A only
└─ PO Status → PartiallyReceived

Delivery 2 (Nov 15):
├─ Receipt GR-002: Receive 50 units of Item B, 150 units of Item C
└─ PO Status → PartiallyReceived (50 units of Item C still pending)

Delivery 3 (Nov 20):
├─ Receipt GR-003: Receive final 50 units of Item C
└─ PO Status → Received (Complete)
```

---

#### Step 7: Verify & Inspect Received Goods (Status: Draft)

**Location:** Goods Receipts → View Details

**Purpose:** Ensure quality and accuracy before finalizing

**Inspection Checklist:**

1. **Quantity Verification:**
   - Count actual units received
   - Compare to packing slip
   - Compare to purchase order
   - Document any variances

2. **Quality Inspection:**
   - Check for damage during shipping
   - Verify product condition
   - Test functionality (if applicable)
   - Check expiration dates (perishables)
   - Record lot/batch numbers for traceability

3. **Documentation:**
   - For each item line, optionally record:
     - **Lot Number:** For batch tracking
     - **Serial Number:** For serialized items
     - **Expiry Date:** For dated goods
     - **Quality Status:** Passed / Failed / Quarantined / Pending
     - **Inspector Name:** Who performed inspection
     - **Inspection Date:** When inspected
     - **Notes:** Observations, issues, concerns

4. **Variance Handling:**
   - **Over-delivery:** Supplier sent more than ordered
     - Option 1: Accept extra units (adjust receipt qty)
     - Option 2: Reject excess (note in receipt)
   - **Under-delivery:** Supplier sent less than ordered
     - Adjust receipt qty to actual received
     - Note shortage reason
     - Coordinate backorder with supplier
   - **Damaged Goods:** Items arrived damaged
     - Record quantity rejected
     - Note damage details
     - Initiate claim with supplier/carrier
   - **Wrong Items:** Incorrect items delivered
     - Do not add to receipt
     - Contact supplier for return/exchange

**Best Practices:**
- Inspect immediately upon delivery while driver present
- Take photos of damage for claims
- Get driver signature on delivery discrepancy notes
- Update receipt with inspection results same day
- Don't delay marking as received - impacts inventory accuracy

---

#### Step 8: Mark as Received (Status: Draft → Received)

**When:** Inspection complete, ready to accept goods into inventory

**Location:** Goods Receipts → Menu (⋮) → Mark as Received

**Prerequisites:**
- ✓ Physical goods inspected and verified
- ✓ Quantities confirmed accurate
- ✓ Quality acceptable
- ✓ Documentation complete (lot numbers, etc.)

**Actions:**
1. Click **"Mark as Received"**
2. Confirm dialog: "Mark this goods receipt as received?"
3. System processes:
   - Receipt Status → **Received**
   - Receipt locked (no further editing)
   - Received Date stamped
   - Received By user recorded
   - Purchase Order Status updated:
     - If all PO items fully received → PO Status = **Received**
     - If PO items partially received → PO Status = **PartiallyReceived**

**⚠️ Important:** This action is **irreversible**. Once marked received:
- Cannot modify quantities
- Cannot add/remove items
- Cannot delete receipt
- Only option is to cancel with reason

**What Happens:**
- Goods are considered "in receiving" but not yet in active inventory
- Ready for posting to inventory
- Awaiting final verification before stock update

---

#### Step 9: Post to Inventory (Status: Received → Posted)

**When:** Ready to make goods available for use/sale

**Location:** Goods Receipts → Menu (⋮) → Post to Inventory

**Prerequisites:**
- ✓ Receipt Status = **Received**
- ✓ All quality checks passed
- ✓ Warehouse location assigned
- ✓ Final approval to add to stock

**Actions:**
1. Click **"Post to Inventory"**
2. Confirm: "Post receipt to inventory? Stock levels will be updated."
3. System processes:
   - Receipt Status → **Posted**
   - **Inventory updated:**
     ```
     For each item in receipt:
       StockLevel.QuantityOnHand += QuantityReceived
       StockLevel.LastUpdated = Now
       StockLevel.LastReceivedDate = Receipt.ReceivedDate
     ```
   - Inventory transaction created (type: "Receipt")
   - Lot/Serial numbers recorded in inventory
   - Stock available for picking/sales
   - Cost updated (if using weighted average costing)

**Result:**
- Goods now in active inventory
- Visible in stock levels
- Available for allocation to orders
- Included in inventory reports

**⚠️ Cannot Undo:** Posting to inventory is permanent. To reverse:
- Must create inventory adjustment
- Must have approval for adjustment
- Requires documentation of reason

---

#### Step 10: Close Purchase Order (Complete)

**When:** All items from PO fully received

**Automatic Process:** System automatically closes PO when:
- All line items have: `Received Quantity >= Ordered Quantity`
- No outstanding items remain
- All goods receipts posted

**Final Status:**
- Purchase Order Status → **Received** (Complete)
- No longer appears in "awaiting receipt" lists
- Archived for historical reference
- Available for reporting and analysis

---

## 🎯 Key Concepts & Business Rules

### Purchase Order Status Flow

```
Draft → Submitted → Approved → Sent → PartiallyReceived → Received
  ↓         ↓          ↓        ↓            ↓
Cancelled Cancelled Cancelled Cancelled  Cancelled
```

**Status Definitions:**
- **Draft**: Being created, can edit
- **Submitted**: Awaiting approval, locked
- **Approved**: Authorized, ready to send
- **Sent**: Sent to supplier, awaiting delivery
- **PartiallyReceived**: Some items received, some pending
- **Received**: All items received, complete
- **Cancelled**: Order cancelled with reason

### Goods Receipt Status Flow

```
Draft → Received → Posted → Cancelled
```

**Status Definitions:**
- **Draft**: Being created, can edit quantities
- **Received**: Finalized, quantities locked, ready for posting
- **Posted**: Inventory updated, stock available
- **Cancelled**: Receipt voided with reason

### Quantity Tracking

**For Each Purchase Order Line Item:**
```
Ordered Quantity:    100 units  (Original PO)
Received Quantity:   80 units   (Across all receipts)
Remaining Quantity:  20 units   (Still to receive)
Cancelled Quantity:  0 units    (Supplier cannot fulfill)
```

**Multiple Receipts Example:**
```
PO Line: 100 units of Item A

Receipt 1:  +30 units → Received: 30,  Remaining: 70
Receipt 2:  +50 units → Received: 80,  Remaining: 20
Receipt 3:  +20 units → Received: 100, Remaining: 0  ✓ Complete
```

### Validation Rules

**Creating Goods Receipt from PO:**
- ✓ PO must exist
- ✓ PO Status must be "Sent" or "PartiallyReceived"
- ✓ At least 1 item must have remaining quantity > 0
- ✓ Cannot receive more than remaining quantity per item
- ✓ Receipt number must be unique
- ✓ Received date cannot be future date
- ✓ Warehouse must be valid and active

**Marking as Received:**
- ✓ Receipt Status must be "Draft"
- ✓ Must have at least 1 item
- ✓ All quantities must be > 0
- ✓ Cannot exceed ordered quantities

**Posting to Inventory:**
- ✓ Receipt Status must be "Received"
- ✓ Warehouse location must exist
- ✓ Items must exist in product catalog
- ✓ Stock levels must be accessible

---

## 🚨 Common Scenarios & Solutions

### Scenario 1: Partial Delivery

**Situation:** Supplier delivers only 60 out of 100 units ordered

**Solution:**
1. Create Goods Receipt from PO
2. Select the item
3. Change "Qty to Receive" from 100 to 60
4. Add note: "Partial delivery - backorder expected [date]"
5. Mark as Received
6. PO Status → **PartiallyReceived**
7. When backorder arrives, create 2nd receipt for remaining 40 units

---

### Scenario 2: Damaged Goods

**Situation:** 10 units damaged in transit out of 50 delivered

**Solution:**
1. Create Goods Receipt with all 50 units
2. In item details, record:
   - Quantity Received: 50
   - Quantity Rejected: 10
   - Quality Status: "Partially Failed"
   - Notes: "10 units damaged - boxes crushed, vendor notified"
3. Mark as Received
4. Post to Inventory → Only 40 units added to stock
5. Initiate claim with supplier for 10 damaged units
6. When replacement arrives, create separate receipt

---

### Scenario 3: Wrong Items Delivered

**Situation:** Supplier sent Item B instead of Item A

**Solution:**
1. **Do NOT create goods receipt yet**
2. Contact supplier immediately
3. Reject delivery or hold in quarantine area
4. Supplier options:
   - Pick up wrong items, deliver correct items
   - Issue credit memo, cancel line item
5. **Only after resolution:**
   - If correct items delivered: Create receipt
   - If line cancelled: Update PO to cancel item

---

### Scenario 4: Over-Delivery

**Situation:** Supplier sent 120 units when only 100 were ordered

**Solution:**

**Option A: Accept Extra Units**
1. Create Goods Receipt
2. Record 120 units received (system allows exceeding ordered qty with warning)
3. Add note: "Over-delivery accepted - matched to supplier invoice"
4. Mark as Received
5. Post to Inventory (all 120 units added)
6. Purchase Order will show "over-received" status

**Option B: Reject Extra Units**
1. Create Goods Receipt
2. Record only 100 units (ordered quantity)
3. Add note: "20 units refused - not authorized, returned to driver"
4. Have driver sign acknowledgment
5. Mark as Received with 100 units
6. Coordinate return/credit with supplier

---

### Scenario 5: Multiple Partial Receipts

**Situation:** Large order split into 3 deliveries over 2 weeks

**Solution:**
```
PO-2025-11-001 (Status: Sent)
├─ Item A: 1000 units
├─ Item B: 500 units
└─ Item C: 2000 units

Week 1 - Delivery 1 (Nov 10):
├─ Create Receipt GR-001
├─ Item A: 400 units
└─ PO Status → PartiallyReceived

Week 1 - Delivery 2 (Nov 12):
├─ Create Receipt GR-002
├─ Item A: 600 units (completes Item A)
├─ Item B: 200 units
└─ PO Status → PartiallyReceived

Week 2 - Delivery 3 (Nov 17):
├─ Create Receipt GR-003
├─ Item B: 300 units (completes Item B)
├─ Item C: 2000 units (completes Item C)
└─ PO Status → Received (Complete)
```

**Best Practices:**
- Create separate receipt for each physical delivery
- Note delivery sequence in receipt notes
- Track BOL/packing slip numbers
- Verify all receipts before closing PO

---

## ✅ Best Practices

### For Purchasing Department

1. **Always verify supplier before sending PO**
   - Confirm supplier is active and approved
   - Check payment terms and credit limit
   - Verify delivery address

2. **Communicate delivery expectations**
   - Notify receiving department of expected deliveries
   - Provide PO copy to warehouse
   - Share special handling instructions

3. **Monitor outstanding POs**
   - Follow up on overdue deliveries
   - Track POs in "Sent" status
   - Escalate delays to suppliers

4. **Review receiving discrepancies**
   - Investigate over/under deliveries
   - Address quality issues promptly
   - Update POs if supplier cannot fulfill

### For Receiving Department

1. **Inspect immediately upon delivery**
   - Count units while driver present
   - Check for visible damage
   - Verify item numbers match PO
   - Get driver signature on discrepancies

2. **Document everything**
   - Take photos of damage
   - Record lot/serial numbers
   - Note delivery time and condition
   - Keep packing slips with receipts

3. **Process receipts same day**
   - Don't let goods sit unprocessed
   - Create receipt in system promptly
   - Mark as received after inspection
   - Post to inventory to make available

4. **Communicate with purchasing**
   - Report discrepancies immediately
   - Notify of quality issues
   - Coordinate backorders
   - Provide feedback on suppliers

### For Warehouse Management

1. **Assign proper locations**
   - Put away goods promptly after receiving
   - Use designated receiving area for inspection
   - Move to permanent locations after posting
   - Update bin locations in system

2. **Maintain traceability**
   - Record lot numbers for expiry tracking
   - Track serial numbers for warranty
   - Follow FIFO/FEFO for perishables
   - Keep goods receipt documents

3. **Monitor inventory accuracy**
   - Verify system qty matches physical
   - Investigate discrepancies
   - Perform cycle counts on received items
   - Report system errors immediately

---

## 📊 Reporting & Analytics

### Key Metrics to Track

**Receiving Efficiency:**
- Average time from delivery to receipt creation
- Average time from receipt to posting
- Receipt processing time by user
- Receipts pending (Draft status)

**Supplier Performance:**
- On-time delivery rate
- Order accuracy rate (qty received vs ordered)
- Damage/defect rate
- Average lead time actual vs expected

**Inventory Impact:**
- Stock levels after receiving
- Receipts by warehouse
- Receipts by item category
- Value of goods received (period)

**Purchase Order Tracking:**
- POs awaiting receipt (Sent status)
- POs partially received
- Average time from sent to received
- PO completion rate

---

## 🔍 Troubleshooting

### Issue: Cannot Create Receipt from PO

**Possible Causes:**
- PO Status is not "Sent" or "PartiallyReceived"
  - **Solution:** Check PO status, may need to send to supplier first
- All items already fully received
  - **Solution:** Verify received quantities, PO may be complete
- PO is cancelled
  - **Solution:** Cannot receive against cancelled PO

---

### Issue: Cannot Mark Receipt as Received

**Possible Causes:**
- No items added to receipt
  - **Solution:** Add at least one item with qty > 0
- Quantities exceed ordered amounts
  - **Solution:** Adjust quantities or approve over-delivery
- Receipt already marked received
  - **Solution:** Check receipt status

---

### Issue: Inventory Not Updated After Posting

**Possible Causes:**
- Receipt not yet posted
  - **Solution:** Mark as Received first, then Post to Inventory
- Stock level record doesn't exist
  - **Solution:** Create stock level for item/warehouse combo
- System error during posting
  - **Solution:** Check logs, retry posting, contact support

---

### Issue: Need to Undo Received Receipt

**Solution:**
- If Status = "Received" (not yet posted):
  - Cancel receipt with reason
  - Create new corrected receipt
- If Status = "Posted" (inventory updated):
  - Cannot undo directly
  - Create inventory adjustment to reverse
  - Requires approval and documentation
  - Create new receipt if needed

---

## 📚 Related Documentation

- **Purchase Orders User Guide**: Complete PO lifecycle
- **Goods Receipt Implementation**: Technical API documentation
- **Inventory Management**: Stock levels and movements
- **Supplier Management**: Vendor setup and management
- **Warehouse Management**: Locations and bin tracking

---

## 🆘 Support & Help

**For Questions:**
- Review this guide
- Check related documentation
- Contact warehouse supervisor
- Email purchasing@company.com

**For System Issues:**
- Check error messages carefully
- Verify data entry (GUIDs, dates, quantities)
- Review validation rules
- Contact IT support: support@company.com

---

**Document Version:** 2.0  
**Last Updated:** November 10, 2025  
**Author:** System Documentation Team  
**Next Review:** February 2026

