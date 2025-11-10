# Purchase Orders - Complete Transaction Guide

**Comprehensive Guide for Managing Purchase Orders in the Store Module**

---

## Overview

The Purchase Orders module manages the complete procurement lifecycle from supplier order creation through goods receipt. It supports approval workflows, item management, financial tracking, and integration with goods receipts.

**Route**: `/store/purchase-orders`  
**Module**: Store Management  
**Related Modules**: Suppliers, Items, Goods Receipts, Inventory

---

## Purchase Order Transaction Flow

### Complete End-to-End Process

```
┌─────────────────────────────────────────────────────────────────────┐
│                    PURCHASE ORDER LIFECYCLE                         │
└─────────────────────────────────────────────────────────────────────┘

1. CREATE ORDER (Draft)
   ├─ Enter order header information
   ├─ Select supplier
   └─ Save as Draft

2. ADD ITEMS (Draft)
   ├─ Add line items one by one
   ├─ Specify quantities and prices
   └─ Totals calculate automatically

3. SUBMIT FOR APPROVAL (Draft → Submitted)
   ├─ Review all items and totals
   ├─ Submit to procurement manager
   └─ Cannot modify after submission

4. APPROVE ORDER (Submitted → Approved)
   ├─ Manager reviews order details
   ├─ Approves for sending to supplier
   └─ Order ready for transmission

5. SEND TO SUPPLIER (Approved → Sent)
   ├─ Generate PDF purchase order
   ├─ Send to supplier via email
   └─ Await goods delivery

6. RECEIVE GOODS (Sent → Received)
   ├─ Option A: Simple receive (mark entire order)
   ├─ Option B: Detailed receive (via Goods Receipts)
   └─ Updates inventory levels

7. COMPLETE (Received)
   └─ Order archived, goods in inventory
```

---

## Step-by-Step Transaction Guide

### 1️⃣ Creating a Purchase Order

**When to Use**: You need to procure items from a supplier

1. Navigate to **Store → Purchase Orders**
2. Click **"Add"** button
3. Fill in the order header:

   **Required Fields**:
   - **Order Number**: Unique identifier (e.g., `PO-2025-001`)
   - **Supplier**: Select from dropdown
   - **Order Date**: Date placing the order

   **Optional Fields**:
   - **Expected Delivery Date**: When you expect goods
   - **Delivery Address**: Where goods should be delivered
   - **Contact Person**: Receiving department contact
   - **Contact Phone**: Contact number
   - **Is Urgent**: Check if high priority
   - **Notes**: Any special instructions

4. Click **"Save"**
5. Order is created with status: **Draft**

**Important**: You cannot add items until the order is saved!

---

### 2️⃣ Adding Items to the Order

**When to Use**: After creating the order header

1. Find your newly created order
2. Click **⋮** menu → **"View Details"**
3. In the Order Items section, click **"Add Item"**

4. Fill in the item dialog:
   - **Item**: Select inventory item from dropdown
   - **Quantity**: Number of units to order
   - **Unit Price**: Price per unit from supplier
   - **Discount Amount**: Item-level discount (optional)
   - **Notes**: Item-specific notes (optional)

5. Click **"Save"**
6. **Order totals recalculate automatically**:
   ```
   Item Total = (Quantity × Unit Price) - Discount
   ```

7. **Repeat** for each item you want to order

**Editing Items** (Draft status only):
- Click **Edit** icon (pencil) to modify quantity or price
- Changes recalculate order totals

**Deleting Items** (Draft status only):
- Click **Delete** icon (trash)
- Confirm deletion
- Order totals update

**⚠️ Important**: Items can only be modified while order is in **Draft** status!

---

### 3️⃣ Submitting for Approval

**When to Use**: Order is complete and ready for review

**Prerequisite**: 
- Order must have at least one item
- All required fields filled

**Steps**:
1. Review order details and all items
2. Verify quantities and prices
3. Check total amount
4. Click **⋮** menu → **"Submit for Approval"**
5. Confirm the submission
6. Status changes: **Draft → Submitted**

**What Happens**:
- Order is locked (no more editing)
- Notification sent to approver
- Order awaits manager approval

**⚠️ Cannot Undo**: Once submitted, you cannot edit. Contact approver to cancel if needed.

---

### 4️⃣ Approving the Order

**Who**: Procurement Manager or Authorized Approver  
**When to Use**: Review submitted purchase orders

**Steps**:
1. Find the **Submitted** order
2. Review all details:
   - Supplier information
   - Line items and quantities
   - Unit prices and totals
   - Expected delivery date
   - Budget availability

3. Click **⋮** menu → **"Approve Order"**
4. Confirm approval
5. Status changes: **Submitted → Approved**

**What Happens**:
- Order is authorized for sending
- Supplier communication can proceed
- Budget is committed

**Alternative**: Click **"Cancel Order"** to reject

---

### 5️⃣ Sending to Supplier

**Who**: Purchasing Department  
**When to Use**: After order approval

**Steps**:
1. Find the **Approved** order
2. Click **⋮** menu → **"Download PDF"**
3. PDF is generated with:
   - Company information
   - Supplier details
   - All line items with quantities/prices
   - Financial summary
   - Terms and conditions
   - Authorized signatures

4. Send PDF to supplier via:
   - Email attachment
   - Supplier portal
   - EDI transmission

5. After sending, click **⋮** menu → **"Send to Supplier"**
6. Confirm the action
7. Status changes: **Approved → Sent**

**What Happens**:
- Order is officially transmitted
- Delivery countdown begins
- System tracks expected delivery date

---

### 6️⃣ Receiving Goods

**When to Use**: Goods arrive from supplier

#### Option A: Simple Receive (Full Order)

**Use When**: All items arrived, no quality issues

1. Find the **Sent** order
2. Click **⋮** menu → **"Mark as Received"**
3. Confirm receipt
4. Status changes: **Sent → Received**
5. System automatically:
   - Sets actual delivery date
   - Updates received quantities
   - Completes the order

#### Option B: Detailed Receive (Via Goods Receipts)

**Use When**: Need quality control, partial deliveries, or detailed tracking

1. Navigate to **Warehouse → Goods Receipts**
2. Click **"Add"** to create new receipt
3. Link to the purchase order
4. Inspect and record:
   - Actual quantities received
   - Quality status (passed/failed)
   - Damaged items
   - Variance from order
   - Location where goods are stored

5. Complete the goods receipt
6. System automatically:
   - Updates purchase order item received quantities
   - Creates inventory transactions
   - Updates stock levels
   - Records put-away locations

**Partial Deliveries**:
- Goods receipt updates received quantities
- Order remains in **Sent** status
- Can create multiple receipts
- When all items received → **Received** status

**Three-Way Matching**:
1. Purchase Order (what we ordered)
2. Goods Receipt (what we received)
3. Supplier Invoice (what we're billed)

---

## Financial Calculations

### Item Level
```
Item Subtotal = Quantity × Unit Price
Item Discount = Discount Amount (if any)
Item Total = Item Subtotal - Item Discount
```

**Example**:
```
10 units × $5.00 = $50.00
Discount: -$2.00
Item Total: $48.00
```

### Order Level
```
Subtotal = Sum of all Item Totals
Tax Amount = Subtotal × Tax Rate (or manual entry)
Order Discount = Overall discount negotiated
Shipping Cost = Transportation charges
Net Amount = Subtotal + Tax + Shipping - Discount
```

**Full Example**:
```
ITEMS:
Item 1: 10 units × $5.00 - $2.00 = $48.00
Item 2: 5 units × $10.00 - $0.00 = $50.00
Item 3: 20 units × $2.50 - $5.00 = $45.00
──────────────────────────────────────────
Subtotal:                        $143.00
Tax (10%):                       +$14.30
Shipping:                        +$10.00
Order Discount:                  -$15.00
──────────────────────────────────────────
NET AMOUNT TO PAY:               $152.30
```

---

## Searching and Filtering

### Basic Search
Type in search box to find by:
- Order Number
- Contact Person
- Delivery Address
- Notes

### Advanced Search
Click **"Advanced Search"** to filter:

**Supplier Filter**:
- All Suppliers
- Specific supplier

**Status Filter**:
- All Statuses
- Draft
- Submitted
- Approved
- Sent
- Received
- Cancelled

**Date Range**:
- From Date: Start of range
- To Date: End of range
- Filters by Order Date

**Example**: Find all Sent orders from Supplier ABC in December 2025

---
   - Order header information
   - Supplier details
   - All line items with quantities and prices
   - Financial summary (totals, tax, discount)

**Use Case**: Send PDF to supplier via email after sending the order

---

## Managing Order Items

### Editing an Item

1. Open the order details (⋮ → View Details)
2. In the **Order Items** table, click the **Edit** icon (pencil)
3. Modify the:
   - Quantity
   - (Other fields are read-only after creation)
4. Click **"Save"**
5. Order totals update automatically

### Deleting an Item

1. Open the order details (⋮ → View Details)
2. In the **Order Items** table, click the **Delete** icon (trash)
3. Confirm the deletion
4. Item is removed
5. Order totals update automatically

**Note**: Items can only be added/edited/deleted when the order is in **Draft** status

---

## Searching and Filtering

### Basic Search
Use the search box to find orders by Order Number, Contact Person, or Notes.

### Advanced Search
Click **"Advanced Search"** to filter by:
- **Supplier**: Filter by specific supplier
- **Status**: Filter by order status
  - Draft
  - Submitted
  - Approved
  - Sent
  - Received
  - Cancelled
- **From Date**: Start date range
- **To Date**: End date range

---

## Understanding the Order List

### Columns Displayed

| Column | Description |
|--------|-------------|


## Order Management Features

### Viewing Order Details
1. Click **⋮** → **"View Details"**
2. See complete order information:
   - Header details
   - All line items
   - Financial summary
   - Status history
   - Related documents

### Downloading PDF
**Available**: Any status  
**Use**: Send to supplier, archiving, printing

1. Click **⋮** → **"Download PDF"**
2. Professional PDF includes:
   - Company logo and information
   - Supplier name and address
   - Order number and date
   - Item list with descriptions
   - Quantities and prices
   - Subtotals and taxes
   - Net amount
   - Payment terms
   - Delivery instructions

### Cancelling Orders
**Available**: Draft, Submitted, or Approved status only  
**Cannot Cancel**: Sent or Received orders

1. Click **⋮** → **"Cancel Order"**
2. Confirm cancellation
3. Status changes to: **Cancelled**
4. Order cannot be reactivated

**When to Cancel**:
- Supplier no longer available
- Budget constraints
- Items no longer needed
- Duplicate order created

---

## Status Workflow Diagram

```
┌──────────┐
│  DRAFT   │ ← Create order, add/edit items
└──────────┘
     │
     │ Submit
     ▼
┌──────────┐
│SUBMITTED │ ← Awaiting approval
└──────────┘
     │
     │ Approve
     ▼
┌──────────┐
│ APPROVED │ ← Ready to send
└──────────┘
     │
     │ Send
     ▼
┌──────────┐
│   SENT   │ ← Waiting for delivery
└──────────┘
     │
     │ Receive
     ▼
┌──────────┐
│ RECEIVED │ ← Order complete
└──────────┘

Cancel allowed from Draft, Submitted, or Approved
     │
     ▼
┌──────────┐
│CANCELLED │ ← Order cancelled
└──────────┘
```

---

## Understanding the Order List Columns

| Column | Description | Example |
|--------|-------------|---------|
| **Order Number** | Unique identifier | PO-2025-001 |
| **Supplier ID** | Supplier reference | acme-corp-123 |
| **Order Date** | When order was created | Nov 10, 2025 |
| **Status** | Current order status | Sent |
| **Total Amount** | Order total value | $152.30 |
| **Expected Delivery** | When goods should arrive | Nov 17, 2025 |
| **Urgent** | Priority flag | ✓ |

---

## Context Menu Actions (⋮)

| Action | Available When | What It Does |
|--------|---------------|-------------|
| **View Details** | Always | Opens full order details |
| **Download PDF** | Always | Generates purchase order PDF |
| **Submit for Approval** | Draft only | Sends for manager approval |
| **Approve Order** | Submitted only | Authorizes the order |
| **Send to Supplier** | Approved only | Marks as transmitted |
| **Mark as Received** | Sent only | Completes order (simple) |
| **Cancel Order** | Draft/Submitted/Approved | Cancels the order |
| **Edit** | Draft only (header) | Modify order details |
| **Delete** | Draft only (if no items) | Remove order |

---

## Best Practices

### Order Creation
✅ **DO**:
- Create draft orders first
- Add all items before submitting
- Verify supplier information
- Set realistic delivery dates
- Include contact information
- Add notes for special requirements

❌ **DON'T**:
- Submit incomplete orders
- Skip approval workflow
- Forget to add items
- Use duplicate order numbers

### Item Management
✅ **DO**:
- Double-check quantities
- Verify unit prices with supplier
- Add item notes for specifications
- Review line totals before submitting

❌ **DON'T**:
- Order excessive quantities
- Skip discount negotiations
- Forget to remove unwanted items
- Mix up unit prices

### Workflow Management
✅ **DO**:
- Review before submitting
- Approve promptly to avoid delays
- Send PDF immediately after approval
- Track expected delivery dates
- Use Goods Receipts for quality control

❌ **DON'T**:
- Skip approval steps
- Send orders without approval
- Forget to mark as received
- Leave orders in limbo

### Financial Control
✅ **DO**:
- Verify all calculations
- Apply negotiated discounts
- Include taxes correctly
- Add shipping costs
- Match against budget

❌ **DON'T**:
- Ignore cost overruns
- Skip discount opportunities
- Forget about taxes
- Overlook shipping charges

---

## Common Scenarios

### Scenario 1: Regular Inventory Replenishment
1. Create order with established supplier
2. Add standard items with negotiated prices
3. Submit for routine approval
4. Send to supplier
5. Use Goods Receipts for detailed receiving

### Scenario 2: Urgent Order
1. Create order and check **Is Urgent**
2. Add required items
3. Submit with urgent flag visible
4. Fast-track approval
5. Send immediately
6. Expedite receiving process

### Scenario 3: Large Capital Purchase
1. Create order with detailed specifications
2. Add items with quotes attached
3. Submit for higher-level approval
4. Await multiple approvals
5. Generate formal PO document
6. Send with payment terms
7. Use detailed goods receipt with inspection

### Scenario 4: Partial Delivery
1. Order sent to supplier
2. First shipment arrives (50% of items)
3. Create Goods Receipt for partial delivery
4. Received quantities update
5. Order remains **Sent** status
6. Second shipment arrives
7. Create second Goods Receipt
8. All items received → **Received** status

### Scenario 5: Order Correction Needed
1. Order in **Draft** status
2. Notice error in quantity or price
3. Edit item directly
4. Review updated totals
5. Save changes
6. Continue with submission

### Scenario 6: Cancel Before Sending
1. Order is **Submitted** or **Approved**
2. Situation changes (budget, supplier, needs)
3. Click **Cancel Order**
4. Order moves to **Cancelled**
5. Can create new order if needed

---

## Integration with Other Modules

### Suppliers Module
- Select supplier from master list
- View supplier details
- Track supplier performance

### Items Module
- Select items from catalog
- View item specifications
- Check current stock levels

### Goods Receipts Module
- Create receipt linked to PO
- Perform quality inspection
- Record actual quantities
- Update inventory levels

### Inventory Module
- Automatic stock updates
- Tracks expected inventory
- Updates on goods receipt

---

## Troubleshooting

### Cannot Add Items
**Issue**: Add Item button is disabled  
**Solution**: Save the order header first, then add items

### Cannot Edit Order
**Issue**: Edit button is grayed out  
**Solution**: Orders can only be edited in Draft status. If submitted, ask approver to cancel.

### Wrong Total Amount
**Issue**: Totals don't match expectations  
**Solution**: Check individual item calculations, verify discount amounts, confirm tax rates

### Cannot Receive Order
**Issue**: Mark as Received option not showing  
**Solution**: Order must be in Sent status. Use status workflow to progress order.

### PDF Won't Generate
**Issue**: Download PDF fails  
**Solution**: Check that order has items, verify system configuration

---

## Tips and Tricks

### Keyboard Shortcuts
- **Ctrl/Cmd + Click**: Open details in new tab
- **Search box**: Start typing to filter instantly

### Quick Actions
- Use Advanced Search filters for bulk operations
- Sort columns by clicking headers
- Export data for external analysis

### Workflow Optimization
- Create templates for common orders
- Set up favorite suppliers
- Use notes field for tracking numbers
- Batch similar orders together

---

## Summary Checklist

### Creating an Order
- [ ] Click Add button
- [ ] Enter order number
- [ ] Select supplier
- [ ] Set order date
- [ ] Add delivery information
- [ ] Save as Draft

### Adding Items
- [ ] Open order details
- [ ] Click Add Item
- [ ] Select item from list
- [ ] Enter quantity
- [ ] Enter unit price
- [ ] Add discount if applicable
- [ ] Save item
- [ ] Verify totals

### Submitting
- [ ] Review all items
- [ ] Check total amount
- [ ] Verify supplier details
- [ ] Submit for approval
- [ ] Notify approver if needed

### Approval
- [ ] Review order details
- [ ] Verify budget availability
- [ ] Check supplier status
- [ ] Approve or reject
- [ ] Provide feedback if rejected

### Sending
- [ ] Download PDF
- [ ] Email to supplier
- [ ] Confirm supplier receipt
- [ ] Mark as Sent in system
- [ ] Note expected delivery

### Receiving
- [ ] Verify goods arrived
- [ ] Option A: Mark as Received
- [ ] Option B: Create Goods Receipt
- [ ] Update inventory
- [ ] Notify accounts payable

---

## Quick Reference Card

| Status | Can Add Items | Can Edit | Can Delete | Next Actions |
|--------|--------------|----------|------------|--------------|
| **Draft** | ✅ Yes | ✅ Yes | ✅ Yes | Submit, Cancel |
| **Submitted** | ❌ No | ❌ No | ❌ No | Approve, Cancel |
| **Approved** | ❌ No | ❌ No | ❌ No | Send, Cancel |
| **Sent** | ❌ No | ❌ No | ❌ No | Receive |
| **Received** | ❌ No | ❌ No | ❌ No | View only |
| **Cancelled** | ❌ No | ❌ No | ❌ No | View only |

---

## Getting Help

**Documentation**: See related guides
- Goods Receipts User Guide
- Supplier Management Guide
- Inventory Management Guide

**Support**: Contact your system administrator

**Training**: Ask for hands-on training sessions

---

**Last Updated**: November 10, 2025  
**Version**: 2.0
| **Expected Delivery** | When goods are expected |
| **Urgent** | Priority indicator (✓) |

---

## Context Menu Actions

### View Details
- **Available**: Always
- **Action**: Opens detailed view of the order with items

### Download PDF
- **Available**: Always
- **Action**: Generates and downloads PDF report

### Submit for Approval
- **Available**: Draft status only
- **Action**: Submits order for approval review

### Approve Order
- **Available**: Submitted status only
- **Action**: Approves the order

### Send to Supplier
- **Available**: Approved status only
- **Action**: Marks order as sent to supplier

### Mark as Received
- **Available**: Sent status only
- **Action**: Marks order as received (goods arrived)

### Cancel Order
- **Available**: Draft, Submitted, or Approved status
- **Action**: Cancels the order (cannot be undone)

---

## Order Details View

### Header Information
- Order Number and Status
- Supplier Name
- Order Date and Delivery Dates
- Expected vs Actual Delivery
- Financial Summary:
  - Total Amount
  - Tax Amount
  - Discount Amount
  - Net Amount
- Delivery Address
- Contact Person and Phone
- Urgent indicator (if flagged)

### Items Table
Shows all items in the order with:
- **Item Name**: Product being ordered
- **SKU**: Product SKU code
- **Quantity**: How many units ordered
- **Unit Price**: Price per unit
- **Discount**: Discount applied to this item
- **Total**: Line total (Quantity × Price - Discount)
- **Received**: How many units received (tracking)
- **Notes**: Item-specific notes
- **Actions**: Edit and Delete buttons (Draft only)

---

## Status Workflow Diagram

```
┌────────┐  Submit   ┌──────────┐  Approve  ┌─────────┐
│ Draft  │─────────→ │Submitted │─────────→ │Approved │
└────────┘           └──────────┘           └─────────┘
    │                     │                       │
    │                     │                       │
  Cancel               Cancel                   Cancel
    │                     │                       │
    ▼                     ▼                       ▼
┌────────┐           ┌──────────┐           ┌─────────┐
│Cancelled│          │Cancelled │           │Cancelled│
└────────┘           └──────────┘           └─────────┘

                                              ┌─────────┐
                                              │Approved │
                                              └─────────┘
                                                   │
                                                   │ Send
                                                   ▼
                                              ┌─────────┐
                                              │  Sent   │
                                              └─────────┘
                                                   │
                                                   │ Receive
                                                   ▼
                                              ┌─────────┐
                                              │Received │
                                              └─────────┘
```

---

## Financial Tracking

### Understanding Totals

**Item Total**:
```
Item Total = (Quantity × Unit Price) - Discount Amount
```

**Order Total**:
```
Subtotal = Sum of all Item Totals
Total Amount = Subtotal + Tax Amount - Order Discount
Net Amount = Total Amount (final amount to pay)
```

**Example**:
```
Item 1: 10 units × $5.00 - $2.00 discount = $48.00
Item 2: 5 units × $10.00 - $0.00 discount = $50.00
────────────────────────────────────────────────────
Subtotal: $98.00
Tax Amount: +$9.80 (10%)
Order Discount: -$5.00
────────────────────────────────────────────────────
Total Amount: $102.80
Net Amount: $102.80 (amount to pay supplier)
```

---

## Best Practices

### Planning
1. **Create draft orders first** - Review before committing
2. **Check supplier availability** before ordering
3. **Negotiate pricing** and update unit prices
4. **Use urgent flag** for priority orders
5. **Set realistic delivery dates**

### Ordering
1. **Add all items at once** to see full total
2. **Apply discounts** as negotiated
3. **Include delivery instructions** in notes
4. **Verify contact information** is current
5. **Download PDF** before sending

### Approval
1. **Review totals** before approving
2. **Check delivery dates** are acceptable
3. **Verify supplier** is correct
4. **Confirm budget** is available
5. **Approve promptly** to avoid delays

### Receiving
1. **Use Goods Receipts module** for detailed receiving
2. **Record received quantities** accurately
3. **Check for damages** during receiving
4. **Update status promptly** when goods arrive
5. **Match quantities** with order

---

## Common Scenarios

### Scenario 1: Standard Order Flow
```
1. Create order (Draft)
2. Add all items
3. Review totals
4. Submit for approval
5. Manager approves
6. Send to supplier (download PDF, email it)
7. Supplier ships goods
8. Receive goods (use Goods Receipts)
9. Mark as received
```

### Scenario 2: Urgent Order
```
1. Create order with "Urgent" flag checked
2. Add items
3. Submit immediately
4. Manager reviews urgent queue
5. Fast-track approval
6. Send to supplier with rush request
7. Track delivery closely
8. Receive and process quickly
```

### Scenario 3: Cancelling an Order
```
1. Order in Draft/Submitted/Approved status
2. Circumstances change (e.g., supplier unavailable)
3. Click "Cancel Order"
4. Confirm cancellation
5. Order moves to Cancelled status
6. Cannot be modified further
7. Create new order if needed
```

### Scenario 4: Partial Receiving
```
1. Order sent to supplier
2. Supplier ships partial order
3. Use Goods Receipts module
4. Create receipt from this PO
5. Record received quantities (less than ordered)
6. System tracks: Ordered vs Received
7. Wait for remaining items
8. Create another receipt when rest arrives
9. When all received, mark PO as received
```

### Scenario 5: Adding Items After Creation
```
1. Order is in Draft status
2. Open order details
3. Click "Add Item"
4. Add new item with quantity and price
5. Totals update automatically
6. Can add multiple items
7. Once satisfied, submit for approval
```

---

## Troubleshooting

### Can't Submit Order
- **Cause**: Order must be in Draft status
- **Solution**: Check status, or create new order

### Can't Add Items
- **Cause**: Order must be in Draft status
- **Solution**: Items can only be added to draft orders

### Can't Edit Items
- **Cause**: Order has been submitted
- **Solution**: Cancel order, create new one, or contact admin

### Can't Delete Order
- **Cause**: Order has progressed beyond Draft
- **Solution**: Cancel the order instead

### PDF Won't Download
- **Cause**: Browser blocking downloads
- **Solution**: Check browser settings, allow downloads from this site

### Totals Don't Match
- **Cause**: Tax or discount not calculated
- **Solution**: Check tax amount and discount amount fields

### Supplier Not in List
- **Cause**: Supplier not created yet
- **Solution**: Go to Suppliers page, create supplier first

---

## Integration with Other Modules

### Goods Receipts
- **Purpose**: Detailed receiving with quality control
- **How**: Create receipt from PO, record received quantities
- **Link**: Goods Receipts module shows PO number

### Suppliers
- **Purpose**: Manage supplier information
- **How**: Select supplier when creating order
- **Link**: Supplier details loaded automatically

### Items
- **Purpose**: Product catalog
- **How**: Select items when adding to order
- **Link**: Item details (SKU, name) populated

### Stock Levels
- **Purpose**: Inventory tracking
- **How**: Received items update stock
- **Link**: Purchase orders affect inventory planning

---

## Keyboard Shortcuts

While in dialogs:
- **Enter**: Submit form (when focus is on a button)
- **Escape**: Close dialog
- **Tab**: Navigate between fields

---

## Tips & Tricks

### Efficiency
- Use **Advanced Search** to quickly find orders by supplier
- **Sort by Order Date** to see recent orders first
- **Filter by Status** to focus on pending approvals
- Keep **Details dialog open** while adding multiple items

### Accuracy
- Always **verify supplier** before submitting
- **Double-check quantities** before saving
- **Review totals** after adding all items
- **Include notes** for special requirements

### Organization
- Use **consistent numbering** for order numbers (e.g., PO-YYYY-NNN)
- **Document processes** in the Notes field
- **Track urgent orders** with the urgent flag
- **Download PDFs** immediately after sending

### Financial
- **Negotiate discounts** before creating orders
- **Apply item-level discounts** for specific items
- **Use order-level discounts** for overall discounts
- **Track tax carefully** for compliance

---

## Reporting

### Available Reports
- **PDF Purchase Order**: Detailed order document for supplier
- **Order List**: Filtered view of orders
- **Financial Summary**: Total amounts in list view

### Custom Reports (Future)
- Orders by supplier
- Orders by status
- Spending analysis
- Delivery performance

---

## Related Documentation

- [PURCHASE_ORDERS_UI_IMPLEMENTATION.md](./PURCHASE_ORDERS_UI_IMPLEMENTATION.md) - Technical implementation
- [PURCHASE_ORDERS_VERIFICATION.md](./PURCHASE_ORDERS_VERIFICATION.md) - Verification report
- [PAGES_ORGANIZATION.md](./PAGES_ORGANIZATION.md) - Navigation guide

---

## Support

For technical issues or questions:
1. Check this user guide first
2. Review the implementation documentation
3. Check the Store module documentation
4. Contact your system administrator

---

*Last Updated: October 25, 2025*  
*Version: 1.0*

