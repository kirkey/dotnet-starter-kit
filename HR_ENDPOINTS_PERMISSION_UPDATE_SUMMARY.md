# HR Endpoints Permission Update - Summary

**Date:** November 16, 2025  
**Status:** In Progress

## Objective
Update all HR endpoint RequirePermission calls from string literals to use FshPermission.NameFor with new granular resources:
- `FshResources.Organization` - Organizational units, departments, designations, shifts, holidays
- `FshResources.Employees` - Employees and employee-related data
- `FshResources.Attendance` - Attendance tracking
- `FshResources.Timesheets` - Timesheet management
- `FshResources.Leaves` - Leave types, requests, balances
- `FshResources.Payroll` - Payroll, pay components, taxes, deductions
- `FshResources.Benefits` - Benefits and enrollments

## Mapping Rules

### Organization Resource
- OrganizationalUnits → FshResources.Organization
- Departments → FshResources.Organization
- Designations → FshResources.Organization
- Shifts → FshResources.Organization
- Holidays → FshResources.Organization

### Employees Resource
- Employees → FshResources.Employees
- EmployeeContacts → FshResources.Employees
- EmployeeDependents → FshResources.Employees
- EmployeeDocuments → FshResources.Employees
- EmployeeEducations → FshResources.Employees
- PerformanceReviews → FshResources.Employees
- DocumentTemplates → FshResources.Employees
- GeneratedDocuments → FshResources.Employees

### Attendance Resource
- Attendance → FshResources.Attendance
- ShiftAssignments → FshResources.Attendance

### Timesheets Resource
- Timesheets → FshResources.Timesheets
- TimesheetLines → FshResources.Timesheets

### Leaves Resource
- LeaveTypes → FshResources.Leaves
- LeaveRequests → FshResources.Leaves
- LeaveBalances → FshResources.Leaves

### Payroll Resource
- Payrolls → FshResources.Payroll
- PayComponents → FshResources.Payroll
- PayComponentRates → FshResources.Payroll
- EmployeePayComponents → FshResources.Payroll
- PayrollLines → FshResources.Payroll
- PayrollDeductions → FshResources.Payroll
- TaxBrackets → FshResources.Payroll
- Taxes → FshResources.Payroll (NEW)
- Deductions → FshResources.Payroll
- BankAccounts → FshResources.Payroll

### Benefits Resource
- Benefits → FshResources.Benefits
- BenefitEnrollments → FshResources.Benefits
- BenefitAllocations → FshResources.Benefits

## Actions Added to FshActions
- Edit
- Manage
- Approve
- Reject
- Submit
- Process
- Complete
- Post
- MarkAsPaid
- Accrue
- Acknowledge

## Completed Endpoints

### ✅ Employees (7 endpoints)
- CreateEmployeeEndpoint
- GetEmployeeEndpoint
- SearchEmployeesEndpoint
- UpdateEmployeeEndpoint
- DeleteEmployeeEndpoint
- RegularizeEmployeeEndpoint
- TerminateEmployeeEndpoint

### ✅ PayComponents (4 endpoints)
- CreatePayComponentEndpoint
- GetPayComponentEndpoint
- UpdatePayComponentEndpoint
- DeletePayComponentEndpoint

## Remaining Endpoints to Update

### Organization Endpoints
- [ ] OrganizationalUnits (5 endpoints)
- [ ] Departments (if exists)
- [ ] Designations (5 endpoints)
- [ ] Shifts (5 endpoints)
- [ ] Holidays (5 endpoints)

### Employee-Related Endpoints
- [ ] EmployeeContacts (5 endpoints)
- [ ] EmployeeDependents (5 endpoints)
- [ ] EmployeeDocuments (5 endpoints)
- [ ] EmployeeEducations (5 endpoints)
- [ ] PerformanceReviews (6 endpoints)
- [ ] DesignationAssignments (3 endpoints)

### Time & Attendance Endpoints
- [ ] Attendance (5 endpoints)
- [ ] Timesheets (5 endpoints)
- [ ] TimesheetLines (5 endpoints)
- [ ] ShiftAssignments (5 endpoints)

### Leave Endpoints
- [ ] LeaveTypes (5 endpoints)
- [ ] LeaveRequests (7 endpoints)
- [ ] LeaveBalances (5 endpoints)

### Payroll Endpoints
- [ ] Payrolls (8 endpoints)
- [ ] PayComponentRates (5 endpoints)
- [ ] EmployeePayComponents (5 endpoints)
- [ ] PayrollLines (5 endpoints)
- [ ] PayrollDeductions (5 endpoints)
- [ ] TaxBrackets (5 endpoints)
- [ ] BankAccounts (5 endpoints)

### Benefits Endpoints
- [ ] Benefits (if exists)
- [ ] BenefitEnrollments (5 endpoints)
- [ ] BenefitAllocations (4 endpoints)

### Documents Endpoints
- [ ] DocumentTemplates (5 endpoints)
- [ ] GeneratedDocuments (5 endpoints)

## Total Progress
- Completed: 11 endpoints
- Remaining: ~150+ endpoints
- Estimated Completion: In Progress

## Notes
- All endpoints need `using Shared.Authorization;` added
- Replace string literals with `FshPermission.NameFor(FshActions.{Action}, FshResources.{Resource})`
- Some actions like "Edit", "Manage" are used in old permissions - these map to FshActions.Edit / FshActions.Manage

