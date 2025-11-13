# 👨‍💼 Employee Designation History & Temporal Queries Guide

**Purpose:** Track employee designation changes over time and query employee status at any specific date  
**Pattern:** Temporal Data / Slowly Changing Dimension (Type 2)  
**Build Date:** November 13, 2025

---

## 🎯 Problem Statement

**Scenario:**
- Employee John Doe hired Jan 2020 as "Technician"
- Promoted to "Senior Technician" Jan 2022
- Promoted to "Supervisor" Jan 2024
- Later assigned "Acting Manager" Jan-Mar 2025

**Requirements:**
1. Query: "How many active employees on Dec 15, 2024?"
2. Query: "What was John's designation on Dec 15, 2024?"
3. Query: "Show all designation changes for John"
4. Query: "Show payroll headcount by designation on any date"

---

## 🏗️ Solution Architecture

### Core Concept: Temporal Tracking

The `DesignationAssignment` entity already captures this with:
- `EffectiveDate` - When designation starts
- `EndDate` - When designation ends (NULL = ongoing)
- `IsPlantilla` - Primary designation flag
- `IsActive` - Soft delete capability

### Three Query Types

```
1. POINT-IN-TIME: Status at a specific date
   SELECT * FROM DesignationAssignments 
   WHERE EffectiveDate <= @date AND (EndDate IS NULL OR EndDate >= @date)

2. HISTORICAL: All changes for an employee
   SELECT * FROM DesignationAssignments 
   WHERE EmployeeId = @id 
   ORDER BY EffectiveDate DESC

3. AGGREGATE: Summary statistics on a date
   SELECT COUNT(*), DesignationId 
   FROM DesignationAssignments 
   WHERE EffectiveDate <= @date AND (EndDate IS NULL OR EndDate >= @date)
   GROUP BY DesignationId
```

---

## 📊 Database Design

### Current Structure (Already Implemented) ✅

```sql
DesignationAssignments
├─ Id (PK)
├─ EmployeeId (FK)
├─ DesignationId (FK)
├─ EffectiveDate (Index)
├─ EndDate (Index)
├─ IsPlantilla (bool)
├─ IsActingAs (bool)
├─ AdjustedSalary (decimal?)
├─ Reason (string?)
├─ IsActive (bool)
├─ CreatedBy
├─ CreatedOn
└─ LastModifiedOn
```

### Recommended Indexes for Performance

```sql
-- Point-in-time queries
CREATE INDEX IX_EDA_PointInTime 
ON DesignationAssignments(EmployeeId, EffectiveDate DESC, EndDate);

-- Employee history
CREATE INDEX IX_EDA_EmployeeHistory 
ON DesignationAssignments(EmployeeId, EffectiveDate DESC);

-- Designation changes
CREATE INDEX IX_EDA_Designations 
ON DesignationAssignments(DesignationId, EffectiveDate DESC, EndDate);

-- Active only (common query)
CREATE INDEX IX_EDA_Active 
ON DesignationAssignments(EffectiveDate DESC, EndDate) 
WHERE IsActive = 1;

-- Payroll periods
CREATE INDEX IX_EDA_PayrollPeriod 
ON DesignationAssignments(EffectiveDate, EndDate) 
WHERE IsPlantilla = 1;
```

---

## 🔍 Query Implementations

### 1. Get Employee's Current Designation

```csharp
public class EmployeeCurrentDesignationSpec : Specification<DesignationAssignment>
{
    public EmployeeCurrentDesignationSpec(DefaultIdType employeeId, DateTime? asOfDate = null)
    {
        var checkDate = asOfDate ?? DateTime.UtcNow;
        
        Query
            .Where(a => a.EmployeeId == employeeId
                && a.IsPlantilla
                && a.IsActive
                && a.EffectiveDate <= checkDate
                && (a.EndDate == null || a.EndDate > checkDate))
            .Include(a => a.Designation)
            .OrderByDescending(a => a.EffectiveDate)
            .Take(1);
    }
}
```

**Usage:**
```csharp
// Get current designation today
var current = await readRepository.FirstOrDefaultAsync(
    new EmployeeCurrentDesignationSpec(employeeId));

// Get designation on specific date
var designationDec15 = await readRepository.FirstOrDefaultAsync(
    new EmployeeCurrentDesignationSpec(employeeId, new DateTime(2024, 12, 15)));
```

---

### 2. Get Employee Designation History

```csharp
public class EmployeeDesignationHistorySpec : Specification<DesignationAssignment>
{
    public EmployeeDesignationHistorySpec(DefaultIdType employeeId)
    {
        Query
            .Where(a => a.EmployeeId == employeeId && a.IsActive)
            .Include(a => a.Designation)
            .OrderByDescending(a => a.EffectiveDate);
    }
}
```

**Usage:**
```csharp
// Show all designation changes
var history = await readRepository.ListAsync(
    new EmployeeDesignationHistorySpec(employeeId));

foreach (var assignment in history)
{
    var endDate = assignment.EndDate?.ToString("MMM d, yyyy") ?? "Current";
    Console.WriteLine($"{assignment.EffectiveDate:MMM d, yyyy} - {endDate}: {assignment.Designation.Title}");
}

// Output:
// Jan 1, 2024 - Current: Supervisor
// Jan 15, 2022 - Dec 31, 2023: Senior Technician
// Jan 1, 2020 - Jan 14, 2022: Technician
```

---

### 3. Active Employees on Specific Date

```csharp
public class ActiveEmployeesOnDateSpec : Specification<DesignationAssignment>
{
    public ActiveEmployeesOnDateSpec(DateTime asOfDate)
    {
        Query
            .Where(a => a.IsPlantilla
                && a.IsActive
                && a.EffectiveDate <= asOfDate
                && (a.EndDate == null || a.EndDate >= asOfDate))
            .Include(a => a.Employee)
            .Include(a => a.Designation)
            .Distinct()
            .OrderBy(a => a.Employee.EmployeeNumber);
    }
}
```

**Usage:**
```csharp
// Count active employees on Dec 15, 2024
var date = new DateTime(2024, 12, 15);
var activeEmployees = await readRepository.ListAsync(
    new ActiveEmployeesOnDateSpec(date));

Console.WriteLine($"Active employees on {date:MMM d, yyyy}: {activeEmployees.Count}");

// Get by designation
var byDesignation = activeEmployees
    .GroupBy(a => a.Designation.Title)
    .Select(g => new { Designation = g.Key, Count = g.Count() });

foreach (var group in byDesignation)
{
    Console.WriteLine($"{group.Designation}: {group.Count}");
}

// Output:
// Supervisor: 5
// Senior Technician: 12
// Technician: 45
```

---

### 4. Payroll Headcount by Designation

```csharp
public class PayrollHeadcountByDesignationSpec : Specification<DesignationAssignment>
{
    public PayrollHeadcountByDesignationSpec(DateTime payrollDate)
    {
        Query
            .Where(a => a.IsPlantilla
                && a.IsActive
                && a.EffectiveDate <= payrollDate
                && (a.EndDate == null || a.EndDate > payrollDate))
            .Include(a => a.Designation)
            .GroupBy(a => a.DesignationId)
            .Select(g => new
            {
                DesignationId = g.Key,
                Count = g.Count(),
                TotalSalary = g.Sum(a => a.Designation.MaxSalary ?? 0)
            });
    }
}
```

---

### 5. Employee with All Historical Designations

```csharp
public class EmployeeWithHistoryDto
{
    public DefaultIdType EmployeeId { get; set; }
    public string EmployeeNumber { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public string CurrentDesignation { get; set; } = default!;
    public DateTime? CurrentDesignationStart { get; set; }
    
    public List<DesignationHistoryDto> DesignationHistory { get; set; } = new();
}

public class DesignationHistoryDto
{
    public string Designation { get; set; } = default!;
    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int TenureMonths { get; set; }
    public bool IsPlantilla { get; set; }
    public bool IsActingAs { get; set; }
}

// Query
var employee = await context.Employees
    .AsNoTracking()
    .Where(e => e.Id == employeeId)
    .Select(e => new EmployeeWithHistoryDto
    {
        EmployeeId = e.Id,
        EmployeeNumber = e.EmployeeNumber,
        FullName = e.FullName,
        CurrentDesignation = e.DesignationAssignments
            .Where(a => a.IsPlantilla && a.IsActive && a.EffectiveDate <= DateTime.UtcNow && (a.EndDate == null || a.EndDate > DateTime.UtcNow))
            .OrderByDescending(a => a.EffectiveDate)
            .Select(a => a.Designation.Title)
            .FirstOrDefault() ?? "Unknown",
        CurrentDesignationStart = e.DesignationAssignments
            .Where(a => a.IsPlantilla && a.IsActive && a.EffectiveDate <= DateTime.UtcNow && (a.EndDate == null || a.EndDate > DateTime.UtcNow))
            .OrderByDescending(a => a.EffectiveDate)
            .Select(a => a.EffectiveDate)
            .FirstOrDefault(),
        DesignationHistory = e.DesignationAssignments
            .Where(a => a.IsActive)
            .OrderByDescending(a => a.EffectiveDate)
            .Select(a => new DesignationHistoryDto
            {
                Designation = a.Designation.Title,
                EffectiveDate = a.EffectiveDate,
                EndDate = a.EndDate,
                TenureMonths = (int)Math.Round(
                    ((a.EndDate ?? DateTime.UtcNow) - a.EffectiveDate).TotalDays / 30.44),
                IsPlantilla = a.IsPlantilla,
                IsActingAs = a.IsActingAs
            })
            .ToList()
    })
    .FirstOrDefaultAsync();
```

---

## 🔌 API Endpoints

### 1. Get Employee Designation History

```csharp
[HttpGet("employees/{employeeId}/designation-history")]
public async Task<IActionResult> GetDesignationHistory(DefaultIdType employeeId)
{
    var employee = await readRepository.FirstOrDefaultAsync(
        new EmployeeWithHistorySpec(employeeId));
    
    return Ok(new
    {
        employee.EmployeeNumber,
        employee.FullName,
        currentDesignation = employee.DesignationAssignments
            .Where(a => a.IsCurrentEffective())
            .FirstOrDefault()?.Designation.Title,
        designationHistory = employee.DesignationAssignments
            .OrderByDescending(a => a.EffectiveDate)
            .Select(a => new
            {
                designation = a.Designation.Title,
                effectiveDate = a.EffectiveDate,
                endDate = a.EndDate,
                duration = $"{a.GetTenureMonths()} months",
                type = a.IsPlantilla ? "Primary" : "Acting"
            })
    });
}
```

**Response:**
```json
{
  "employeeNumber": "EMP-001",
  "fullName": "John Doe",
  "currentDesignation": "Supervisor",
  "designationHistory": [
    {
      "designation": "Supervisor",
      "effectiveDate": "2024-01-01",
      "endDate": null,
      "duration": "11 months",
      "type": "Primary"
    },
    {
      "designation": "Senior Technician",
      "effectiveDate": "2022-01-15",
      "endDate": "2023-12-31",
      "duration": "23 months",
      "type": "Primary"
    },
    {
      "designation": "Technician",
      "effectiveDate": "2020-01-01",
      "endDate": "2022-01-14",
      "duration": "25 months",
      "type": "Primary"
    }
  ]
}
```

---

### 2. Get Active Employees on Date

```csharp
[HttpPost("employees/active-on-date")]
public async Task<IActionResult> GetActiveEmployeesOnDate(
    [FromBody] EmployeesOnDateRequest request)
{
    var activeEmployees = await readRepository.ListAsync(
        new ActiveEmployeesOnDateSpec(request.AsOfDate));
    
    var byDesignation = activeEmployees
        .GroupBy(a => a.Designation.Title)
        .Select(g => new
        {
            designation = g.Key,
            count = g.Count(),
            employees = g.Select(a => new
            {
                employeeNumber = a.Employee.EmployeeNumber,
                fullName = a.Employee.FullName,
                organizationalUnit = a.Employee.OrganizationalUnit.Name,
                effectiveDate = a.EffectiveDate
            })
        });
    
    return Ok(new
    {
        asOfDate = request.AsOfDate,
        totalActiveEmployees = activeEmployees.Count,
        byDesignation
    });
}
```

**Request:**
```json
{
  "asOfDate": "2024-12-15"
}
```

**Response:**
```json
{
  "asOfDate": "2024-12-15",
  "totalActiveEmployees": 62,
  "byDesignation": [
    {
      "designation": "Supervisor",
      "count": 5,
      "employees": [
        {
          "employeeNumber": "EMP-001",
          "fullName": "John Doe",
          "organizationalUnit": "Area 1",
          "effectiveDate": "2024-01-01"
        }
      ]
    }
  ]
}
```

---

### 3. Get Employee Status at Point in Time

```csharp
[HttpGet("employees/{employeeId}/status-on-date")]
public async Task<IActionResult> GetEmployeeStatusOnDate(
    DefaultIdType employeeId,
    [FromQuery] DateTime asOfDate)
{
    var employee = await employeeRepository.GetByIdAsync(employeeId);
    if (employee is null)
        throw new EmployeeNotFoundException(employeeId);
    
    var designation = await readRepository.FirstOrDefaultAsync(
        new EmployeeCurrentDesignationSpec(employeeId, asOfDate));
    
    // Check if employee was active on this date
    var wasActive = employee.HireDate <= asOfDate && 
                    (employee.TerminationDate == null || employee.TerminationDate > asOfDate);
    
    return Ok(new
    {
        asOfDate,
        employeeNumber = employee.EmployeeNumber,
        fullName = employee.FullName,
        wasActive,
        designation = designation?.Designation.Title ?? "Not Assigned",
        designationStartDate = designation?.EffectiveDate,
        tenure = employee.HireDate.HasValue ? 
            $"{((asOfDate - employee.HireDate.Value).TotalDays / 365.25):F1} years" : 
            "Unknown"
    });
}
```

**Response:**
```json
{
  "asOfDate": "2024-12-15",
  "employeeNumber": "EMP-001",
  "fullName": "John Doe",
  "wasActive": true,
  "designation": "Supervisor",
  "designationStartDate": "2024-01-01",
  "tenure": "4.9 years"
}
```

---

## 📋 Search/Filter Request

```csharp
public class SearchEmployeeHistoryRequest : PaginationFilter
{
    /// <summary>
    /// Organizational unit ID to filter employees
    /// </summary>
    public DefaultIdType? OrganizationalUnitId { get; set; }
    
    /// <summary>
    /// Designation ID to find all employees who held it
    /// </summary>
    public DefaultIdType? DesignationId { get; set; }
    
    /// <summary>
    /// Point-in-time query: Get employees active on this date
    /// </summary>
    public DateTime? PointInTimeDate { get; set; }
    
    /// <summary>
    /// Show acting designations too
    /// </summary>
    public bool IncludeActingDesignations { get; set; } = false;
    
    /// <summary>
    /// Employment status filter
    /// </summary>
    public string? EmploymentStatus { get; set; }
    
    /// <summary>
    /// Date range: From
    /// </summary>
    public DateTime? FromDate { get; set; }
    
    /// <summary>
    /// Date range: To
    /// </summary>
    public DateTime? ToDate { get; set; }
}

// Specification
public class SearchEmployeeDesignationHistorySpec 
    : EntitiesByPaginationFilterSpec<DesignationAssignment, EmployeeDesignationHistoryDto>
{
    public SearchEmployeeDesignationHistorySpec(SearchEmployeeHistoryRequest request)
        : base(request)
    {
        Query
            .Include(a => a.Employee)
            .Include(a => a.Designation)
            .Where(a => a.IsActive)
            .Where(a => a.IsPlantilla || request.IncludeActingDesignations)
            .Where(a => a.OrganizationalUnitId == request.OrganizationalUnitId, 
                request.OrganizationalUnitId.HasValue)
            .Where(a => a.DesignationId == request.DesignationId, 
                request.DesignationId.HasValue)
            .Where(a => a.EffectiveDate >= request.FromDate, request.FromDate.HasValue)
            .Where(a => a.EffectiveDate <= request.ToDate, request.ToDate.HasValue);
        
        // Point in time query
        if (request.PointInTimeDate.HasValue)
        {
            var date = request.PointInTimeDate.Value;
            Query.Where(a => a.EffectiveDate <= date && (a.EndDate == null || a.EndDate > date));
        }
        
        Query.OrderByDescending(a => a.EffectiveDate);
    }
}
```

---

## 💡 Business Use Cases

### Use Case 1: Annual Review Report
```
"Show all employees and their designations as of Dec 31, 2024"
→ Get snapshot of workforce on review date
→ Calculate tenure and promotion history
→ Use for performance reviews
```

### Use Case 2: Payroll Processing
```
"Process payroll for Dec 2024 period (Dec 1-31)"
→ Get employee status on each pay date
→ Apply correct salary for each designation period
→ Calculate prorated salary if designation changed mid-month
```

### Use Case 3: Designation Analysis
```
"How many times have employees in Area 1 been promoted in 2024?"
→ Count designation changes per employee
→ Filter by promotion (higher salary)
→ Group by department/area
```

### Use Case 4: Compliance/Audit
```
"Show all changes to John Doe's designation in 2024"
→ Full audit trail with dates
→ Identify who made changes (AuditableEntity)
→ Export for compliance review
```

### Use Case 5: Succession Planning
```
"Who held the Supervisor role in the last 3 years?"
→ Show all employees who ever held position
→ When they held it
→ Whether they still hold it
→ Identify successors and their progression
```

---

## 🎯 Implementation Checklist

- ✅ DesignationAssignment entity (already done)
- ✅ EffectiveDate & EndDate fields (already done)
- ✅ IsPlantilla & IsActingAs flags (already done)
- ⏳ Add database indexes for performance
- ⏳ Create Specification classes for temporal queries
- ⏳ Create endpoint for designation history
- ⏳ Create endpoint for point-in-time queries
- ⏳ Create endpoint for active employees on date
- ⏳ Create request/response DTOs
- ⏳ Add unit tests for date-based logic
- ⏳ Create reports/dashboards

---

## 🚀 Next Steps

1. **Add Specifications** to `DesignationAssignments/Specifications/`
   - EmployeeCurrentDesignationSpec.cs
   - EmployeeDesignationHistorySpec.cs
   - ActiveEmployeesOnDateSpec.cs

2. **Create Handlers** in Application layer
   - GetEmployeeDesignationHistoryRequest
   - GetActiveEmployeesOnDateRequest
   - SearchEmployeeHistoryRequest

3. **Create Endpoints** in Infrastructure
   - GET /employees/{id}/designation-history
   - POST /employees/active-on-date
   - GET /employees/{id}/status-on-date

4. **Add Tests** for temporal logic
   - Test point-in-time queries
   - Test designation transitions
   - Test active employee count

---

**You now have a complete temporal query system for managing employee designation history!** 🎉

