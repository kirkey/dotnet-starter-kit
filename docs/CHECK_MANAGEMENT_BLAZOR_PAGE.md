# Check Management Page - Implementation Summary

## Overview
A comprehensive Blazor page for managing checks in the accounting system, following established patterns from other accounting pages (Accounting Periods, Accruals, Payees).

## Files Created

### 1. CheckViewModel.cs
**Location:** `/src/apps/blazor/client/Pages/Accounting/Checks/CheckViewModel.cs`

**Purpose:** View model for check add/edit operations

**Properties:**
- Basic Info: Id, CheckNumber, BankAccountCode, BankAccountName, Status
- Payment Details: Amount, PayeeName, VendorId, PayeeId
- Dates: IssuedDate, ClearedDate, VoidedDate
- Status Tracking: IsPrinted, IsStopPayment, PrintedDate, PrintedBy
- Linked Transactions: PaymentId, ExpenseId
- Additional: Memo, VoidReason, StopPaymentReason, Description, Notes

### 2. Checks.razor
**Location:** `/src/apps/blazor/client/Pages/Accounting/Checks/Checks.razor`

**Route:** `/accounting/checks`

**Features:**

#### Advanced Search Filters
- Check Number (text search)
- Bank Account Code (text search)
- Status (dropdown: Available, Issued, Cleared, Void, Stale, StopPayment)
- Payee Name (text search)
- Amount Range (From/To with currency formatting)
- Issued Date Range (From/To date pickers)
- Printed Checks Only (toggle)

#### Context Actions Menu
Dynamic actions based on check status:
- **Available Checks:** Issue Check, Void Check
- **Issued Checks:** Mark as Printed, Mark as Cleared, Stop Payment, Void Check

#### Edit Form Content
- Check identification (Check Number, Bank Account via autocomplete)
- Status badge (color-coded by status)
- Read-only payment details (Amount, Payee, Dates) for issued checks
- Descriptive fields (Description, Notes)
- Status indicators (Printed, Stop Payment with icons)

#### Action Dialogs
1. **Issue Check Dialog**
   - Amount input (required, currency format)
   - Issue Date picker
   - Payee Name (required)
   - Memo field

2. **Void Check Dialog**
   - Void Reason (required, min 5 chars)
   - Void Date picker

3. **Clear Check Dialog**
   - Cleared Date picker (required)

4. **Stop Payment Dialog**
   - Stop Payment Reason (required, min 10 chars)
   - Stop Payment Date picker

### 3. Checks.razor.cs
**Location:** `/src/apps/blazor/client/Pages/Accounting/Checks/Checks.razor.cs`

**Key Components:**

#### Entity Table Context
- **Entity:** CheckSearchResponse
- **ID Type:** DefaultIdType (Guid)
- **View Model:** CheckViewModel

#### Table Fields (10 columns)
1. Check Number
2. Account (Bank Account Code)
3. Account Name
4. Status (with color-coded badges)
5. Amount (currency formatted)
6. Payee
7. Issued Date
8. Cleared Date
9. Printed (icon indicator)
10. Stop Payment (icon indicator)

#### CRUD Operations
- **Search:** Paginated search with 9 filter criteria
- **Create:** Register new check (Available status)
- **Read:** Get check details by ID
- **Update:** N/A (updates done via specialized operations)
- **Delete:** Disabled (use Void instead)

#### Specialized Operations
1. **Issue Check** (`OnIssueCheck`)
   - Validates amount > 0
   - Validates payee name is provided
   - Calls `CheckIssueEndpointAsync`

2. **Void Check** (`OnVoidCheck`)
   - Validates void reason min 5 characters
   - Calls `CheckVoidEndpointAsync`

3. **Clear Check** (`OnClearCheck`)
   - Calls `CheckClearEndpointAsync`

4. **Stop Payment** (`OnStopPayment`)
   - Validates stop payment reason min 10 characters
   - Calls `CheckStopPaymentEndpointAsync`

5. **Print Check** (`OnPrintCheck`)
   - Direct action (no dialog)
   - Auto-assigns "System" as printed by
   - Calls `CheckPrintEndpointAsync`

#### Status Color Mapping
- **Available:** Info (Blue)
- **Issued:** Primary (Purple)
- **Cleared:** Success (Green)
- **Void:** Error (Red)
- **Stale:** Warning (Orange)
- **StopPayment:** Warning (Orange)

## API Integration

### Endpoints Used
- `POST /accounting/checks/search` - Search checks with filters
- `POST /accounting/checks` - Register new check
- `GET /accounting/checks/{id}` - Get check details
- `POST /accounting/checks/issue` - Issue check for payment
- `POST /accounting/checks/void` - Void a check
- `POST /accounting/checks/clear` - Mark check as cleared
- `POST /accounting/checks/stop-payment` - Request stop payment
- `POST /accounting/checks/print` - Mark check as printed

### Commands/Queries
- CheckSearchQuery (with 14 filter properties)
- CheckCreateCommand
- CheckIssueCommand
- CheckVoidCommand
- CheckClearCommand
- CheckStopPaymentCommand
- CheckPrintCommand

### Responses
- CheckSearchResponse (summary for list view)
- CheckGetResponse (detailed view)

## Design Patterns & Consistency

### Following Established Patterns
1. **EntityTable Component** - Standard CRUD table with advanced search
2. **Page Header** - Title, header, and subheader
3. **MudBlazor Components** - Consistent UI library usage
4. **AutocompleteChartOfAccountCode** - Bank account selection
5. **Dialog Options** - Standard dialog configuration
6. **Error Handling** - Try-catch with Snackbar notifications
7. **Validation** - Client-side validation before API calls

### Naming Conventions
- File naming: PascalCase (Checks.razor, CheckViewModel.cs)
- Route: lowercase with hyphens (/accounting/checks)
- Methods: OnAction pattern (OnIssueCheck, OnVoidCheck)
- Private fields: _camelCase
- Dialog visibility: _dialogNameVisible

## User Experience

### Workflow
1. **Register Checks:** Create available checks from check book
2. **Issue Checks:** Assign checks to payments/payees with amount
3. **Track Status:** Monitor check lifecycle (Available → Issued → Cleared)
4. **Handle Exceptions:** Void errors or request stop payments
5. **Reconciliation:** Mark checks as cleared during bank reconciliation

### Status Progression
```
Available → Issued → Cleared
     ↓         ↓
   Void     Void
     ↓         ↓
          StopPayment
             ↓
           Stale
```

### Validation Rules
- Check number required (alphanumeric)
- Bank account code required
- Issue amount must be > 0
- Payee name required when issuing
- Void reason min 5 characters
- Stop payment reason min 10 characters
- Only available checks can be deleted
- Only issued checks can be cleared
- Issued checks cannot be reused

## Navigation Setup

### Route Registration
**Route:** `/accounting/checks`

To add to navigation menu, update the appropriate navigation configuration file:
```csharp
new NavItem
{
    Title = "Checks",
    Href = "/accounting/checks",
    Icon = Icons.Material.Filled.Receipt,
    RequiredPermissions = new[] { FshResources.Accounting }
}
```

### Permissions Required
- **View:** `Permissions.Accounting.View`
- **Create:** `Permissions.Accounting.Create`
- **Update:** `Permissions.Accounting.Update` (for issue, void, clear, print, stop payment)

## Testing Checklist

### Functionality
- [ ] Register new check (Available status)
- [ ] Issue check with valid amount and payee
- [ ] Void available check
- [ ] Void issued check
- [ ] Clear issued check
- [ ] Stop payment on issued check
- [ ] Mark check as printed
- [ ] Search by check number
- [ ] Search by status
- [ ] Search by date range
- [ ] Search by amount range
- [ ] Advanced search filters work correctly

### Validation
- [ ] Cannot issue check without payee name
- [ ] Cannot issue check with zero/negative amount
- [ ] Void reason must be at least 5 characters
- [ ] Stop payment reason must be at least 10 characters
- [ ] Only available/issued checks show void action
- [ ] Only issued checks show clear/stop payment actions
- [ ] Status badges display correct colors

### Edge Cases
- [ ] Handle API errors gracefully
- [ ] Pagination works with large datasets
- [ ] Sorting works on all columns
- [ ] Dialog close/cancel functions properly
- [ ] Refresh data after operations
- [ ] Read-only fields cannot be edited

## Future Enhancements

### Potential Additions
1. **Bulk Operations**
   - Register multiple checks from check book
   - Batch void stale checks
   - Export checks for reconciliation

2. **Reporting**
   - Outstanding checks report
   - Cleared checks by period
   - Voided checks audit trail

3. **Integration**
   - Link to vendor payments
   - Link to expense transactions
   - Check printing with templates

4. **Advanced Features**
   - Automatic stale check marking
   - Bank reconciliation integration
   - Check signature tracking
   - Multi-signature requirements

## Related Documentation
- Check Domain Entity: `/src/api/modules/Accounting/Accounting.Domain/Entities/Check.cs`
- Check Application Layer: `/src/api/modules/Accounting/Accounting.Application/Checks/`
- Check Endpoints: `/src/api/modules/Accounting/Accounting.Infrastructure/Endpoints/Checks/`
- API Documentation: `/src/api/modules/Accounting/CHECK_MANAGEMENT_IMPLEMENTATION.md`
