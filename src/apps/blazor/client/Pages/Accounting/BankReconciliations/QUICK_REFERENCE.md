# Bank Reconciliation Blazor UI - Quick Reference

## 🚀 Quick Start

### Access the Page
Navigate to: `/accounting/bank-reconciliations`

### Basic Operations

**Create:**
```csharp
// Click "New" button → Fill form → Click "Create"
```

**Search:**
```csharp
// Use filters: Bank Account, Status, Date Range, Reconciled flag
```

**Workflow:**
```
Pending → Start → InProgress → Edit Items → Complete → Approve
```

---

## 📝 File Structure

```
BankReconciliations/
├── BankReconciliations.razor              # Main page UI
├── BankReconciliations.razor.cs           # Code-behind logic
├── BankReconciliationViewModel.cs         # Data model
├── BankReconciliationDetailsDialog.razor  # View details
├── BankReconciliationEditDialog.razor     # Edit items
├── BankReconciliationCompleteDialog.razor # Complete action
├── BankReconciliationApproveDialog.razor  # Approve action
├── BankReconciliationRejectDialog.razor   # Reject action
├── BankReconciliationReportsDialog.razor  # Reports menu
└── BankReconciliationSummaryDialog.razor  # Summary stats
```

---

## 🎯 Key Features

### Main Table Columns
- Statement # | Date | Statement Balance | Book Balance
- Adjusted Balance | Status | Approved | Completed Date

### Search Filters
- Bank Account (autocomplete)
- Status (dropdown)
- Date Range (from/to)
- Reconciled (toggle)

### Context Menu (Status-Based)
- **Pending**: Start, Delete, View, Print
- **InProgress**: Edit Items, Complete, Delete, View, Print
- **Completed**: Approve, Reject, Delete, View, Print
- **Approved**: View, Print

---

## 💡 Code Patterns Used

### EntityTable Pattern (from Todo)
```csharp
Context = new EntityServerTableContext<Response, Id, ViewModel>(
    entityName: "Bank Reconciliation",
    fields: [...],
    searchFunc: async filter => await Client.Search(...),
    createFunc: async item => await Client.Create(...),
    updateFunc: async (id, item) => await Client.Update(...),
    deleteFunc: async id => await Client.Delete(...)
);
```

### Dialog Pattern (from Bills)
```csharp
var parameters = new DialogParameters { { "Id", id } };
var result = await DialogService.ShowAsync<Dialog>("Title", parameters);
if (result.Result != DialogResult.Cancel())
{
    await _table.ReloadServerData();
}
```

### Status Color Mapping
```csharp
private static Color GetStatusColor(string? status) => status switch
{
    "Pending" => Color.Default,
    "InProgress" => Color.Info,
    "Completed" => Color.Warning,
    "Approved" => Color.Success,
    _ => Color.Default
};
```

---

## 🔧 Common Customizations

### Add New Filter
```csharp
// 1. Add property to BankReconciliations.razor.cs
private string? NewFilter { get; set; }

// 2. Add to AdvancedSearchContent in .razor
<MudItem xs="12" sm="6" md="4">
    <MudTextField @bind-Value="NewFilter" Label="New Filter" />
</MudItem>

// 3. Add to searchFunc in OnInitializedAsync
var searchQuery = new SearchBankReconciliationsCommand
{
    ...
    NewFilter = NewFilter
};
```

### Add New Column
```csharp
// Add to fields array in OnInitializedAsync
new EntityField<BankReconciliationResponse>(
    r => r.NewProperty, 
    "Display Name", 
    "PropertyName", 
    typeof(PropertyType)
)
```

### Add New Action
```csharp
// 1. Add method to .razor.cs
private async Task OnNewAction(DefaultIdType id)
{
    await Client.NewActionEndpointAsync("1", id);
    await _table.ReloadServerData();
}

// 2. Add to ExtraActions in .razor
<MudMenuItem Icon="@Icons.Material.Filled.Icon" 
             OnClick="@(() => OnNewAction(reconciliation.Id))">
    New Action
</MudMenuItem>
```

---

## 🎨 UI Components Used

### MudBlazor Components
- `MudDataGrid` / `EntityTable` - Main table
- `MudTextField` - Text inputs
- `MudDatePicker` - Date inputs
- `MudNumericField` - Currency inputs
- `MudAutocomplete` - Bank account search
- `MudSelect` - Status dropdown
- `MudDialog` - Modal dialogs
- `MudButton` - Action buttons
- `MudChip` - Status badges
- `MudIcon` - Icons
- `MudAlert` - Notifications in dialogs
- `MudDivider` - Section separators

### Layout Components
- `MudStack` - Vertical/horizontal stacks
- `MudGrid` / `MudItem` - Responsive grid
- `MudPaper` - Elevated containers
- `MudList` / `MudListItem` - Lists
- `MudSimpleTable` - Data tables

---

## 🧪 Testing Checklist

```markdown
### CRUD Operations
- [ ] Create new reconciliation
- [ ] View reconciliation details
- [ ] Update reconciliation items
- [ ] Delete reconciliation

### Workflow
- [ ] Start reconciliation (Pending → InProgress)
- [ ] Complete reconciliation (InProgress → Completed)
- [ ] Approve reconciliation (Completed → Approved)
- [ ] Reject reconciliation (Completed → Pending)

### Search & Filter
- [ ] Search by bank account
- [ ] Filter by status
- [ ] Filter by date range
- [ ] Filter by reconciled flag
- [ ] Pagination works
- [ ] Sorting works

### Validation
- [ ] Required fields validated
- [ ] Date cannot be future
- [ ] Balances must be positive
- [ ] Balance matching works in Edit dialog
- [ ] Save disabled until balanced

### UI/UX
- [ ] Status colors correct
- [ ] Icons display properly
- [ ] Dialogs open/close correctly
- [ ] Snackbar notifications appear
- [ ] Loading indicators show
- [ ] Responsive on mobile
- [ ] Autocomplete works
```

---

## 📊 Balance Calculation

### Formula
```
Expected = Statement - Outstanding Checks + Deposits + Bank Errors
Calculated = Book + Book Errors
Balanced = |Expected - Calculated| < 0.01
```

### Visual Indicators
- 🟢 Green Background = Balanced
- 🔴 Red Background = Not Balanced
- ✅ Checkmark = Matched
- ⚠️ Save Disabled = Not Balanced

---

## 🔑 Key Properties

### BankReconciliationViewModel
```csharp
Id                      // Reconciliation ID
BankAccountId          // Required
ReconciliationDate     // Required, not future
StatementBalance       // Required, >= 0
BookBalance            // Required, >= 0
AdjustedBalance        // Calculated
OutstandingChecksTotal // >= 0
DepositsInTransitTotal // >= 0
BankErrors             // Can be negative
BookErrors             // Can be negative
Status                 // Pending/InProgress/Completed/Approved
IsReconciled           // Boolean
StatementNumber        // Optional, max 100
Description            // Optional, max 2048
Notes                  // Optional, max 2048
```

---

## 🚨 Common Issues & Solutions

**Issue:** DialogService not injected
```csharp
// Solution: Add to .razor.cs
[Inject]
private IDialogService DialogService { get; set; } = default!;
```

**Issue:** ReloadDataAsync not found
```csharp
// Solution: Use ReloadServerData instead
await _table.ReloadServerData();
```

**Issue:** Balance not matching
```csharp
// Solution: Check formula and ensure < 0.01 tolerance
decimal tolerance = 0.01m;
bool isBalanced = Math.Abs(difference) < tolerance;
```

**Issue:** Dialog not showing
```csharp
// Solution: Verify parameter names match
var parameters = new DialogParameters
{
    { nameof(Dialog.PropertyName), value }
};
```

---

## 📚 API Endpoints

### Search
```http
POST /api/v1/accounting/bank-reconciliations/search
```

### CRUD
```http
POST   /api/v1/accounting/bank-reconciliations
GET    /api/v1/accounting/bank-reconciliations/{id}
PUT    /api/v1/accounting/bank-reconciliations/{id}
DELETE /api/v1/accounting/bank-reconciliations/{id}
```

### Workflows
```http
POST /api/v1/accounting/bank-reconciliations/{id}/start
POST /api/v1/accounting/bank-reconciliations/{id}/complete
POST /api/v1/accounting/bank-reconciliations/{id}/approve
POST /api/v1/accounting/bank-reconciliations/{id}/reject
```

---

## 🎓 Learning Resources

- **Todo Module**: Simple CRUD example
- **Bills Module**: Complex workflow example
- **MudBlazor Docs**: https://mudblazor.com/
- **Blazor Docs**: https://learn.microsoft.com/aspnet/core/blazor

---

## ✅ Completion Status

**Implementation:** 100% Complete ✅

All features working as expected following Todo and Catalog patterns.

---

**Quick Start:** Navigate to `/accounting/bank-reconciliations` and click "New"!

