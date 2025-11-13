# 🏢 Organizational Hierarchy Design - Department, Division, Section

**Date:** November 13, 2025  
**Module:** HumanResources  
**Pattern:** Hierarchical Organization Structure  

---

## 🎯 Business Requirement

Electric Cooperatives need a **flexible organizational hierarchy**:

```
Company
  └── Department (Required)
        └── Division (Optional)
              └── Section (Optional - always under Division)
                    └── Multiple Sections per Division supported
```

**Key Rules:**
- Departments are required (top level)
- Divisions are optional (under departments)
- Sections are optional but **must be under a Division** (cannot be directly under Department)
- A Division can have multiple Sections
- Employees can be assigned at any level

### Employee Assignment Scenarios

1. **Scenario A:** Employee → Department only
   - Example: HR Manager reports directly to HR Department (no division)
   
2. **Scenario B:** Employee → Department → Division
   - Example: Line Worker reports to Operations Department, Distribution Division (no section)
   
3. **Scenario C:** Employee → Department → Division → Section
   - Example: Meter Reader reports to Operations Department, Distribution Division, Metering Section

4. **Scenario D:** Multiple Sections in Division
   - Example: Distribution Division has:
     - Line Maintenance Section
     - Metering Section
     - Transformer Maintenance Section

---

## 🎨 Recommended Design: Self-Referencing Hierarchy

### Best Practice: Single `OrganizationalUnit` Entity

Instead of 3 separate entities (Department, Division, Section), use **one flexible entity** with self-referencing hierarchy:

```csharp
public class OrganizationalUnit : AuditableEntity, IAggregateRoot
{
    // Core Properties
    public string Code { get; private set; } = default!;
    // Name from AuditableEntity base class
    public OrganizationalUnitType Type { get; private set; } // Department, Division, Section
    
    // Hierarchy
    public DefaultIdType? ParentId { get; private set; }
    public OrganizationalUnit? Parent { get; private set; }
    public List<OrganizationalUnit> Children { get; private set; } = new();
    
    // Management
    public DefaultIdType? ManagerId { get; private set; }
    public Employee? Manager { get; private set; }
    
    // Configuration
    public bool IsActive { get; private set; }
    public int Level { get; private set; } // 1=Dept, 2=Division, 3=Section
    
    // Business Properties
    public string? CostCenter { get; private set; }
    public string? Location { get; private set; }
}

public enum OrganizationalUnitType
{
    Department = 1,
    Division = 2,
    Section = 3
}
```

### Employee Assignment

```csharp
public class Employee : AuditableEntity, IAggregateRoot
{
    // Direct organizational assignment
    public DefaultIdType OrganizationalUnitId { get; private set; }
    public OrganizationalUnit OrganizationalUnit { get; private set; } = default!;
    
    // The employee belongs to the most specific unit
    // Hierarchy is derived from OrganizationalUnit.Parent relationships
}
```

---

## 📊 Database Schema

```sql
CREATE TABLE hr.OrganizationalUnits (
    Id uniqueidentifier PRIMARY KEY,
    TenantId nvarchar(64) NOT NULL,
    CompanyId uniqueidentifier NOT NULL,
    
    -- Core
    Code nvarchar(50) NOT NULL,
    Name nvarchar(256) NOT NULL,
    Type int NOT NULL, -- 1=Dept, 2=Division, 3=Section
    
    -- Hierarchy
    ParentId uniqueidentifier NULL,
    Level int NOT NULL,
    HierarchyPath nvarchar(500), -- e.g., "/DEPT-001/DIV-002/SEC-003/"
    
    -- Management
    ManagerId uniqueidentifier NULL,
    
    -- Business
    CostCenter nvarchar(50),
    Location nvarchar(200),
    IsActive bit NOT NULL DEFAULT 1,
    
    -- Audit
    CreatedBy nvarchar(256),
    CreatedOn datetimeoffset NOT NULL,
    LastModifiedBy nvarchar(256),
    LastModifiedOn datetimeoffset,
    
    -- Constraints
    CONSTRAINT FK_OrganizationalUnits_Parent 
        FOREIGN KEY (ParentId) REFERENCES hr.OrganizationalUnits(Id),
    CONSTRAINT FK_OrganizationalUnits_Company 
        FOREIGN KEY (CompanyId) REFERENCES hr.Companies(Id),
    CONSTRAINT FK_OrganizationalUnits_Manager 
        FOREIGN KEY (ManagerId) REFERENCES hr.Employees(Id)
);

-- Indexes
CREATE UNIQUE INDEX IX_OrganizationalUnits_Code 
    ON hr.OrganizationalUnits(TenantId, CompanyId, Code);
    
CREATE INDEX IX_OrganizationalUnits_ParentId 
    ON hr.OrganizationalUnits(ParentId);
    
CREATE INDEX IX_OrganizationalUnits_Type 
    ON hr.OrganizationalUnits(Type);
    
CREATE INDEX IX_OrganizationalUnits_HierarchyPath 
    ON hr.OrganizationalUnits(HierarchyPath);
```

---

## 🎯 Usage Examples

### Example 1: Create Department Only

```csharp
// Create HR Department (no parent)
var hrDept = OrganizationalUnit.Create(
    companyId: companyId,
    code: "HR-001",
    name: "Human Resources",
    type: OrganizationalUnitType.Department,
    parentId: null,
    managerId: hrDirectorId);
```

### Example 2: Create Division Under Department

```csharp
// Create Distribution Division under Operations Department
var distDiv = OrganizationalUnit.Create(
    companyId: companyId,
    code: "DIST-001",
    name: "Distribution Division",
    type: OrganizationalUnitType.Division,
    parentId: operationsDeptId, // Parent is Department
    managerId: divisionManagerId);
```

### Example 3: Create Section Under Division

```csharp
// Create Metering Section under Distribution Division
var meterSection = OrganizationalUnit.Create(
    companyId: companyId,
    code: "METER-001",
    name: "Metering Section",
    type: OrganizationalUnitType.Section,
    parentId: distributionDivisionId, // Parent is Division
    managerId: sectionHeadId);
```

### Example 4: Create Multiple Sections Under Division

```csharp
// Create multiple sections under Distribution Division
var lineMaintenanceSection = OrganizationalUnit.Create(
    companyId: companyId,
    code: "LINE-001",
    name: "Line Maintenance Section",
    type: OrganizationalUnitType.Section,
    parentId: distributionDivisionId, // Parent is Division
    managerId: lineSupervisorId);

var meteringSection = OrganizationalUnit.Create(
    companyId: companyId,
    code: "METER-001",
    name: "Metering Section",
    type: OrganizationalUnitType.Section,
    parentId: distributionDivisionId, // Same parent Division
    managerId: meteringSupervisorId);

var transformerSection = OrganizationalUnit.Create(
    companyId: companyId,
    code: "TRANS-001",
    name: "Transformer Maintenance Section",
    type: OrganizationalUnitType.Section,
    parentId: distributionDivisionId, // Same parent Division
    managerId: transformerSupervisorId);
```

### Example 5: Assign Employee

```csharp
// Employee assigned to Section (most specific)
var employee = Employee.Create(
    companyId: companyId,
    employeeNumber: "EMP-001",
    name: "John Doe",
    organizationalUnitId: meterSectionId); // Assigned to Section

// Query to get full hierarchy:
var section = await GetOrganizationalUnitWithParents(meterSectionId);
// Returns: HR Dept → Distribution Division → Metering Section
```

---

## 🔍 Querying the Hierarchy

### Get Full Hierarchy Path

```csharp
public class GetOrganizationalUnitHierarchyRequest : IRequest<OrganizationalUnitHierarchyResponse>
{
    public DefaultIdType OrganizationalUnitId { get; init; }
}

public class GetOrganizationalUnitHierarchyHandler : IRequestHandler<...>
{
    public async Task<OrganizationalUnitHierarchyResponse> Handle(...)
    {
        var unit = await repository.GetByIdAsync(request.OrganizationalUnitId);
        var hierarchy = new List<OrganizationalUnitDto>();
        
        // Walk up the parent chain
        var current = unit;
        while (current != null)
        {
            hierarchy.Insert(0, new OrganizationalUnitDto
            {
                Id = current.Id,
                Code = current.Code,
                Name = current.Name,
                Type = current.Type,
                Level = current.Level
            });
            
            current = current.Parent;
        }
        
        return new OrganizationalUnitHierarchyResponse
        {
            Units = hierarchy,
            FullPath = string.Join(" → ", hierarchy.Select(h => h.Name))
        };
    }
}
```

### Get All Employees in a Unit (Including Sub-units)

```csharp
public class GetEmployeesByOrganizationalUnitSpec : Specification<Employee>
{
    public GetEmployeesByOrganizationalUnitSpec(DefaultIdType unitId, bool includeChildren)
    {
        Query.Where(e => e.OrganizationalUnitId == unitId);
        
        if (includeChildren)
        {
            // Include employees from child units
            Query.Include(e => e.OrganizationalUnit)
                .Where(e => e.OrganizationalUnit.HierarchyPath.StartsWith(hierarchyPath));
        }
    }
}
```

---

## 📋 Entity Structure

### OrganizationalUnit Entity (Complete)

```csharp
namespace FSH.Starter.WebApi.HumanResources.Domain;

/// <summary>
/// Represents a hierarchical organizational unit (Department, Division, or Section).
/// </summary>
/// <remarks>
/// Supports hierarchical structure:
/// - Department (Level 1, no parent) - Required top level
/// - Division (Level 2, parent = Department) - Optional middle level
/// - Section (Level 3, parent = Division) - Optional, must be under Division
/// 
/// Business rules:
/// - Code must be unique within company
/// - Departments have no parent
/// - Divisions must have Department parent
/// - Sections must have Division parent (cannot be directly under Department)
/// - A Division can have multiple Sections
/// - Cannot create circular references
/// - Manager must be an active employee
/// </remarks>
public class OrganizationalUnit : AuditableEntity, IAggregateRoot
{
    private OrganizationalUnit() { }

    private OrganizationalUnit(
        DefaultIdType id,
        DefaultIdType companyId,
        string code,
        string name,
        OrganizationalUnitType type,
        DefaultIdType? parentId,
        int level)
    {
        Id = id;
        CompanyId = companyId;
        Code = code;
        Name = name;
        Type = type;
        ParentId = parentId;
        Level = level;
        IsActive = true;

        QueueDomainEvent(new OrganizationalUnitCreated { OrganizationalUnit = this });
    }

    /// <summary>
    /// Company this organizational unit belongs to.
    /// </summary>
    public DefaultIdType CompanyId { get; private set; }
    public Company Company { get; private set; } = default!;

    /// <summary>
    /// Unique code for this organizational unit.
    /// Example: "HR-001", "DIST-DIV-001", "METER-SEC-001"
    /// </summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Type of organizational unit.
    /// </summary>
    public OrganizationalUnitType Type { get; private set; }

    /// <summary>
    /// Parent organizational unit ID.
    /// Null for top-level departments.
    /// </summary>
    public DefaultIdType? ParentId { get; private set; }
    public OrganizationalUnit? Parent { get; private set; }

    /// <summary>
    /// Child organizational units.
    /// </summary>
    public List<OrganizationalUnit> Children { get; private set; } = new();

    /// <summary>
    /// Hierarchy level (1=Department, 2=Division, 3=Section).
    /// </summary>
    public int Level { get; private set; }

    /// <summary>
    /// Materialized path for efficient hierarchy queries.
    /// Example: "/HR-001/DIST-001/METER-001/"
    /// </summary>
    public string? HierarchyPath { get; private set; }

    /// <summary>
    /// Manager/Head of this organizational unit.
    /// </summary>
    public DefaultIdType? ManagerId { get; private set; }
    public Employee? Manager { get; private set; }

    /// <summary>
    /// Cost center code for accounting integration.
    /// </summary>
    public string? CostCenter { get; private set; }

    /// <summary>
    /// Physical location of this organizational unit.
    /// </summary>
    public string? Location { get; private set; }

    /// <summary>
    /// Whether this organizational unit is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new organizational unit.
    /// </summary>
    public static OrganizationalUnit Create(
        DefaultIdType companyId,
        string code,
        string name,
        OrganizationalUnitType type,
        DefaultIdType? parentId = null,
        DefaultIdType? managerId = null,
        string? costCenter = null,
        string? location = null)
    {
        // Determine level based on type
        int level = type switch
        {
            OrganizationalUnitType.Department => 1,
            OrganizationalUnitType.Division => 2,
            OrganizationalUnitType.Section => 3,
            _ => throw new ArgumentException("Invalid organizational unit type")
        };

        var unit = new OrganizationalUnit(
            DefaultIdType.NewGuid(),
            companyId,
            code,
            name,
            type,
            parentId,
            level)
        {
            ManagerId = managerId,
            CostCenter = costCenter,
            Location = location
        };

        return unit;
    }

    /// <summary>
    /// Updates organizational unit information.
    /// </summary>
    public OrganizationalUnit Update(
        string? name,
        DefaultIdType? managerId,
        string? costCenter,
        string? location)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (ManagerId != managerId)
        {
            ManagerId = managerId;
            isUpdated = true;
        }

        if (!string.Equals(CostCenter, costCenter, StringComparison.OrdinalIgnoreCase))
        {
            CostCenter = costCenter;
            isUpdated = true;
        }

        if (!string.Equals(Location, location, StringComparison.OrdinalIgnoreCase))
        {
            Location = location;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new OrganizationalUnitUpdated { OrganizationalUnit = this });
        }

        return this;
    }

    /// <summary>
    /// Moves this organizational unit to a new parent.
    /// </summary>
    public OrganizationalUnit MoveTo(DefaultIdType? newParentId, int newLevel)
    {
        if (ParentId != newParentId)
        {
            ParentId = newParentId;
            Level = newLevel;
            QueueDomainEvent(new OrganizationalUnitMoved 
            { 
                OrganizationalUnitId = Id,
                NewParentId = newParentId,
                NewLevel = newLevel
            });
        }

        return this;
    }

    /// <summary>
    /// Updates the hierarchy path (called after parent changes).
    /// </summary>
    public void UpdateHierarchyPath(string path)
    {
        HierarchyPath = path;
    }

    /// <summary>
    /// Activates this organizational unit.
    /// </summary>
    public OrganizationalUnit Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new OrganizationalUnitActivated { OrganizationalUnitId = Id });
        }
        return this;
    }

    /// <summary>
    /// Deactivates this organizational unit.
    /// </summary>
    public OrganizationalUnit Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new OrganizationalUnitDeactivated { OrganizationalUnitId = Id });
        }
        return this;
    }
}

/// <summary>
/// Type of organizational unit.
/// </summary>
public enum OrganizationalUnitType
{
    Department = 1,
    Division = 2,
    Section = 3
}
```

---

## 🎯 Validation Rules

```csharp
public class CreateOrganizationalUnitValidator : AbstractValidator<CreateOrganizationalUnitCommand>
{
    public CreateOrganizationalUnitValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(@"^[A-Z0-9-]+$");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Type)
            .IsInEnum();

        // Department cannot have parent
        RuleFor(x => x.ParentId)
            .Null()
            .When(x => x.Type == OrganizationalUnitType.Department)
            .WithMessage("Department cannot have a parent");

        // Division must have Department parent
        RuleFor(x => x.ParentId)
            .NotNull()
            .When(x => x.Type == OrganizationalUnitType.Division)
            .WithMessage("Division must have a Department parent");

        // Section must have Division parent
        RuleFor(x => x.ParentId)
            .NotNull()
            .When(x => x.Type == OrganizationalUnitType.Section)
            .WithMessage("Section must have a Division parent");
        
        // Additional validation: Section parent must be Division type
        RuleFor(x => x)
            .MustAsync(async (command, cancellation) => 
            {
                if (command.Type != OrganizationalUnitType.Section || !command.ParentId.HasValue)
                    return true;
                    
                var parent = await repository.GetByIdAsync(command.ParentId.Value, cancellation);
                return parent?.Type == OrganizationalUnitType.Division;
            })
            .When(x => x.Type == OrganizationalUnitType.Section)
            .WithMessage("Section parent must be a Division");

        RuleFor(x => x.CostCenter)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.CostCenter));

        RuleFor(x => x.Location)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Location));
    }
}
```

---

## 🎨 Benefits of This Design

### ✅ Flexibility
```
✓ Supports Department only (employees can be directly under department)
✓ Supports Department → Division (employees can be under division)
✓ Supports Department → Division → Section (full hierarchy)
✓ Supports multiple Sections under one Division
✓ Enforces business rule: Sections always under Divisions
✓ Easy to add more levels if needed
```

### ✅ Simplicity
```
✓ Single entity instead of 3
✓ Consistent API for all levels
✓ No complex joins
✓ Easy to understand
```

### ✅ Query Performance
```
✓ HierarchyPath for fast lookups
✓ Level field for filtering
✓ Indexed parent relationships
✓ Materialized paths
```

### ✅ Reporting
```
✓ Easy org charts
✓ Hierarchy reports
✓ Roll-up calculations
✓ Cost center analysis
```

---

## 📊 Example Organizational Structure

```
Sample Electric Cooperative
├── HR Department (HR-001)
│   ├── Recruitment Division (HR-REC-001)
│   │   ├── Sourcing Section (HR-REC-SRC-001)
│   │   └── Screening Section (HR-REC-SCR-001)
│   └── Training Division (HR-TRN-001)
│       ├── Onboarding Section (HR-TRN-ONB-001)
│       └── Development Section (HR-TRN-DEV-001)
│
├── Operations Department (OPS-001)
│   ├── Distribution Division (OPS-DIST-001)
│   │   ├── Line Maintenance Section (OPS-DIST-LINE-001)
│   │   ├── Metering Section (OPS-DIST-METER-001)
│   │   └── Transformer Section (OPS-DIST-TRANS-001)
│   ├── Generation Division (OPS-GEN-001)
│   │   ├── Power Plant Section (OPS-GEN-PLANT-001)
│   │   └── Maintenance Section (OPS-GEN-MAINT-001)
│   └── Substation Division (OPS-SUB-001)
│       └── Substation Maintenance Section (OPS-SUB-MAINT-001)
│
└── Finance Department (FIN-001)
    ├── Accounting Division (FIN-ACC-001)
    │   ├── General Ledger Section (FIN-ACC-GL-001)
    │   └── Accounts Payable Section (FIN-ACC-AP-001)
    └── Collections Division (FIN-COLL-001)
        ├── Residential Collections Section (FIN-COLL-RES-001)
        └── Commercial Collections Section (FIN-COLL-COM-001)
```

---

## 🚀 Implementation Plan

### Phase 1: Create OrganizationalUnit Entity (4 hours)
```
✓ Create domain entity
✓ Create events
✓ Create exceptions
✓ Add validation rules
```

### Phase 2: Create CQRS Operations (4 hours)
```
✓ Create command/handler
✓ Get by ID query
✓ Search with hierarchy
✓ Update command
✓ Move command
```

### Phase 3: Infrastructure (3 hours)
```
✓ EF Core configuration
✓ Indexes and constraints
✓ Repository
✓ Endpoints
```

### Phase 4: Update Employee (1 hour)
```
✓ Add OrganizationalUnitId to Employee
✓ Update Employee create/update
✓ Add navigation properties
```

**Total Time:** 12 hours (1.5 days)

---

## ✅ Summary

### Recommended Approach: ✅ **Single OrganizationalUnit Entity**

**Why?**
- ✅ Maximum flexibility
- ✅ Simpler codebase (1 entity vs 3)
- ✅ Easy to query hierarchy
- ✅ Supports all scenarios
- ✅ Easy to extend (add more levels)
- ✅ Better performance (fewer joins)

**Employee Assignment:**
- Employee → OrganizationalUnit (most specific level)
- Hierarchy is derived from Parent relationships
- Can query "all employees in Department" including children
- Can query "employees in Section only"

**Next Steps:**
1. Create `OrganizationalUnit` entity
2. Add `OrganizationalUnitId` to `Employee`
3. Remove planned `Department` and `Position` entities
4. Implement CQRS operations
5. Add hierarchy query helpers

This design will serve your Electric Cooperative needs perfectly! 🎉

