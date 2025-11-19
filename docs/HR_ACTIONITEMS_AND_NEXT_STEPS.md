# 🎬 HR API & UI Audit - Action Items & Next Steps

**Date:** November 19, 2025  
**Prepared by:** GitHub Copilot  
**Status:** Ready for Implementation

---

## 📋 Immediate Action Items (This Week)

### ✅ Action 1: Enable HRAnalytics Endpoint
**Priority:** HIGH | **Effort:** 30 minutes

**Current State:**
```csharp
// In HumanResourcesModule.cs
// app.MapHrAnalyticsEndpoints();  ← COMMENTED OUT
```

**Action:**
1. Uncomment the line
2. Verify HRAnalytics endpoint implementation
3. Test endpoint connectivity
4. Document available metrics

**Acceptance Criteria:**
- Endpoint is registered and accessible
- Returns valid analytics data
- Documented in API specification

---

### ✅ Action 2: Generate API Client (NSwag)
**Priority:** CRITICAL | **Effort:** 2-4 hours

**Current State:**
```
/src/apps/blazor/infrastructure/Api/Client.cs
├── Accounting clients ✅
├── Catalog clients ✅
├── Store clients ✅
├── Warehouse clients ✅
└── HumanResources clients ❌ MISSING
```

**Actions:**
1. Review NSwag configuration
   ```
   nswag.json (or equivalent config)
   ```

2. Ensure HR endpoints are included in OpenAPI spec
   ```bash
   dotnet build (verify endpoints compile)
   ```

3. Generate client
   ```bash
   nswag run
   ```

4. Verify generated classes
   - Check all CRUD operations present
   - Validate DTOs generated correctly
   - Test basic connectivity

5. Document generated classes
   - Location: `/src/apps/blazor/infrastructure/Api/`
   - Generated files list

**Acceptance Criteria:**
- [ ] Client.cs includes HumanResources methods
- [ ] All request/response DTOs generated
- [ ] No compilation errors in Blazor project
- [ ] At least 5 methods tested successfully

**Testing Commands:**
```csharp
// In Blazor page
var client = new HumanResourcesClient(httpClient);

// Test Create
var response = await client.CreateEmployeeAsync(new CreateEmployeeRequest { ... });

// Test Search
var results = await client.SearchEmployeesAsync(new SearchEmployeesRequest { ... });

// Test Get
var employee = await client.GetEmployeeAsync(employeeId);
```

---

### ✅ Action 3: Verify Database Migrations
**Priority:** HIGH | **Effort:** 1 hour

**Actions:**
1. Check migration files exist
   ```
   /src/api/modules/HumanResources/HumanResources.Infrastructure/Persistence/Migrations/
   ```

2. Verify all 39 entities have migration support
3. Test migration on local database
   ```bash
   dotnet ef database update -p HumanResources.Infrastructure
   ```

4. Confirm schema created correctly
   ```sql
   SELECT * FROM information_schema.tables 
   WHERE table_schema = 'HumanResources'
   ```

**Expected Tables:** 39+

---

### ✅ Action 4: Compile & Test API
**Priority:** CRITICAL | **Effort:** 30 minutes

**Actions:**
1. Full solution build
   ```bash
   dotnet build FSH.Starter.sln
   ```

2. Run API server
   ```bash
   dotnet run --project src/api/server/Server.csproj
   ```

3. Navigate to Swagger
   ```
   https://localhost:5001/swagger
   ```

4. Verify HR endpoints visible
   - Filter by "humanresources"
   - Should show 38+ endpoints

5. Test 3-5 endpoints manually
   - Create Organization
   - Create Employee
   - Search Employees
   - Create Payroll
   - Get Payroll

**Acceptance Criteria:**
- [ ] API builds cleanly
- [ ] Swagger UI shows all HR endpoints
- [ ] Test endpoints return 200 OK
- [ ] Request/response schemas correct

---

## 📅 Phase 1: Setup & Infrastructure (Week 1)

### Phase 1.1: API Client & Connectivity
**Duration:** 1 day | **Owner:** Backend Developer

**Deliverables:**
- [ ] API client generated and tested
- [ ] Blazor → API connectivity verified
- [ ] Sample API call working in Blazor
- [ ] HRAnalytics endpoint enabled

**Definition of Done:**
```csharp
// This should work without errors
var client = new HumanResourcesClient(httpClient);
var employees = await client.SearchEmployeesAsync(
    new SearchEmployeesRequest { PageNumber = 1, PageSize = 10 }
);
Console.WriteLine($"Found {employees.Data.Count} employees");
```

---

### Phase 1.2: Create Shared HR Component Library
**Duration:** 2-3 days | **Owner:** Frontend Developer

**Components to Create:**

1. **EmployeePicker.razor** (Reusable)
   ```razor
   @typeparam TItem where TItem : class
   
   <MudSelect T="EmployeeResponse" Label="Employee">
       @foreach(var emp in employees)
       {
           <MudSelectItem Value="emp">@emp.FirstName @emp.LastName</MudSelectItem>
       }
   </MudSelect>
   ```

2. **OrganizationalUnitTreeView.razor**
   ```razor
   <MudTreeView Items="organizationalUnits" @bind-SelectedValue="selectedUnit">
       @foreach(var unit in organizationalUnits)
       {
           <MudTreeViewItem @key="unit.Id" Value="unit">
               @unit.Name
           </MudTreeViewItem>
       }
   </MudTreeView>
   ```

3. **DesignationSelector.razor**
   ```razor
   <MudSelect T="DesignationResponse" Label="Designation">
       @foreach(var designation in designations)
       {
           <MudSelectItem Value="designation">
               @designation.Code - @designation.Title 
               (₱@designation.MinSalary - ₱@designation.MaxSalary)
           </MudSelectItem>
       }
   </MudSelect>
   ```

4. **StatusBadge.razor**
   ```razor
   @switch(Status) {
       case "Draft": <MudChip Color="Color.Default">Draft</MudChip> break;
       case "Approved": <MudChip Color="Color.Success">Approved</MudChip> break;
       case "Rejected": <MudChip Color="Color.Error">Rejected</MudChip> break;
   }
   ```

5. **PayrollCalculationSummary.razor**
   ```razor
   <MudGrid>
       <MudItem xs="12">
           <MudField Label="Gross Pay">₱@Payroll.GrossPay.ToString("N2")</MudField>
       </MudItem>
       <MudItem xs="12">
           <MudField Label="Total Deductions">₱@Payroll.TotalDeductions.ToString("N2")</MudField>
       </MudItem>
       <MudItem xs="12">
           <MudField Label="Net Pay">₱@Payroll.NetPay.ToString("N2")</MudField>
       </MudItem>
   </MudGrid>
   ```

**Folder Structure:**
```
/src/apps/blazor/client/Components/HumanResources/
├── EmployeePicker.razor
├── OrganizationalUnitTreeView.razor
├── DesignationSelector.razor
├── StatusBadge.razor
├── PayrollCalculationSummary.razor
├── LeaveBalanceCard.razor
├── AttendanceCalendar.razor
└── ApprovalWorkflow.razor
```

**Definition of Done:**
- [ ] All 8 components created
- [ ] Each component tested independently
- [ ] Proper prop binding implemented
- [ ] Responsive design verified

---

### Phase 1.3: Create Base Page Templates
**Duration:** 1 day | **Owner:** Frontend Developer

**Template 1: EntityCrudPage.razor**
```razor
@page "/humanresources/{Module}/{Action?}/{Id?}"
@typeparam TResponse where TResponse : class
@typeparam TCreateRequest where TCreateRequest : new()

<MudContainer MaxWidth="MaxWidth.Large" Class="py-8">
    @if (IsLoading)
    {
        <MudSkeleton SkeletonType="SkeletonType.Table" />
    }
    else if (IsDetailView)
    {
        <EntityDetail @bind-Item="SelectedItem" OnSave="SaveItemAsync" />
    }
    else
    {
        <EntityTable Items="Items" OnEdit="EditItemAsync" OnDelete="DeleteItemAsync" />
    }
</MudContainer>
```

**Template 2: FormPage.razor**
```razor
@typeparam TRequest where TRequest : new()

<MudForm @ref="form">
    <MudGrid>
        <MudItem xs="12">
            <MudTextField @bind-Value="Model.PropertyName" Label="Label" />
        </MudItem>
        <MudItem xs="12" Class="d-flex justify-end gap-2">
            <MudButton OnClick="SubmitAsync" Variant="Variant.Filled" Color="Color.Primary">
                Save
            </MudButton>
            <MudButton OnClick="Cancel" Variant="Variant.Outlined">
                Cancel
            </MudButton>
        </MudItem>
    </MudGrid>
</MudForm>
```

**Definition of Done:**
- [ ] Base templates created
- [ ] Proper async patterns implemented
- [ ] Error handling in place
- [ ] Loading states visible

---

## 📅 Phase 2: Critical Employee Management (Week 1-2)

### Phase 2.1: Organization Setup Pages
**Duration:** 2 days | **Owner:** Frontend Developer

#### OrganizationalUnits.razor
```razor
@page "/humanresources/organization/units"
@inject HumanResourcesClient ApiClient
@inject IDialogService DialogService
@inject ISnackbar Snackbar

<MudContainer MaxWidth="MaxWidth.Large" Class="py-8">
    <MudGrid>
        <MudItem xs="12">
            <MudButton Variant="Variant.Filled" 
                      Color="Color.Primary" 
                      OnClick="@(() => CreateUnitAsync())">
                Create Unit
            </MudButton>
        </MudItem>
        <MudItem xs="12">
            <MudTreeView Items="units" @bind-SelectedValue="selectedUnit">
                @foreach(var unit in units)
                {
                    <MudTreeViewItem @key="unit.Id" Value="@unit">
                        <Content>
                            <div class="d-flex justify-space-between" style="width: 100%;">
                                <span>@unit.Code - @unit.Name</span>
                                <div>
                                    <MudIconButton Size="Size.Small" 
                                                  Icon="@Icons.Material.Filled.Edit" 
                                                  OnClick="@(() => EditUnitAsync(unit))" />
                                    <MudIconButton Size="Size.Small" 
                                                  Icon="@Icons.Material.Filled.Delete" 
                                                  OnClick="@(() => DeleteUnitAsync(unit))" />
                                </div>
                            </div>
                        </Content>
                    </MudTreeViewItem>
                }
            </MudTreeView>
        </MudItem>
    </MudGrid>
</MudContainer>

@code {
    private List<OrganizationalUnitResponse> units = new();
    private OrganizationalUnitResponse? selectedUnit;

    protected override async Task OnInitializedAsync()
    {
        await LoadUnitsAsync();
    }

    private async Task LoadUnitsAsync()
    {
        try
        {
            var response = await ApiClient.SearchOrganizationalUnitsAsync(
                new SearchOrganizationalUnitsRequest 
                { 
                    PageNumber = 1, 
                    PageSize = 100 
                }
            );
            units = response.Data.ToList();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading units: {ex.Message}", Severity.Error);
        }
    }

    private async Task CreateUnitAsync()
    {
        // Open dialog for creation
    }

    private async Task EditUnitAsync(OrganizationalUnitResponse unit)
    {
        // Open dialog for editing
    }

    private async Task DeleteUnitAsync(OrganizationalUnitResponse unit)
    {
        // Confirm and delete
    }
}
```

**Acceptance Criteria:**
- [ ] Hierarchical display working
- [ ] CRUD operations functional
- [ ] Error handling present
- [ ] Loading states shown

#### Designations.razor
```razor
@page "/humanresources/organization/designations"

<!-- Similar structure to above -->
<!-- Show salary ranges for each area -->
<!-- Allow area-specific configuration -->
```

**Definition of Done:**
- [ ] Both pages working end-to-end
- [ ] API calls successful
- [ ] Error handling in place
- [ ] Data persists correctly

---

### Phase 2.2: Employee Master Page (Multi-step Wizard)
**Duration:** 2 days | **Owner:** Frontend Developer

#### Employees.razor (Main Page)
```razor
@page "/humanresources/employees"
@page "/humanresources/employees/create"
@page "/humanresources/employees/{EmployeeId}/edit"

@if (IsCreating)
{
    <CreateEmployeeWizard OnComplete="EmployeeCreatedAsync" />
}
else
{
    <EmployeeList OnEdit="EditEmployeeAsync" />
}
```

#### CreateEmployeeWizard.razor (Sub-component)
```razor
<!-- Step 1: Basic Information -->
<MudStepper @bind-ActiveIndex="activeIndex">
    <MudStep Title="Basic Information" Description="Personal details">
        <MudGrid>
            <MudItem xs="12" sm="6">
                <MudTextField @bind-Value="Employee.FirstName" 
                             Label="First Name" 
                             Required="true"
                             Validation="@(new NotEmptyValidator())" />
            </MudItem>
            <MudItem xs="12" sm="6">
                <MudTextField @bind-Value="Employee.LastName" 
                             Label="Last Name" 
                             Required="true" />
            </MudItem>
            <MudItem xs="12" sm="6">
                <MudTextField @bind-Value="Employee.Email" 
                             Label="Email" 
                             InputType="InputType.Email" 
                             Required="true" />
            </MudItem>
            <MudItem xs="12" sm="6">
                <MudTextField @bind-Value="Employee.PhoneNumber" 
                             Label="Phone" 
                             Required="true" />
            </MudItem>
            <MudItem xs="12" sm="6">
                <MudDatePicker @bind-Date="Employee.DateOfBirth" 
                              Label="Date of Birth" 
                              Required="true" />
            </MudItem>
            <MudItem xs="12" sm="6">
                <MudSelect @bind-Value="Employee.EmploymentType" 
                          Label="Employment Type" 
                          Required="true">
                    <MudSelectItem Value="@("Permanent")">Permanent</MudSelectItem>
                    <MudSelectItem Value="@("Contract")">Contract</MudSelectItem>
                    <MudSelectItem Value="@("Temporary")">Temporary</MudSelectItem>
                </MudSelect>
            </MudItem>
        </MudGrid>
    </MudStep>

    <!-- Step 2: Government IDs -->
    <MudStep Title="Government IDs" Description="Philippines ID numbers">
        <EmployeeGovernmentIds @bind-Employee="Employee" />
    </MudStep>

    <!-- Step 3: Employment Details -->
    <MudStep Title="Employment Details" Description="Job assignment">
        <MudGrid>
            <MudItem xs="12">
                <OrganizationalUnitSelector @bind-SelectedUnit="Employee.OrganizationalUnitId" />
            </MudItem>
            <MudItem xs="12">
                <DesignationSelector @bind-SelectedDesignation="Employee.DesignationId" />
            </MudItem>
            <MudItem xs="12">
                <MudTextField @bind-Value="Employee.BasicSalary" 
                             Label="Basic Salary" 
                             InputType="InputType.Number" />
            </MudItem>
        </MudGrid>
    </MudStep>

    <!-- Step 4: Contact Details -->
    <MudStep Title="Contact Details" Description="Address and links">
        <EmployeeContactForm @bind-Contact="Employee.Contact" />
    </MudStep>

    <!-- Step 5: Bank Information -->
    <MudStep Title="Bank Information" Description="Payroll routing">
        <BankAccountForm @bind-BankAccount="Employee.BankAccount" />
    </MudStep>

    <!-- Step 6: Confirmation -->
    <MudStep Title="Confirm & Create" Description="Review and save">
        <EmployeeConfirmation Employee="Employee" OnConfirm="SubmitAsync" />
    </MudStep>
</MudStepper>
```

**Definition of Done:**
- [ ] All 6 wizard steps implemented
- [ ] Validation at each step
- [ ] Progress tracking visible
- [ ] Can go back/forward through steps
- [ ] Final submission creates employee

---

### Phase 2.3: Employee Detail View
**Duration:** 1 day | **Owner:** Frontend Developer

#### EmployeeDetail.razor
```razor
@page "/humanresources/employees/{EmployeeId}"

<MudTabs>
    <MudTabPanel Text="Profile" Icon="@Icons.Material.Filled.Person">
        <EmployeeProfileTab Employee="Employee" OnSave="UpdateAsync" />
    </MudTabPanel>

    <MudTabPanel Text="Contacts" Icon="@Icons.Material.Filled.Phone">
        <EmployeeContactsTab EmployeeId="EmployeeId" />
    </MudTabPanel>

    <MudTabPanel Text="Dependents" Icon="@Icons.Material.Filled.Family">
        <EmployeeDependentsTab EmployeeId="EmployeeId" />
    </MudTabPanel>

    <MudTabPanel Text="Education" Icon="@Icons.Material.Filled.School">
        <EmployeeEducationTab EmployeeId="EmployeeId" />
    </MudTabPanel>

    <MudTabPanel Text="Documents" Icon="@Icons.Material.Filled.Description">
        <EmployeeDocumentsTab EmployeeId="EmployeeId" />
    </MudTabPanel>

    <MudTabPanel Text="Bank" Icon="@Icons.Material.Filled.AccountBalance">
        <EmployeeBankTab EmployeeId="EmployeeId" />
    </MudTabPanel>

    <MudTabPanel Text="History" Icon="@Icons.Material.Filled.History">
        <EmployeeAuditTab EmployeeId="EmployeeId" />
    </MudTabPanel>
</MudTabs>
```

**Definition of Done:**
- [ ] All tabs functional
- [ ] Data loads correctly
- [ ] Edits save properly
- [ ] Relationships display correctly

---

## 📅 Phase 3: Time & Attendance (Week 2-3)

### Phase 3.1: Attendance Page
```razor
@page "/humanresources/attendance"

<MudContainer MaxWidth="MaxWidth.Large">
    <MudGrid>
        <MudItem xs="12">
            <MudButton OnClick="TodayAsync">Today</MudButton>
            <MudDatePicker @bind-Date="selectedDate" Label="Select Date" />
        </MudItem>
        
        <MudItem xs="12">
            <MudDataGrid Items="attendanceRecords" SortMode="SortMode.Multiple">
                <Columns>
                    <PropertyColumn Property="x => x.Employee.FirstName" />
                    <PropertyColumn Property="x => x.Employee.LastName" />
                    <PropertyColumn Property="x => x.InTime" Format="HH:mm" />
                    <PropertyColumn Property="x => x.OutTime" Format="HH:mm" />
                    <PropertyColumn Property="x => x.Status" />
                    <TemplateColumn>
                        <CellTemplate>
                            <MudButton Size="Size.Small" OnClick="@(() => EditAsync(context.Item))">
                                Edit
                            </MudButton>
                        </CellTemplate>
                    </TemplateColumn>
                </Columns>
            </MudDataGrid>
        </MudItem>
    </MudGrid>
</MudContainer>
```

### Phase 3.2: Timesheet Page
### Phase 3.3: Shift Management
### Phase 3.4: Holiday Calendar

---

## 📅 Phase 4: Leave Management (Week 3-4)

### Phase 4.1: Leave Request Workflow
```razor
<!-- Employee view: Submit leave request -->
<!-- Manager view: Approve/Reject requests -->
<!-- Leave balance auto-update on approval -->
```

### Phase 4.2: Leave Balance Display
### Phase 4.3: Leave Reports

---

## 📅 Phase 5: Payroll (Week 5-7)

### Phase 5.1: Payroll Creation Wizard
```razor
<!-- Step 1: Period Selection -->
<!-- Step 2: Employee Selection -->
<!-- Step 3: Component Configuration -->
<!-- Step 4: Deduction Review -->
<!-- Step 5: Tax Review -->
<!-- Step 6: Processing -->
```

### Phase 5.2: Payroll Reports
### Phase 5.3: Bank File Export

---

## 🧪 Testing Checklist for Each Component

```
Before marking component as DONE:

Functional Testing:
- [ ] CRUD operations work
- [ ] Data loads from API
- [ ] Validation prevents invalid data
- [ ] Error messages display correctly
- [ ] Success messages confirm actions

UI/UX Testing:
- [ ] Responsive design verified (mobile/tablet/desktop)
- [ ] Loading states visible
- [ ] Disabled states correct
- [ ] Proper spacing and alignment
- [ ] Icons/colors consistent

Integration Testing:
- [ ] API connectivity verified
- [ ] Data persists to database
- [ ] Related records update correctly
- [ ] Workflows complete end-to-end

Performance Testing:
- [ ] Page loads in < 2 seconds
- [ ] Search returns results in < 1 second
- [ ] No memory leaks
- [ ] Handles 100+ records smoothly
```

---

## 📞 Known Issues & Workarounds

### Issue 1: NSwag Client Generation
**Symptom:** Generated client classes missing HR methods  
**Root Cause:** NSwag configuration may not include HR endpoints  
**Workaround:**
```json
// nswag.json - Ensure OpenAPI URL includes all controllers
{
  "swaggerUrl": "https://localhost:5001/swagger/v1/swagger.json"
}
```

### Issue 2: Multi-Tenant Context in UI
**Symptom:** API returns 403 Forbidden  
**Root Cause:** Tenant ID not passed in headers  
**Workaround:** Ensure HttpClient includes tenant header
```csharp
httpClient.DefaultRequestHeaders.Add("X-Tenant", TenantId);
```

### Issue 3: Date Format Mismatch
**Symptom:** Date validation errors  
**Root Cause:** UI sending ISO 8601, API expects different format  
**Workaround:** Ensure both use ISO 8601
```csharp
DateTime.UtcNow.ToString("O") // ✅ Correct format
```

---

## 📊 Success Metrics

### For Phase 1 Completion:
- [ ] API client fully generated with 150+ methods
- [ ] Zero compilation errors in Blazor project
- [ ] 5+ shared components created and tested
- [ ] Base templates ready for implementation
- [ ] All HR endpoints accessible via Swagger

### For Phase 2 Completion:
- [ ] Organization setup pages functional (CRUD working)
- [ ] Employee wizard creates employees end-to-end
- [ ] Employee detail view displays all information
- [ ] All 6 wizard steps completed
- [ ] Data persists correctly to database

### For Full UI Completion (All Phases):
- [ ] 29 pages implemented
- [ ] 8+ shared components created
- [ ] 5+ workflows functional
- [ ] 100% CRUD coverage
- [ ] Zero compilation warnings
- [ ] 95%+ test coverage
- [ ] Performance benchmarks met (<2s page load)

---

## 👥 Resource Requirements

**Recommended Team:**
- 1 Backend Developer (for API client generation, API fixes)
- 1-2 Frontend Developers (for UI implementation)
- 1 QA Engineer (for testing and validation)

**Estimated Timeline:** 4-5 weeks with this team

**Parallel Work:**
- Backend → Generate client + enable analytics
- Frontend → Build components + create pages
- QA → Test components as they're completed

---

## 🚀 Go-Live Checklist

Before deploying HR module to production:

**API:**
- [ ] All endpoints tested manually
- [ ] Payroll calculations verified (Philippines compliance)
- [ ] Leave balance logic tested
- [ ] Multi-tenant isolation verified
- [ ] Performance tested with large datasets
- [ ] Backup/recovery procedures documented

**UI:**
- [ ] All workflows tested end-to-end
- [ ] Error messages helpful and clear
- [ ] Loading states present on all async operations
- [ ] Mobile responsive design verified
- [ ] Accessibility (WCAG 2.1 AA) verified
- [ ] User documentation complete

**Operations:**
- [ ] Database migrations tested on staging
- [ ] Rollback procedure documented
- [ ] Monitoring/alerts configured
- [ ] Support documentation prepared
- [ ] User training completed

---

**Document Generated:** November 19, 2025  
**Status:** ✅ READY FOR IMPLEMENTATION  
**Next Step:** Start Phase 1 - API Client Generation

