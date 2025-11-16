# HR Endpoints Permission Update - Quick Reference Guide

## Pattern to Follow

### Before (Old Pattern):
```csharp
using FSH.Starter.WebApi.HumanResources.Application.{Module}.{Operation}.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.{Module};

public static class {Operation}{Module}Endpoint
{
    internal static RouteHandlerBuilder Map{Operation}{Module}Endpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .Map{Method}("/{route}", async (...) => { ... })
            .RequirePermission("Permissions.{OldResource}.{Action}")  // ❌ String literal
            .MapToApiVersion(1);
    }
}
```

### After (New Pattern):
```csharp
using FSH.Starter.WebApi.HumanResources.Application.{Module}.{Operation}.v1;
using Shared.Authorization;  // ✅ ADD THIS

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.{Module};

public static class {Operation}{Module}Endpoint
{
    internal static RouteHandlerBuilder Map{Operation}{Module}Endpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .Map{Method}("/{route}", async (...) => { ... })
            .RequirePermission(FshPermission.NameFor(FshActions.{Action}, FshResources.{Resource}))  // ✅ Use helper
            .MapToApiVersion(1);
    }
}
```

---

## Resource Mapping Quick Reference

| Old Permission String | New FshResources | Notes |
|----------------------|------------------|-------|
| `Permissions.Employees.*` | `FshResources.Employees` | Core employee data |
| `Permissions.EmployeeContacts.*` | `FshResources.Employees` | Employee sub-entity |
| `Permissions.EmployeeDependents.*` | `FshResources.Employees` | Employee sub-entity |
| `Permissions.EmployeeDocuments.*` | `FshResources.Employees` | Employee sub-entity |
| `Permissions.EmployeeEducations.*` | `FshResources.Employees` | Employee sub-entity |
| `Permissions.PerformanceReviews.*` | `FshResources.Employees` | Employee-related |
| `Permissions.OrganizationalUnits.*` | `FshResources.Organization` | Company structure |
| `Permissions.Departments.*` | `FshResources.Organization` | Company structure |
| `Permissions.Designations.*` | `FshResources.Organization` | Company structure |
| `Permissions.Shifts.*` | `FshResources.Organization` | Company structure |
| `Permissions.Holidays.*` | `FshResources.Organization` | Company structure |
| `Permissions.Attendance.*` | `FshResources.Attendance` | Time tracking |
| `Permissions.ShiftAssignments.*` | `FshResources.Attendance` | Time tracking |
| `Permissions.Timesheets.*` | `FshResources.Timesheets` | Timesheet management |
| `Permissions.TimesheetLines.*` | `FshResources.Timesheets` | Timesheet sub-entity |
| `Permissions.LeaveTypes.*` | `FshResources.Leaves` | Leave management |
| `Permissions.LeaveRequests.*` | `FshResources.Leaves` | Leave management |
| `Permissions.LeaveBalances.*` | `FshResources.Leaves` | Leave management |
| `Permissions.Payrolls.*` | `FshResources.Payroll` | Payroll processing |
| `Permissions.PayComponents.*` | `FshResources.Payroll` | Payroll component |
| `Permissions.PayComponentRates.*` | `FshResources.Payroll` | Payroll component |
| `Permissions.EmployeePayComponents.*` | `FshResources.Payroll` | Payroll component |
| `Permissions.PayrollLines.*` | `FshResources.Payroll` | Payroll sub-entity |
| `Permissions.PayrollDeductions.*` | `FshResources.Payroll` | Payroll component |
| `Permissions.TaxBrackets.*` | `FshResources.Payroll` | Payroll component |
| `Permissions.Taxes.*` | `FshResources.Payroll` | Payroll component |
| `Permissions.Deductions.*` | `FshResources.Payroll` | Payroll component |
| `Permissions.BankAccounts.*` | `FshResources.Payroll` | Payroll component |
| `Permissions.Benefits.*` | `FshResources.Benefits` | Benefits management |
| `Permissions.BenefitEnrollments.*` | `FshResources.Benefits` | Benefits management |
| `Permissions.BenefitAllocations.*` | `FshResources.Benefits` | Benefits management |
| `Permissions.DocumentTemplates.*` | `FshResources.Employees` | Document management |
| `Permissions.GeneratedDocuments.*` | `FshResources.Employees` | Document management |

---

## Action Mapping Quick Reference

| Old Permission Action | FshActions Constant | Usage |
|----------------------|---------------------|-------|
| `*.View` | `FshActions.View` | Get by ID, Search, List |
| `*.Search` | `FshActions.Search` | Search operations |
| `*.Create` | `FshActions.Create` | Create new record |
| `*.Update` | `FshActions.Update` | Update record |
| `*.Edit` | `FshActions.Edit` | Edit record (synonym) |
| `*.Delete` | `FshActions.Delete` | Delete record |
| `*.Manage` | `FshActions.Manage` | Admin operations |
| `*.Approve` | `FshActions.Approve` | Approve requests |
| `*.Reject` | `FshActions.Reject` | Reject requests |
| `*.Submit` | `FshActions.Submit` | Submit for approval |
| `*.Process` | `FshActions.Process` | Process/compute |
| `*.Complete` | `FshActions.Complete` | Mark complete |
| `*.Post` | `FshActions.Post` | Post to ledger |
| `*.MarkAsPaid` | `FshActions.MarkAsPaid` | Mark payment |
| `*.Accrue` | `FshActions.Accrue` | Accrue balances |
| `*.Acknowledge` | `FshActions.Acknowledge` | Acknowledge receipt |
| `*.Regularize` | `FshActions.Regularize` | Employee regularize |
| `*.Terminate` | `FshActions.Terminate` | Terminate record |
| `*.Import` | `FshActions.Import` | Import data |
| `*.Export` | `FshActions.Export` | Export data |

---

## Real Examples

### Example 1: Timesheets Create
```csharp
// Before:
.RequirePermission("Permissions.Timesheets.Create")

// After:
.RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Timesheets))
```

### Example 2: Leave Request Approve
```csharp
// Before:
.RequirePermission("Permissions.LeaveRequests.Approve")

// After:
.RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Leaves))
```

### Example 3: Payroll Process
```csharp
// Before:
.RequirePermission("Permissions.Payrolls.Process")

// After:
.RequirePermission(FshPermission.NameFor(FshActions.Process, FshResources.Payroll))
```

### Example 4: Employee Contacts View
```csharp
// Before:
.RequirePermission("Permissions.Employees.View")

// After:
.RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
```

### Example 5: Organizational Unit Manage
```csharp
// Before:
.RequirePermission("Permissions.OrganizationalUnits.Manage")

// After:
.RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Organization))
```

---

## Find & Replace Strategy

### Step 1: Add using directive to all files
```bash
# Find files missing the using
rg -l "RequirePermission" src/api/modules/HumanResources/HumanResources.Infrastructure/Endpoints | \
  xargs -I {} bash -c 'grep -q "using Shared.Authorization" {} || echo {}'
```

### Step 2: Update by module folder
Process each folder systematically:
1. Timesheets/ (high priority)
2. Attendance/ (high priority)
3. Payrolls/ (high priority)
4. LeaveRequests/ (high priority)
5. OrganizationalUnits/ (medium)
6. ... etc

### Step 3: Validate after each module
```bash
dotnet build src/api/modules/HumanResources/HumanResources.Infrastructure
```

---

## Checklist for Each Endpoint File

- [ ] Add `using Shared.Authorization;` at top
- [ ] Replace `RequirePermission("Permissions.{Old}.{Action}")` with `RequirePermission(FshPermission.NameFor(FshActions.{Action}, FshResources.{Resource}))`
- [ ] Verify correct resource mapping (see table above)
- [ ] Verify correct action mapping (see table above)
- [ ] Build and check for errors
- [ ] Commit changes

---

## Bulk Update Script Template

```bash
#!/bin/bash

# Update Timesheets endpoints
for file in src/api/modules/HumanResources/HumanResources.Infrastructure/Endpoints/Timesheets/**/*.cs; do
  # Add using if not present
  if ! grep -q "using Shared.Authorization" "$file"; then
    sed -i '' '1a\
using Shared.Authorization;\
' "$file"
  fi
  
  # Replace permission strings
  sed -i '' 's/RequirePermission("Permissions\.Timesheets\.Create")/RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Timesheets))/g' "$file"
  sed -i '' 's/RequirePermission("Permissions\.Timesheets\.View")/RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Timesheets))/g' "$file"
  # ... etc
done

dotnet build src/api/modules/HumanResources/HumanResources.Infrastructure
```

---

## Common Pitfalls

❌ **Wrong:** Using old resource name
```csharp
FshResources.EmployeeContacts  // ❌ Doesn't exist
```

✅ **Correct:** Use parent resource
```csharp
FshResources.Employees  // ✅ Employee-related entities use Employees resource
```

❌ **Wrong:** Inconsistent action names
```csharp
"Permissions.Employees.Edit" → FshActions.Update  // ❌ Mismatch
```

✅ **Correct:** Match the action
```csharp
"Permissions.Employees.Edit" → FshActions.Edit  // ✅ Edit action exists now
```

❌ **Wrong:** Missing using directive
```csharp
// No using Shared.Authorization;
.RequirePermission(FshPermission.NameFor(...))  // ❌ Won't compile
```

✅ **Correct:** Add using
```csharp
using Shared.Authorization;  // ✅ Required for FshPermission/FshActions/FshResources
.RequirePermission(FshPermission.NameFor(...))
```

---

## Testing Updated Endpoints

### 1. Build Test
```bash
dotnet build src/api/modules/HumanResources/HumanResources.Infrastructure
```

### 2. Runtime Test
1. Start application
2. Log in with HR permissions
3. Test endpoint via Swagger or API client
4. Verify permission is checked correctly

### 3. Permission Test
1. Create test role without HR permissions
2. Assign to test user
3. Verify endpoint returns 403 Forbidden

---

**Quick Reference Complete!**

Use this guide to update the remaining ~150 HR endpoints following the established pattern.

