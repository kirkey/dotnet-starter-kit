# Accounting Pages Implementation Guide

## Executive Summary

This document provides complete implementation guidance for enhancing 5 accounting pages following the pattern established in the Check Management page. Each page will have:

- Advanced search with multiple filters
- Context-sensitive action menus
- Specialized operation dialogs
- Enhanced visual display with badges and formatting
- Proper validation and error handling

---

## Quick Reference: Check Management Pattern

### Key Components
1. **ViewModel** - Matches update command structure
2. **.razor File** - Contains UI with AdvancedSearchContent, ActionsContent, EditFormContent, and Dialogs
3. **.razor.cs File** - Contains EntityServerTableContext, action handlers, validation logic

### Standard Structure

```razor
@page "/route"

<PageHeader Title="..." Header="..." SubHeader="..." />

<EntityTable @ref="_table" TEntity="ResponseType" TId="DefaultIdType" TRequest="ViewModel" Context="@Context">
    
    <AdvancedSearchContent Context="search">
        <!-- Search filters -->
    </AdvancedSearchContent>

    <ActionsContent Context="item">
        <!-- Context menu actions based on status -->
    </ActionsContent>

    <EditFormContent Context="context">
        <!-- Add/Edit form fields -->
    </EditFormContent>
</EntityTable>

<!-- Action Dialogs -->
<MudDialog @bind-IsVisible="_dialogVisible">
    <!-- Dialog content -->
</MudDialog>
```

---

## 1. Chart of Accounts Implementation

### Files
- `/Pages/Accounting/ChartOfAccounts/ChartOfAccounts.razor`
- `/Pages/Accounting/ChartOfAccounts/ChartOfAccounts.razor.cs`
- `/Pages/Accounting/ChartOfAccounts/ChartOfAccountViewModel.cs` (already exists as inherited from UpdateCommand)

### ViewModel (Enhanced Properties)
```csharp
public class ChartOfAccountViewModel : UpdateChartOfAccountCommand
{
    // Inherited: AccountCode, AccountName, AccountType, UsoaCategory, ParentAccountId,
    // ParentCode, Balance, IsControlAccount, NormalBalance, IsUsoaCompliant, etc.
    
    // Display properties (read-only, populated from GetResponse)
    public bool IsActive { get; set; } = true;
    public int AccountLevel { get; set; }
    public bool HasChildAccounts { get; set; }
    public int TransactionCount { get; set; }
}
```

### Advanced Search Filters
```razor
<AdvancedSearchContent Context="search">
    <MudItem xs="12" sm="6" md="4">
        <MudTextField @bind-Value="search.AccountCode"
                      Label="Account Code"
                      Variant="Variant.Outlined" />
    </MudItem>
    <MudItem xs="12" sm="6" md="4">
        <MudTextField @bind-Value="search.AccountName"
                      Label="Account Name"
                      Variant="Variant.Outlined" />
    </MudItem>
    <MudItem xs="12" sm="6" md="4">
        <MudSelect @bind-Value="search.AccountType"
                   Label="Account Type"
                   Variant="Variant.Outlined"
                   Clearable="true">
            <MudSelectItem Value="@("Asset")">Asset</MudSelectItem>
            <MudSelectItem Value="@("Liability")">Liability</MudSelectItem>
            <MudSelectItem Value="@("Equity")">Equity</MudSelectItem>
            <MudSelectItem Value="@("Revenue")">Revenue</MudSelectItem>
            <MudSelectItem Value="@("Expense")">Expense</MudSelectItem>
        </MudSelect>
    </MudItem>
    <MudItem xs="12" sm="6" md="4">
        <AutocompleteUsoaCategory @bind-Value="search.UsoaCategory"
                                  Label="USOA Category"
                                  Variant="Variant.Outlined" />
    </MudItem>
    <MudItem xs="12" sm="6" md="4">
        <MudNumericField @bind-Value="search.BalanceFrom"
                         Label="Balance From"
                         Format="N2"
                         Variant="Variant.Outlined" />
    </MudItem>
    <MudItem xs="12" sm="6" md="4">
        <MudNumericField @bind-Value="search.BalanceTo"
                         Label="Balance To"
                         Format="N2"
                         Variant="Variant.Outlined" />
    </MudItem>
    <MudItem xs="12" sm="6" md="4">
        <MudSwitch @bind-Value="search.ActiveOnly"
                   Label="Active Accounts Only"
                   Color="Color.Primary" />
    </MudItem>
    <MudItem xs="12" sm="6" md="4">
        <MudSwitch @bind-Value="search.ControlAccountsOnly"
                   Label="Control Accounts Only"
                   Color="Color.Primary" />
    </MudItem>
</AdvancedSearchContent>
```

### Context Actions
```razor
<ActionsContent Context="account">
    @if (account.IsActive)
    {
        <MudMenuItem Icon="@Icons.Material.Filled.Block" OnClick="@(() => OnDeactivateAccount(account.Id))">
            Deactivate Account
        </MudMenuItem>
    }
    else
    {
        <MudMenuItem Icon="@Icons.Material.Filled.CheckCircle" OnClick="@(() => OnActivateAccount(account.Id))">
            Activate Account
        </MudMenuItem>
    }
    <MudMenuItem Icon="@Icons.Material.Filled.Receipt" OnClick="@(() => OnViewTransactions(account.Id))">
        View Transactions
    </MudMenuItem>
    <MudMenuItem Icon="@Icons.Material.Filled.Download" OnClick="@(() => OnExportAccount(account.Id))">
        Export Details
    </MudMenuItem>
</ActionsContent>
```

### Code-Behind Methods
```csharp
// Activate/Deactivate handlers
private void OnActivateAccount(DefaultIdType accountId) { /* Implementation */ }
private async Task SubmitActivateAccount() { /* Call API */ }

private void OnDeactivateAccount(DefaultIdType accountId) { /* Implementation */ }
private async Task SubmitDeactivateAccount() { /* Call API, validation */ }

// View transactions
private void OnViewTransactions(DefaultIdType accountId)
{
    Navigation.NavigateTo($"/accounting/general-ledger?accountId={accountId}");
}

// Export
private async Task OnExportAccount(DefaultIdType accountId) { /* Implementation */ }
```

---

## 2. Accruals Implementation

### Files
- `/Pages/Accounting/Accruals/Accruals.razor`
- `/Pages/Accounting/Accruals/Accruals.razor.cs`
- `/Pages/Accounting/Accruals/AccrualViewModel.cs` (already exists)

### Key Enhancements

#### Advanced Search
```razor
<MudItem xs="12" sm="6" md="4">
    <MudTextField @bind-Value="search.AccrualNumber" Label="Accrual Number" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudDatePicker @bind-Date="search.AccrualDateFrom" Label="Accrual Date From" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudDatePicker @bind-Date="search.AccrualDateTo" Label="Accrual Date To" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudNumericField @bind-Value="search.AmountFrom" Label="Amount From" Format="N2" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudNumericField @bind-Value="search.AmountTo" Label="Amount To" Format="N2" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudSelect @bind-Value="search.Status" Label="Status" Clearable="true">
        <MudSelectItem Value="@("Active")">Active</MudSelectItem>
        <MudSelectItem Value="@("Reversed")">Reversed</MudSelectItem>
    </MudSelect>
</MudItem>
```

#### Context Actions
```razor
<ActionsContent Context="accrual">
    @if (!accrual.IsReversed)
    {
        <MudMenuItem Icon="@Icons.Material.Filled.Undo" OnClick="@(() => OnReverseAccrual(accrual.Id))">
            Reverse Accrual
        </MudMenuItem>
    }
    <MudMenuItem Icon="@Icons.Material.Filled.Receipt" OnClick="@(() => OnViewJournalEntry(accrual.Id))">
        View Journal Entry
    </MudMenuItem>
    <MudMenuItem Icon="@Icons.Material.Filled.Print" OnClick="@(() => OnPrintVoucher(accrual.Id))">
        Print Voucher
    </MudMenuItem>
</ActionsContent>
```

#### Reverse Dialog
```razor
<MudDialog @bind-IsVisible="_reverseDialogVisible">
    <TitleContent>
        <MudText Typo="Typo.h6">Reverse Accrual</MudText>
    </TitleContent>
    <DialogContent>
        <MudDatePicker @bind-Date="_reverseCommand.ReversalDate"
                       Label="Reversal Date"
                       Required="true" />
        <MudTextField @bind-Value="_reverseCommand.ReversalReason"
                      Label="Reason (Optional)"
                      Lines="3" />
    </DialogContent>
    <DialogActions>
        <MudButton OnClick="@(() => _reverseDialogVisible = false)">Cancel</MudButton>
        <MudButton Color="Color.Warning" Variant="Variant.Filled" OnClick="SubmitReverseAccrual">
            Reverse
        </MudButton>
    </DialogActions>
</MudDialog>
```

---

## 3. Budgets with Budget Details Implementation

### Main Budgets Page

#### Files
- `/Pages/Accounting/Budgets/Budgets.razor`
- `/Pages/Accounting/Budgets/Budgets.razor.cs`
- `/Pages/Accounting/Budgets/BudgetViewModel.cs`

#### Advanced Search
```razor
<MudItem xs="12" sm="6" md="4">
    <MudTextField @bind-Value="search.BudgetName" Label="Budget Name" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudNumericField @bind-Value="search.FiscalYear" Label="Fiscal Year" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <AutocompleteAccountingPeriodId @bind-Value="search.PeriodId" Label="Period" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudSelect @bind-Value="search.Status" Label="Status" Clearable="true">
        <MudSelectItem Value="@("Draft")">Draft</MudSelectItem>
        <MudSelectItem Value="@("Approved")">Approved</MudSelectItem>
        <MudSelectItem Value="@("Active")">Active</MudSelectItem>
        <MudSelectItem Value="@("Closed")">Closed</MudSelectItem>
    </MudSelect>
</MudItem>
```

#### Context Actions
```razor
<ActionsContent Context="budget">
    <MudMenuItem Icon="@Icons.Material.Filled.List" Href="@($"/accounting-budgetdetails/{budget.Id}")">
        View Budget Lines
    </MudMenuItem>
    @if (budget.Status == "Draft")
    {
        <MudMenuItem Icon="@Icons.Material.Filled.Approval" OnClick="@(() => OnApproveBudget(budget.Id))">
            Approve Budget
        </MudMenuItem>
    }
    @if (budget.Status == "Approved")
    {
        <MudMenuItem Icon="@Icons.Material.Filled.PlayArrow" OnClick="@(() => OnActivateBudget(budget.Id))">
            Activate Budget
        </MudMenuItem>
    }
    @if (budget.Status == "Active")
    {
        <MudMenuItem Icon="@Icons.Material.Filled.Lock" OnClick="@(() => OnCloseBudget(budget.Id))">
            Close Budget
        </MudMenuItem>
    }
    <MudMenuItem Icon="@Icons.Material.Filled.ContentCopy" OnClick="@(() => OnCopyBudget(budget.Id))">
        Copy Budget
    </MudMenuItem>
    <MudMenuItem Icon="@Icons.Material.Filled.Print" OnClick="@(() => OnPrintBudget(budget.Id))">
        Print Report
    </MudMenuItem>
</ActionsContent>
```

### Budget Details Page

#### Enhancements
- Grid of budget line items
- Add/Edit/Delete inline
- Account autocomplete
- Monthly distribution
- Real-time totals
- Variance calculations
- Import from Excel

---

## 4. Payees Implementation

### Files
- `/Pages/Accounting/Payees/Payees.razor`
- `/Pages/Accounting/Payees/Payees.razor.cs`
- `/Pages/Accounting/Payees/PayeeViewModel.cs` (already exists)

### Key Enhancements

#### Advanced Search
```razor
<MudItem xs="12" sm="6" md="4">
    <MudTextField @bind-Value="search.PayeeCode" Label="Payee Code" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudTextField @bind-Value="search.Name" Label="Payee Name" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudTextField @bind-Value="search.TIN" Label="TIN" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <AutocompleteChartOfAccountCode @bind-Value="search.ExpenseAccountCode"
                                    Label="Expense Account" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudTextField @bind-Value="search.Address" Label="Address Contains" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudSwitch @bind-Value="search.HasImageOnly" Label="Has Image Only" />
</MudItem>
```

#### Context Actions
```razor
<ActionsContent Context="payee">
    <MudMenuItem Icon="@Icons.Material.Filled.Payment" OnClick="@(() => OnViewPaymentHistory(payee.Id))">
        Payment History
    </MudMenuItem>
    <MudMenuItem Icon="@Icons.Material.Filled.Receipt" OnClick="@(() => OnViewInvoices(payee.Id))">
        Outstanding Invoices
    </MudMenuItem>
    @if (!string.IsNullOrEmpty(payee.Tin))
    {
        <MudMenuItem Icon="@Icons.Material.Filled.Print" OnClick="@(() => OnPrint1099(payee.Id))">
            Print 1099
        </MudMenuItem>
    }
    <MudMenuItem Icon="@Icons.Material.Filled.Email" OnClick="@(() => OnSendEmail(payee.Id))">
        Send Email
    </MudMenuItem>
</ActionsContent>
```

#### Enhanced Display
```razor
// Image template in table
private RenderFragment<PayeeResponse> TemplateImage => dto => __builder =>
{
    @if (!string.IsNullOrEmpty(dto.ImageUrl))
    {
        <MudAvatar Image="@ImageUrlService.GetAbsoluteUrl(dto.ImageUrl)" Size="Size.Small" />
    }
};

// TIN with masking
private RenderFragment<PayeeResponse> TemplateTIN => dto => __builder =>
{
    @if (!string.IsNullOrEmpty(dto.Tin))
    {
        <MudText Typo="Typo.body2">XXX-XX-@dto.Tin.Substring(dto.Tin.Length - 4)</MudText>
    }
};
```

---

## 5. Projects with Project Costing Implementation

### Main Projects Page

#### Files
- `/Pages/Accounting/Projects/Projects.razor`
- `/Pages/Accounting/Projects/Projects.razor.cs`
- `/Pages/Accounting/Projects/ProjectViewModel.cs`

#### Advanced Search
```razor
<MudItem xs="12" sm="6" md="4">
    <MudTextField @bind-Value="search.ProjectCode" Label="Project Code" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudTextField @bind-Value="search.ProjectName" Label="Project Name" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudTextField @bind-Value="search.ProjectManager" Label="Project Manager" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudSelect @bind-Value="search.Status" Label="Status" Clearable="true">
        <MudSelectItem Value="@("Planning")">Planning</MudSelectItem>
        <MudSelectItem Value="@("Active")">Active</MudSelectItem>
        <MudSelectItem Value="@("On Hold")">On Hold</MudSelectItem>
        <MudSelectItem Value="@("Completed")">Completed</MudSelectItem>
        <MudSelectItem Value="@("Closed")">Closed</MudSelectItem>
    </MudSelect>
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudDatePicker @bind-Date="search.StartDateFrom" Label="Start Date From" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudDatePicker @bind-Date="search.StartDateTo" Label="Start Date To" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudNumericField @bind-Value="search.BudgetFrom" Label="Budget From" Format="N2" />
</MudItem>
<MudItem xs="12" sm="6" md="4">
    <MudNumericField @bind-Value="search.BudgetTo" Label="Budget To" Format="N2" />
</MudItem>
```

#### Context Actions
```razor
<ActionsContent Context="project">
    <MudMenuItem Icon="@Icons.Material.Filled.AttachMoney" Href="@($"/accounting-projects/{project.Id}/costing")">
        View Project Costs
    </MudMenuItem>
    <MudMenuItem Icon="@Icons.Material.Filled.Add" OnClick="@(() => OnAddCostEntry(project.Id))">
        Add Cost Entry
    </MudMenuItem>
    @if (project.Status == "Planning")
    {
        <MudMenuItem Icon="@Icons.Material.Filled.PlayArrow" OnClick="@(() => OnStartProject(project.Id))">
            Start Project
        </MudMenuItem>
    }
    @if (project.Status == "Active")
    {
        <MudMenuItem Icon="@Icons.Material.Filled.Pause" OnClick="@(() => OnHoldProject(project.Id))">
            Hold Project
        </MudMenuItem>
        <MudMenuItem Icon="@Icons.Material.Filled.CheckCircle" OnClick="@(() => OnCompleteProject(project.Id))">
            Complete Project
        </MudMenuItem>
    }
    @if (project.Status == "Completed")
    {
        <MudMenuItem Icon="@Icons.Material.Filled.Lock" OnClick="@(() => OnCloseProject(project.Id))">
            Close Project
        </MudMenuItem>
    }
    <MudMenuItem Icon="@Icons.Material.Filled.Print" OnClick="@(() => OnPrintReport(project.Id))">
        Print Report
    </MudMenuItem>
</ActionsContent>
```

#### Enhanced Display
```razor
// Progress bar template
private RenderFragment<ProjectResponse> TemplateProgress => dto => __builder =>
{
    var percentage = dto.Budget > 0 ? (dto.ActualCost / dto.Budget) * 100 : 0;
    var color = percentage > 100 ? Color.Error : percentage > 90 ? Color.Warning : Color.Success;
    
    <MudProgressLinear Value="@percentage" Color="@color" Size="Size.Small" />
    <MudText Typo="Typo.caption">@percentage.ToString("F1")%</MudText>
};

// Variance template
private RenderFragment<ProjectResponse> TemplateVariance => dto => __builder =>
{
    var variance = dto.Budget - dto.ActualCost;
    var color = variance < 0 ? Color.Error : Color.Success;
    
    <MudText Typo="Typo.body2" Color="@color">
        @variance.ToString("C2")
    </MudText>
};
```

### Project Costing Page

#### Features
- Cost entry grid with add/edit/delete
- Entry type dropdown (Labor, Material, Equipment, Other)
- Account selection
- Real-time budget tracking
- Document attachments
- Export to Excel

---

## Implementation Checklist

### For Each Page:

#### Phase 1: Planning
- [ ] Review existing API endpoints
- [ ] Identify required new endpoints
- [ ] Design ViewModel structure
- [ ] Plan advanced search filters
- [ ] Design context actions
- [ ] Plan dialogs and workflows

#### Phase 2: Implementation
- [ ] Create/update ViewModel
- [ ] Update .razor file with advanced search
- [ ] Add context actions menu
- [ ] Create action dialogs
- [ ] Update .razor.cs with handlers
- [ ] Add validation logic
- [ ] Implement error handling

#### Phase 3: Enhancement
- [ ] Add status badges
- [ ] Add formatted displays
- [ ] Add progress indicators
- [ ] Add summary cards
- [ ] Add export functionality
- [ ] Add responsive design

#### Phase 4: Testing
- [ ] Test all CRUD operations
- [ ] Test search filters
- [ ] Test workflow actions
- [ ] Test validation
- [ ] Test error scenarios
- [ ] Test permissions
- [ ] Test mobile view

#### Phase 5: Documentation
- [ ] Document page features
- [ ] Document workflows
- [ ] Document API integration
- [ ] Create user guide
- [ ] Update navigation docs

---

## API Endpoints Required

### Chart of Accounts
- POST `/accounting/chart-of-accounts/{id}/activate`
- POST `/accounting/chart-of-accounts/{id}/deactivate`
- GET `/accounting/chart-of-accounts/{id}/transactions`

### Accruals
- POST `/accounting/accruals/{id}/reverse`
- GET `/accounting/accruals/{id}/journal-entry`

### Budgets
- POST `/accounting/budgets/{id}/approve`
- POST `/accounting/budgets/{id}/activate`
- POST `/accounting/budgets/{id}/close`
- POST `/accounting/budgets/{id}/copy`
- POST `/accounting/budgets/{id}/lines` (Budget lines CRUD)

### Payees
- GET `/accounting/payees/{id}/payment-history`
- GET `/accounting/payees/{id}/invoices`
- POST `/accounting/payees/{id}/activate`
- POST `/accounting/payees/{id}/deactivate`

### Projects
- POST `/accounting/projects/{id}/start`
- POST `/accounting/projects/{id}/hold`
- POST `/accounting/projects/{id}/complete`
- POST `/accounting/projects/{id}/close`
- GET `/accounting/projects/{id}/costs`
- POST `/accounting/projects/{id}/costs` (Add cost entry)

---

## Summary

This guide provides the complete blueprint for enhancing all 5 accounting pages following the Check Management pattern. Each page will have:

1. **Consistent UI/UX** - Same look and feel across all pages
2. **Advanced Functionality** - Rich search, context actions, workflows
3. **Professional Display** - Badges, progress bars, formatted values
4. **Robust Validation** - Client-side validation before API calls
5. **Error Handling** - Proper try-catch with user-friendly messages
6. **Documentation** - Complete implementation and user guides

The pattern is proven with Check Management and can be replicated for each entity with entity-specific customizations.
