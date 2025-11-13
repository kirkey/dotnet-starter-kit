# ✅ Position Design Decision - Area-Specific Implementation

**Date:** November 13, 2025  
**Decision:** ✅ **Position MUST be linked to OrganizationalUnit (Area)**  
**Reason:** Support area-specific job roles with identical titles

---

## 🎯 The Problem You Described

> "There are many positions that under difference departments. For example, area1, area2, area3 and so on are departments. Each of them has their own position names that are identical to other areas."

### Your Exact Scenario
```
Area 1 Department
  ├── Supervisor (Area 1's Supervisor position)
  ├── Technician (Area 1's Technician position)
  └── Helper (Area 1's Helper position)

Area 2 Department
  ├── Supervisor (Area 2's Supervisor position) ← Same title!
  ├── Technician (Area 2's Technician position) ← Same title!
  └── Helper (Area 2's Helper position) ← Same title!

Area 3 Department
  ├── Supervisor (Area 3's Supervisor position)
  ├── Technician (Area 3's Technician position)
  └── Helper (Area 3's Helper position)
```

---

## ✅ The Solution

### Position Design Decision

**Position MUST have:**
```csharp
public DefaultIdType OrganizationalUnitId { get; private set; }
public OrganizationalUnit OrganizationalUnit { get; private set; }
```

**NOT:**
```csharp
// ❌ WRONG - Position at Company level
public DefaultIdType CompanyId { get; private set; }
```

### Why?
1. **Different positions** - Same title, different area
2. **Different salaries** - Area 1 $40K, Area 2 $42K
3. **Different descriptions** - Area-specific job responsibilities
4. **Area management** - Area managers control their own positions
5. **Reporting** - Easy to filter by area

---

## 📊 Database Design

### Unique Constraint Per Area
```sql
CONSTRAINT IX_Positions_Code_OrgUnit 
    UNIQUE (TenantId, OrganizationalUnitId, Code)
```

**Allows:**
```
OrganizationalUnit: Area 1, Code: "SUP-001" ✅
OrganizationalUnit: Area 2, Code: "SUP-001" ✅
OrganizationalUnit: Area 3, Code: "SUP-001" ✅

All different Position records!
```

---

## 🎯 Example

### Setup
```csharp
// Create Areas
var area1 = OrganizationalUnit.Create(..., "AREA-001", "Area 1");
var area2 = OrganizationalUnit.Create(..., "AREA-002", "Area 2");

// Create Positions in Area 1
var area1_supervisor = Position.Create(
    code: "SUP-001",
    title: "Supervisor",
    organizationalUnitId: area1.Id,  // ← Linked to Area 1
    minSalary: 40000,
    maxSalary: 55000);

// Create Positions in Area 2 (Same code, same title, different position)
var area2_supervisor = Position.Create(
    code: "SUP-001",  // ← Same code
    title: "Supervisor",  // ← Same title
    organizationalUnitId: area2.Id,  // ← But linked to Area 2
    minSalary: 42000,  // ← Different salary
    maxSalary: 58000);
```

---

## 🔑 Key Benefits

| Benefit | Why |
|---------|-----|
| **Area Control** | Area managers manage their own positions |
| **Flexible Salary** | Each area has different pay rates |
| **Flexible Description** | Each area has different job requirements |
| **Unique Codes** | Code "SUP-001" can exist in all areas |
| **Easy Queries** | Filter by area to get area's positions |
| **Scalable** | Add new areas without changes |
| **Reporting** | Report positions per area |

---

## ✅ Implementation Status

### Currently Complete
- ✅ Company entity (Full CRUD)
- ✅ OrganizationalUnit (Department/Division/Section, Full CRUD)

### To Be Created
- ⏳ Position entity (Area-specific, 6-7 hours)
- ⏳ Employee entity (Links to Area + Position)
- ⏳ 23 other entities for payroll, attendance, etc.

---

## 📋 Architecture Diagram

```
Company (ELECTRIC COOPERATIVE)
│
├── OrganizationalUnit (Area 1)
│   ├── Position: Supervisor (Pos1, $40K-$55K)
│   ├── Position: Technician (Pos2, $30K-$42K)
│   └── Employee: John (Area1, Pos1)
│
├── OrganizationalUnit (Area 2)
│   ├── Position: Supervisor (Pos4, $42K-$58K) ← Different position!
│   ├── Position: Technician (Pos5, $31K-$43K)
│   └── Employee: Jane (Area2, Pos4)
│
└── OrganizationalUnit (Area 3)
    ├── Position: Supervisor (Pos7, $41K-$56K) ← Different position!
    ├── Position: Technician (Pos8, $30.5K-$42.5K)
    └── Employee: Mike (Area3, Pos7)
```

---

## 🎉 Summary

**Position entity design:**
- ✅ Linked to OrganizationalUnit (Area)
- ✅ Same title allowed across areas
- ✅ Different salary ranges per area
- ✅ Different descriptions per area
- ✅ Unique constraint: Code per Area
- ✅ Perfect for your Electric Cooperative

**This is the CORRECT design for your scenario!** 🎯

