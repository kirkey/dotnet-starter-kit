# Accounts Payable (AP) Endpoints - Permission Audit & Fix Summary

**Date:** November 17, 2025  
**Status:** ✅ FIXED  
**Module:** `/Accounting/Accounting.Infrastructure/Endpoints/AccountsPayableAccounts/v1`

---

## Overview

All AccountsPayableAccounts endpoints have been reviewed and **2 permission misalignments were corrected** to align with their workflows using appropriate `FshActions` from the authorization framework.

---

## Endpoints Summary

| Endpoint | HTTP Method | Action | Permission | Workflow | Status |
|----------|-------------|--------|-----------|----------|--------|
| Create | POST | Create | `FshActions.Create` | Create new AP account | ✅ Correct |
| Get | GET | View | `FshActions.View` | Retrieve single account | ✅ Correct |
| Search | POST | View | `FshActions.View` | List/search accounts | ✅ Correct |
| Update | PUT | Update | `FshActions.Update` | Edit account details | ✅ Correct |
| Delete | DELETE | Delete | `FshActions.Delete` | Remove account | ✅ Correct |
| Reconcile | POST | Update | `FshActions.Update` | Reconcile with ledger | ✅ Correct |
| UpdateBalance | PUT | Update | `FshActions.Update` | Update aging balance | ✅ Correct |
| **RecordDiscountLost** | POST | **Post** | **`FshActions.Post`** | **Record transaction** | ✅ **FIXED** |
| **RecordPayment** | POST | **Post** | **`FshActions.Post`** | **Record transaction** | ✅ **FIXED** |

---

## Changes Made

### 1. ✅ APAccountRecordDiscountLostEndpoint
**File:** `APAccountRecordDiscountLostEndpoint.cs`

**Before:**
```csharp
.RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
```

**After:**
```csharp
.RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
```

**Reason:** 
- Recording a discount lost is a **transaction/event recording** action (accounting POST operation)
- Not creating a new AP account entity
- Uses `FshActions.Post` for accounting transaction posting

**Workflow:** `POST /{id}/discounts-lost` → Records missed early payment discount opportunity

---

### 2. ✅ APAccountRecordPaymentEndpoint
**File:** `APAccountRecordPaymentEndpoint.cs`

**Before:**
```csharp
.RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
```

**After:**
```csharp
.RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
```

**Reason:**
- Recording a payment is a **transaction/event recording** action (accounting POST operation)
- Not creating a new AP account entity
- Uses `FshActions.Post` for accounting transaction posting

**Workflow:** `POST /{id}/payments` → Records vendor payment and tracks discounts

---

## Workflow Alignment Rules Applied

| Workflow Type | HTTP Method | FshAction | Example |
|---------------|-------------|-----------|---------|
| **CRUD - Read** | GET | `View` | GetEndpoint ✅ |
| **CRUD - List** | POST | `View` | SearchEndpoint ✅ |
| **CRUD - Create** | POST | `Create` | CreateEndpoint ✅ |
| **CRUD - Update** | PUT | `Update` | UpdateEndpoint ✅ |
| **CRUD - Delete** | DELETE | `Delete` | DeleteEndpoint ✅ |
| **Transaction Recording** | POST | `Post` | RecordPayment, RecordDiscountLost ✅ |
| **Balance Update** | PUT | `Update` | UpdateBalance ✅ |
| **Reconciliation** | POST | `Update` | Reconcile ✅ |

---

## Detailed Endpoint Analysis

### ✅ Standard CRUD Operations (Correct)

1. **APAccountCreateEndpoint**
   - HTTP: `POST /`
   - Permission: `Create`
   - Workflow: Create new AP account entity
   - Status: ✅ Correct

2. **APAccountGetEndpoint**
   - HTTP: `GET /{id}`
   - Permission: `View`
   - Workflow: Retrieve single account
   - Status: ✅ Correct

3. **APAccountSearchEndpoint**
   - HTTP: `POST /search`
   - Permission: `View`
   - Workflow: Search/list with filtering
   - Status: ✅ Correct

4. **APAccountUpdateEndpoint**
   - HTTP: `PUT /{id}`
   - Permission: `Update`
   - Workflow: Edit account metadata
   - Status: ✅ Correct

5. **APAccountDeleteEndpoint**
   - HTTP: `DELETE /{id}`
   - Permission: `Delete`
   - Workflow: Remove account (if balance zero)
   - Status: ✅ Correct

### ✅ Balance & Reconciliation Operations (Correct)

6. **APAccountUpdateBalanceEndpoint**
   - HTTP: `PUT /{id}/balance`
   - Permission: `Update`
   - Workflow: Update aging buckets and balance
   - Status: ✅ Correct
   - Rationale: Modifying account state (UPDATE operation)

7. **APAccountReconcileEndpoint**
   - HTTP: `POST /{id}/reconcile`
   - Permission: `Update`
   - Workflow: Reconcile with subsidiary ledgers
   - Status: ✅ Correct
   - Rationale: Updates reconciliation status (UPDATE operation)

### ✅ Transaction Recording Operations (Fixed)

8. **APAccountRecordDiscountLostEndpoint** ✅ FIXED
   - HTTP: `POST /{id}/discounts-lost`
   - Permission: `Post` (was: `Create`)
   - Workflow: Record missed discount transaction
   - Status: ✅ Fixed
   - Rationale: Accounting transaction posting, not entity creation

9. **APAccountRecordPaymentEndpoint** ✅ FIXED
   - HTTP: `POST /{id}/payments`
   - Permission: `Post` (was: `Create`)
   - Workflow: Record vendor payment transaction
   - Status: ✅ Fixed
   - Rationale: Accounting transaction posting, not entity creation

---

## Key Distinctions

### When to use `Create` vs `Post`

**Use `FshActions.Create`:**
- Creating a new **entity/resource** (AP Account, Customer, etc.)
- Results in a new record in the database
- Example: `POST /accounts-payable` → Creates new AP account

**Use `FshActions.Post`:**
- Recording **transactions/events** (Payments, Discounts, etc.)
- Posting accounting entries
- State transitions that don't create primary entities
- Example: `POST /accounts-payable/{id}/payments` → Records payment transaction

### When to use `Update` for POST operations

**Use `FshActions.Update` for POST:**
- Reconciliation operations (modifying reconciliation state)
- Operations that update existing entity state
- Example: `POST /accounts-payable/{id}/reconcile` → Updates reconciliation status

---

## FshActions Reference for AP Accounts

Available actions and their proper usage:

```csharp
// CRUD Operations
View      ✅ GET, POST /search
Create    ✅ POST (entity creation)
Update    ✅ PUT, POST (reconcile, state updates)
Delete    ✅ DELETE

// Transaction Operations
Post      ✅ POST (record payment, record discount lost)

// Other Available Actions (not used in AP Accounts)
Approve   // Approval workflows
Process   // Processing operations
Submit    // Submission workflows
```

---

## Verification Checklist

- [x] All endpoints have `RequirePermission` attribute
- [x] CRUD operations properly aligned:
  - [x] GET/Search → `View` ✅
  - [x] POST (create entity) → `Create` ✅
  - [x] PUT → `Update` ✅
  - [x] DELETE → `Delete` ✅
- [x] Transaction recording operations → `Post` ✅
- [x] Reconciliation operations → `Update` ✅
- [x] Balance updates → `Update` ✅
- [x] All use `FshResources.Accounting` consistently ✅

---

## Impact Analysis

### Before Fix
- ❌ Payment recording had same permission as account creation
- ❌ Discount recording had same permission as account creation
- ❌ Potential security issue: Users with "Create Account" permission could record transactions
- ❌ Inconsistent with accounting transaction workflows

### After Fix
- ✅ Clear separation: Account creation vs transaction posting
- ✅ Proper permission granularity for accounting operations
- ✅ Users need `Post` permission to record transactions
- ✅ Users need `Create` permission to create new accounts
- ✅ Aligned with accounting best practices

---

## Best Practices Applied

1. **Entity Creation vs Transaction Recording**
   - Entity creation = `Create`
   - Transaction recording = `Post`

2. **State Modifications**
   - Direct updates = `Update`
   - Reconciliation = `Update` (modifies state)
   - Balance updates = `Update`

3. **Read Operations**
   - Single retrieval = `View` with GET
   - List/Search = `View` with POST

4. **Accounting Transaction Pattern**
   - Use `Post` for all accounting transaction recordings
   - Separates transaction posting from entity management

---

## Conclusion

✅ **All Accounts Payable endpoints now have properly aligned permissions!**

**Summary:**
- **2 endpoints fixed** (RecordDiscountLost, RecordPayment)
- **7 endpoints already correct**
- **100% compliance** with workflow-based permission patterns

The endpoints now follow proper accounting workflow patterns with appropriate permission assignments that distinguish between:
- Entity management (Create/Update/Delete)
- Transaction recording (Post)
- Data retrieval (View)

---

**Audit Date:** November 17, 2025  
**Changes Applied:** 2 permission corrections  
**Result:** ✅ PASSED - All Endpoints Compliant

