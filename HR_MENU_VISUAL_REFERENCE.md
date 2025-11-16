# HR Menu Structure - Visual Reference

**Date:** November 16, 2025  
**Status:** ✅ COMPLETE

---

## 📊 Complete Menu Hierarchy

```
┌─────────────────────────────────────────────────────────────────────┐
│                         HUMAN RESOURCES MENU                        │
└─────────────────────────────────────────────────────────────────────┘

📁 Human Resources (Parent Section)
│
├─ 📋 ORGANIZATION & SETUP
│  ├─ 🌳 Organizational Units (/hr/organizational-units) [ComingSoon]
│  ├─ 🏢 Departments (/hr/departments) [ComingSoon]
│  ├─ 💼 Designations (/hr/designations) [ComingSoon]
│  ├─ ⏰ Shifts (/hr/shifts) [ComingSoon]
│  └─ 📅 Holidays (/hr/holidays) [ComingSoon]
│
├─ 📋 EMPLOYEE MANAGEMENT
│  ├─ 👥 Employees (/hr/employees) [ComingSoon]
│  ├─ 📞 Employee Contacts (/hr/employee-contacts) [ComingSoon]
│  ├─ 👨‍👩‍👧‍👦 Employee Dependents (/hr/employee-dependents) [ComingSoon]
│  ├─ 📄 Employee Documents (/hr/employee-documents) [ComingSoon]
│  ├─ 🎓 Employee Education (/hr/employee-educations) [ComingSoon]
│  └─ 📊 Performance Reviews (/hr/performance-reviews) [ComingSoon]
│
├─ 📋 TIME & ATTENDANCE
│  ├─ 👆 Attendance (/hr/attendances) [ComingSoon]
│  ├─ 🕐 Timesheets (/hr/timesheets) [ComingSoon]
│  └─ 📋 Shift Assignments (/hr/shift-assignments) [ComingSoon]
│
├─ 📋 LEAVE MANAGEMENT
│  ├─ 📂 Leave Types (/hr/leave-types) [ComingSoon]
│  ├─ 📅 Leave Requests (/hr/leave-requests) [ComingSoon]
│  └─ 💼 Leave Balances (/hr/leave-balances) [ComingSoon]
│
├─ 📋 PAYROLL
│  ├─ 💰 Payroll Run (/hr/payrolls) [ComingSoon]
│  ├─ 💵 Pay Components (/hr/pay-components) [ComingSoon]
│  ├─ 📈 Pay Component Rates (/hr/pay-component-rates) [ComingSoon]
│  ├─ 👤 Employee Pay Components (/hr/employee-pay-components) [ComingSoon]
│  ├─ ➖ Deductions (/hr/deductions) [ComingSoon]
│  ├─ 💸 Payroll Deductions (/hr/payroll-deductions) [ComingSoon]
│  ├─ 📊 Tax Brackets (/hr/tax-brackets) [ComingSoon]
│  ├─ 🧾 Taxes (/hr/taxes) [ComingSoon]
│  └─ 🏦 Bank Accounts (/hr/bank-accounts) [ComingSoon]
│
├─ 📋 BENEFITS & ENROLLMENT
│  ├─ 🎁 Benefits (/hr/benefits) [ComingSoon]
│  ├─ ✅ Benefit Enrollments (/hr/benefit-enrollments) [ComingSoon]
│  └─ 🏷️ Benefit Allocations (/hr/benefit-allocations) [ComingSoon]
│
└─ 📋 DOCUMENTS & REPORTS
   ├─ 📝 Document Templates (/hr/document-templates) [ComingSoon]
   └─ 📑 Generated Documents (/hr/generated-documents) [ComingSoon]

```

---

## 📊 Statistics

| Category | Count |
|----------|-------|
| **Total Menu Items** | 31 |
| **Functional Groups** | 7 |
| **Organization & Setup** | 5 items |
| **Employee Management** | 6 items |
| **Time & Attendance** | 3 items |
| **Leave Management** | 3 items |
| **Payroll** | 9 items |
| **Benefits & Enrollment** | 3 items |
| **Documents & Reports** | 2 items |

---

## 🎨 Group Color Coding (for future UI enhancement)

```
🟦 ORGANIZATION & SETUP    - Blue (Company Structure)
🟩 EMPLOYEE MANAGEMENT     - Green (Core HR)
🟨 TIME & ATTENDANCE       - Yellow (Time Tracking)
🟧 LEAVE MANAGEMENT        - Orange (Time Off)
🟥 PAYROLL                 - Red (Compensation)
🟪 BENEFITS & ENROLLMENT   - Purple (Employee Benefits)
🟫 DOCUMENTS & REPORTS     - Brown (Documentation)
```

---

## 🔗 Route Mapping

All routes follow the pattern: `/hr/{entity-plural}`

### Organization & Setup Routes
- `/hr/organizational-units` → OrganizationalUnits.razor
- `/hr/departments` → Departments.razor
- `/hr/designations` → Designations.razor
- `/hr/shifts` → Shifts.razor
- `/hr/holidays` → Holidays.razor

### Employee Management Routes
- `/hr/employees` → Employees.razor
- `/hr/employee-contacts` → EmployeeContacts.razor
- `/hr/employee-dependents` → EmployeeDependents.razor
- `/hr/employee-documents` → EmployeeDocuments.razor
- `/hr/employee-educations` → EmployeeEducations.razor
- `/hr/performance-reviews` → PerformanceReviews.razor

### Time & Attendance Routes
- `/hr/attendances` → Attendances.razor
- `/hr/timesheets` → Timesheets.razor
- `/hr/shift-assignments` → ShiftAssignments.razor

### Leave Management Routes
- `/hr/leave-types` → LeaveTypes.razor
- `/hr/leave-requests` → LeaveRequests.razor
- `/hr/leave-balances` → LeaveBalances.razor

### Payroll Routes
- `/hr/payrolls` → Payrolls.razor
- `/hr/pay-components` → PayComponents.razor
- `/hr/pay-component-rates` → PayComponentRates.razor
- `/hr/employee-pay-components` → EmployeePayComponents.razor
- `/hr/deductions` → Deductions.razor
- `/hr/payroll-deductions` → PayrollDeductions.razor
- `/hr/tax-brackets` → TaxBrackets.razor
- `/hr/taxes` → Taxes.razor
- `/hr/bank-accounts` → BankAccounts.razor

### Benefits & Enrollment Routes
- `/hr/benefits` → Benefits.razor
- `/hr/benefit-enrollments` → BenefitEnrollments.razor
- `/hr/benefit-allocations` → BenefitAllocations.razor

### Documents & Reports Routes
- `/hr/document-templates` → DocumentTemplates.razor
- `/hr/generated-documents` → GeneratedDocuments.razor

---

## 🔐 Permission Matrix

| Action | Permission Name | Description |
|--------|----------------|-------------|
| View | `Permissions.HumanResources.View` | View HR pages and data |
| Search | `Permissions.HumanResources.Search` | Search HR records |
| Create | `Permissions.HumanResources.Create` | Create new HR records |
| Update | `Permissions.HumanResources.Update` | Edit existing HR records |
| Delete | `Permissions.HumanResources.Delete` | Delete HR records |
| Import | `Permissions.HumanResources.Import` | Import HR data from files |
| Export | `Permissions.HumanResources.Export` | Export HR data to files |

---

## 📱 Responsive Design Targets

### Desktop (>1280px)
- Full menu visible in sidebar
- All 7 groups expanded by default
- Icons + text labels

### Tablet (768px - 1280px)
- Collapsible sidebar
- Groups collapsed by default
- Icons + text labels

### Mobile (<768px)
- Drawer menu
- Groups collapsed
- Icons only (with tooltips)

---

## 🎯 User Roles & Access Patterns

### HR Manager
**Access:** All HR permissions
**Common Tasks:**
1. Review employee records
2. Process payroll
3. Approve leave requests
4. Manage benefits enrollment

### HR Clerk
**Access:** View + Search + Create (limited Delete)
**Common Tasks:**
1. Enter new employees
2. Update employee info
3. Track attendance
4. Process timesheets

### Department Manager
**Access:** View + Search (own department only)
**Common Tasks:**
1. View team members
2. Approve leave requests
3. Review attendance
4. Access reports

### Employee (Self-Service)
**Access:** View only (own data)
**Common Tasks:**
1. View payslips
2. Request leave
3. View leave balance
4. Update personal info

---

## 🚀 Implementation Roadmap

### Phase 1: Core HR (High Priority)
- ✅ Menu structure created
- 🔲 Employees page
- 🔲 Organizational Units page
- 🔲 Designations page
- 🔲 Employee Contacts page

### Phase 2: Time & Attendance
- 🔲 Attendance page
- 🔲 Timesheets page
- 🔲 Shifts page
- 🔲 Shift Assignments page

### Phase 3: Leave Management
- 🔲 Leave Types page
- 🔲 Leave Requests page
- 🔲 Leave Balances page

### Phase 4: Payroll
- 🔲 Payroll Run page
- 🔲 Pay Components page
- 🔲 Taxes page
- 🔲 Deductions page

### Phase 5: Benefits & Documents
- 🔲 Benefits page
- 🔲 Benefit Enrollments page
- 🔲 Document Templates page

---

## 🔧 Development Guidelines

### Creating New HR Pages

1. **Create folder structure:**
   ```
   /Pages/HR/{EntityName}/
   ├── {EntityName}.razor
   ├── {EntityName}.razor.cs
   └── Components/ (if needed)
   ```

2. **Follow naming conventions:**
   - Route: `/hr/{entity-plural}`
   - Component: `{EntityName}.razor`
   - ViewModel: `{EntityName}ViewModel.cs`

3. **Use EntityTable pattern:**
   ```razor
   @page "/hr/employees"
   
   <PageHeader Title="Employees" 
               Header="Employees" 
               SubHeader="Manage employee records." />
   
   <EntityTable TEntity="EmployeeResponse" 
                TId="DefaultIdType" 
                TRequest="EmployeeViewModel" />
   ```

4. **Update MenuService.cs:**
   ```csharp
   PageStatus = PageStatus.Completed // Mark as done
   ```

---

## 📝 Notes

- All menu items are currently marked as `ComingSoon`
- Update `PageStatus` to `InProgress` or `Completed` as pages are built
- Icons use Material Design icon set
- All routes require `Permissions.HumanResources.View` permission
- Menu auto-hides based on user permissions

---

**Visual Reference Complete! ✅**

