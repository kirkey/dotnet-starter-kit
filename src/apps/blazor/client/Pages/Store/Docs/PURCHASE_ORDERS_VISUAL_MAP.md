# Purchase Orders - Visual Implementation Map

**Quick Visual Reference for the Complete Implementation**

---

## 📂 File Structure

```
Pages/Store/PurchaseOrders/
│
├── 📄 PurchaseOrders.razor                     (Main page - EntityTable)
├── 📄 PurchaseOrders.razor.cs                  (Page logic - 7 operations)
│
├── 📄 PurchaseOrderDetailsDialog.razor         (Details dialog)
├── 📄 PurchaseOrderDetailsDialog.razor.cs      (Dialog logic)
│
├── 📄 PurchaseOrderItems.razor                 (Items component - inline @code)
│
├── 📄 PurchaseOrderItemDialog.razor            (Add/Edit item dialog)
├── 📄 PurchaseOrderItemDialog.razor.cs         (Dialog logic)
│
└── 📄 PurchaseOrderItemModel.cs                (Data model)
```

---

## 🔄 Workflow Diagram

```
┌──────────────────────────────────────────────────────────────────────┐
│                     PURCHASE ORDER WORKFLOW                           │
└──────────────────────────────────────────────────────────────────────┘

    CREATE ORDER           SUBMIT            APPROVE           SEND
         │                    │                  │               │
         ▼                    ▼                  ▼               ▼
    ┌────────┐          ┌───────────┐      ┌─────────┐    ┌────────┐
    │ Draft  │─────────→│ Submitted │─────→│Approved │───→│  Sent  │
    └────────┘  Submit  └───────────┘Approve└─────────┘Send└────────┘
         │                    │                  │               │
         │                    │                  │               │
    Cancel              Cancel            Cancel          Receive
         │                    │                  │               │
         ▼                    ▼                  ▼               ▼
    ┌────────┐          ┌───────────┐      ┌─────────┐    ┌─────────┐
    │Cancelled│         │ Cancelled │      │Cancelled│    │Received │
    └────────┘          └───────────┘      └─────────┘    └─────────┘
```

---

## 🎯 User Interface Flow

```
┌──────────────────────────────────────────────────────────────────────┐
│  MAIN PAGE: /store/purchase-orders                                    │
├──────────────────────────────────────────────────────────────────────┤
│                                                                       │
│  [🔍 Search] [Advanced Search ▼] [+ Add]                             │
│                                                                       │
│  Advanced Filters:                                                   │
│  [Supplier ▼] [Status ▼] [From Date] [To Date]                      │
│                                                                       │
│  ┌────────────────────────────────────────────────────────────────┐  │
│  │ Order# │Supplier│ Date │ Status │ Total  │Expected│Urgent│ ⋮ │  │
│  ├────────────────────────────────────────────────────────────────┤  │
│  │ PO-001 │Acme Co.│ 1/25 │ Draft  │ $500.00│  2/1   │  ✓  │ ⋮ │  │
│  │ PO-002 │Best Inc│ 1/24 │ Sent   │$1200.00│  2/5   │     │ ⋮ │  │
│  │ PO-003 │Acme Co.│ 1/23 │Received│ $750.00│  1/30  │     │ ⋮ │  │
│  └────────────────────────────────────────────────────────────────┘  │
│                                                                       │
│  Context Menu (⋮):                                                   │
│  • View Details               (always)                               │
│  • Download PDF               (always)                               │
│  • Submit for Approval        (Draft only)                           │
│  • Approve Order              (Submitted only)                       │
│  • Send to Supplier           (Approved only)                        │
│  • Mark as Received           (Sent only)                            │
│  • Cancel Order               (Draft/Submitted/Approved)             │
│                                                                       │
└──────────────────────────────────────────────────────────────────────┘

                              ▼ Click "View Details"

┌──────────────────────────────────────────────────────────────────────┐
│  DETAILS DIALOG: Purchase Order Details                              │
├──────────────────────────────────────────────────────────────────────┤
│                                                                       │
│  Order Number: PO-001        Status: Draft                           │
│  Supplier: Acme Co.          Date: January 25, 2025                  │
│  Expected Delivery: February 1, 2025                                 │
│                                                                       │
│  Total Amount:    $500.00                                            │
│  Tax Amount:       $50.00                                            │
│  Discount Amount: -$25.00                                            │
│  Net Amount:      $525.00                                            │
│                                                                       │
│  Delivery Address: 123 Main St, City, ST 12345                      │
│  Contact Person: John Doe                                            │
│  Contact Phone: (555) 123-4567                                       │
│  Priority: 🔸 Urgent                                                  │
│                                                                       │
│  ─────────────────────────────────────────────────────────────────   │
│                                                                       │
│  Order Items                                        [+ Add Item]     │
│                                                                       │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │Item    │SKU  │Qty│Price  │Disc. │Total  │Recv.│Notes│Actions│   │
│  ├──────────────────────────────────────────────────────────────┤   │
│  │Widget A│WA-01│10 │$25.00 │$2.00 │$248.00│ 0   │     │[✏️][🗑️]│   │
│  │Widget B│WB-02│5  │$50.00 │$0.00 │$250.00│ 0   │Rush │[✏️][🗑️]│   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                       │
│                                                    [Close]           │
│                                                                       │
└──────────────────────────────────────────────────────────────────────┘

                         ▼ Click "Add Item"

┌──────────────────────────────────────────────────────────────────────┐
│  ADD ITEM DIALOG                                                      │
├──────────────────────────────────────────────────────────────────────┤
│                                                                       │
│  Select an item and enter order details.                            │
│                                                                       │
│  Item: [Select item...        ▼]  * Required                        │
│                                                                       │
│  Quantity: [    10     ]  * Required                                │
│                                                                       │
│  Unit Price: [   25.00   ]  * Required                              │
│                                                                       │
│  Discount Amount: [    2.00    ]  (Optional)                        │
│                                                                       │
│  Total: $248.00  (calculated automatically)                         │
│                                                                       │
│  Notes: [________________________________]                           │
│                                                                       │
│                                    [Cancel]  [Save]                  │
│                                                                       │
└──────────────────────────────────────────────────────────────────────┘

                       ▼ Click "Download PDF"

┌──────────────────────────────────────────────────────────────────────┐
│  PDF GENERATION                                                       │
├──────────────────────────────────────────────────────────────────────┤
│                                                                       │
│  ℹ️ Generating PDF report...                                         │
│                                                                       │
│  1. API call to GeneratePurchaseOrderPdfEndpoint                    │
│  2. Server generates professional PDF document                       │
│  3. Stream returned to client                                        │
│  4. Convert to base64                                                │
│  5. JavaScript interop triggers browser download                     │
│                                                                       │
│  ✅ PDF report downloaded successfully                                │
│                                                                       │
│  File: PurchaseOrder_abc123_20251025143022.pdf                      │
│                                                                       │
└──────────────────────────────────────────────────────────────────────┘
```

---

## 🎨 Status Colors and Indicators

### Status Display
```
Draft      → Gray/Default  → Order being created
Submitted  → Info/Blue     → Awaiting approval
Approved   → Primary/Blue  → Ready to send
Sent       → Warning/Orange→ With supplier, awaiting delivery
Received   → Success/Green → Goods received
Cancelled  → Error/Red     → Order cancelled
```

### Urgent Indicator
```
🔸 Urgent chip → Orange warning color
   Shown in details view when IsUrgent = true
   Sortable/filterable in main list
```

---

## 🔌 API Endpoint Mapping

```
┌──────────────────────────────────────────────────────────────────┐
│ USER ACTION              │ API ENDPOINT                          │
├──────────────────────────────────────────────────────────────────┤
│ Load page               │ SearchPurchaseOrdersEndpointAsync     │
│ Load supplier filter    │ SearchSuppliersEndpointAsync          │
│ Create order            │ CreatePurchaseOrderEndpointAsync      │
│ Update order            │ UpdatePurchaseOrderEndpointAsync      │
│ Delete order            │ DeletePurchaseOrderEndpointAsync      │
│ View details            │ GetPurchaseOrderEndpointAsync         │
│ Load supplier name      │ GetSupplierEndpointAsync              │
│ Submit for approval     │ SubmitPurchaseOrderEndpointAsync      │
│ Approve order           │ ApprovePurchaseOrderEndpointAsync     │
│ Send to supplier        │ SendPurchaseOrderEndpointAsync        │
│ Mark as received        │ ReceivePurchaseOrderEndpointAsync     │
│ Cancel order            │ CancelPurchaseOrderEndpointAsync      │
│ Download PDF            │ GeneratePurchaseOrderPdfEndpointAsync │
│ Load order items        │ GetPurchaseOrderItemsEndpointAsync    │
│ Add item                │ AddPurchaseOrderItemEndpointAsync     │
│ Update item quantity    │ UpdatePurchaseOrderItemQuantityAsync  │
│ Delete item             │ RemovePurchaseOrderItemEndpointAsync  │
└──────────────────────────────────────────────────────────────────┘
```

---

## 📊 Data Flow Diagram

```
                        ┌─────────────────┐
                        │   USER INPUT    │
                        └────────┬────────┘
                                 │
                                 ▼
                        ┌─────────────────┐
                        │  FORM/DIALOG    │
                        │   (Blazor UI)   │
                        └────────┬────────┘
                                 │
                         Validate Input
                                 │
                                 ▼
                        ┌─────────────────┐
                        │  VIEWMODEL /    │
                        │  MODEL CLASS    │
                        └────────┬────────┘
                                 │
                          .Adapt<>()
                                 │
                                 ▼
                        ┌─────────────────┐
                        │   COMMAND /     │
                        │   REQUEST       │
                        └────────┬────────┘
                                 │
                        API Client Call
                                 │
                                 ▼
                        ┌─────────────────┐
                        │  API ENDPOINT   │
                        │   (Backend)     │
                        └────────┬────────┘
                                 │
                         Process Request
                                 │
                                 ▼
                        ┌─────────────────┐
                        │    DATABASE     │
                        └────────┬────────┘
                                 │
                          Read/Write
                                 │
                                 ▼
                        ┌─────────────────┐
                        │    RESPONSE     │
                        └────────┬────────┘
                                 │
                          .Adapt<>()
                                 │
                                 ▼
                        ┌─────────────────┐
                        │  UI UPDATE /    │
                        │  NOTIFICATION   │
                        └─────────────────┘
```

---

## 🔐 Status-Based Action Matrix

```
┌──────────────┬──────┬─────┬────────┬─────────┬──────┬─────────┬────────┬──────┬────────┐
│   STATUS     │ VIEW │ PDF │ SUBMIT │ APPROVE │ SEND │ RECEIVE │ CANCEL │ EDIT │ DELETE │
├──────────────┼──────┼─────┼────────┼─────────┼──────┼─────────┼────────┼──────┼────────┤
│ Draft        │  ✅  │ ✅  │   ✅   │    ❌   │  ❌  │    ❌   │   ✅   │  ✅  │   ✅   │
│ Submitted    │  ✅  │ ✅  │   ❌   │    ✅   │  ❌  │    ❌   │   ✅   │  ❌  │   ❌   │
│ Approved     │  ✅  │ ✅  │   ❌   │    ❌   │  ✅  │    ❌   │   ✅   │  ❌  │   ❌   │
│ Sent         │  ✅  │ ✅  │   ❌   │    ❌   │  ❌  │    ✅   │   ❌   │  ❌  │   ❌   │
│ Received     │  ✅  │ ✅  │   ❌   │    ❌   │  ❌  │    ❌   │   ❌   │  ❌  │   ❌   │
│ Cancelled    │  ✅  │ ✅  │   ❌   │    ❌   │  ❌  │    ❌   │   ❌   │  ❌  │   ❌   │
└──────────────┴──────┴─────┴────────┴─────────┴──────┴─────────┴────────┴──────┴────────┘
```

---

## 📦 Component Hierarchy

```
PurchaseOrders.razor (Main Page)
│
├── PageHeader
│
├── EntityTable
│   ├── AdvancedSearchContent
│   │   ├── MudSelect (Supplier)
│   │   ├── MudSelect (Status)
│   │   ├── MudDatePicker (From Date)
│   │   └── MudDatePicker (To Date)
│   │
│   ├── ExtraActions (Context Menu)
│   │   ├── View Details
│   │   ├── Download PDF
│   │   ├── Submit for Approval
│   │   ├── Approve Order
│   │   ├── Send to Supplier
│   │   ├── Mark as Received
│   │   └── Cancel Order
│   │
│   └── EditFormContent (Create/Update Form)
│       ├── MudTextField (Order Number)
│       ├── AutocompleteSupplier
│       ├── MudDatePicker (Order Date)
│       ├── MudDatePicker (Expected Delivery)
│       ├── MudTextField (Delivery Address)
│       ├── MudTextField (Contact Person)
│       ├── MudTextField (Contact Phone)
│       ├── MudCheckBox (Is Urgent)
│       ├── MudNumericField (Tax Amount)
│       ├── MudNumericField (Discount Amount)
│       └── MudTextField (Notes)
│
└── Dialogs (opened via methods)
    ├── PurchaseOrderDetailsDialog
    │   ├── MudSimpleTable (Header Info)
    │   └── PurchaseOrderItems (Embedded Component)
    │       ├── MudTable (Items List)
    │       ├── Add Item Button
    │       └── Edit/Delete per item
    │
    └── PurchaseOrderItemDialog
        ├── AutocompleteItem
        ├── MudNumericField (Quantity)
        ├── MudNumericField (Unit Price)
        ├── MudNumericField (Discount Amount)
        └── MudTextField (Notes)
```

---

## 💰 Financial Calculation Flow

```
┌────────────────────────────────────────────────────────────┐
│                   ITEM CALCULATION                          │
└────────────────────────────────────────────────────────────┘

Quantity: 10 units
Unit Price: $25.00
Discount: $2.00

Item Total = (Quantity × Unit Price) - Discount
Item Total = (10 × $25.00) - $2.00
Item Total = $250.00 - $2.00
Item Total = $248.00

┌────────────────────────────────────────────────────────────┐
│                   ORDER CALCULATION                         │
└────────────────────────────────────────────────────────────┘

Item 1 Total: $248.00
Item 2 Total: $250.00
─────────────────────
Subtotal: $498.00

Tax Amount: +$50.00
Order Discount: -$25.00
─────────────────────
Total Amount: $523.00
Net Amount: $523.00 (amount to pay)
```

---

## 🧪 Test Scenarios

### Scenario 1: Happy Path - Complete Order
```
1. Create order (Draft) ✅
2. Add 2 items ✅
3. Review totals ✅
4. Submit for approval (→ Submitted) ✅
5. Approve order (→ Approved) ✅
6. Download PDF ✅
7. Send to supplier (→ Sent) ✅
8. Supplier ships goods 📦
9. Mark as received (→ Received) ✅
```

### Scenario 2: PDF Download
```
1. Select any order ✅
2. Click "Download PDF" ✅
3. System generates PDF 📄
4. File downloads automatically ⬇️
5. Email PDF to supplier 📧
```

### Scenario 3: Order Cancellation
```
1. Order in Draft/Submitted/Approved status ✅
2. Circumstances change ❌
3. Click "Cancel Order" ✅
4. Confirm cancellation ✅
5. Order moves to Cancelled status ✅
6. Cannot be modified further 🔒
```

### Scenario 4: Item Management
```
1. Create draft order ✅
2. Add item A (10 units @ $25) ✅
3. Add item B (5 units @ $50) ✅
4. Total updates to $750 💰
5. Edit item A quantity to 15 ✏️
6. Total updates to $875 💰
7. Delete item B 🗑️
8. Total updates to $625 💰
```

---

## 📋 Validation Rules Summary

### Order Form
```
✅ Order Number     → Required, string, max 50
✅ Supplier         → Required selection
✅ Order Date       → Required, valid date
✅ Expected Delivery→ Optional, valid date, >= Order Date
✅ Delivery Address → Optional, string, max 500
✅ Contact Person   → Optional, string, max 100
✅ Contact Phone    → Optional, string, max 20
✅ Is Urgent        → Boolean flag
✅ Tax Amount       → Optional, decimal, >= 0
✅ Discount Amount  → Optional, decimal, >= 0
✅ Notes            → Optional, string, max 1000
```

### Item Form
```
✅ Item            → Required selection
✅ Quantity        → Required, integer, >= 1
✅ Unit Price      → Required, decimal, >= 0.01
✅ Discount Amount → Optional, decimal, >= 0, <= (Quantity × Unit Price)
✅ Notes           → Optional, string, max 500
```

---

## 🔗 Integration Points

### Goods Receipts Module
```
Purchase Order (Sent) ─────→ Goods Receipt (Created from PO)
         │                            │
    Item details                 Receive quantities
         │                            │
    Expected qty                  Actual received
         │                            │
         └────────────────────────────┘
              Tracking & Variance
```

### Suppliers Module
```
Purchase Order ─────→ Supplier Selection
         │                    │
    Create order         Load supplier info
         │                    │
    Display name         Populate details
         │                    │
         └────────────────────┘
            Relationship
```

### Items Module
```
Purchase Order Item ─────→ Item Selection
         │                       │
    Add to order            Load item details
         │                       │
    Display SKU/Name        Default pricing
         │                       │
         └───────────────────────┘
              Product Link
```

---

## 🎯 Quick Reference

### Route
```
/store/purchase-orders
```

### Main Classes
```
PurchaseOrders                    (Main page)
PurchaseOrderViewModel            (Form binding)
PurchaseOrderDetailsDialog        (Details view)
PurchaseOrderItems                (Items component)
PurchaseOrderItemDialog           (Add/Edit item)
PurchaseOrderItemModel            (Item data model)
```

### Key Methods
```
ViewOrderDetails()                View order details
SubmitOrder()                     Submit for approval
ApproveOrder()                    Approve order
SendOrder()                       Send to supplier
ReceiveOrder()                    Mark as received
CancelOrder()                     Cancel order
DownloadPdf()                     Generate and download PDF
```

### Commands/Requests
```
SearchPurchaseOrdersCommand       Search/filter orders
CreatePurchaseOrderCommand        Create new order
UpdatePurchaseOrderCommand        Update order
SubmitPurchaseOrderRequest        Submit workflow
ApprovePurchaseOrderRequest       Approve workflow
SendPurchaseOrderRequest          Send workflow
ReceivePurchaseOrderRequest       Receive workflow
CancelPurchaseOrderRequest        Cancel workflow
AddPurchaseOrderItemCommand       Add item
UpdatePurchaseOrderItemQuantity   Update item
```

---

## ✨ Key Features Summary

- ✅ **Full CRUD** with EntityTable
- ✅ **6 Workflow Operations** (Submit, Approve, Send, Receive, Cancel, PDF)
- ✅ **4 Dialogs/Components** (Details, Items, Add Item, Edit Item)
- ✅ **4 Search Filters** (Supplier, Status, Date range)
- ✅ **Multi-Item Support** with automatic totals
- ✅ **Financial Tracking** (totals, tax, discounts)
- ✅ **PDF Generation** with professional formatting
- ✅ **Status-based Actions** with proper validation
- ✅ **Goods Receipt Integration** for receiving
- ✅ **Comprehensive Documentation** (4+ documents)

---

*Visual map created: October 25, 2025*  
*Status: ✅ Complete and Production Ready*

