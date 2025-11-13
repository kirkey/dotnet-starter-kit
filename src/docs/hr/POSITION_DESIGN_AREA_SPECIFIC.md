# ✅ Position Design for Multi-Area Organization

**Date:** November 13, 2025  
**Scenario:** Areas (Departments) with Area-Specific Positions  
**Status:** Design & Implementation Plan

---

## 🎯 Your Organizational Structure

### Current Reality
```
Company
├── Area 1 (Department)
│   ├── Supervisor (Position)
│   ├── Technician (Position)
│   └── Helper (Position)
│
├── Area 2 (Department)
│   ├── Supervisor (Position)  ← Same title, different department
│   ├── Technician (Position)  ← Same title, different department
│   └── Helper (Position)      ← Same title, different department
│
└── Area 3 (Department)
    ├── Supervisor (Position)  ← Same title, different department
    ├── Technician (Position)  ← Same title, different department
    └── Helper (Position)      ← Same title, different department
```

### Key Insight
**Positions are NOT company-wide - they are department/area-specific!**

---

## 🎨 Correct Design Pattern

### Position Entity (Area-Specific)
```csharp
public class Position : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique code for this position within the organization.
    /// Example: "SUPERVISOR-001", "TECH-001"
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// Position title.
    /// Example: "Area Supervisor", "Line Technician", "Field Helper"
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// The organizational unit (Department/Division/Section) this position belongs to.
    /// Same title can exist in different areas with different position records.
    /// </summary>
    public DefaultIdType OrganizationalUnitId { get; private set; }
    public OrganizationalUnit OrganizationalUnit { get; private set; } = default!;

    /// <summary>
    /// Detailed job description for this position in this area.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Minimum salary for this position in this area.
    /// Can vary by area due to cost of living differences.
    /// </summary>
    public decimal? MinSalary { get; private set; }

    /// <summary>
    /// Maximum salary for this position in this area.
    /// </summary>
    public decimal? MaxSalary { get; private set; }

    /// <summary>
    /// Whether this position is currently active/available.
    /// </summary>
    public bool IsActive { get; private set; }
}
```

---

## 📊 Database Schema

### Position Table
```sql
CREATE TABLE hr.Positions (
    Id uniqueidentifier PRIMARY KEY,
    TenantId nvarchar(64) NOT NULL,
    
    -- Core
    Code nvarchar(50) NOT NULL,
    Title nvarchar(256) NOT NULL,
    
    -- Organization Link (THIS IS KEY!)
    OrganizationalUnitId uniqueidentifier NOT NULL,
    
    -- Details
    Description nvarchar(2000),
    MinSalary decimal(16,2),
    MaxSalary decimal(16,2),
    IsActive bit NOT NULL DEFAULT 1,
    
    -- Audit
    CreatedBy nvarchar(256),
    CreatedOn datetimeoffset NOT NULL,
    LastModifiedBy nvarchar(256),
    LastModifiedOn datetimeoffset,
    
    -- Constraints
    CONSTRAINT FK_Positions_OrganizationalUnit 
        FOREIGN KEY (OrganizationalUnitId) REFERENCES hr.OrganizationalUnits(Id),
    CONSTRAINT IX_Positions_Code_OrgUnit 
        UNIQUE (TenantId, OrganizationalUnitId, Code)  -- Code unique per area
);

-- Indexes
CREATE INDEX IX_Positions_OrganizationalUnitId ON hr.Positions(OrganizationalUnitId);
CREATE INDEX IX_Positions_IsActive ON hr.Positions(IsActive);
```

---

## 💡 Key Design Decisions

### 1. Position Code is Unique Per Area (OrganizationalUnit)
```sql
CONSTRAINT IX_Positions_Code_OrgUnit 
    UNIQUE (TenantId, OrganizationalUnitId, Code)
```

**Allows:**
```
Area1
├── SUPERVISOR-001 ✅ (Area1's Supervisor position)
└── TECH-001 ✅ (Area1's Technician position)

Area2
├── SUPERVISOR-001 ✅ (Area2's Supervisor position) - Different record!
└── TECH-001 ✅ (Area2's Technician position) - Different record!
```

### 2. Same Title, Different Positions
```
Position 1: Area1 - Supervisor (OrganizationalUnitId = Area1)
Position 2: Area2 - Supervisor (OrganizationalUnitId = Area2)
Position 3: Area3 - Supervisor (OrganizationalUnitId = Area3)

All have title "Supervisor" but:
- Different Position Ids
- Different salary ranges per area
- Different descriptions per area
- Different job requirements per area
```

### 3. Salary Variation by Area
```csharp
var area1Supervisor = new Position
{
    Title = "Supervisor",
    OrganizationalUnitId = area1.Id,
    MinSalary = 40000,  // Area1 rate
    MaxSalary = 55000
};

var area2Supervisor = new Position
{
    Title = "Supervisor",
    OrganizationalUnitId = area2.Id,
    MinSalary = 42000,  // Area2 rate (higher cost of living)
    MaxSalary = 58000
};
```

---

## 🔄 Complete Flow

### Setup: Create Positions for Each Area
```csharp
// Create Area 1
var area1 = OrganizationalUnit.Create(
    companyId, "AREA-001", "Area 1", 
    OrganizationalUnitType.Department);

// Create Position in Area 1
var supervisorPos1 = Position.Create(
    code: "SUP-001",
    title: "Supervisor",
    organizationalUnitId: area1.Id,
    description: "Supervises field operations in Area 1",
    minSalary: 40000,
    maxSalary: 55000);

var techPos1 = Position.Create(
    code: "TECH-001",
    title: "Technician",
    organizationalUnitId: area1.Id,
    description: "Installs and maintains equipment in Area 1",
    minSalary: 30000,
    maxSalary: 42000);

// Create Area 2
var area2 = OrganizationalUnit.Create(
    companyId, "AREA-002", "Area 2", 
    OrganizationalUnitType.Department);

// Create Position in Area 2 (Same titles!)
var supervisorPos2 = Position.Create(
    code: "SUP-001",  // ✅ Can be same code in different area
    title: "Supervisor",  // ✅ Can be same title in different area
    organizationalUnitId: area2.Id,
    description: "Supervises field operations in Area 2",
    minSalary: 42000,  // ✅ Different salary for different area
    maxSalary: 58000);

var techPos2 = Position.Create(
    code: "TECH-001",  // ✅ Can be same code in different area
    title: "Technician",  // ✅ Can be same title in different area
    organizationalUnitId: area2.Id,
    description: "Installs and maintains equipment in Area 2",
    minSalary: 31000,  // ✅ Different salary for different area
    maxSalary: 43500);
```

### Assign Employee to Position in Area
```csharp
var employee = Employee.Create(
    employeeNumber: "EMP-001",
    name: "John Doe",
    companyId: companyId,
    organizationalUnitId: area1.Id,  // Assigned to Area 1
    positionId: supervisorPos1.Id);  // Supervisor position in Area 1
```

### Query Positions by Area
```csharp
// Get all positions in Area 1
var area1Positions = await repository.ListAsync(
    new PositionsByOrganizationalUnitSpec(area1.Id));

// Get Supervisor positions across all areas
var allSupervisors = await repository.ListAsync(
    new PositionsByTitleSpec("Supervisor"));

// Get positions with salary range
var highPayPositions = await repository.ListAsync(
    new PositionsBySalaryRangeSpec(minAmount: 50000, maxAmount: 100000));
```

---

## 🎯 Complete Position Entity

```csharp
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using FSH.Starter.WebApi.HumanResources.Domain.Events;

namespace FSH.Starter.WebApi.HumanResources.Domain;

/// <summary>
/// Represents a job position within a specific organizational unit (area/department).
/// </summary>
/// <remarks>
/// Key Design Points:
/// - Positions are organizational unit specific (different per area)
/// - Same position title can exist in multiple areas with different position records
/// - Each position can have different salary ranges per area
/// - Unique constraint: Code per Area (OrganizationalUnit) per Tenant
/// 
/// Example:
/// - Area 1: Position "Supervisor" with MinSalary 40K-55K
/// - Area 2: Position "Supervisor" with MinSalary 42K-58K
/// Both have same title but different Position entities
/// </remarks>
public class Position : AuditableEntity, IAggregateRoot
{
    private Position() { }

    private Position(
        DefaultIdType id,
        string code,
        string title,
        DefaultIdType organizationalUnitId,
        string? description)
    {
        Id = id;
        Code = code;
        Title = title;
        OrganizationalUnitId = organizationalUnitId;
        Description = description;
        IsActive = true;

        QueueDomainEvent(new PositionCreated { Position = this });
    }

    /// <summary>
    /// Unique code for this position within the organizational unit.
    /// Example: "SUP-001", "TECH-001"
    /// </summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Position title/job title.
    /// Example: "Area Supervisor", "Line Technician", "Field Helper"
    /// Same title can exist in different areas.
    /// </summary>
    public string Title { get; private set; } = default!;

    /// <summary>
    /// The organizational unit (Area/Department/Division/Section) this position belongs to.
    /// Foreign key to OrganizationalUnit.
    /// 
    /// This is the KEY difference:
    /// - Same title "Supervisor" can exist in Area1, Area2, Area3
    /// - Each is a separate Position record with different salary ranges
    /// - Linked to different OrganizationalUnit
    /// </summary>
    public DefaultIdType OrganizationalUnitId { get; private set; }
    public OrganizationalUnit OrganizationalUnit { get; private set; } = default!;

    /// <summary>
    /// Detailed job description for this position in this organizational unit.
    /// Can vary per area.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Minimum salary for this position in this area.
    /// Can vary by area due to cost of living, location, or market differences.
    /// </summary>
    public decimal? MinSalary { get; private set; }

    /// <summary>
    /// Maximum salary for this position in this area.
    /// </summary>
    public decimal? MaxSalary { get; private set; }

    /// <summary>
    /// Whether this position is currently active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new position in a specific organizational unit.
    /// </summary>
    public static Position Create(
        string code,
        string title,
        DefaultIdType organizationalUnitId,
        string? description = null,
        decimal? minSalary = null,
        decimal? maxSalary = null)
    {
        var position = new Position(
            DefaultIdType.NewGuid(),
            code,
            title,
            organizationalUnitId,
            description)
        {
            MinSalary = minSalary,
            MaxSalary = maxSalary
        };

        return position;
    }

    /// <summary>
    /// Updates position information.
    /// </summary>
    public Position Update(
        string? title,
        string? description,
        decimal? minSalary,
        decimal? maxSalary)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(title) && !string.Equals(Title, title, StringComparison.OrdinalIgnoreCase))
        {
            Title = title;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (MinSalary != minSalary)
        {
            MinSalary = minSalary;
            isUpdated = true;
        }

        if (MaxSalary != maxSalary)
        {
            MaxSalary = maxSalary;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new PositionUpdated { Position = this });
        }

        return this;
    }

    /// <summary>
    /// Activates this position.
    /// </summary>
    public Position Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new PositionActivated { PositionId = Id });
        }
        return this;
    }

    /// <summary>
    /// Deactivates this position.
    /// </summary>
    public Position Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new PositionDeactivated { PositionId = Id });
        }
        return this;
    }
}
```

---

## 📋 Position Events

```csharp
namespace FSH.Starter.WebApi.HumanResources.Domain.Events;

/// <summary>
/// Event raised when a position is created.
/// </summary>
public record PositionCreated : DomainEvent
{
    public Position Position { get; init; } = default!;
}

/// <summary>
/// Event raised when a position is updated.
/// </summary>
public record PositionUpdated : DomainEvent
{
    public Position Position { get; init; } = default!;
}

/// <summary>
/// Event raised when a position is activated.
/// </summary>
public record PositionActivated : DomainEvent
{
    public DefaultIdType PositionId { get; init; }
}

/// <summary>
/// Event raised when a position is deactivated.
/// </summary>
public record PositionDeactivated : DomainEvent
{
    public DefaultIdType PositionId { get; init; }
}
```

---

## 🎯 Key Advantages of This Design

### ✅ Supports Your Exact Scenario
```
Area 1 → Supervisor (Position 1) - MinSalary: 40K
Area 2 → Supervisor (Position 2) - MinSalary: 42K
Area 3 → Supervisor (Position 3) - MinSalary: 41K

All have same title "Supervisor" but different Position records!
```

### ✅ Area-Specific Salary Management
```
Same job title, different pay per area
- Area 1 (Low cost): $40K-$55K
- Area 2 (Medium cost): $42K-$58K
- Area 3 (High cost): $44K-$60K
```

### ✅ Area-Specific Job Descriptions
```
Supervisor in Area 1:
  "Manages field operations team in northern region"

Supervisor in Area 2:
  "Manages field operations team in central region"
```

### ✅ Flexible Querying
```csharp
// Get all supervisors across all areas
var supervisors = await repo.ListAsync(
    new PositionsByTitleSpec("Supervisor"));

// Get all positions in Area 1
var area1Positions = await repo.ListAsync(
    new PositionsByOrganizationalUnitSpec(area1.Id));

// Get positions with salary range
var midSalaryPositions = await repo.ListAsync(
    new PositionsBySalaryRangeSpec(30000, 50000));
```

### ✅ Employee Assignment
```csharp
var emp1 = Employee.Create(
    "EMP-001",
    "John Doe",
    area1.Id,           // Assigned to Area 1
    supervisorPos1.Id); // Supervisor position in Area 1

var emp2 = Employee.Create(
    "EMP-002",
    "Jane Smith",
    area2.Id,           // Assigned to Area 2
    supervisorPos2.Id); // Supervisor position in Area 2 (Same title, different position)
```

---

## 🎉 Summary

**Position Design is CORRECT when:**
- ✅ Linked to OrganizationalUnit (not just Company)
- ✅ Same title can exist in multiple areas
- ✅ Each area gets its own Position record
- ✅ Salary can vary per area
- ✅ Description can vary per area
- ✅ Unique constraint: Code per Area

**This supports your Electric Cooperative perfectly:**
- Area 1 with its own Supervisor, Technician, Helper positions
- Area 2 with its own Supervisor, Technician, Helper positions
- Area 3 with its own Supervisor, Technician, Helper positions
- All can have different salary ranges
- All linked to their respective areas

