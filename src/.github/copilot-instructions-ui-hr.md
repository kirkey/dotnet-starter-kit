# 👥 COPILOT INSTRUCTIONS - UI HR MODULE

**Last Updated**: November 22, 2025  
**Status**: ✅ Production Ready - HR Module UI Patterns  
**Scope**: Human Resources-specific UI patterns and workflows

> **📌 Reference other files:**
> - `copilot-instructions-ui-foundation.md` - Core UI principles
> - `copilot-instructions-ui-components.md` - Core components
> - `copilot-instructions-ui-accounting.md` - Accounting patterns
> - `copilot-instructions-ui-store.md` - Store patterns

---

## ✅ EMPLOYEE MANAGEMENT WORKFLOWS

- ✅ **Wizard Pattern**: Multi-step employee creation with MudStepper
- ✅ **Sub-Component Architecture**: Contacts, Dependents, Education sub-pages
- ✅ **Tabbed Views**: Current/Historical designation assignments
- ✅ **Related Data Navigation**: Navigate between employee sub-pages
- ✅ **Government ID Validation**: Philippines-specific ID formats
- ✅ **Bank Accounts Management**: Payroll routing and direct deposit setup

**Employee Creation Wizard Pattern:**
```csharp
<MudDialog>
    <DialogContent>
        <MudStepper Linear="false" @ref="_stepper">
            
            <!-- Step 1: Personal Information -->
            <MudStep Title="Personal Info" Icon="@Icons.Material.Filled.Person">
                <MudStack Spacing="3">
                    <MudText Typo="Typo.subtitle1">Enter Employee Basic Information</MudText>
                    
                    <MudGrid Spacing="2">
                        <MudItem xs="12" sm="6">
                            <MudTextField @bind-Value="_firstName"
                                          Label="First Name"
                                          Required="true"
                                          Variant="Variant.Outlined" />
                        </MudItem>
                        <!-- More fields -->
                    </MudGrid>
                </MudStack>
            </MudStep>

            <!-- Step 2: Government IDs -->
            <MudStep Title="Government IDs" Icon="@Icons.Material.Filled.VerifiedUser">
                <MudStack Spacing="3">
                    <MudText Typo="Typo.subtitle1">Government Identification Numbers</MudText>
                    <MudAlert Severity="Severity.Info">All IDs required for payroll compliance</MudAlert>
                    
                    <MudGrid Spacing="2">
                        <MudItem xs="12" sm="6">
                            <MudTextField @bind-Value="_sssNumber"
                                          Label="SSS Number"
                                          Placeholder="XX-XXXXXXXXX-X"
                                          Required="true"
                                          HelperText="Format: XX-XXXXXXXXX-X" />
                        </MudItem>
                        <!-- More ID fields -->
                    </MudGrid>
                </MudStack>
            </MudStep>

            <!-- Step 3: Employment Details -->
            <MudStep Title="Employment" Icon="@Icons.Material.Filled.Work">
                <!-- Employment fields -->
            </MudStep>

            <!-- Step 4: Review & Submit -->
            <MudStep Title="Review" Icon="@Icons.Material.Filled.CheckCircle">
                <!-- Summary view -->
            </MudStep>

        </MudStepper>
    </DialogContent>
    
    <DialogActions>
        <MudButton OnClick="@(() => _stepper?.StepBack())">Back</MudButton>
        <MudButton Color="Color.Primary" OnClick="@(() => _stepper?.StepForward())">Next</MudButton>
        <MudButton Color="Color.Success" OnClick="@SubmitEmployee">Submit</MudButton>
    </DialogActions>
</MudDialog>
```

---

## ✅ SUB-COMPONENT PATTERN

- ✅ **Reusable Sub-Pages**: EmployeeContacts, EmployeeDependents, EmployeeEducation
- ✅ **Parameter-Based**: Accept EmployeeId as parameter
- ✅ **CRUD in Sub-Component**: Add/Edit/Delete within sub-page
- ✅ **Inline Dialogs**: Use custom dialogs for add/edit
- ✅ **List Display**: MudList with custom formatting

**Sub-Component Pattern:**
```csharp
@code {
    [Parameter]
    public DefaultIdType EmployeeId { get; set; }

    private List<EmployeeContactResponse> _contacts = new();
    private bool _loading;

    protected override async Task OnInitializedAsync()
    {
        await LoadContacts();
    }

    private async Task LoadContacts()
    {
        _loading = true;
        try
        {
            var request = new SearchEmployeeContactsRequest
            {
                EmployeeId = EmployeeId
            };
            var result = await Client.SearchEmployeeContactsEndpointAsync("1", request);
            _contacts = result?.Items?.ToList() ?? new();
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task AddContact()
    {
        var parameters = new DialogParameters
        {
            { nameof(EmployeeContactDialog.EmployeeId), EmployeeId },
            { nameof(EmployeeContactDialog.Contact), null }
        };
        
        var dialog = await DialogService.ShowModalAsync<EmployeeContactDialog>(parameters);
        if (!dialog.Canceled)
        {
            await LoadContacts();
        }
    }
}
```

**Sub-Component Display:**
```csharp
<MudPaper Elevation="0" Class="pa-4 mb-4">
    <MudStack Spacing="3">
        <MudStack Row="true" AlignItems="AlignItems.Center" Justify="Justify.SpaceBetween">
            <MudText Typo="Typo.subtitle2">Contact Information</MudText>
            <MudButton StartIcon="@Icons.Material.Filled.Add" 
                      Size="Size.Small" 
                      Variant="Variant.Text" 
                      Color="Color.Primary" 
                      OnClick="AddContact">
                Add Contact
            </MudButton>
        </MudStack>

        @if (_contacts == null)
        {
            <MudProgressCircular IsIndeterminate="true" />
        }
        else if (_contacts.Count == 0)
        {
            <MudAlert Severity="Severity.Info">
                No emergency contacts added yet. Click "Add Contact" to add one.
            </MudAlert>
        }
        else
        {
            <MudList T="EmployeeContactResponse" Clickable="false" Dense="true">
                @foreach (var contact in _contacts)
                {
                    <MudListItem T="EmployeeContactResponse">
                        <MudStack Spacing="1" Class="full-width">
                            <MudStack Row="true" Justify="Justify.SpaceBetween">
                                <MudStack Spacing="0">
                                    <MudText Typo="Typo.body2">
                                        <strong>@($"{contact.FirstName} {contact.LastName}")</strong>
                                    </MudText>
                                    <MudText Typo="Typo.caption">@contact.Relationship</MudText>
                                </MudStack>
                                <MudStack Row="true" Spacing="1">
                                    <MudButton Size="Size.Small" 
                                              Variant="Variant.Text" 
                                              Color="Color.Primary" 
                                              OnClick="@(() => EditContact(contact))">
                                        Edit
                                    </MudButton>
                                    <MudButton Size="Size.Small" 
                                              Variant="Variant.Text" 
                                              Color="Color.Error" 
                                              OnClick="@(() => DeleteContact(contact.Id))">
                                        Delete
                                    </MudButton>
                                </MudStack>
                            </MudStack>

                            <MudStack Spacing="0" Class="mt-2">
                                @if (!string.IsNullOrWhiteSpace(contact.PhoneNumber))
                                {
                                    <MudText Typo="Typo.caption">
                                        <MudIcon Size="Size.Small" Icon="@Icons.Material.Filled.Phone" />
                                        @contact.PhoneNumber
                                    </MudText>
                                }
                            </MudStack>
                        </MudStack>
                    </MudListItem>
                    <MudDivider />
                }
            </MudList>
        }
    </MudStack>
</MudPaper>
```

---

## ✅ TABBED VIEWS PATTERN

- ✅ **MudTabs**: Organize related views
- ✅ **Current/Historical**: Separate active and past records
- ✅ **Icons on Tabs**: Visual indicators for tab content
- ✅ **EntityTable per Tab**: Each tab can have its own table

**Tabbed View Pattern:**
```csharp
<MudTabs Outlined="true" Rounded="true" Border="true">
    
    <!-- Tab 1: Current Assignments -->
    <MudTabPanel Text="Current Assignments" Icon="@Icons.Material.Filled.Assignment">
        <MudPaper Elevation="0" Class="pa-4">
            <EntityTable TEntity="DesignationAssignmentResponse" 
                        TId="DefaultIdType" 
                        TRequest="DesignationAssignmentViewModel" 
                        Context="CurrentContext">
                <!-- Current assignments configuration -->
            </EntityTable>
        </MudPaper>
    </MudTabPanel>

    <!-- Tab 2: Historical Assignments -->
    <MudTabPanel Text="Assignment History" Icon="@Icons.Material.Filled.History">
        <MudPaper Elevation="0" Class="pa-4">
            <EntityTable TEntity="DesignationAssignmentHistoryResponse" 
                        TId="DefaultIdType" 
                        TRequest="DesignationAssignmentHistoryViewModel" 
                        Context="HistoryContext">
                <!-- Historical assignments configuration -->
            </EntityTable>
        </MudPaper>
    </MudTabPanel>

</MudTabs>
```

---

## ✅ RELATED DATA NAVIGATION

- ✅ **Menu Actions**: Navigate to related sub-pages
- ✅ **MudMenu**: Dropdown menu for multiple actions
- ✅ **Icon-Based**: Clear icons for each action
- ✅ **Context Passing**: Pass entity ID via navigation

**Navigation Menu Pattern:**
```csharp
<ExtraActions Context="employee">
    <MudMenu Label="Actions" 
             Variant="Variant.Filled"
             EndIcon="@Icons.Material.Filled.KeyboardArrowDown" 
             IconColor="Color.Secondary">
        <MudMenuItem Icon="@Icons.Material.Filled.Visibility"
                     OnClick="@(() => ViewProfile(employee.Id))">
            View Profile
        </MudMenuItem>
        <MudDivider />
        <MudMenuItem Icon="@Icons.Material.Filled.Contacts"
                     OnClick="@(() => NavigateTo("/hr/employees/contacts", employee.Id))">
            View Contacts
        </MudMenuItem>
        <MudMenuItem Icon="@Icons.Material.Filled.FamilyRestroom"
                     OnClick="@(() => NavigateTo("/hr/employees/dependents", employee.Id))">
            View Dependents
        </MudMenuItem>
        <MudMenuItem Icon="@Icons.Material.Filled.School"
                     OnClick="@(() => NavigateTo("/hr/employees/educations", employee.Id))">
            View Education
        </MudMenuItem>
    </MudMenu>
</ExtraActions>

@code {
    private void NavigateTo(string basePath, DefaultIdType employeeId)
    {
        NavigationManager.NavigateTo($"{basePath}/{employeeId}");
    }
}
```

---

## ✅ BANK ACCOUNTS MANAGEMENT PATTERN

### **Overview**
The Bank Accounts module manages employee bank accounts for payroll routing and direct deposit processing. Supports both domestic (ACH) and international (SWIFT/IBAN) accounts with comprehensive verification workflows.

**Implementation Status**: ✅ Complete & Production Ready

### **Core Features**
- ✅ **Create**: Add domestic and international bank accounts
- ✅ **Read**: List with search, filter, and pagination
- ✅ **Update**: Edit account details and status
- ✅ **Delete**: Remove accounts (audit trail retained)
- ✅ **Verify**: Manual and micro-deposit verification methods
- ✅ **Primary Account**: Designate primary for payroll routing
- ✅ **Activation**: Control account availability for payroll
- ✅ **Embedded Component**: Reusable in employee profile

### **File Structure**
```
/Pages/Hr/Employees/BankAccounts/
├── BankAccounts.razor                    # Main page (UI only)
├── BankAccounts.razor.cs                 # Code-behind (all logic)
├── BankAccountDialog.razor               # Add/Edit dialog
├── BankAccountsComponent.razor           # Embedded sub-component
├── BankAccountsHelpDialog.razor          # Help documentation
└── BankAccountVerificationDialog.razor   # Verification workflow

/Components/Hr/
└── AutocompleteEmployee.razor            # Employee selection component
```

### **Architecture Pattern**

**Separation of Concerns:**
```
BankAccounts.razor (UI Markup Only)
├─ @page, @namespace, @using statements
├─ PageHeader component
├─ Filter alert
├─ Help button
├─ EntityTable declaration with @ref
├─ ExtraActions (references code-behind methods)
└─ EditFormContent (references properties)
└─ NO @code section

BankAccounts.razor.cs (Code-Behind)
├─ [Inject] services (Client, DialogService, Snackbar, NavigationManager)
├─ [SupplyParameterFromQuery] FilterEmployeeId
├─ Properties: FilterSuffix, Context, DialogOptions, _table reference
├─ OnInitializedAsync() - EntityServerTableContext setup
├─ ShowBankAccountsHelp() - Help dialog
├─ ClearFilter() - Navigation
├─ OnSetPrimary() - Action handler
├─ OnVerifyAccount() - Action handler
├─ OnUnverifyAccount() - Action handler
├─ OnActivateAccount() - Action handler
└─ OnDeactivateAccount() - Action handler

BankAccountViewModel Class
├─ All properties for create/update operations
├─ ToCreateCommand() - Conversion method
└─ ToUpdateCommand() - Conversion method
```

### **Main Page Pattern (BankAccounts.razor)**

**Header Section:**
```razor
<PageHeader Title="@($"Bank Accounts{FilterSuffix}")" 
            Header="Bank Accounts" 
            SubHeader="Manage employee bank details for payroll routing and direct deposit." />
```

**Filter Alert:**
```razor
@if (!string.IsNullOrEmpty(FilterEmployeeId))
{
    <MudAlert Severity="Severity.Info" Class="mb-4">
        Viewing bank accounts for Employee ID: <strong>@FilterEmployeeId</strong>
        <MudButton Size="Size.Small" Variant="Variant.Text" OnClick="ClearFilter">
            Clear Filter
        </MudButton>
    </MudAlert>
}
```

**Toolbar:**
```razor
<MudPaper Elevation="2" Class="pa-4 mb-4">
    <MudStack Row="true" Spacing="2" Wrap="Wrap.Wrap" AlignItems="AlignItems.Center">
        <MudSpacer />
        <MudButtonGroup Color="Color.Info" Variant="Variant.Outlined" Size="Size.Small">
            <MudButton StartIcon="@Icons.Material.Filled.Help" OnClick="@ShowBankAccountsHelp">
                Help
            </MudButton>
        </MudButtonGroup>
    </MudStack>
</MudPaper>
```

**Table with Extra Actions:**
```razor
<EntityTable @ref="_table" 
             TEntity="BankAccountResponse" 
             TId="DefaultIdType" 
             TRequest="BankAccountViewModel" 
             Context="@Context">
    
    <ExtraActions Context="account">
        @if (account.Id != DefaultIdType.Empty)
        {
            <MudMenuItem OnClick="@(() => OnSetPrimary(account.Id))" 
                        Disabled="@account.IsPrimary">
                <MudIcon Icon="@Icons.Material.Filled.CheckCircle" Class="me-2" />
                Set as Primary
            </MudMenuItem>
            
            @if (!account.IsVerified)
            {
                <MudMenuItem OnClick="@(() => OnVerifyAccount(account.Id))">
                    <MudIcon Icon="@Icons.Material.Filled.VerifiedUser" Class="me-2" />
                    Verify Account
                </MudMenuItem>
            }
            
            @if (account.IsActive)
            {
                <MudMenuItem OnClick="@(() => OnDeactivateAccount(account.Id))">
                    <MudIcon Icon="@Icons.Material.Filled.Lock" Class="me-2" />
                    Deactivate
                </MudMenuItem>
            }
            else
            {
                <MudMenuItem OnClick="@(() => OnActivateAccount(account.Id))">
                    <MudIcon Icon="@Icons.Material.Filled.LockOpen" Class="me-2" />
                    Activate
                </MudMenuItem>
            }
        }
    </ExtraActions>

    <EditFormContent Context="context">
        <!-- Form fields here -->
    </EditFormContent>
</EntityTable>
```

### **Code-Behind Pattern (BankAccounts.razor.cs)**

**Service Injections & Properties:**
```csharp
public partial class BankAccounts
{
    [SupplyParameterFromQuery] public string? FilterEmployeeId { get; set; }
    
    public string FilterSuffix => !string.IsNullOrEmpty(FilterEmployeeId) 
        ? $" - Employee {FilterEmployeeId}" 
        : string.Empty;

    protected EntityServerTableContext<BankAccountResponse, DefaultIdType, BankAccountViewModel> 
        Context { get; set; } = null!;

    private readonly DialogOptions _helpDialogOptions = new() 
    { 
        CloseOnEscapeKey = true, 
        MaxWidth = MaxWidth.Large, 
        FullWidth = true 
    };
    
    private EntityTable<BankAccountResponse, DefaultIdType, BankAccountViewModel>? _table;
}
```

**EntityServerTableContext Setup:**
```csharp
protected override Task OnInitializedAsync()
{
    Context = new EntityServerTableContext<BankAccountResponse, DefaultIdType, BankAccountViewModel>(
        entityName: "Bank Account",
        entityNamePlural: "Bank Accounts",
        entityResource: "BankAccounts",
        fields:
        [
            new EntityField<BankAccountResponse>(r => r.BankName, "Bank Name", "BankName"),
            new EntityField<BankAccountResponse>(r => r.AccountHolderName, "Account Holder", "AccountHolderName"),
            new EntityField<BankAccountResponse>(r => r.Last4Digits, "Last 4 Digits", "Last4Digits"),
            new EntityField<BankAccountResponse>(r => r.AccountType, "Type", "AccountType"),
            new EntityField<BankAccountResponse>(r => r.IsPrimary ? "Primary" : "Secondary", "Status", "IsPrimary"),
            new EntityField<BankAccountResponse>(r => r.IsVerified ? "Verified" : "Unverified", "Verified", "IsVerified"),
        ],
        enableAdvancedSearch: true,
        idFunc: response => response.Id,
        searchFunc: async filter =>
        {
            var request = new SearchBankAccountsRequest
            {
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                EmployeeId = !string.IsNullOrEmpty(FilterEmployeeId) 
                    ? Guid.Parse(FilterEmployeeId) 
                    : null
            };
            var result = await Client.SearchBankAccountsEndpointAsync("1", request);
            return result.Adapt<PaginationResponse<BankAccountResponse>>();
        },
        createFunc: async bankAccount =>
        {
            await Client.CreateBankAccountEndpointAsync("1", bankAccount.ToCreateCommand());
        },
        updateFunc: async (id, bankAccount) =>
        {
            await Client.UpdateBankAccountEndpointAsync("1", id, bankAccount.ToUpdateCommand());
        },
        deleteFunc: async id =>
        {
            await Client.DeleteBankAccountEndpointAsync("1", id);
        });

    return Task.CompletedTask;
}
```

**Action Methods Pattern:**
```csharp
private async Task OnSetPrimary(DefaultIdType accountId)
{
    var confirmed = await DialogService.ShowMessageBox(
        title: "Set as Primary",
        markupMessage: new MarkupString(
            "<p>Set this account as the primary bank account for payroll deposits?</p>" +
            "<p><strong>Note:</strong> Only one account can be primary per employee.</p>"),
        yesText: "Yes",
        noText: "No");

    if (confirmed.HasValue && confirmed.Value)
    {
        try
        {
            var account = await Client.GetBankAccountEndpointAsync("1", accountId);
            var command = new UpdateBankAccountCommand()
            {
                Id = account.Id,
                BankName = account.BankName,
                AccountHolderName = account.AccountHolderName,
                SwiftCode = account.SwiftCode,
                Iban = account.Iban,
                Notes = account.Notes,
                IsPrimary = true,
                IsActive = account.IsActive
            };
            await Client.UpdateBankAccountEndpointAsync("1", accountId, command);

            Snackbar?.Add("Account set as primary for payroll routing.", Severity.Success);
            await _table?.ReloadDataAsync()!;
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error setting primary account: {ex.Message}", Severity.Error);
        }
    }
}
```

### **Embedded Component Pattern (BankAccountsComponent.razor)**

**Header with Add Button:**
```razor
<MudPaper Elevation="0" Class="pa-4 mb-4">
    <MudStack Spacing="3">
        <MudStack Row="true" AlignItems="AlignItems.Center" Justify="Justify.SpaceBetween">
            <MudText Typo="Typo.subtitle2">Bank Accounts for Payroll</MudText>
            <MudButton StartIcon="@Icons.Material.Filled.Add" 
                      Size="Size.Small" 
                      Variant="Variant.Text" 
                      Color="Color.Primary" 
                      OnClick="AddBankAccount">
                Add Bank Account
            </MudButton>
        </MudStack>
```

**List Display with Status Chips:**
```razor
        @if (_loading)
        {
            <MudProgressCircular IsIndeterminate="true" />
        }
        else if (_accounts == null || _accounts.Count == 0)
        {
            <MudAlert Severity="Severity.Info">
                No bank accounts added yet. Click "Add Bank Account" to add one.
            </MudAlert>
        }
        else
        {
            <MudList T="BankAccountResponse" Dense="true">
                @foreach (var account in _accounts)
                {
                    <MudListItem T="BankAccountResponse">
                        <MudStack Spacing="1" Class="full-width">
                            <MudStack Row="true" Justify="Justify.SpaceBetween" AlignItems="AlignItems.Center">
                                <MudStack Spacing="0">
                                    <MudText Typo="Typo.body2">
                                        <strong>@account.BankName</strong>
                                    </MudText>
                                    <MudText Typo="Typo.caption">@account.AccountHolderName</MudText>
                                </MudStack>
                                <MudStack Row="true" Spacing="1">
                                    <MudButton Size="Size.Small" Variant="Variant.Text" Color="Color.Primary" 
                                              OnClick="@(() => EditAccount(account.Id))">
                                        Edit
                                    </MudButton>
                                    <MudButton Size="Size.Small" Variant="Variant.Text" Color="Color.Error" 
                                              OnClick="@(() => DeleteAccount(account.Id))">
                                        Delete
                                    </MudButton>
                                </MudStack>
                            </MudStack>

                            <MudStack Spacing="0" Class="mt-2">
                                <MudText Typo="Typo.caption">
                                    <MudIcon Size="Size.Small" Icon="@Icons.Material.Filled.CreditCard" Class="mr-1" />
                                    @account.AccountType
                                </MudText>
                                <MudText Typo="Typo.caption">
                                    <MudIcon Size="Size.Small" Icon="@Icons.Material.Filled.Lock" Class="mr-1" />
                                    ****@account.Last4Digits
                                </MudText>
                                <MudStack Row="true" Spacing="2" Class="mt-2">
                                    <MudChip T="string" Size="Size.Small" Color="@(account.IsPrimary ? Color.Primary : Color.Default)">
                                        @(account.IsPrimary ? "Primary" : "Secondary")
                                    </MudChip>
                                    <MudChip T="string" Size="Size.Small" Color="@(account.IsVerified ? Color.Success : Color.Warning)">
                                        @(account.IsVerified ? "Verified" : "Unverified")
                                    </MudChip>
                                    @if (!account.IsActive)
                                    {
                                        <MudChip T="string" Size="Size.Small" Color="Color.Error">
                                            Inactive
                                        </MudChip>
                                    }
                                </MudStack>
                            </MudStack>
                        </MudStack>
                    </MudListItem>
                    <MudDivider />
                }
            </MudList>
        }
    </MudStack>
</MudPaper>
```

**Component Code-Behind Pattern:**
```csharp
public partial class BankAccountsComponent
{
    [Parameter]
    public DefaultIdType EmployeeId { get; set; }

    [Inject] public required IApiClient Client { get; set; }
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }

    private List<BankAccountResponse>? _accounts;
    private bool _loading;

    protected override async Task OnInitializedAsync()
    {
        await LoadAccounts();
    }

    private async Task LoadAccounts()
    {
        _loading = true;
        try
        {
            var request = new SearchBankAccountsRequest
            {
                EmployeeId = EmployeeId,
                PageNumber = 1,
                PageSize = 100
            };
            var response = await Client.SearchBankAccountsEndpointAsync("1", request);
            var result = response.Adapt<PaginationResponse<BankAccountResponse>>();
            _accounts = result.Items.ToList();
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading bank accounts: {ex.Message}", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task AddBankAccount()
    {
        var options = new DialogOptions 
        { 
            CloseButton = true, 
            MaxWidth = MaxWidth.Medium, 
            FullWidth = true 
        };
        var result = await DialogService.ShowAsync<BankAccountDialog>(
            "Add Bank Account",
            new DialogParameters { { "EmployeeId", EmployeeId } },
            options);

        if (result is not null)
        {
            await LoadAccounts();
        }
    }

    private async Task EditAccount(DefaultIdType accountId)
    {
        try
        {
            var account = await Client.GetBankAccountEndpointAsync("1", accountId);
            var options = new DialogOptions 
            { 
                CloseButton = true, 
                MaxWidth = MaxWidth.Medium, 
                FullWidth = true 
            };
            var result = await DialogService.ShowAsync<BankAccountDialog>(
                "Edit Bank Account",
                new DialogParameters { { "BankAccount", account } },
                options);

            if (result is not null)
            {
                await LoadAccounts();
            }
        }
        catch (Exception ex)
        {
            Snackbar?.Add($"Error loading bank account: {ex.Message}", Severity.Error);
        }
    }

    private async Task DeleteAccount(DefaultIdType accountId)
    {
        var confirmed = await DialogService.ShowMessageBox(
            "Delete Bank Account",
            "Are you sure you want to delete this bank account?",
            "Delete",
            "Cancel");

        if (confirmed == true)
        {
            try
            {
                await Client.DeleteBankAccountEndpointAsync("1", accountId);
                Snackbar?.Add("Bank account deleted successfully.", Severity.Success);
                await LoadAccounts();
            }
            catch (Exception ex)
            {
                Snackbar?.Add($"Error deleting bank account: {ex.Message}", Severity.Error);
            }
        }
    }
}
```

### **Dialog Pattern (BankAccountDialog.razor)**

**Employee & Bank Information Section:**
```razor
<MudItem xs="12" sm="6" md="6">
    <AutocompleteEmployee @bind-Value="context.SelectedEmployee"
                         Label="Employee"
                         Required="true"
                         RequiredError="Employee is required"
                         Variant="Variant.Filled"
                         HelperText="Search and select employee"
                         Disabled="@(!Context.AddEditModal.IsCreate)" />
</MudItem>

<MudItem xs="12" sm="6" md="6">
    <MudTextField @bind-Value="context.BankName"
                  For="@(() => context.BankName)"
                  Label="Bank Name"
                  Variant="Variant.Filled"
                  Required="true"
                  HelperText="E.g., BDO, BPI, Metrobank" />
</MudItem>

<MudItem xs="12" sm="6" md="6">
    <MudTextField @bind-Value="context.AccountHolderName"
                  For="@(() => context.AccountHolderName)"
                  Label="Account Holder Name"
                  Variant="Variant.Filled"
                  Required="true"
                  HelperText="As shown in bank records" />
</MudItem>

<MudItem xs="12" sm="6" md="6">
    <MudTextField @bind-Value="context.AccountType"
                  For="@(() => context.AccountType)"
                  Label="Account Type"
                  Variant="Variant.Filled"
                  Required="true"
                  HelperText="Checking, Savings, Money Market, etc." />
</MudItem>
```

**Account Details Section:**
```razor
<MudDivider DividerType="DividerType.FullWidth" Class="my-4" />
<MudItem xs="12">
    <MudText Typo="Typo.subtitle2" GutterBottom="true">Account Details</MudText>
</MudItem>

<MudItem xs="12" sm="6" md="6">
    <MudTextField @bind-Value="context.AccountNumber"
                  For="@(() => context.AccountNumber)"
                  Label="Account Number"
                  Variant="Variant.Filled"
                  Required="true"
                  HelperText="Will be masked for security" />
</MudItem>

<MudItem xs="12" sm="6" md="6">
    <MudTextField @bind-Value="context.RoutingNumber"
                  For="@(() => context.RoutingNumber)"
                  Label="Routing Number"
                  Variant="Variant.Filled"
                  Required="true"
                  HelperText="9-digit ABA/ACH code (US)" />
</MudItem>
```

**International Details Section:**
```razor
<MudDivider DividerType="DividerType.FullWidth" Class="my-4" />
<MudItem xs="12">
    <MudText Typo="Typo.subtitle2" GutterBottom="true">International Details (Optional)</MudText>
</MudItem>

<MudItem xs="12" sm="6" md="6">
    <MudTextField @bind-Value="context.SwiftCode"
                  For="@(() => context.SwiftCode)"
                  Label="SWIFT Code"
                  Variant="Variant.Filled"
                  HelperText="8-11 alphanumeric code" />
</MudItem>

<MudItem xs="12" sm="6" md="6">
    <MudTextField @bind-Value="context.Iban"
                  For="@(() => context.Iban)"
                  Label="IBAN"
                  Variant="Variant.Filled"
                  HelperText="International Bank Account Number" />
</MudItem>

<MudItem xs="12" sm="6" md="6">
    <MudTextField @bind-Value="context.CurrencyCode"
                  For="@(() => context.CurrencyCode)"
                  Label="Currency Code"
                  Variant="Variant.Filled"
                  HelperText="USD, EUR, PHP, etc." />
</MudItem>
```

### **ViewModel Pattern**

**BankAccountViewModel Class:**
```csharp
public class BankAccountViewModel
{
    // Core properties from response
    public DefaultIdType Id { get; set; }
    public DefaultIdType EmployeeId { get; set; }
    public EmployeeResponse? SelectedEmployee { get; set; }

    // Account information
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    public string? RoutingNumber { get; set; }
    public string? AccountType { get; set; }
    public string? AccountHolderName { get; set; }

    // International details
    public string? SwiftCode { get; set; }
    public string? Iban { get; set; }
    public string? CurrencyCode { get; set; }

    // Status properties
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public DateTime? VerificationDate { get; set; }
    public string? Last4Digits { get; set; }
    public string? Notes { get; set; }

    // Conversion methods
    public CreateBankAccountCommand ToCreateCommand() => new()
    {
        EmployeeId = EmployeeId,
        AccountNumber = AccountNumber ?? string.Empty,
        RoutingNumber = RoutingNumber ?? string.Empty,
        BankName = BankName ?? string.Empty,
        AccountType = AccountType ?? string.Empty,
        AccountHolderName = AccountHolderName ?? string.Empty,
        SwiftCode = SwiftCode,
        Iban = Iban,
        Notes = Notes
    };

    public UpdateBankAccountCommand ToUpdateCommand() => new()
    {
        Id = Id,
        BankName = BankName,
        AccountHolderName = AccountHolderName,
        SwiftCode = SwiftCode,
        Iban = Iban,
        Notes = Notes,
        IsPrimary = IsPrimary,
        IsActive = IsActive
    };
}
```

### **Integration with Employees Page**

**Menu Action:**
```razor
<MudMenuItem Icon="@Icons.Material.Filled.AccountBalance"
             OnClick="@(() => NavigateTo("/human-resources/employees/bank-accounts", employee.Id))">
    View Bank Accounts
</MudMenuItem>
```

**Embedded Component in Edit Modal:**
```razor
<MudItem xs="12" Class="mt-6">
    <MudDivider />
</MudItem>
<MudItem xs="12" Class="mt-4">
    <BankAccountsComponent EmployeeId="@context.Id" />
</MudItem>
```

### **Help Dialog Pattern**

**Dialog Structure:**
```razor
<MudDialog>
    <DialogContent>
        <MudStack Spacing="3">
            <MudText Typo="Typo.h6">Bank Accounts Help</MudText>

            <MudExpansionPanels>
                <MudExpansionPanel Text="What are Bank Accounts Used For?">
                    <!-- Content sections with examples -->
                </MudExpansionPanel>

                <MudExpansionPanel Text="Adding Bank Accounts">
                    <!-- Step-by-step instructions -->
                </MudExpansionPanel>

                <MudExpansionPanel Text="Account Verification Process">
                    <!-- Verification methods and procedures -->
                </MudExpansionPanel>

                <!-- More panels for workflows, scenarios, troubleshooting -->
            </MudExpansionPanels>
        </MudStack>
    </DialogContent>
    <DialogActions>
        <MudButton OnClick="Close">Close</MudButton>
    </DialogActions>
</MudDialog>

@code {
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    private Task Close()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }
}
```

---

## 🎯 BANK ACCOUNTS MENU CONFIGURATION

**Location**: MenuService.cs - Human Resource → Payroll section

```csharp
new MenuSectionSubItemModel 
{ 
    Title = "Bank Accounts", 
    Icon = Icons.Material.Filled.AccountBalance, 
    Href = "/human-resources/employees/bank-accounts",  // ✅ Correct route
    Action = FshActions.View, 
    Resource = FshResources.Payroll, 
    PageStatus = PageStatus.Completed  // ✅ Marked as complete
}
```

---

## 📊 BANK ACCOUNTS UI CHECKLIST

### **Main Page Features**
- [ ] BankAccounts.razor with UI only (no @code)
- [ ] BankAccounts.razor.cs with complete code-behind
- [ ] PageHeader with dynamic title and description
- [ ] Filter alert for employee-specific view
- [ ] Help button in toolbar
- [ ] EntityTable with search and pagination
- [ ] Extra actions menu (Set Primary, Verify, Activate/Deactivate)
- [ ] Edit form with all account fields

### **Embedded Component (BankAccountsComponent)**
- [ ] MudPaper with proper styling
- [ ] Header with title and Add button
- [ ] Loading state (MudProgressCircular)
- [ ] Empty state alert
- [ ] MudList for item display
- [ ] Edit/Delete buttons per item
- [ ] Status chips (Primary/Secondary, Verified/Unverified, Active/Inactive)
- [ ] Icons for visual clarity

### **Dialogs**
- [ ] BankAccountDialog for create/edit
- [ ] Employee autocomplete (AutocompleteEmployee)
- [ ] Domestic account fields (Account #, Routing #)
- [ ] International account fields (SWIFT, IBAN, Currency)
- [ ] Account status checkboxes (Primary, Active)
- [ ] BankAccountVerificationDialog for verification workflow
- [ ] BankAccountsHelpDialog with comprehensive help content

### **Code Quality**
- [ ] Service injection with [Inject] attributes
- [ ] [SupplyParameterFromQuery] for FilterEmployeeId
- [ ] EntityServerTableContext setup in OnInitializedAsync
- [ ] Dialog options configuration (_helpDialogOptions)
- [ ] Action methods with error handling
- [ ] Snackbar feedback for user actions
- [ ] Dialog result checking (is not null pattern)
- [ ] Table reload after mutations
- [ ] BankAccountViewModel with conversion methods

### **Integration**
- [ ] Menu item in MenuService (Human Resource → Payroll)
- [ ] Menu item in Employees.razor actions menu
- [ ] Embedded component in Employees.razor edit modal
- [ ] Query parameter support for employee filtering
- [ ] Navigation between employee and bank accounts

### **Documentation**
- [ ] Help dialog with 10+ sections
- [ ] Examples for all workflows
- [ ] Troubleshooting section
- [ ] Security & privacy notes
- [ ] Screenshots/visual guides

### **User Experience**
- [ ] Confirmation dialogs for important actions
- [ ] Error handling with clear messages
- [ ] Loading states during API calls
- [ ] Empty state messaging
- [ ] Filter clear functionality
- [ ] Responsive design for all screen sizes
- [ ] Accessibility compliance (ARIA labels, keyboard navigation)

---

## ✅ VERIFICATION STATUS

**HR Module**: ✅ A+ Verified & Documented  
**Bank Accounts Feature**: ✅ Complete & Production Ready

**Last verified**: November 22, 2025

