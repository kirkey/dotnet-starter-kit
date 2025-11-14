# ✅ Complete HR Architecture - Area-Specific Positions

**Date:** November 13, 2025  
**Updated Design:** OrganizationalUnit + Area-Specific Positions  

---

## 🏢 Complete Entity Relationships

```
Company (ELECTRIC COOPERATIVE)
│
├── OrganizationalUnit (Area 1)
│   ├── Position: Supervisor (Pos1 - $40K-$55K)
│   ├── Position: Technician (Pos2 - $30K-$42K)
│   ├── Position: Helper (Pos3 - $20K-$28K)
│   └── Employee: John Doe (Area 1, Supervisor Pos1)
│       └── Salary: $45K
│
├── OrganizationalUnit (Area 2)
│   ├── Position: Supervisor (Pos4 - $42K-$58K)
│   ├── Position: Technician (Pos5 - $31K-$43K)
│   ├── Position: Helper (Pos6 - $21K-$29K)
│   └── Employee: Jane Smith (Area 2, Supervisor Pos4)
│       └── Salary: $47K
│
└── OrganizationalUnit (Area 3)
    ├── Position: Supervisor (Pos7 - $41K-$56K)
    ├── Position: Technician (Pos8 - $30.5K-$42.5K)
    ├── Position: Helper (Pos9 - $20.5K-$28.5K)
    └── Employee: Mike Johnson (Area 3, Supervisor Pos7)
        └── Salary: $46K
```

---

## 🔑 Key Design Principles

### 1. OrganizationalUnit = Area (Department)
```
Each area is an OrganizationalUnit with:
- Code: "AREA-001", "AREA-002", "AREA-003"
- Name: "Area 1", "Area 2", "Area 3"
- Type: Department
- Level: 1
- Manager: Area Manager
- CostCenter: For accounting
```

### 2. Position = Area-Specific Job Role
```
Each position belongs to ONE area:
- Position "Supervisor" in Area 1 ≠ Position "Supervisor" in Area 2
- Different Position IDs
- Different salary ranges
- Different job descriptions
- Same title, different positions
```

### 3. Unique Constraint Per Area
```sql
CONSTRAINT IX_Positions_Code_OrgUnit 
    UNIQUE (TenantId, OrganizationalUnitId, Code)

Example:
- Area 1: Code "SUP-001" ✅
- Area 2: Code "SUP-001" ✅ (Different Position record)
- Area 3: Code "SUP-001" ✅ (Different Position record)
```

---

## 📊 Entity Count

```
Updated Entity List: 26 Total (DATABASE-DRIVEN)

Organization:
  ✅ Company (1)
  ✅ OrganizationalUnit (Areas/Departments) (1)
  ✅ Designation (Area-Specific Roles) (1)

Employee Management:
  ✅ Employee (5)
    - EmployeeContact
    - EmployeeDependent
    - EmployeeDocument
    - EmployeeEducation

Time & Attendance:
  ✅ Attendance (4)
    - Timesheet
    - TimesheetLine
    - Shift
    - ShiftAssignment

Leave Management:
  ✅ Holiday (3)
    - LeaveType
    - LeaveBalance
    - LeaveRequest

Payroll (DATABASE-DRIVEN):
  ✅ Payroll (8) ← **ENHANCED**
    - PayrollLine
    - PayrollDeduction
    - PayComponent ← **ENHANCED with 24 new fields**
    - PayComponentRate ← **NEW: Brackets & rates**
    - EmployeePayComponent ← **NEW: Per-employee overrides**
    - TaxBracket (can be replaced by PayComponentRate)
    - BankAccount

Benefits:
  ✅ Benefit (3)
    - BenefitEnrollment
    - BenefitAllocation
    - PerformanceReview

TOTAL: 26 entities (3 new/enhanced for database-driven payroll)
```

---

## 🎯 NEW: Database-Driven Payroll

### Key Enhancement: Fully Configurable Payroll

**Before:** Hard-coded rates and formulas in code  
**After:** All rates, formulas, and configurations in database

**Benefits:**
- ✅ Admin can update SSS/PhilHealth/Pag-IBIG rates without code deployment
- ✅ Tax brackets update via database, not code
- ✅ Custom pay components per company/area
- ✅ Employee-specific allowances and deductions
- ✅ Historical rate tracking for compliance
- ✅ Audit trail for all changes

**See:** `DATABASE_DRIVEN_PAYROLL_ARCHITECTURE.md` for complete details

---

## 🎯 Example Scenario: Your Electric Cooperative

### Setup Phase 1: Create Areas
```csharp
// Create Area 1
var area1 = OrganizationalUnit.Create(
    companyId: cooperativeId,
    code: "AREA-001",
    name: "Area 1",
    type: OrganizationalUnitType.Department,
    managerId: area1ManagerId,
    costCenter: "AREA1-CC",
    location: "Northern Region");

// Create Area 2
var area2 = OrganizationalUnit.Create(
    companyId: cooperativeId,
    code: "AREA-002",
    name: "Area 2",
    type: OrganizationalUnitType.Department,
    managerId: area2ManagerId,
    costCenter: "AREA2-CC",
    location: "Central Region");

// Create Area 3
var area3 = OrganizationalUnit.Create(
    companyId: cooperativeId,
    code: "AREA-003",
    name: "Area 3",
    type: OrganizationalUnitType.Department,
    managerId: area3ManagerId,
    costCenter: "AREA3-CC",
    location: "Southern Region");
```

### Setup Phase 2: Create Positions per Area

**Area 1 Positions:**
```csharp
var area1Supervisor = Position.Create(
    code: "SUP-001",
    title: "Area Supervisor",
    organizationalUnitId: area1.Id,
    description: "Supervises field operations in Area 1",
    minSalary: 40000,
    maxSalary: 55000);

var area1Technician = Position.Create(
    code: "TECH-001",
    title: "Line Technician",
    organizationalUnitId: area1.Id,
    description: "Installs and maintains distribution lines in Area 1",
    minSalary: 30000,
    maxSalary: 42000);

var area1Helper = Position.Create(
    code: "HELP-001",
    title: "Field Helper",
    organizationalUnitId: area1.Id,
    description: "Assists technicians in Area 1",
    minSalary: 20000,
    maxSalary: 28000);
```

**Area 2 Positions (Same titles, different records):**
```csharp
var area2Supervisor = Position.Create(
    code: "SUP-001",  // ✅ Same code as Area 1!
    title: "Area Supervisor",  // ✅ Same title as Area 1!
    organizationalUnitId: area2.Id,
    description: "Supervises field operations in Area 2",
    minSalary: 42000,  // ✅ Different salary!
    maxSalary: 58000);

var area2Technician = Position.Create(
    code: "TECH-001",  // ✅ Same code as Area 1!
    title: "Line Technician",  // ✅ Same title as Area 1!
    organizationalUnitId: area2.Id,
    description: "Installs and maintains distribution lines in Area 2",
    minSalary: 31000,  // ✅ Different salary!
    maxSalary: 43000);

var area2Helper = Position.Create(
    code: "HELP-001",
    title: "Field Helper",
    organizationalUnitId: area2.Id,
    minSalary: 21000,  // ✅ Different salary!
    maxSalary: 29000);
```

**Area 3 Positions (Same pattern):**
```csharp
var area3Supervisor = Position.Create(
    code: "SUP-001",
    title: "Area Supervisor",
    organizationalUnitId: area3.Id,
    description: "Supervises field operations in Area 3",
    minSalary: 41000,
    maxSalary: 56000);

// ... similar for Technician and Helper
```

### Setup Phase 3: Assign Employees

**Area 1 Team:**
```csharp
var johnDoe = Employee.Create(
    employeeNumber: "EMP-001",
    name: "John Doe",
    companyId: cooperativeId,
    organizationalUnitId: area1.Id,  // ✅ Area 1
    positionId: area1Supervisor.Id); // ✅ Area 1 Supervisor position
johnDoe.SetSalary(45000);

var janeDoe = Employee.Create(
    employeeNumber: "EMP-002",
    name: "Jane Doe",
    companyId: cooperativeId,
    organizationalUnitId: area1.Id,  // ✅ Area 1
    positionId: area1Technician.Id); // ✅ Area 1 Technician position
janeDoe.SetSalary(35000);
```

**Area 2 Team (Same position titles, different employees):**
```csharp
var mikeSmith = Employee.Create(
    employeeNumber: "EMP-003",
    name: "Mike Smith",
    companyId: cooperativeId,
    organizationalUnitId: area2.Id,  // ✅ Area 2
    positionId: area2Supervisor.Id); // ✅ Area 2 Supervisor position (different from John's)
mikeSmith.SetSalary(47000);  // ✅ Area 2 salary

var sarahJohnson = Employee.Create(
    employeeNumber: "EMP-004",
    name: "Sarah Johnson",
    companyId: cooperativeId,
    organizationalUnitId: area2.Id,  // ✅ Area 2
    positionId: area2Technician.Id); // ✅ Area 2 Technician position (different from Jane's)
sarahJohnson.SetSalary(36000);  // ✅ Area 2 salary
```

---

## 🔍 Query Examples

### Query All Supervisors Across All Areas
```csharp
var allSupervisors = await positionRepository.ListAsync(
    new PositionsByTitleSpec("Area Supervisor"));

// Returns:
// - Position 1: Area 1 Supervisor ($40K-$55K)
// - Position 4: Area 2 Supervisor ($42K-$58K)
// - Position 7: Area 3 Supervisor ($41K-$56K)
```

### Query Positions in Area 1 Only
```csharp
var area1Positions = await positionRepository.ListAsync(
    new PositionsByOrganizationalUnitSpec(area1.Id));

// Returns:
// - Supervisor (Area 1)
// - Technician (Area 1)
// - Helper (Area 1)
```

### Query Employees in Area 2
```csharp
var area2Employees = await employeeRepository.ListAsync(
    new EmployeesByOrganizationalUnitSpec(area2.Id));

// Returns:
// - Mike Smith (Area 2 Supervisor)
// - Sarah Johnson (Area 2 Technician)
// - ... other area 2 employees
```

### Query Employees with Specific Position Title
```csharp
var supervisors = await employeeRepository.ListAsync(
    new EmployeesByPositionTitleSpec("Area Supervisor"));

// Returns:
// - John Doe (Area 1 Supervisor)
// - Mike Smith (Area 2 Supervisor)
// - ... supervisors from all areas
```

---

## 📋 CQRS Operations for Position

### Create Position (Area-Specific)
```
POST /api/v1/humanresources/positions
{
  "code": "SUP-001",
  "title": "Area Supervisor",
  "organizationalUnitId": "area1-guid",
  "description": "...",
  "minSalary": 40000,
  "maxSalary": 55000
}
```

### Get Position Details
```
GET /api/v1/humanresources/positions/{id}
```

### Search Positions (with filters)
```
POST /api/v1/humanresources/positions/search
{
  "organizationalUnitId": "area1-guid",  // ✅ Filter by area
  "title": "Area Supervisor",             // ✅ Filter by title
  "salaryMin": 40000,
  "salaryMax": 60000,
  "isActive": true,
  "pageNumber": 1,
  "pageSize": 10
}
```

### Update Position
```
PUT /api/v1/humanresources/positions/{id}
{
  "title": "Senior Area Supervisor",
  "minSalary": 42000,
  "maxSalary": 58000
}
```

### Delete Position
```
DELETE /api/v1/humanresources/positions/{id}
```

---

## ✅ Benefits of Area-Specific Positions

| Benefit | Why Important |
|---------|--------------|
| **Area-Specific Salary** | Areas have different cost of living |
| **Flexible Job Descriptions** | Same role has different responsibilities per area |
| **Cost Center Tracking** | Each area manages its own costs |
| **Area Manager Control** | Area managers manage their own positions and staff |
| **Reporting by Area** | Easy to filter and report per area |
| **Multi-Area Queries** | Can query across all areas or by specific area |
| **Scalability** | Add new areas without code changes |
| **Accurate Payroll** | Salary data tied to area and position |

---

## 🎉 Summary

**This design is PERFECT for your Electric Cooperative because:**

✅ **Areas (Departments)** - Managed as OrganizationalUnits
✅ **Area-Specific Positions** - Same title can exist in multiple areas
✅ **Salary by Area** - Different salaries for same role per area
✅ **Area Management** - Area managers control their own staff and positions
✅ **Easy Querying** - Filter by area, position, salary range, etc.
✅ **Scalable** - Add new areas anytime without changing system
✅ **SAAS-Ready** - Supports any organizational structure

**Exactly matches your scenario:**
```
Area 1 → Supervisor (Pos 1, $40K-$55K)
Area 2 → Supervisor (Pos 2, $42K-$58K)
Area 3 → Supervisor (Pos 3, $41K-$56K)
All with same title but different positions and salaries!
```

