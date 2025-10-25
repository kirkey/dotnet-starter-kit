# Purchase Orders - User Guide

**Quick Reference for Using the Purchase Orders UI**

---

## Overview

The Purchase Orders module helps you manage procurement from suppliers with workflow operations, item management, and PDF generation.

**Route**: `/store/purchase-orders`

---

## Quick Start

### Creating a New Purchase Order

1. Click the **"Add"** button
2. Fill in the form:
   - **Order Number**: Unique identifier (e.g., PO-2025-001)
   - **Supplier**: Select from dropdown (required)
   - **Order Date**: Date of the order
   - **Expected Delivery Date**: When you expect delivery
   - **Delivery Address**: Where goods should be delivered
   - **Contact Person**: Your receiving contact
   - **Contact Phone**: Contact phone number
   - **Is Urgent**: Check if this is a priority order
   - **Tax Amount**: Tax to be charged
   - **Discount Amount**: Any discount negotiated
   - **Notes**: Additional information
3. Click **"Save"**
4. Order is created with status: **Draft**

---

## Adding Items to an Order

1. Find your order in the list
2. Click the **⋮** menu icon → **View Details**
3. In the **Order Items** section, click **"Add Item"**
4. Fill in the Add Item dialog:
   - **Item**: Select from dropdown (required)
   - **Quantity**: How many units (required)
   - **Unit Price**: Price per unit (required)
   - **Discount Amount**: Discount for this item (optional)
   - **Notes**: Item-specific notes (optional)
5. Click **"Save"**
6. Item appears in the table
7. **Order totals update automatically**
8. Repeat for all items

---

## Purchase Order Workflow

### Step 1: Submit for Approval

1. Find your **Draft** order
2. Click the **⋮** menu icon → **Submit for Approval**
3. Confirm the action
4. Status changes to **Submitted**
5. Order is now awaiting approval

### Step 2: Approve Order

1. Find the **Submitted** order
2. Click the **⋮** menu icon → **Approve Order**
3. Confirm the action
4. Status changes to **Approved**
5. Order is now ready to send to supplier

### Step 3: Send to Supplier

1. Find the **Approved** order
2. Click the **⋮** menu icon → **Send to Supplier**
3. Confirm the action
4. Status changes to **Sent**
5. Order has been sent to the supplier
6. Download and email the PDF to supplier

### Step 4: Mark as Received

1. When goods arrive, find the **Sent** order
2. Click the **⋮** menu icon → **Mark as Received**
3. Confirm the action
4. Status changes to **Received**
5. Order is now complete

**Alternative**: Use the Goods Receipts module for detailed receiving with quality control

---

## Downloading PDF

1. Find any order in the list
2. Click the **⋮** menu icon → **Download PDF**
3. System generates a professional PDF report
4. PDF automatically downloads to your computer
5. Filename format: `PurchaseOrder_{ID}_{timestamp}.pdf`
6. PDF includes:
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
| **Order Number** | Unique order identifier |
| **Supplier ID** | Supplier identifier |
| **Order Date** | Date order was created |
| **Status** | Current status |
| **Total Amount** | Total order value |
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

