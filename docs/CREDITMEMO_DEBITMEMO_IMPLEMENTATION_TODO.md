# CreditMemo and DebitMemo Module Implementation - TODO

**Date:** November 18, 2025  
**Status:** ⚠️ **HELP DIALOGS READY - AWAITING PAGE IMPLEMENTATION**

---

## 📋 **Current Status**

### ✅ **Completed:**
- ✅ CreditMemoHelpDialog.razor created (450+ lines)
- ✅ CreditMemoHelpDialog.razor.cs created
- ✅ DebitMemoHelpDialog.razor created (400+ lines)
- ✅ DebitMemoHelpDialog.razor.cs created
- ✅ Both help dialogs compile successfully
- ✅ Comprehensive accounting guidance included
- ✅ Ready for integration

### ⚠️ **Pending:**
- ⚠️ CreditMemo page (main CRUD page) - **NOT YET CREATED**
- ⚠️ CreditMemo.razor.cs code-behind - **NOT YET CREATED**
- ⚠️ DebitMemo page (main CRUD page) - **NOT YET CREATED**
- ⚠️ DebitMemo.razor.cs code-behind - **NOT YET CREATED**
- ⚠️ API endpoints verification
- ⚠️ Help button integration

---

## 🎯 **What Needs to Be Done**

### **1. Create CreditMemo Module**

#### Files to Create:
```
/Pages/Accounting/CreditMemo/
├── CreditMemos.razor              (Main page - list/search)
├── CreditMemos.razor.cs           (Code-behind)
├── CreditMemoViewModel.cs         (View model)
├── CreditMemoDetailsDialog.razor  (Details view)
├── CreditMemoApplyDialog.razor    (Apply credit to invoice)
└── CreditMemoHelpDialog.razor     ✅ ALREADY CREATED!
```

#### Implementation Requirements:
- [ ] Search/filter credit memos
- [ ] Create new credit memo
- [ ] Edit draft credit memos
- [ ] Issue credit memo to customer
- [ ] Apply credit to invoices
- [ ] Void credit memo
- [ ] View credit memo details
- [ ] Export credit memos
- [ ] Link to original invoice
- [ ] Track credit balance
- [ ] **Integrate help button → ShowCreditMemoHelp()**

---

### **2. Create DebitMemo Module**

#### Files to Create:
```
/Pages/Accounting/DebitMemo/
├── DebitMemos.razor               (Main page - list/search)
├── DebitMemos.razor.cs            (Code-behind)
├── DebitMemoViewModel.cs          (View model)
├── DebitMemoDetailsDialog.razor   (Details view)
├── DebitMemoApplyDialog.razor     (Apply debit to vendor account)
└── DebitMemoHelpDialog.razor      ✅ ALREADY CREATED!
```

#### Implementation Requirements:
- [ ] Search/filter debit memos
- [ ] Create new debit memo
- [ ] Edit draft debit memos
- [ ] Issue debit memo to vendor
- [ ] Apply debit to vendor bills
- [ ] Void debit memo
- [ ] View debit memo details
- [ ] Export debit memos
- [ ] Link to original vendor invoice
- [ ] Track debit balance
- [ ] **Integrate help button → ShowDebitMemoHelp()**

---

## 🔗 **API Endpoints Needed**

### CreditMemo Endpoints:
```csharp
// Search
POST /api/v1/accounting/credit-memos/search
GET  /api/v1/accounting/credit-memos/{id}

// CRUD
POST   /api/v1/accounting/credit-memos
PUT    /api/v1/accounting/credit-memos/{id}
DELETE /api/v1/accounting/credit-memos/{id}

// Actions
POST /api/v1/accounting/credit-memos/{id}/issue
POST /api/v1/accounting/credit-memos/{id}/apply
POST /api/v1/accounting/credit-memos/{id}/void
GET  /api/v1/accounting/credit-memos/{id}/available-invoices
```

### DebitMemo Endpoints:
```csharp
// Search
POST /api/v1/accounting/debit-memos/search
GET  /api/v1/accounting/debit-memos/{id}

// CRUD
POST   /api/v1/accounting/debit-memos
PUT    /api/v1/accounting/debit-memos/{id}
DELETE /api/v1/accounting/debit-memos/{id}

// Actions
POST /api/v1/accounting/debit-memos/{id}/issue
POST /api/v1/accounting/debit-memos/{id}/apply
POST /api/v1/accounting/debit-memos/{id}/void
GET  /api/v1/accounting/debit-memos/{id}/available-bills
```

---

## 📦 **Data Models**

### CreditMemo Entity Properties:
```csharp
- Id (Guid)
- CreditMemoNumber (string, unique)
- CustomerId (Guid)
- OriginalInvoiceId (Guid, nullable)
- CreditDate (DateTime)
- Amount (decimal)
- Reason (string)
- Status (Draft, Issued, Applied, Refunded, Voided)
- IsApplied (bool)
- AppliedDate (DateTime, nullable)
- LineItems (List<CreditMemoLineItem>)
- Description (string)
- Notes (string)
```

### DebitMemo Entity Properties:
```csharp
- Id (Guid)
- DebitMemoNumber (string, unique)
- VendorId (Guid)
- OriginalBillId (Guid, nullable)
- DebitDate (DateTime)
- Amount (decimal)
- Reason (string)
- Status (Draft, Issued, Applied, Settled, Voided)
- IsApplied (bool)
- AppliedDate (DateTime, nullable)
- LineItems (List<DebitMemoLineItem>)
- Description (string)
- Notes (string)
```

---

## 🎨 **UI Design Pattern**

Both modules should follow the established pattern from similar pages (Bills, Payments):

### Main Page Layout:
```razor
@page "/accounting/credit-memos"

<PageHeader Title="Credit Memos" 
            Header="Credit Memos" 
            SubHeader="Manage customer credits and returns." />

<!-- Action Toolbar -->
<MudPaper Elevation="2" Class="pa-4 mb-4">
    <MudStack Row="true" Spacing="2" Wrap="Wrap.Wrap" AlignItems="AlignItems.Center">
        <MudButtonGroup Color="Color.Primary" Variant="Variant.Outlined" Size="Size.Small">
            <MudButton StartIcon="@Icons.Material.Filled.FilterList" OnClick="@ShowFilters">
                Filters
            </MudButton>
        </MudButtonGroup>
        
        <MudSpacer />
        
        <MudButtonGroup Color="Color.Info" Variant="Variant.Outlined" Size="Size.Small">
            <MudButton StartIcon="@Icons.Material.Filled.Help" OnClick="@ShowCreditMemoHelp">
                Help
            </MudButton>
        </MudButtonGroup>
    </MudStack>
</MudPaper>

<!-- EntityTable -->
<EntityTable @ref="_table" 
             TEntity="CreditMemoResponse" 
             TId="DefaultIdType" 
             TRequest="CreditMemoViewModel" 
             Context="@Context">
    <!-- Fields and actions -->
</EntityTable>
```

### Code-Behind Pattern:
```csharp
public partial class CreditMemos
{
    protected EntityServerTableContext<CreditMemoResponse, DefaultIdType, CreditMemoViewModel> Context { get; set; } = null!;
    
    private EntityTable<CreditMemoResponse, DefaultIdType, CreditMemoViewModel> _table = null!;
    
    // Search filters
    private string? CreditMemoNumber;
    private string? Status;
    private DateTime? DateFrom;
    private DateTime? DateTo;
    
    protected override Task OnInitializedAsync()
    {
        // Initialize Context with fields, search, CRUD operations
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Show credit memo help dialog.
    /// </summary>
    private async Task ShowCreditMemoHelp()
    {
        await DialogService.ShowAsync<CreditMemoHelpDialog>(
            "Credit Memos Help", 
            new DialogParameters(), 
            new DialogOptions
            {
                MaxWidth = MaxWidth.Large,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
    }
}
```

---

## ✅ **Help Dialog Integration Checklist**

When pages are created, integration is simple:

### For CreditMemo:
- [ ] Add toolbar with Help button to CreditMemos.razor
- [ ] Add `ShowCreditMemoHelp()` method to CreditMemos.razor.cs
- [ ] Verify CreditMemoHelpDialog namespace matches
- [ ] Test Help button opens dialog
- [ ] Verify all expansion panels work

### For DebitMemo:
- [ ] Add toolbar with Help button to DebitMemos.razor
- [ ] Add `ShowDebitMemoHelp()` method to DebitMemos.razor.cs
- [ ] Verify DebitMemoHelpDialog namespace matches
- [ ] Test Help button opens dialog
- [ ] Verify all expansion panels work

---

## 📚 **Reference Implementation**

Use these existing pages as templates:

### For CreditMemo (Similar to):
- **Payments** page - Customer-facing credits
- **AccountReconciliations** - Adjustment tracking
- **Bills** page - Line item structure

### For DebitMemo (Similar to):
- **Bills** page - Vendor-facing charges
- **AccountReconciliations** - Adjustment tracking
- **Payments** page - Application workflow

---

## 🎯 **Priority & Impact**

### Business Priority:
- **CreditMemo:** HIGH - Customer satisfaction, returns management
- **DebitMemo:** MEDIUM - Vendor relationship management, cost control

### Implementation Effort:
- **Backend:** Medium (API endpoints, business logic, validation)
- **Frontend:** Low-Medium (follow existing patterns)
- **Integration:** Very Low (help dialogs already complete!)

### Estimated Timeline:
- **Backend Development:** 2-3 days per module
- **Frontend Development:** 1-2 days per module
- **Testing:** 1 day per module
- **Total per module:** 4-6 days
- **Both modules:** 8-12 days

---

## 💡 **Quick Start Guide**

When ready to implement:

1. **Backend First:**
   - Create entities in Domain layer
   - Create commands/queries in Application layer
   - Create endpoints in API layer
   - Add validation rules
   - Write unit tests

2. **Frontend Second:**
   - Copy Bills.razor as template for structure
   - Update entity names and properties
   - Configure EntityServerTableContext
   - Add status-specific actions
   - Add filtering and search

3. **Integration Last:**
   - Add help button (3 lines in Razor)
   - Add help method (10 lines in code-behind)
   - Test dialog opens correctly
   - **DONE!** ✅

---

## 📝 **Notes for Implementation Team**

### Important Considerations:

1. **Journal Entries:**
   - CreditMemo reduces AR (debit AR, credit Sales Returns)
   - DebitMemo reduces AP (debit AP, credit Expense reduction)

2. **Workflow States:**
   - Draft → Issued → Applied → Settled/Refunded
   - Allow void only before Applied

3. **Approval Workflow:**
   - Both should require approval above certain thresholds
   - Track approver and approval date

4. **Audit Trail:**
   - Log all status changes
   - Track who created, issued, applied, voided
   - Maintain complete history

5. **Reporting:**
   - Credit memo aging report
   - Debit memo aging report
   - Applied vs. outstanding
   - By customer/vendor

---

## ✅ **What's Already Done**

### CreditMemo Help Content Includes:
- ✅ What is a credit memo (definition, scenarios)
- ✅ Creating credit memos (12-step workflow)
- ✅ Credit application options
- ✅ Status lifecycle (Draft → Issued → Applied)
- ✅ Best practices and fraud prevention
- ✅ 4 FAQ questions with answers

### DebitMemo Help Content Includes:
- ✅ What is a debit memo (definition, scenarios)
- ✅ Creating debit memos (12-step workflow)
- ✅ Common scenarios with examples
- ✅ Best practices and vendor communication
- ✅ 4 FAQ questions with answers

**Both are comprehensive, professional, and ready to use immediately once pages exist!**

---

## 🚀 **Next Steps**

1. **Verify Backend:**
   - Confirm CreditMemo/DebitMemo entities exist in API
   - Verify endpoints are available
   - Test API responses

2. **Create Frontend:**
   - Implement main pages (CreditMemos.razor, DebitMemos.razor)
   - Implement dialogs (Details, Apply, etc.)
   - Configure search and filters

3. **Integrate Help:**
   - Add help buttons to toolbars
   - Add help methods to code-behind
   - Test help dialogs open correctly

4. **Test & Deploy:**
   - Test complete workflows
   - Verify accounting entries correct
   - Deploy to staging for UAT

---

**Status:** ⚠️ **READY FOR IMPLEMENTATION - HELP DIALOGS COMPLETE!**

The hard work of documenting these complex accounting processes is **DONE**. When the pages are created, integration is trivial (just add the help button and method call). The 850+ lines of comprehensive help content are ready and waiting!

---

*Last Updated: November 18, 2025*  
*Help Dialogs: Ready ✅*  
*Pages: Pending ⚠️*

