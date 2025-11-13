# ✅ Position Implementation Plan - Area-Specific Roles

**Date:** November 13, 2025  
**Scope:** Create Position entity linked to OrganizationalUnit  
**Complexity:** Medium (Similar to OrganizationalUnit)  

---

## 🎯 Position Implementation Overview

Position is NOT a company-wide entity. It is **area-specific** (OrganizationalUnit-specific).

### Key Difference
```
❌ WRONG: Position at Company level
   Company → Position (Supervisor, Technician, etc.)

✅ CORRECT: Position per Area
   Company → Area 1 → Positions (Supervisor, Technician, etc.)
   Company → Area 2 → Positions (Supervisor, Technician, etc.)
```

---

## 📋 Implementation Checklist

### Phase 1: Domain Layer (2-3 hours)

| Component | Status | Details |
|-----------|--------|---------|
| Position.cs | ⏳ TODO | Core entity with OrganizationalUnitId FK |
| PositionEvents.cs | ⏳ TODO | PositionCreated, PositionUpdated, etc. |
| PositionExceptions.cs | ⏳ TODO | PositionNotFoundException, etc. |
| GlobalUsings.cs | ✅ EXISTING | Already exists |

**Files to Create: 3**

### Phase 2: Application Layer (4-5 hours)

| Operation | Command | Handler | Validator | Response | Specs |
|-----------|---------|---------|-----------|----------|-------|
| **Create** | ✅ | ✅ | ✅ | ✅ | - |
| **Get** | ✅ | ✅ | - | ✅ | ✅ |
| **Search** | ✅ | ✅ | - | ✅ | ✅ |
| **Update** | ✅ | ✅ | ✅ | ✅ | - |
| **Delete** | ✅ | ✅ | - | ✅ | - |

**Files to Create:**
- Create: 4 files (Command, Handler, Validator, Response)
- Get: 3 files (Request, Handler, Response)
- Search: 2 files (Request, Handler)
- Update: 4 files (Command, Handler, Validator, Response)
- Delete: 3 files (Command, Handler, Response)
- Specs: 4 files (ById, ByOrgUnit, ByTitle, BySalaryRange)

**Total: 20 files**

### Phase 3: Infrastructure Layer (3-4 hours)

| Component | Status | Details |
|-----------|--------|---------|
| Endpoints (5) | ✅ | Create, Get, Search, Update, Delete |
| DbContext | ✅ | Add DbSet<Designation> |
| Configuration | ✅ | EF Core mapping |
| Repository | ✅ | Generic repository (already exists) |
| Initializer | ✅ | Seed data |
| Module | ✅ | Register services and endpoints |

**Files to Create: 6**

---

## 🔑 Critical Design Points

### 1. Foreign Key to OrganizationalUnit
```csharp
public DefaultIdType OrganizationalUnitId { get; private set; }
public OrganizationalUnit OrganizationalUnit { get; private set; } = default!;
```

### 2. Unique Code Per Area (Constraint)
```sql
CONSTRAINT IX_Positions_Code_OrgUnit 
    UNIQUE (TenantId, OrganizationalUnitId, Code)
```

### 3. Salary Range Per Area
```csharp
public decimal? MinSalary { get; private set; }
public decimal? MaxSalary { get; private set; }
```

### 4. Same Title, Different Positions
```
Position 1: Area 1 - "Supervisor" (MinSalary: 40K)
Position 2: Area 2 - "Supervisor" (MinSalary: 42K)
Position 3: Area 3 - "Supervisor" (MinSalary: 41K)
```

---

## 📊 Database Schema

```sql
CREATE TABLE hr.Positions (
    Id uniqueidentifier PRIMARY KEY,
    TenantId nvarchar(64) NOT NULL,
    
    -- Core
    Code nvarchar(50) NOT NULL,
    Name nvarchar(256) NOT NULL,
    
    -- Organization Link (KEY!)
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
        FOREIGN KEY (OrganizationalUnitId) 
        REFERENCES hr.OrganizationalUnits(Id),
    CONSTRAINT IX_Positions_Code_OrgUnit 
        UNIQUE (TenantId, OrganizationalUnitId, Code)
);

-- Indexes
CREATE INDEX IX_Positions_OrganizationalUnitId ON hr.Positions(OrganizationalUnitId);
CREATE INDEX IX_Positions_IsActive ON hr.Positions(IsActive);
```

---

## 🎯 Implementation Steps

### Step 1: Domain Layer (30 minutes)

**Create Position.cs:**
```csharp
public class Position : AuditableEntity, IAggregateRoot
{
    public string Code { get; private set; }
    public string Title { get; private set; }
    public DefaultIdType OrganizationalUnitId { get; private set; }
    public OrganizationalUnit OrganizationalUnit { get; private set; }
    public string? Description { get; private set; }
    public decimal? MinSalary { get; private set; }
    public decimal? MaxSalary { get; private set; }
    public bool IsActive { get; private set; }

    public static Position Create(...) { }
    public Position Update(...) { }
    public Position Activate() { }
    public Position Deactivate() { }
}
```

**Create PositionEvents.cs:**
```csharp
public record PositionCreated : DomainEvent { }
public record PositionUpdated : DomainEvent { }
public record PositionActivated : DomainEvent { }
public record PositionDeactivated : DomainEvent { }
```

**Create PositionExceptions.cs:**
```csharp
public class PositionNotFoundException : NotFoundException { }
public class PositionCodeAlreadyExistsException : ConflictException { }
```

### Step 2: Application Layer (2-3 hours)

Create folders:
```
Positions/
├── Create/v1/ (4 files)
├── Get/v1/ (3 files)
├── Search/v1/ (2 files)
├── Update/v1/ (4 files)
├── Delete/v1/ (3 files)
└── Specifications/ (4 files)
```

Follow OrganizationalUnit pattern exactly.

### Step 3: Infrastructure Layer (1-2 hours)

**Add to DbContext:**
```csharp
public DbSet<Designation> Positions { get; set; } = null!;
```

**Create Configuration:**
```csharp
public class PositionConfiguration : IEntityTypeConfiguration<Designation>
{
    public void Configure(EntityTypeBuilder<Designation> builder)
    {
        builder.ToTable("Positions", SchemaNames.HumanResources);
        builder.HasKey(p => p.Id);
        
        builder.HasIndex(p => new { p.OrganizationalUnitId, p.Code })
            .IsUnique();
        
        // ... other configurations
    }
}
```

**Create 5 Endpoints:**
- CreatePositionEndpoint
- GetPositionEndpoint
- SearchPositionsEndpoint
- UpdatePositionEndpoint
- DeletePositionEndpoint

**Update Module:**
```csharp
var positionGroup = app.MapGroup("positions").WithTags("positions");
positionGroup.MapPositionCreateEndpoint();
positionGroup.MapPositionGetEndpoint();
positionGroup.MapPositionsSearchEndpoint();
positionGroup.MapPositionUpdateEndpoint();
positionGroup.MapPositionDeleteEndpoint();
```

---

## 🎓 Learning Points

### Copy OrganizationalUnit Pattern
Position implementation is almost identical to OrganizationalUnit:
- ✅ Same CQRS structure
- ✅ Same 5 endpoints
- ✅ Same validation pattern
- ✅ Same exception pattern
- ✅ Same event pattern
- ✅ Same specification pattern

**Difference:** Position has FK to OrganizationalUnit instead of being hierarchical.

### Follow This Exact Pattern
```
OrganizationalUnit (Self-hierarchical)
└── Position (Many per unit, same titles across units)
    └── Employee (One position per employee)
```

---

## 📊 Estimated Timeline

| Phase | Time | Notes |
|-------|------|-------|
| **Domain Layer** | 30 min | 3 files (Entity, Events, Exceptions) |
| **Application Layer** | 3 hours | 20 files (Copy OrganizationalUnit pattern) |
| **Infrastructure** | 2 hours | DbContext, Config, Endpoints, Module |
| **Build & Test** | 1 hour | Verify compilation |
| **TOTAL** | ~6-7 hours | Can be done in 1 day |

---

## ✅ Files to Create (29 Total)

### Domain (3 files)
- Position.cs
- Exceptions/PositionExceptions.cs
- Events/PositionEvents.cs (or add to CompanyEvents.cs)

### Application (20 files)
- Create/v1/CreatePositionCommand.cs
- Create/v1/CreatePositionValidator.cs
- Create/v1/CreatePositionHandler.cs
- Create/v1/CreatePositionResponse.cs
- Get/v1/GetPositionRequest.cs
- Get/v1/GetPositionHandler.cs
- Get/v1/PositionResponse.cs
- Search/v1/SearchPositionsRequest.cs
- Search/v1/SearchPositionsHandler.cs
- Update/v1/UpdatePositionCommand.cs
- Update/v1/UpdatePositionValidator.cs
- Update/v1/UpdatePositionHandler.cs
- Update/v1/UpdatePositionResponse.cs
- Delete/v1/DeletePositionCommand.cs
- Delete/v1/DeletePositionHandler.cs
- Delete/v1/DeletePositionResponse.cs
- Specifications/PositionByIdSpec.cs
- Specifications/PositionByOrganizationalUnitSpec.cs
- Specifications/PositionByTitleSpec.cs
- Specifications/SearchPositionsSpec.cs

### Infrastructure (6 files)
- Endpoints/v1/CreatePositionEndpoint.cs
- Endpoints/v1/GetPositionEndpoint.cs
- Endpoints/v1/SearchPositionsEndpoint.cs
- Endpoints/v1/UpdatePositionEndpoint.cs
- Endpoints/v1/DeletePositionEndpoint.cs
- Persistence/Configurations/PositionConfiguration.cs

### Updates (2 files)
- HumanResourcesDbContext.cs (Add DbSet)
- HumanResourcesModule.cs (Register endpoints)

---

## 🎉 Final Result

After implementation, you'll have:

✅ **Position entity** - Area-specific job roles  
✅ **Full CQRS** - Create, Get, Search, Update, Delete  
✅ **Area-linked** - Each position belongs to one area  
✅ **Flexible salary** - Different salaries per area  
✅ **Flexible description** - Different descriptions per area  
✅ **5 endpoints** - REST API for all operations  
✅ **4 specifications** - Efficient querying  

---

**Ready to implement? Follow OrganizationalUnit pattern exactly!** 🚀

