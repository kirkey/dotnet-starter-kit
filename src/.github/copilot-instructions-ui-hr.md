# 👥 COPILOT INSTRUCTIONS - UI HR MODULE

**Last Updated**: November 20, 2025  
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

## 🎯 MENUSERVICE & NAVIGATION

### **✅ MenuService Pattern**
- ✅ **Centralized Menu**: Single service for all navigation
- ✅ **Hierarchical Structure**: Sections → Items → Sub-Items
- ✅ **Permission-Based**: Resource and Action per menu item
- ✅ **Status Indicators**: PageStatus for in-progress/completed
- ✅ **Group Headers**: Visual grouping within sections

**MenuService Structure:**
```csharp
new MenuSectionItemModel
{
    Title = "Human Resources",
    Icon = Icons.Material.Filled.People,
    IsParent = true,
    MenuItems =
    [
        new MenuSectionSubItemModel 
        { 
            Title = "Employees", 
            Icon = Icons.Material.Filled.Badge, 
            Href = "/hr/employees" 
        },
        new MenuSectionSubItemModel 
        { 
            Title = "Designations", 
            Icon = Icons.Material.Filled.WorkOutline, 
            Href = "/hr/designations" 
        },
        new MenuSectionSubItemModel 
        { 
            Title = "Organizational Units", 
            Icon = Icons.Material.Filled.AccountTree, 
            Href = "/hr/organizational-units" 
        }
    ]
}
```

---

## 📊 HR UI CHECKLIST

### **Employee Management**
- [ ] Wizard pattern for multi-step employee creation
- [ ] Personal information validation
- [ ] Government ID format validation (Philippines)
- [ ] Sub-components for contacts, dependents, education
- [ ] Related data navigation menu
- [ ] Profile management capabilities

### **Designation Assignments**
- [ ] Current/Historical tabbed views
- [ ] Assignment type selection (Plantilla/Acting As)
- [ ] Date range validation (effective/end dates)
- [ ] Assignment history tracking
- [ ] Promotion workflows

### **Sub-Component Architecture**
- [ ] Accept entity ID as parameter
- [ ] Load related data on initialization
- [ ] Inline CRUD operations
- [ ] Custom dialogs for add/edit
- [ ] MudList display with actions
- [ ] Empty state handling

### **Navigation & Menu**
- [ ] MenuService configuration with sections
- [ ] Permission-based menu items
- [ ] Status indicators (InProgress/Completed)
- [ ] Group headers for organization
- [ ] External links support (target="_blank")

### **Wizard Implementation**
- [ ] MudStepper with multiple steps
- [ ] Step icons and titles
- [ ] Linear/non-linear navigation
- [ ] Validation per step
- [ ] Summary/review step
- [ ] Back/Next/Submit actions

---

## ✅ VERIFICATION STATUS

**HR Module**: ✅ A+ Verified & Documented  

**Last verified**: November 20, 2025

