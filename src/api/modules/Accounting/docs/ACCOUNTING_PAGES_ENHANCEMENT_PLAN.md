# Accounting Pages Enhancement Plan

## Overview
Enhance existing accounting pages (Chart of Accounts, Accruals, Budgets, Payees, Projects) following the comprehensive pattern established in the Check Management page.

## Enhancement Pattern (Based on Check Management)

### Standard Features to Add:
1. **Advanced Search Filters** - Multiple criteria with proper field types
2. **Context Actions Menu** - Status-based actions appearing in row menu
3. **Action Dialogs** - Specialized operations (activate, reverse, close, etc.)
4. **Status Badges** - Color-coded status indicators
5. **Enhanced Display** - Formatted values (currency, dates, booleans)
6. **Read-only Fields** - Display-only data for issued/completed records
7. **Proper Validation** - Client-side validation before API calls
8. **Error Handling** - Try-catch with Snackbar notifications

---

## 1. Chart of Accounts Enhancement

### Current State
- Basic CRUD operations
- Simple form with account fields
- Basic table display

### Enhancements Needed

#### Advanced Search Filters (AdvancedSearchContent)
```csharp
- Account Code (text)
- Account Name (text)
- Account Type (dropdown: Asset, Liability, Equity, Revenue, Expense)
- USOA Category (autocomplete)
- Parent Account (autocomplete)
- Balance Range (From/To numeric)
- Status (dropdown: Active, Inactive)
- Control Account Only (toggle)
- Detail Account Only (toggle)
```

#### Context Actions Menu (ActionsContent)
```csharp
- Activate Account (if inactive)
- Deactivate Account (if active)
- View Transactions (navigate to GL)
- Export Account Details
```

#### Action Dialogs
1. **Activate Account Dialog**
   - Confirmation message
   - Effective date

2. **Deactivate Account Dialog**
   - Reason for deactivation (required)
   - Effective date
   - Warning about active transactions

#### Enhanced Display
- Status badge (Active/Inactive with colors)
- Balance with currency formatting
- Account Type badge
- Hierarchy level indicator
- Control vs Detail indicator

#### Additional Features
- Import/Export buttons in page header
- Account hierarchy tree view toggle
- Balance summary cards
- Quick filters for common account types

---

## 2. Accruals Enhancement

### Current State
- Basic CRUD
- Minimal fields displayed

### Enhancements Needed

#### Advanced Search Filters
```csharp
- Accrual Number (text)
- Date Range (From/To)
- Amount Range (From/To)
- Status (dropdown: Active, Reversed)
- Account Code (autocomplete)
- Description (text search)
```

#### Context Actions Menu
```csharp
- Reverse Accrual (if not reversed)
- View Journal Entry (if posted)
- Print Accrual Voucher
```

#### Action Dialogs
1. **Reverse Accrual Dialog**
   - Reversal date (required, defaults to current date)
   - Reversal reason (optional)
   - Confirmation warning

#### Enhanced Display
- Status badge (Active/Reversed)
- Amount with currency formatting
- Reversal indicator icon
- Days outstanding (for active accruals)

#### Additional Features
- Accrual aging report button
- Month-end accrual summary
- Auto-reversal scheduling (future)

---

## 3. Budgets with Budget Details Enhancement

### Current State
- Basic budget list
- Separate budget details page

### Enhancements Needed

#### Budgets Main Page

##### Advanced Search Filters
```csharp
- Budget Name (text)
- Fiscal Year (dropdown)
- Period (autocomplete period)
- Department (autocomplete if applicable)
- Status (dropdown: Draft, Approved, Active, Closed)
- Budget Type (dropdown: Operating, Capital, etc.)
```

##### Context Actions Menu
```csharp
- View Budget Lines (navigate to details)
- Approve Budget (if draft)
- Activate Budget (if approved)
- Close Budget (if active)
- Copy Budget (create new from existing)
- Print Budget Report
```

##### Action Dialogs
1. **Approve Budget Dialog**
   - Approver name
   - Approval date
   - Comments

2. **Close Budget Dialog**
   - Close date
   - Close reason
   - Warning about finalization

3. **Copy Budget Dialog**
   - New budget name (required)
   - Target fiscal year
   - Adjustment percentage (optional)

##### Enhanced Display
- Status badge with workflow indicator
- Total budget amount
- Actual vs Budget progress bar
- Variance percentage with color coding
- Period coverage indicator

#### Budget Details Page (Budget Lines)

##### Features
- Budget line items grid
- Add/Edit/Delete line items
- Account selection with autocomplete
- Monthly/Quarterly distribution
- Inline editing capability
- Totals and variance calculations
- Import budget lines from Excel

---

## 4. Payees Enhancement

### Current State
- Basic CRUD
- Image upload
- Simple table

### Enhancements Needed

#### Advanced Search Filters
```csharp
- Payee Code (text)
- Payee Name (text)
- TIN (text)
- Expense Account (autocomplete)
- Address (text search)
- Has Image (toggle)
- Status (Active/Inactive if applicable)
```

#### Context Actions Menu
```csharp
- View Payment History
- View Outstanding Invoices (if linked)
- Print 1099 Form (if TIN exists)
- Send Email (if email exists)
- Deactivate/Activate
```

#### Action Dialogs
1. **Deactivate Payee Dialog**
   - Reason (required)
   - Check for outstanding payments warning

#### Enhanced Display
- Image thumbnail in table
- TIN formatted (with masking for security)
- Expense account badge
- Payment statistics (total paid, count)
- Last payment date
- Active/Inactive status badge

#### Additional Features
- Payment history tab/dialog
- 1099 export functionality
- Bulk operations (activate/deactivate multiple)

---

## 5. Projects with Project Costing Enhancement

### Current State
- Basic project list
- Separate project costing page

### Enhancements Needed

#### Projects Main Page

##### Advanced Search Filters
```csharp
- Project Code (text)
- Project Name (text)
- Project Manager (text/autocomplete)
- Status (dropdown: Planning, Active, On Hold, Completed, Closed)
- Start Date Range (From/To)
- End Date Range (From/To)
- Budget Range (From/To)
- Customer/Department (autocomplete if applicable)
```

##### Context Actions Menu
```csharp
- View Project Costs (navigate to costing)
- Add Cost Entry (quick add)
- Start Project (if planning)
- Hold Project (if active)
- Complete Project (if active)
- Close Project (if completed)
- Print Project Report
- Budget vs Actual Report
```

##### Action Dialogs
1. **Start Project Dialog**
   - Actual start date
   - Initial notes

2. **Complete Project Dialog**
   - Completion date
   - Completion notes
   - Final budget check

3. **Close Project Dialog**
   - Close date
   - Close reason
   - Final variance explanation

##### Enhanced Display
- Status badge with workflow
- Budget vs Actual progress bar
- Percentage complete
- Budget variance with color coding
- Timeline indicator
- Cost summary

#### Project Costing Page (Cost Entries)

##### Features
- Cost entry grid
- Add/Edit/Delete cost entries
- Entry type (Labor, Material, Equipment, Other)
- Date, amount, description
- Category/Account selection
- Document attachment support
- Real-time budget tracking
- Cost approval workflow
- Export cost entries

---

## Implementation Priority

### Phase 1 (High Priority)
1. ✅ Check Management (COMPLETE)
2. Chart of Accounts (most critical for GL)
3. Budgets with Budget Details (critical for financial planning)

### Phase 2 (Medium Priority)
4. Payees (important for AP)
5. Projects with Project Costing (important for job costing)

### Phase 3 (Lower Priority but Important)
6. Accruals (specialized accounting)

---

## Common Code Patterns

### Status Badge Helper
```csharp
private static Color GetStatusColor(string? status) => status switch
{
    "Active" => Color.Success,
    "Inactive" => Color.Default,
    "Draft" => Color.Info,
    "Approved" => Color.Primary,
    "Completed" => Color.Success,
    "Closed" => Color.Dark,
    "Reversed" => Color.Warning,
    _ => Color.Default
};
```

### Currency Formatting Template
```razor
@if (dto.Amount.HasValue)
{
    <MudText Typo="Typo.body2" Class="font-weight-bold">@dto.Amount.Value.ToString("C2")</MudText>
}
```

### Status Icon Template
```razor
@if (dto.IsActive)
{
    <MudIcon Icon="@Icons.Material.Filled.CheckCircle" Color="Color.Success" Size="Size.Small" />
}
else
{
    <MudIcon Icon="@Icons.Material.Filled.Cancel" Color="Color.Default" Size="Size.Small" />
}
```

### Progress Bar Template
```razor
<MudProgressLinear 
    Value="@GetProgressPercentage(dto.Actual, dto.Budget)" 
    Color="@GetProgressColor(dto.Actual, dto.Budget)" 
    Size="Size.Small" />
```

---

## API Endpoints Checklist

For each entity, ensure these endpoints exist:
- ✅ POST /{entity}/search - Search with filters
- ✅ POST /{entity} - Create
- ✅ GET /{entity}/{id} - Get by ID
- ✅ PUT /{entity}/{id} - Update
- ✅ DELETE /{entity}/{id} - Delete
- ⚠️ POST /{entity}/{id}/activate - Activate (if applicable)
- ⚠️ POST /{entity}/{id}/deactivate - Deactivate (if applicable)
- ⚠️ POST /{entity}/{id}/workflow-action - Other workflow actions
- ⚠️ POST /{entity}/import - Import from Excel (if applicable)
- ⚠️ GET /{entity}/export - Export to Excel (if applicable)

---

## Files to Create/Update Per Entity

### Chart of Accounts Example
1. **Update:** ChartOfAccounts.razor - Add advanced search and actions
2. **Update:** ChartOfAccounts.razor.cs - Add action handlers
3. **Keep:** ChartOfAccountViewModel.cs (if exists, otherwise create)

### Pattern for All Entities
- Enhanced .razor file with AdvancedSearchContent and ActionsContent
- Enhanced .razor.cs with action handlers and dialogs
- ViewModel matching the update command
- Status helper methods
- Template render fragments for special display

---

## Next Steps

1. Create enhanced ChartOfAccounts page (follow Check Management pattern)
2. Create enhanced Budgets page with Budget Details integration
3. Create enhanced Payees page with image and status management
4. Create enhanced Projects page with Project Costing integration
5. Create enhanced Accruals page with reversal functionality
6. Update navigation and test all pages
7. Document each enhancement in separate files

---

## Notes

- All enhancements follow the Check Management pattern
- Maintain consistency with existing MudBlazor components
- Use EntityTable component for standard CRUD
- Add proper error handling and validation
- Include comprehensive documentation
- Test all workflows and edge cases
- Consider mobile responsiveness
- Add proper permissions checks
- Include audit trail where applicable
