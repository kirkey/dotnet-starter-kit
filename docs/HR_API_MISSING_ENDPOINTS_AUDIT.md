# HR API - Missing Endpoints Audit Report

## Executive Summary

**Review Date:** November 19, 2025

This audit identifies application handlers that have been created but are NOT wired into the API endpoint routing in `HrModule.cs`.

---

## 📊 Audit Results

### Endpoint Directories (38 total)
```
✅ = Mapped in HrModule.cs
❌ = NOT mapped (Missing!)
```

| # | Endpoint Directory | Status | Notes |
|---|---|---|---|
| 1 | Attendance | ✅ | Mapped: `MapAttendanceEndpoints()` |
| 2 | AttendanceReports | ✅ | Mapped: `MapAttendanceReportsEndpoints()` |
| 3 | BankAccounts | ✅ | Mapped: `MapBankAccountsEndpoints()` |
| 4 | BenefitAllocations | ✅ | Mapped: `MapBenefitAllocationsEndpoints()` |
| 5 | BenefitEnrollments | ✅ | Mapped: `MapBenefitEnrollmentsEndpoints()` |
| 6 | Benefits | ✅ | Mapped: `MapBenefitEndpoints()` |
| 7 | Deductions | ✅ | Mapped: `MapDeductionEndpoints()` |
| 8 | DesignationAssignments | ✅ | Mapped: `MapDesignationAssignmentsEndpoints()` |
| 9 | Designations | ✅ | Mapped: `MapDesignationsEndpoints()` |
| 10 | DocumentTemplates | ✅ | Mapped: `MapDocumentTemplatesEndpoints()` |
| 11 | EmployeeContacts | ✅ | Mapped: `MapEmployeeContactsEndpoints()` |
| 12 | EmployeeDashboards | ✅ | Mapped: `MapEmployeeDashboardsEndpoints()` |
| 13 | EmployeeDependents | ✅ | Mapped: `MapEmployeeDependentsEndpoints()` |
| 14 | EmployeeDocuments | ✅ | Mapped: `MapEmployeeDocumentsEndpoints()` |
| 15 | EmployeeEducations | ✅ | Mapped: `MapEmployeeEducationsEndpoints()` |
| 16 | EmployeePayComponents | ✅ | Mapped: `MapEmployeePayComponentsEndpoints()` |
| 17 | Employees | ✅ | Mapped: `MapEmployeesEndpoints()` |
| 18 | GeneratedDocuments | ❌ | **MISSING** - Not mapped! |
| 19 | HRAnalytics | ❌ | **COMMENTED OUT** - `// app.MapHrAnalyticsEndpoints();` |
| 20 | Holidays | ❌ | **MISSING** - Not mapped! |
| 21 | LeaveBalances | ✅ | Mapped: `MapLeaveBalancesEndpoints()` |
| 22 | LeaveReports | ✅ | Mapped: `MapLeaveReportsEndpoints()` |
| 23 | LeaveRequests | ✅ | Mapped: `MapLeaveRequestsEndpoints()` |
| 24 | LeaveTypes | ✅ | Mapped: `MapLeaveTypesEndpoints()` |
| 25 | OrganizationalUnits | ✅ | Mapped: `MapOrganizationalUnitsEndpoints()` |
| 26 | PayComponentRates | ✅ | Mapped: `MapPayComponentRatesEndpoints()` |
| 27 | PayComponents | ❌ | **MISSING** - Not mapped! |
| 28 | PayrollDeductions | ✅ | Mapped: `MapPayrollDeductionsEndpoints()` |
| 29 | PayrollLines | ✅ | Mapped: `MapPayrollLinesEndpoints()` |
| 30 | PayrollReports | ✅ | Mapped: `MapPayrollReportsEndpoints()` |
| 31 | Payrolls | ✅ | Mapped: `MapPayrollsEndpoints()` |
| 32 | PerformanceReviews | ✅ | Mapped: `MapPerformanceReviewsEndpoints()` |
| 33 | ShiftAssignments | ✅ | Mapped: `MapShiftAssignmentEndpoints()` |
| 34 | Shifts | ❌ | **MISSING** - Not mapped! |
| 35 | TaxBrackets | ✅ | Mapped: `MapTaxBracketEndpoints()` |
| 36 | Taxes | ✅ | Mapped: `MapTaxEndpoints()` |
| 37 | TimesheetLines | ✅ | Mapped: `MapTimesheetLinesEndpoints()` |
| 38 | Timesheets | ✅ | Mapped: `MapTimesheetsEndpoints()` |

---

## 🚨 Missing Endpoint Mappings

### 1. GeneratedDocuments
**Status:** ❌ MISSING

**Issue:**
- Endpoint directory exists: `/Endpoints/GeneratedDocuments/`
- NOT mapped in HrModule.cs
- Handler exists in Application layer
- Repository registered in HrModule.cs (hr:generateddocuments)

**Files:**
- Application handlers: Created
- Infrastructure endpoints: Created
- Service registration: ✓ Present
- Routing mapping: ✗ **MISSING**

**Fix Required:**
```csharp
// Add to HrModule.cs AddRoutes():
app.MapGeneratedDocumentsEndpoints();

// Add using:
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.GeneratedDocuments;
```

---

### 2. HRAnalytics
**Status:** ❌ COMMENTED OUT

**Issue:**
- Currently disabled with comment: `// app.MapHrAnalyticsEndpoints();`
- Repository registered in HrModule.cs (hr:hrcanalytics)
- Handler exists in Application layer
- Intentionally disabled (verify if temporary)

**Current Code (Line ~79):**
```csharp
// app.MapHrAnalyticsEndpoints();  // <-- COMMENTED OUT
```

**Action Required:**
- Uncomment if analytics should be available
- Or remove directory if no longer needed

---

### 3. Holidays
**Status:** ❌ MISSING

**Issue:**
- Endpoint directory exists: `/Endpoints/Holidays/`
- NOT mapped in HrModule.cs
- Handler may exist in Application layer
- Repository registered in HrModule.cs (hr:holidays)

**Files:**
- Application handlers: To verify
- Infrastructure endpoints: To verify
- Service registration: ✓ Present  
- Routing mapping: ✗ **MISSING**

**Fix Required:**
```csharp
// Add to HrModule.cs AddRoutes():
app.MapHolidaysEndpoints();

// Add using:
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Holidays;
```

---

### 4. PayComponents
**Status:** ❌ MISSING

**Issue:**
- Endpoint directory exists: `/Endpoints/PayComponents/`
- NOT mapped in HrModule.cs
- Handler exists in Application layer
- Repository registered in HrModule.cs (hr:paycomponents)

**Files:**
- Application handlers: Created
- Infrastructure endpoints: Created
- Service registration: ✓ Present
- Routing mapping: ✗ **MISSING**

**Fix Required:**
```csharp
// Add to HrModule.cs AddRoutes():
app.MapPayComponentsEndpoints();

// Add using:
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponents;
```

---

### 5. Shifts
**Status:** ❌ MISSING

**Issue:**
- Endpoint directory exists: `/Endpoints/Shifts/`
- NOT mapped in HrModule.cs
- Handler exists in Application layer
- Repository registered in HrModule.cs (hr:shifts)

**Files:**
- Application handlers: Created
- Infrastructure endpoints: Created
- Service registration: ✓ Present
- Routing mapping: ✗ **MISSING**

**Fix Required:**
```csharp
// Add to HrModule.cs AddRoutes():
app.MapShiftsEndpoints();

// Add using:
using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts;
```

---

## 📋 Implementation Checklist

### For Each Missing Endpoint, Verify:

- [ ] **GeneratedDocuments**
  - [ ] Check if `/Endpoints/GeneratedDocuments/GeneratedDocumentsEndpoints.cs` exists
  - [ ] Verify `MapGeneratedDocumentsEndpoints()` method exists
  - [ ] Add using statement to HrModule.cs
  - [ ] Add mapping call to AddRoutes()
  - [ ] Test endpoints work

- [ ] **HRAnalytics** (Decide: Uncomment or Remove)
  - [ ] If keeping: Uncomment `// app.MapHrAnalyticsEndpoints();`
  - [ ] If removing: Delete endpoints directory

- [ ] **Holidays**
  - [ ] Check if `/Endpoints/Holidays/HolidaysEndpoints.cs` exists
  - [ ] Verify `MapHolidaysEndpoints()` method exists
  - [ ] Add using statement to HrModule.cs
  - [ ] Add mapping call to AddRoutes()
  - [ ] Test endpoints work

- [ ] **PayComponents**
  - [ ] Check if `/Endpoints/PayComponents/PayComponentsEndpoints.cs` exists
  - [ ] Verify `MapPayComponentsEndpoints()` method exists
  - [ ] Add using statement to HrModule.cs
  - [ ] Add mapping call to AddRoutes()
  - [ ] Test endpoints work

- [ ] **Shifts**
  - [ ] Check if `/Endpoints/Shifts/ShiftsEndpoints.cs` exists
  - [ ] Verify `MapShiftsEndpoints()` method exists
  - [ ] Add using statement to HrModule.cs
  - [ ] Add mapping call to AddRoutes()
  - [ ] Test endpoints work

---

## 🔍 Verification Steps

### 1. Verify Endpoint Configuration Files Exist

```bash
# Should exist but check mapping status:
ls -1 /src/api/modules/HumanResources/Hr.Infrastructure/Endpoints/{GeneratedDocuments,Holidays,PayComponents,Shifts}/
```

### 2. Check for MapXxxEndpoints() Methods

Each endpoints directory should have a main configuration file like:
- `GeneratedDocumentsEndpoints.cs` with `MapGeneratedDocumentsEndpoints()`
- `HolidaysEndpoints.cs` with `MapHolidaysEndpoints()`
- `PayComponentsEndpoints.cs` with `MapPayComponentsEndpoints()`
- `ShiftsEndpoints.cs` with `MapShiftsEndpoints()`

### 3. Verify Repositories Exist

Check HrModule.cs RegisterHumanResourcesServices():
- All needed repositories ARE registered ✓
- Services can be injected ✓
- Only routing is missing ✗

### 4. Verify Application Handlers Exist

```bash
# Check for handlers:
find src/api/modules/HumanResources/Hr.Application -type d -name "GeneratedDocuments"
find src/api/modules/HumanResources/Hr.Application -type d -name "Holidays"
find src/api/modules/HumanResources/Hr.Application -type d -name "PayComponents"
find src/api/modules/HumanResources/Hr.Application -type d -name "Shifts"
```

---

## 📈 Impact Analysis

### APIs Currently Unavailable
```
❌ GeneratedDocuments (1 endpoint group)
❌ Holidays (1 endpoint group)
❌ PayComponents (1 endpoint group)
❌ Shifts (1 endpoint group)
⚠️  HRAnalytics (Intentionally disabled?)
```

### Estimated Endpoints Missing
Each endpoint group typically has:
- Create (POST)
- Read/Get (GET)
- Search (POST)
- Update (PUT)
- Delete (DELETE)

**Estimated missing:** 20-25 endpoints across 5 groups

### Business Impact
- Holidays: Cannot manage company holidays (LOW - not critical)
- PayComponents: Cannot manage pay components (MEDIUM - needed for payroll)
- Shifts: Cannot manage shift patterns (MEDIUM - needed for attendance)
- GeneratedDocuments: Cannot access generated documents (LOW - secondary feature)
- HRAnalytics: Analytics feature offline (LOW - reporting only)

---

## 🛠️ Recommended Actions

### Immediate (Priority 1 - Data Integrity)
1. **PayComponents** - Needed for payroll calculations
   - Un-comment or add mapping
   - Verify working with existing data
   - Test integration with Payroll endpoints

2. **Shifts** - Needed for attendance tracking
   - Add mapping
   - Verify Shift Assignments work correctly
   - Test with existing data

### Short-term (Priority 2 - Feature Completeness)
3. **GeneratedDocuments** - Supporting feature
   - Add mapping
   - Verify Document Templates integration works

4. **Holidays** - Management feature
   - Add mapping
   - Verify Leave Balance calculations respect holidays

### Review (Priority 3 - Strategic)
5. **HRAnalytics** - Analytics feature
   - Decide: Enable or Remove?
   - If enabling: Uncomment and test
   - If removing: Delete code and documentation

---

## ✅ Summary

**Total Endpoints:** 38 directories
**Mapped:** 33 (86.8%)
**Missing:** 4 (10.5%)
**Disabled:** 1 (2.6%)

**Action Items:** 5
**Critical:** 2 (PayComponents, Shifts)
**Important:** 2 (GeneratedDocuments, Holidays)
**Review:** 1 (HRAnalytics)

---

## 📝 Notes

- All repositories are properly registered in HrModule.cs
- All using statements should be added to HrModule.cs
- Each missing endpoint needs a MapXxxEndpoints() method call
- Verify endpoint implementations are complete before enabling
- Test after enabling to ensure no conflicts

