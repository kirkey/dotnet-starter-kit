# Journal Entries & Journal Entry Lines Endpoints - Permission Audit & Fix Summary

**Date:** November 17, 2025  
**Status:** ✅ FIXED  
**Module:** `/Accounting/Accounting.Infrastructure/Endpoints/JournalEntries/v1` and `/JournalEntryLines/v1`  
**Priority:** ⭐⭐ CRITICAL - Core accounting transaction entry point

---

## Overview

Both JournalEntries and JournalEntryLines endpoints have been reviewed and **1 critical permission misalignment was corrected** to align with workflow semantics using appropriate `FshActions` from the authorization framework.

---

## JournalEntries Endpoints Summary

| Endpoint | HTTP Method | Action | Permission | Workflow | Status |
|----------|-------------|--------|-----------|----------|--------|
| Create | POST | Create | `FshActions.Create` | Create new entry | ✅ Correct |
| Get | GET | View | `FshActions.View` | Retrieve single entry | ✅ Correct |
| Search | POST | View | `FshActions.View` | List/search entries | ✅ Correct |
| Update | PUT | Update | `FshActions.Update` | Edit unposted entry | ✅ Correct |
| Delete | DELETE | Delete | `FshActions.Delete` | Delete unposted entry | ✅ Correct |
| Post | POST | Post | `FshActions.Post` | Post to general ledger | ✅ Correct |
| Approve | POST | Approve | `FshActions.Approve` | Approve pending entry | ✅ Correct |
| **Reject** | POST | **Reject** | **`FshActions.Reject`** | **Reject pending entry** | ✅ **FIXED** |
| Reverse | POST | Post | `FshActions.Post` | Reverse posted entry | ✅ Correct |

---

## Changes Made

### 1. ✅ JournalEntryRejectEndpoint
**File:** `JournalEntryRejectEndpoint.cs`

**Before:**
```csharp
.RequirePermission(FshPermission.NameFor(FshActions.Approve, FshResources.Accounting))
```

**After:**
```csharp
.RequirePermission(FshPermission.NameFor(FshActions.Reject, FshResources.Accounting))
```

**Reason:** 
- Rejection is a **distinct workflow action** from approval
- Should use `FshActions.Reject` to enable proper RBAC
- Allows managers to approve but not reject (or vice versa)
- Clear audit trail for rejection decisions

**Workflow:** `POST /{id}/reject` → Rejects pending journal entry with reason

---

## JournalEntryLines Endpoints Summary

| Endpoint | HTTP Method | Action | Permission | Workflow | Status |
|----------|-------------|--------|-----------|----------|--------|
| Create | POST | Create | `FshActions.Create` | Create line item | ✅ Correct |
| Get | GET | View | `FshActions.View` | Retrieve single line | ✅ Correct |
| Search | GET | View | `FshActions.View` | List lines by entry | ✅ Correct |
| Update | PUT | Update | `FshActions.Update` | Edit line item | ✅ Correct |
| Delete | DELETE | Delete | `FshActions.Delete` | Delete line item | ✅ Correct |

**Status:** ✅ All 5 endpoints properly aligned with workflow - NO FIXES NEEDED

---

## Workflow Alignment Rules Applied

### JournalEntries Workflow
```
Create (Create)
    ↓
Update/Modify (Update)
    ↓
Approve (Approve) ←→ Reject (Reject)  [FIXED: Was Approve, now Reject]
    ↓
Post to GL (Post)
    ↓
Reverse/Undo (Post - creates reversing entry)
```

### JournalEntryLines Workflow
```
Create (Create)
    ↓
Update (Update)
    ↓
Delete (Delete)
    ↓
Posted with Parent Entry
```

---

## Detailed Endpoint Analysis

### ✅ Standard CRUD Operations (Correct)

**JournalEntries:**
1. **JournalEntryCreateEndpoint**
   - HTTP: `POST /`
   - Permission: `Create`
   - Workflow: Create new GL entry with balanced debits/credits
   - Status: ✅ Correct

2. **JournalEntryGetEndpoint**
   - HTTP: `GET /{id}`
   - Permission: `View`
   - Workflow: Retrieve entry details
   - Status: ✅ Correct

3. **JournalEntrySearchEndpoint**
   - HTTP: `POST /search`
   - Permission: `View`
   - Workflow: Search with filtering and pagination
   - Status: ✅ Correct

4. **JournalEntryUpdateEndpoint**
   - HTTP: `PUT /{id}`
   - Permission: `Update`
   - Workflow: Edit unposted entry fields
   - Status: ✅ Correct
   - Note: Only unposted entries can be edited

5. **JournalEntryDeleteEndpoint**
   - HTTP: `DELETE /{id}`
   - Permission: `Delete`
   - Workflow: Remove unposted entry
   - Status: ✅ Correct
   - Note: Posted entries cannot be deleted (must use reverse)

**JournalEntryLines:**
- All 5 CRUD operations (Create, Get, Search, Update, Delete) use correct actions
- Status: ✅ All correct

### ✅ Accounting Transaction Operations (Correct)

6. **JournalEntryPostEndpoint**
   - HTTP: `POST /{id}/post`
   - Permission: `Post`
   - Workflow: Post balanced entry to general ledger
   - Status: ✅ Correct
   - Rationale: `Post` is the proper action for GL posting

7. **JournalEntryReverseEndpoint**
   - HTTP: `POST /{id}/reverse`
   - Permission: `Post`
   - Workflow: Create reversing entry with opposite debits/credits
   - Status: ✅ Correct
   - Rationale: Creating a reversing entry is a posting operation

### ✅ Approval Workflow Operations (Fixed)

8. **JournalEntryApproveEndpoint** ✅
   - HTTP: `POST /{id}/approve`
   - Permission: `Approve`
   - Workflow: Approve pending entry for posting
   - Status: ✅ Correct
   - Rationale: Approval workflow requires `Approve` permission

9. **JournalEntryRejectEndpoint** ✅ FIXED
   - HTTP: `POST /{id}/reject`
   - Permission: `Reject` (was: `Approve`)
   - Workflow: Reject pending entry with reason
   - Status: ✅ Fixed
   - Rationale: Rejection is distinct action from approval; needs separate permission

---

## Key Distinctions in Journal Entry Workflow

| Permission | Usage | Workflow Stage | Access |
|-----------|-------|-----------------|--------|
| `Create` | Create new entry | Entry creation | Finance Coordinator |
| `View` | Read entry data | Any time | All staff |
| `Update` | Modify unposted entry | Before approval | Entry creator |
| `Delete` | Remove unposted entry | Before approval | Finance Manager |
| `Approve` | **Approve pending** | **Approval stage** | **Finance Manager/Approver** |
| `Reject` | **Reject pending** | **Approval stage** | **Finance Manager/Approver** |
| `Post` | Post to GL | After approval | GL posting role |
| `Post` | Reverse entry | Post-posting | GL posting role |

---

## Impact Analysis

### Before Fix
- ❌ Rejection had same permission as approval
- ❌ Could not distinguish approval vs rejection permissions
- ❌ No way to grant rejection rights without approval rights
- ❌ Violates segregation of duties principle

### After Fix
- ✅ Clear separation: Approval vs Rejection permissions
- ✅ Proper permission granularity for GL control
- ✅ Users can have `Reject` but not `Approve` (or vice versa)
- ✅ Supports segregation of duties

---

## Verification Checklist

- [x] All JournalEntries endpoints have `RequirePermission` attribute
- [x] All JournalEntryLines endpoints have `RequirePermission` attribute
- [x] CRUD operations properly aligned:
  - [x] GET/Search → `View` ✅
  - [x] POST (create) → `Create` ✅
  - [x] PUT → `Update` ✅
  - [x] DELETE → `Delete` ✅
- [x] Accounting transaction operations → `Post` ✅
- [x] Approval workflows properly configured:
  - [x] Approve → `Approve` ✅
  - [x] Reject → `Reject` ✅
- [x] All use `FshResources.Accounting` consistently ✅
- [x] MapToApiVersion(1) present in all endpoints ✅
- [x] No compilation errors ✅

---

## Best Practices Applied

1. **Workflow Semantics**
   - Separate permissions for distinct operations
   - Clear intent: Approve is different from Reject
   - Enables proper segregation of duties

2. **Accounting Transaction Integrity**
   - `Post` action for GL posting operations
   - Clear audit trail for all transactions
   - Immutability of posted entries enforced

3. **RBAC Principles**
   - Granular permission assignment
   - Users can have `Reject` without `Approve`
   - Compliance with internal controls

4. **Double-Entry Accounting**
   - Entries must be balanced (debits = credits)
   - Posted entries cannot be edited (only reversed)
   - Clear workflow progression

---

## Conclusion

✅ **All Journal Entries and Journal Entry Lines endpoints now have properly aligned permissions!**

**Summary:**
- **9 JournalEntries endpoints reviewed** (1 fix applied)
- **5 JournalEntryLines endpoints reviewed** (0 fixes needed)
- **14 endpoints total** → **100% workflow-aligned**
- **1 critical fix:** Rejection now uses proper `Reject` action
- **0 compilation errors**

The endpoints now follow proper GL transaction workflows with appropriate permission assignments that enable:
- Proper segregation of duties
- Clear audit trails
- Compliance with internal controls
- Enterprise-grade RBAC enforcement

---

**Audit Date:** November 17, 2025  
**Changes Applied:** 1 critical permission correction  
**Result:** ✅ PASSED - All Endpoints Compliant


