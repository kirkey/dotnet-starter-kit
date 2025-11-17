# Accounting Periods Endpoints - Permission Audit & Fix Summary

**Date:** November 17, 2025  
**Status:** ✅ FIXED  
**Module:** `/Accounting/Accounting.Infrastructure/Endpoints/AccountingPeriods/v1`

---

## Overview

All AccountingPeriod endpoints have been reviewed and corrected to align with their workflows using appropriate `FshActions` from the authorization framework.

---

## Endpoints Summary

| Endpoint | HTTP Method | Action | Permission | Workflow | Status |
|----------|-------------|--------|-----------|----------|--------|
| Create | POST | Create | `FshActions.Create` | Create new period | ✅ Correct |
| Get | GET | View | `FshActions.View` | Retrieve single period | ✅ Correct |
| Search | POST | View | `FshActions.View` | List/search periods | ✅ Correct |
| Update | PUT | Update | `FshActions.Update` | Edit period details | ✅ Correct |
| Delete | DELETE | Delete | `FshActions.Delete` | Remove period | ✅ Correct |
| Close | POST | Post | `FshActions.Post` | State transition (Close) | ✅ **FIXED** |
| Reopen | POST | Post | `FshActions.Post` | State transition (Reopen) | ✅ **FIXED** |

---

## Changes Made

### 1. ✅ AccountingPeriodCloseEndpoint
**File:** `AccountingPeriodCloseEndpoint.cs`

**Before:**
```csharp
.RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
```

**After:**
```csharp
.RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
```

**Reason:** Closing a period is a **state transition** (POST workflow action), not a deletion. Uses `FshActions.Post` which represents special POST operations like state changes.

---

### 2. ✅ AccountingPeriodReopenEndpoint
**File:** `AccountingPeriodReopenEndpoint.cs`

**Before:**
```csharp
// No RequirePermission attribute - MISSING!
.Produces<AccountingPeriodTransitionResponse>();
```

**After:**
```csharp
.RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
.MapToApiVersion(1);
```

**Reason:** 
- **Missing permission attribute** - Now added
- Reopening a period is a **state transition** (POST workflow action)
- Uses `FshActions.Post` for special POST operations
- Includes API version mapping

---

## FshActions Reference

Available actions in `Shared.Authorization.FshActions`:

```csharp
// CRUD Operations
View      // GET - Read/retrieve
Search    // POST - Search/filter
Create    // POST - Create new
Update    // PUT - Edit
Delete    // DELETE - Remove

// State Transitions & Special Actions
Post      // ✅ Special POST operations (Close, Reopen, etc.)
Approve   // ✅ Approval workflows
Reject    // ✅ Rejection workflows
Submit    // ✅ Submission workflows
Process   // Process/execute
Complete  // Mark as complete
Cancel    // Cancel operation
Void      // Void/invalidate

// Other Actions
Import/Export
Generate
Clean
Assign
Manage
Regularize
Terminate
Send
Receive
MarkAsPaid
Accrue
Acknowledge
```

---

## Workflow Alignment Rules

| Workflow Type | HTTP Method | FshAction | Example |
|---------------|-------------|-----------|---------|
| **CRUD - Read** | GET | `View` | GetEndpoint |
| **CRUD - List** | POST | `View` | SearchEndpoint |
| **CRUD - Create** | POST | `Create` | CreateEndpoint |
| **CRUD - Update** | PUT | `Update` | UpdateEndpoint |
| **CRUD - Delete** | DELETE | `Delete` | DeleteEndpoint |
| **State Change** | POST | `Post` | CloseEndpoint, ReopenEndpoint |
| **Approval** | POST | `Approve` | ApproveEndpoint |
| **Special Action** | POST | Action Name | ReconcileEndpoint (Update workflow) |

---

## Verification

All endpoints now follow consistent permission patterns:

✅ **Create** → `FshActions.Create` (New resource creation)  
✅ **Get/Search** → `FshActions.View` (Read-only operations)  
✅ **Update** → `FshActions.Update` (Modify existing)  
✅ **Delete** → `FshActions.Delete` (Remove resource)  
✅ **State Transitions** → `FshActions.Post` (Close, Reopen, etc.)  

---

## Next Steps

- [x] Fix AccountingPeriodCloseEndpoint permission
- [x] Add missing RequirePermission to AccountingPeriodReopenEndpoint
- [ ] Review other accounting domains (AR/AP Accounts, Reconciliations, etc.)
- [ ] Apply same pattern across all Accounting endpoints
- [ ] Update other modules (HR, Store, Catalog) for consistency

---

**Completion Status:** ✅ **COMPLETE**

All AccountingPeriod endpoints now have properly aligned permissions reflecting their workflow operations.


