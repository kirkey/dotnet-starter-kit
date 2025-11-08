# EF Configuration Fixes for Approval Workflow

## Overview
Fixed Entity Framework Core configuration files to match the updated entity properties after implementing `AuditableEntityWithApproval` base class.

---

## Files Fixed

### 1. **BillConfiguration.cs**
**Problem:** Referenced `ApprovalStatus`, `ApprovedBy`, and `ApprovedDate` properties that no longer exist on Bill entity.

**Changes:**
- ❌ Removed `ApprovalStatus` property configuration
- ❌ Removed `ApprovedBy` property configuration (now in base class)
- ❌ Removed `ApprovedDate` property configuration
- ❌ Removed `IX_Bills_ApprovalStatus` index
- ❌ Removed `IX_Bills_ApprovalStatus_BillDate` composite index
- ✅ Kept `Status` property (from base class)
- ✅ Kept `IX_Bills_Status` index

**Reason:** Bill now inherits from `AuditableEntityWithApproval` which provides:
- `Status` (replaces `ApprovalStatus`)
- `ApprovedBy` (Guid type, not string)
- `ApproverName` (string, for display)
- `ApprovedOn` (replaces `ApprovedDate`)

---

### 2. **JournalEntryConfiguration.cs**
**Problem:** Referenced `ApprovalStatus`, `ApprovedBy`, and `ApprovedDate` properties that no longer exist on JournalEntry entity.

**Changes:**
- ❌ Removed `ApprovalStatus` property configuration
- ❌ Removed `ApprovedBy` property configuration
- ❌ Removed `ApprovedDate` property configuration
- ❌ Removed `IX_JournalEntries_ApprovalStatus` index
- ❌ Removed `IX_JournalEntries_ApprovedBy` index
- ❌ Removed `IX_JournalEntries_ApprovedDate` index
- ❌ Removed `IX_JournalEntries_ApprovalStatus_Date` composite index
- ✅ Added `IX_JournalEntries_Status` index
- ✅ Added `IX_JournalEntries_Status_Date` composite index

**Reason:** JournalEntry now inherits from `AuditableEntityWithApproval`.

---

### 3. **FiscalPeriodCloseConfiguration.cs**
**Problem:** Referenced `InitiatedDate` property that doesn't exist, and `IsCompleted` on CloseTaskItem which should be `IsComplete`.

**Changes:**
- ❌ Removed `InitiatedDate` references
- ✅ Changed to `CloseInitiatedDate` (correct property name)
- ✅ Updated index names to match
- ✅ Updated composite index from `Status_InitiatedDate` to `Status_CloseInitiatedDate`
- ✅ Fixed `IsCompleted` to `IsComplete` in CloseTaskItem owned entity
- ✅ Renamed lambda parameter in OwnsMany to avoid confusion

**Reason:** Entity property names didn't match what was configured.

---

## Property Mapping Changes

### Bill & JournalEntry (AuditableEntityWithApproval)

| Old Property | New Property | Type | Notes |
|-------------|--------------|------|-------|
| `ApprovalStatus` | `Status` | string | Base class property |
| `ApprovedBy` (string) | `ApprovedBy` | Guid? | Base class, now stores user ID |
| - | `ApproverName` | string? | New - stores display name |
| `ApprovedDate` | `ApprovedOn` | DateTime? | Base class property |

### Additional Base Class Properties Not Configured
These are automatically handled by EF conventions:
- `Request` (string?) - Approval request payload
- `Feedback` (string?) - Short feedback
- `Remarks` (string?) - Detailed remarks
- `PreparedBy` (Guid?) - Who prepared
- `PreparerName` (string?) - Preparer display name  
- `PreparedOn` (DateTime?) - When prepared
- `ReviewedBy` (Guid?) - Who reviewed
- `ReviewerName` (string?) - Reviewer display name
- `ReviewedOn` (DateTime?) - When reviewed
- `RecommendedBy` (Guid?) - Who recommended
- `RecommenderName` (string?) - Recommender display name
- `RecommendedOn` (DateTime?) - When recommended

---

## Index Strategy

### Removed Indexes
- Indexes on deprecated properties (`ApprovalStatus`, `ApprovedDate`)
- Redundant composite indexes

### Kept/Added Indexes
- `IX_{Entity}_Status` - Single column index on Status
- `IX_{Entity}_Status_{Date}` - Composite indexes for common queries
- All other existing indexes remain unchanged

---

## Migration Impact

When generating migrations, these changes will:

1. **Drop columns:**
   - `ApprovalStatus` (Bills, JournalEntries)
   - `ApprovedDate` (Bills, JournalEntries)
   - `InitiatedDate` (FiscalPeriodCloses) - never existed

2. **Add columns:**
   - `ApprovedOn` DateTime (from base class)
   - `ApproverName` VARCHAR
   - `PreparedBy`, `PreparerName`, `PreparedOn`
   - `ReviewedBy`, `ReviewerName`, `ReviewedOn`
   - `RecommendedBy`, `RecommenderName`, `RecommendedOn`
   - `Request`, `Feedback`, `Remarks`

3. **Modify columns:**
   - `ApprovedBy` changes from VARCHAR to UNIQUEIDENTIFIER (Guid)

4. **Drop indexes:**
   - Old approval-related indexes

5. **Add indexes:**
   - New Status-based indexes

---

## Testing Checklist

Before deploying:

- [ ] Generate migration: `dotnet ef migrations add UpdateApprovalWorkflowColumns`
- [ ] Review migration SQL for data loss warnings
- [ ] Test migration on development database
- [ ] Verify existing data is preserved where possible
- [ ] Update any raw SQL queries that reference old columns
- [ ] Update Blazor UI components that reference old properties
- [ ] Test approval workflows end-to-end
- [ ] Verify indexes are created correctly
- [ ] Check query performance with new indexes

---

## Related Files

### Domain Entities
- `/Accounting.Domain/Entities/Bill.cs`
- `/Accounting.Domain/Entities/JournalEntry.cs`
- `/Accounting.Domain/Entities/FiscalPeriodClose.cs`

### Base Classes
- `/framework/Core/Domain/Entities/AuditableEntityWithApproval.cs`

### Application Layer
- `/Accounting.Application/Bills/**/*.cs`
- `/Accounting.Application/JournalEntries/**/*.cs`

---

**Status:** ✅ All configuration errors fixed  
**Next Step:** Generate and test migrations  
**Last Updated:** November 8, 2025

