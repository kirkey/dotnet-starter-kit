# 💼 COPILOT INSTRUCTIONS - UI ACCOUNTING MODULE

**Last Updated**: November 20, 2025  
**Status**: ✅ Production Ready - Accounting Module UI Patterns  
**Scope**: Accounting-specific UI patterns and workflows

> **📌 Reference other files:**
> - `copilot-instructions-ui-foundation.md` - Core UI principles
> - `copilot-instructions-ui-components.md` - Core components
> - `copilot-instructions-ui-store.md` - Store patterns
> - `copilot-instructions-ui-hr.md` - HR patterns

---

## ✅ FINANCIAL DATA DISPLAY

- ✅ **Decimal Formatting**: Use `Format="N2"` for currency/amounts
- ✅ **Status Indicators**: Use MudChip with color coding for statuses
- ✅ **Balance Validation**: Visual indicators for balanced/unbalanced entries
- ✅ **Date Pickers**: Consistent date format `DateFormat="MMMM dd, yyyy"`
- ✅ **Fiscal Period Selection**: Use autocomplete components for period references

**Financial Display Pattern:**
```csharp
<MudNumericField T="decimal"
                 @bind-Value="@Amount"
                 Label="Amount"
                 Format="N2"
                 Variant="Variant.Outlined"
                 Adornment="Adornment.Start"
                 AdornmentIcon="@Icons.Material.Filled.AttachMoney" />

<MudChip T="string" 
         Color="@GetStatusColor(status)" 
         Size="Size.Large">
    @status
</MudChip>

// Status color helper
private static Color GetStatusColor(string? status) => status switch
{
    "Pending" => Color.Warning,
    "Approved" => Color.Info,
    "Rejected" => Color.Error,
    "Posted" => Color.Success,
    _ => Color.Default
};
```

---

## ✅ MULTI-LINE ENTRY COMPONENTS

- ✅ **Line Item Editor**: Use MudTable for double-entry journal lines
- ✅ **Real-time Balance**: Calculate totals in footer
- ✅ **Mutual Exclusion**: Disable debit when credit entered (and vice versa)
- ✅ **Visual Validation**: Show balance status with chips/alerts

**Line Editor Pattern:**
```csharp
<MudTable Items="@Lines" Dense="true">
    <HeaderContent>
        <MudTh>Account</MudTh>
        <MudTh Style="text-align:right">Debit</MudTh>
        <MudTh Style="text-align:right">Credit</MudTh>
        <MudTh>Actions</MudTh>
    </HeaderContent>
    <RowTemplate Context="line">
        <MudTd>
            <AutocompleteChartOfAccountId @bind-Value="line.AccountId" />
        </MudTd>
        <MudTd Style="text-align:right">
            <MudNumericField @bind-Value="line.DebitAmount"
                            Format="N2"
                            Disabled="@(line.CreditAmount > 0)" />
        </MudTd>
        <MudTd Style="text-align:right">
            <MudNumericField @bind-Value="line.CreditAmount"
                            Format="N2"
                            Disabled="@(line.DebitAmount > 0)" />
        </MudTd>
        <MudTd>
            <MudIconButton Icon="@Icons.Material.Filled.Delete"
                          OnClick="@(() => RemoveLine(line))" />
        </MudTd>
    </RowTemplate>
    <FooterContent>
        <MudTd><strong>TOTALS</strong></MudTd>
        <MudTd Style="text-align:right">
            <strong>@TotalDebits.ToString("N2")</strong>
        </MudTd>
        <MudTd Style="text-align:right">
            <strong>@TotalCredits.ToString("N2")</strong>
        </MudTd>
        <MudTd>
            @if (IsBalanced)
            {
                <MudChip Color="Color.Success" Icon="@Icons.Material.Filled.CheckCircle">
                    Balanced
                </MudChip>
            }
            else
            {
                <MudChip Color="Color.Warning" Icon="@Icons.Material.Filled.Warning">
                    Out of Balance: @Difference.ToString("C")
                </MudChip>
            }
        </MudTd>
    </FooterContent>
</MudTable>
```

---

## ✅ WORKFLOW ACTION MENUS

- ✅ **ExtraActions in EntityTable**: Use for context-specific actions
- ✅ **Conditional Actions**: Show/hide based on entity status
- ✅ **Workflow Progression**: Approve → Post → Reverse patterns
- ✅ **Action Icons**: Use meaningful Material icons

**Workflow Pattern:**
```csharp
<ExtraActions Context="entry">
    <MudMenuItem Icon="@Icons.Material.Filled.Visibility" 
                 OnClick="@(() => OnViewDetails(entry.Id))">
        View Details
    </MudMenuItem>
    
    @if (entry.ApprovalStatus == "Pending")
    {
        <MudMenuItem Icon="@Icons.Material.Filled.CheckCircle" 
                     OnClick="@(() => OnApprove(entry.Id))">
            Approve
        </MudMenuItem>
        <MudMenuItem Icon="@Icons.Material.Filled.Cancel" 
                     OnClick="@(() => OnReject(entry.Id))">
            Reject
        </MudMenuItem>
    }
    
    @if (entry is { ApprovalStatus: "Approved", IsPosted: false })
    {
        <MudMenuItem Icon="@Icons.Material.Filled.PostAdd" 
                     OnClick="@(() => OnPost(entry.Id))">
            Post to GL
        </MudMenuItem>
    }
    
    @if (entry.IsPosted)
    {
        <MudMenuItem Icon="@Icons.Material.Filled.Replay" 
                     OnClick="@(() => OnReverse(entry.Id))">
            Reverse Entry
        </MudMenuItem>
    }
</ExtraActions>
```

---

## ✅ ADVANCED SEARCH FILTERS

- ✅ **Date Range Filters**: From/To date pickers
- ✅ **Status Dropdowns**: MudSelect with predefined statuses
- ✅ **Amount Range**: Min/Max numeric fields
- ✅ **Reference Search**: Text fields with search icons
- ✅ **Boolean Toggles**: MudSwitch for yes/no filters

**Filter Pattern:**
```csharp
<AdvancedSearchContent>
    <MudItem xs="12" sm="6" md="4">
        <MudTextField @bind-Value="@ReferenceNumber"
                      Label="Reference Number"
                      Variant="Variant.Outlined"
                      Adornment="Adornment.Start"
                      AdornmentIcon="@Icons.Material.Filled.Search" />
    </MudItem>
    
    <MudItem xs="12" sm="6" md="4">
        <MudSelect T="string"
                   @bind-Value="@ApprovalStatus"
                   Label="Approval Status"
                   Variant="Variant.Outlined"
                   Clearable="true">
            <MudSelectItem Value="@("Pending")">Pending</MudSelectItem>
            <MudSelectItem Value="@("Approved")">Approved</MudSelectItem>
            <MudSelectItem Value="@("Rejected")">Rejected</MudSelectItem>
        </MudSelect>
    </MudItem>
    
    <MudItem xs="12" sm="6" md="4">
        <MudDatePicker @bind-Date="@FromDate"
                       Label="Date From"
                       DateFormat="MMMM dd, yyyy"
                       Variant="Variant.Outlined" />
    </MudItem>
    
    <MudItem xs="12" sm="6" md="4">
        <MudNumericField T="decimal?"
                         @bind-Value="@MinAmount"
                         Label="Min Amount"
                         Format="N2"
                         Variant="Variant.Outlined"
                         Adornment="Adornment.Start"
                         AdornmentIcon="@Icons.Material.Filled.AttachMoney" />
    </MudItem>
    
    <MudItem xs="12" sm="6" md="4">
        <MudSwitch @bind-Value="@IsPosted"
                   Label="Posted Entries Only"
                   Color="Color.Primary"
                   ThreeState="true" />
    </MudItem>
</AdvancedSearchContent>
```

---

## ✅ ACTION BUTTON GROUPS

- ✅ **Grouped Actions**: Use MudButtonGroup for related actions
- ✅ **Color Coding**: Primary for main actions, Secondary for filters, Info for help
- ✅ **Icon Buttons**: Always include meaningful start icons
- ✅ **Helper Actions**: Reports, Export, Settings, Help

**Action Group Pattern:**
```csharp
<MudPaper Elevation="2" Class="pa-4 mb-4">
    <MudStack Row="true" Spacing="2" Wrap="Wrap.Wrap">
        <MudButtonGroup Color="Color.Primary" Variant="Variant.Outlined" Size="Size.Small">
            <MudButton StartIcon="@Icons.Material.Filled.Assessment" 
                       OnClick="@ShowReports">
                Reports
            </MudButton>
            <MudButton StartIcon="@Icons.Material.Filled.Receipt" 
                       OnClick="@ShowBillingPeriods">
                Billing Periods
            </MudButton>
        </MudButtonGroup>

        <MudButtonGroup Color="Color.Info" Variant="Variant.Outlined" Size="Size.Small">
            <MudButton StartIcon="@Icons.Material.Filled.Help" 
                       OnClick="@ShowHelp">
                Help
            </MudButton>
        </MudButtonGroup>
    </MudStack>
</MudPaper>
```

---

## ✅ FINANCIAL STATEMENT VIEWS

- ✅ **Tabbed Interface**: Use MudTabs for multiple statements
- ✅ **Statement Components**: Separate components for Balance Sheet, Income Statement, Cash Flow
- ✅ **Refresh Actions**: Icon buttons for refresh functionality
- ✅ **Full-Width Layout**: MaxWidth.False for financial data

**Financial Statement Pattern:**
```csharp
<MudContainer MaxWidth="MaxWidth.False" Class="pa-6">
    <MudCard>
        <MudCardHeader>
            <CardHeaderContent>
                <MudText Typo="Typo.h6">Financial Statements</MudText>
            </CardHeaderContent>
            <CardHeaderActions>
                <MudIconButton Icon="@Icons.Material.Filled.Help" 
                               Color="Color.Info" 
                               OnClick="@ShowHelp" />
                <MudIconButton Icon="@Icons.Material.Filled.Refresh" 
                               OnClick="@RefreshCurrentStatement" />
            </CardHeaderActions>
        </MudCardHeader>
        <MudCardContent>
            <MudTabs Elevation="1" Rounded="true" Color="Color.Primary">
                <MudTabPanel Text="Balance Sheet" Icon="@Icons.Material.Filled.AccountBalance">
                    <BalanceSheetView @ref="_balanceSheetView" />
                </MudTabPanel>
                <MudTabPanel Text="Income Statement" Icon="@Icons.Material.Filled.TrendingUp">
                    <IncomeStatementView @ref="_incomeStatementView" />
                </MudTabPanel>
                <MudTabPanel Text="Cash Flow" Icon="@Icons.Material.Filled.AttachMoney">
                    <CashFlowStatementView @ref="_cashFlowStatementView" />
                </MudTabPanel>
            </MudTabs>
        </MudCardContent>
    </MudCard>
</MudContainer>
```

---

## 📊 ACCOUNTING UI CHECKLIST

### **Financial Data Entry**
- [ ] Decimal fields use `Format="N2"` for 2 decimal places
- [ ] Amount fields have money icon adornment
- [ ] Debit/Credit mutual exclusion implemented
- [ ] Real-time balance calculation shown
- [ ] Visual balance indicators (Balanced/Out of Balance)

### **Workflow Actions**
- [ ] Status-based conditional actions implemented
- [ ] Approval workflow (Pending → Approved → Posted)
- [ ] Reversal capability for posted entries
- [ ] Action icons are meaningful and consistent
- [ ] Confirmation dialogs for destructive actions

### **Search & Filtering**
- [ ] Date range filters (From/To)
- [ ] Status dropdown filters
- [ ] Amount range filters (Min/Max)
- [ ] Reference number search
- [ ] Boolean toggle filters (Posted/Unposted)

### **Help System**
- [ ] Help dialog accessible from page
- [ ] Expansion panels for organized help content
- [ ] Step-by-step procedures documented
- [ ] Contextual tips with MudAlert
- [ ] Icon-based feature lists

### **Financial Statements**
- [ ] Tabbed interface for multiple statements
- [ ] Refresh capability
- [ ] Full-width layout for data display
- [ ] Component-based architecture
- [ ] Print/Export functionality

---

## ✅ VERIFICATION STATUS

**Accounting Module**: ✅ A+ Verified & Documented  

**Last verified**: November 20, 2025

