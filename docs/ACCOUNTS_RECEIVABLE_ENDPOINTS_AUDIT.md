# Accounts Receivable (AR) Endpoints - Permission Audit & Fix Summary

**Date:** November 17, 2025  
**Status:** ✅ FIXED  
**Module:** `/Accounting/Accounting.Infrastructure/Endpoints/AccountsReceivableAccounts/v1`

---

## Overview

All AccountsReceivableAccounts endpoints have been reviewed and **2 permission misalignments were corrected** to align with their workflows using appropriate `FshActions` from the authorization framework.

---

## Endpoints Summary

| Endpoint | HTTP Method | Action | Permission | Workflow | Status |
|----------|-------------|--------|-----------|----------|--------|
| Create | POST | Create | `FshActions.Create` | Create new AR account | ✅ Correct |
| Get | GET | View | `FshActions.View` | Retrieve single account | ✅ Correct |
| Search | POST | View | `FshActions.View` | List/search accounts | ✅ Correct |
| Reconcile | POST | Update | `FshActions.Update` | Reconcile with ledger | ✅ Correct |
| UpdateAllowance | PUT | Update | `FshActions.Update` | Update doubtful accounts allowance | ✅ Correct |
| UpdateBalance | PUT | Update | `FshActions.Update` | Update aging balance | ✅ Correct |
| **RecordCollection** | POST | **Receive** | **`FshActions.Receive`** | **Record payment received** | ✅ **FIXED** |
| **RecordWriteOff** | POST | **Post** | **`FshActions.Post`** | **Record transaction** | ✅ **FIXED** |

---

## Changes Made

### 1. ✅ ARAccountRecordCollectionEndpoint
**File:** `ARAccountRecordCollectionEndpoint.cs`

**Before:**
```csharp
.RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
```

**After:**
```csharp
.RequirePermission(FshPermission.NameFor(FshActions.Receive, FshResources.Accounting))
```

**Reason:** 
- Recording a collection (payment received) is a **RECEIVE operation** in accounting
- Not creating a new AR account entity
- Uses `FshActions.Receive` for payment receipt workflows
- Semantically more accurate: receiving payment from customers

**Workflow:** `POST /{id}/collections` → Records cash collection and updates YTD statistics

---

### 2. ✅ ARAccountRecordWriteOffEndpoint
**File:** `ARAccountRecordWriteOffEndpoint.cs`

**Before:**
```csharp
.RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
```

**After:**
```csharp
.RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
```

**Reason:**
- Recording a write-off is a **transaction recording** action (accounting POST operation)
- Not creating a new AR account entity
- Uses `FshActions.Post` for accounting transaction posting
- Records bad debt expense and removes from AR balance

**Workflow:** `POST /{id}/write-offs` → Records bad debt write-off transaction

---

## Workflow Alignment Rules Applied

| Workflow Type | HTTP Method | FshAction | Example | AR Application |
|---------------|-------------|-----------|---------|-----------------|
| **CRUD - Read** | GET | `View` | GetEndpoint | ✅ ARAccountGetEndpoint |
| **CRUD - List** | POST | `View` | SearchEndpoint | ✅ ARAccountSearchEndpoint |
| **CRUD - Create** | POST | `Create` | CreateEndpoint | ✅ ARAccountCreateEndpoint |
| **State Update** | PUT | `Update` | UpdateBalance/Allowance | ✅ ARAccountUpdateAllowanceEndpoint |
| **Payment Receipt** | POST | `Receive` | RecordCollection | ✅ ARAccountRecordCollectionEndpoint |
| **Transaction Recording** | POST | `Post` | RecordWriteOff | ✅ ARAccountRecordWriteOffEndpoint |
| **Reconciliation** | POST | `Update` | Reconcile | ✅ ARAccountReconcileEndpoint |

---

## Detailed Endpoint Analysis

### ✅ Standard CRUD Operations (Correct)

1. **ARAccountCreateEndpoint**
   - HTTP: `POST /`
   - Permission: `Create`
   - Workflow: Create new AR account entity
   - Status: ✅ Correct

2. **ARAccountGetEndpoint**
   - HTTP: `GET /{id}`
   - Permission: `View`
   - Workflow: Retrieve single account details
   - Status: ✅ Correct

3. **ARAccountSearchEndpoint**
   - HTTP: `POST /search`
   - Permission: `View`
   - Workflow: Search/list with filtering
   - Status: ✅ Correct

### ✅ Account State Management Operations (Correct)

4. **ARAccountUpdateBalanceEndpoint**
   - HTTP: `PUT /{id}/balance`
   - Permission: `Update`
   - Workflow: Update aging buckets and balance
   - Status: ✅ Correct
   - Rationale: Modifying account state (UPDATE operation)

5. **ARAccountUpdateAllowanceEndpoint**
   - HTTP: `PUT /{id}/allowance`
   - Permission: `Update`
   - Workflow: Update allowance for doubtful accounts
   - Status: ✅ Correct
   - Rationale: Modifying reserve/allowance amount (UPDATE operation)

6. **ARAccountReconcileEndpoint**
   - HTTP: `POST /{id}/reconcile`
   - Permission: `Update`
   - Workflow: Reconcile with subsidiary customer ledgers
   - Status: ✅ Correct
   - Rationale: Updates reconciliation status (UPDATE operation)

### ✅ Transaction Recording Operations (Fixed)

7. **ARAccountRecordCollectionEndpoint** ✅ FIXED
   - HTTP: `POST /{id}/collections`
   - Permission: `Receive` (was: `Create`)
   - Workflow: Record payment received from customer
   - Status: ✅ Fixed
   - Rationale: Cash collection is a RECEIVE operation, not entity creation

8. **ARAccountRecordWriteOffEndpoint** ✅ FIXED
   - HTTP: `POST /{id}/write-offs`
   - Permission: `Post` (was: `Create`)
   - Workflow: Record bad debt write-off transaction
   - Status: ✅ Fixed
   - Rationale: Write-off is a transaction posting, not entity creation

---

## Key Distinctions Applied

### Business Operation Classifications

**`FshActions.Create`** → Creating new **entities** (AR Account, Customer, etc.)
- Creates a new database record
- Establishes a new business relationship
- Example: `POST /accounts-receivable` → Creates new AR account

**`FshActions.Receive`** → **Receiving payments/cash** from customers
- Records cash collection
- Reduces AR balance
- Updates customer aging
- Example: `POST /accounts-receivable/{id}/collections` → Receives payment

**`FshActions.Post`** → Recording **transactions/accounting entries**
- Posts accounting journal entries
- Records bad debt write-offs
- Records adjustments
- Example: `POST /accounts-receivable/{id}/write-offs` → Records write-off transaction

**`FshActions.Update`** → **Modifying existing entity state**
- Updates balances, allowances
- Changes reconciliation status
- Adjusts aging buckets
- Example: `PUT /accounts-receivable/{id}/allowance` → Updates reserve amount

---

## Available FshActions Reference

```csharp
// CRUD Operations
View      ✅ GET, POST /search - Read operations
Create    ✅ POST - Entity creation
Update    ✅ PUT, POST - State modifications
Delete    ✅ DELETE - Entity removal

// AR-Specific Operations
Receive   ✅ POST - Record payment received
Post      ✅ POST - Record transactions/entries
Void      - Void transactions
MarkAsPaid - Mark invoice as paid

// Other Available Actions
Approve   - Approval workflows
Reject    - Rejection workflows
Submit    - Submission workflows
Process   - Processing operations
Complete  - Mark as complete
Cancel    - Cancel operation
```

---

## Verification Checklist

- [x] All endpoints have `RequirePermission` attribute
- [x] CRUD operations properly aligned:
  - [x] GET/Search → `View` ✅
  - [x] POST (create entity) → `Create` ✅
  - [x] PUT → `Update` ✅
- [x] Cash collection operation → `Receive` ✅
- [x] Transaction recording operations → `Post` ✅
- [x] Account state updates → `Update` ✅
- [x] Reconciliation operations → `Update` ✅
- [x] All use `FshResources.Accounting` consistently ✅

---

## Impact Analysis

### Before Fix
- ❌ Payment collection had same permission as account creation
- ❌ Write-off recording had same permission as account creation
- ❌ No distinction between entity management and transaction recording
- ❌ Potential security issue: Users with "Create Account" permission could record transactions

### After Fix
- ✅ Clear separation: Account creation vs transaction recording vs payment receipt
- ✅ Proper permission granularity for AR operations
- ✅ Users need `Receive` permission to record cash collections
- ✅ Users need `Post` permission to record write-offs
- ✅ Users need `Create` permission to create new accounts
- ✅ Aligned with accounting best practices

---

## Best Practices Applied

1. **Entity Management vs Transaction Recording**
   - Entity creation = `Create`
   - Transaction recording = `Post`
   - Cash receipt = `Receive`

2. **Operation Classification**
   - Read-only = `View`
   - Entity modification = `Update`
   - Cash transactions = `Receive`
   - Accounting entries = `Post`

3. **Accounting Transaction Pattern**
   - `Receive` for cash/payment receipts
   - `Post` for journal entries and adjustments
   - `Update` for balance and state changes

4. **Security & Auditability**
   - Clear action tracking for compliance
   - Separate permissions for different transaction types
   - Audit trail distinguishes operational purposes

---

## Comparison with AP Accounts

| Operation | AP Accounts | AR Accounts |
|-----------|------------|------------|
| Record Payment | `Post` | `Receive` |
| Record Discount | `Post` | N/A |
| Record Write-off | N/A | `Post` |
| Record Collection | N/A | `Receive` |

**Rationale:** AR uses `Receive` for customer payments (cash in), AP uses `Post` for vendor payments (cash out) and discounts.

---

## Conclusion

✅ **All Accounts Receivable endpoints now have properly aligned permissions!**

**Summary:**
- **2 endpoints fixed** (RecordCollection, RecordWriteOff)
- **6 endpoints already correct**
- **100% compliance** with workflow-based permission patterns

The endpoints now follow proper accounting workflow patterns with appropriate permission assignments that distinguish between:
- Entity management (Create/Update/Delete)
- Cash receipt operations (Receive)
- Transaction recording (Post)
- Data retrieval (View)

---

**Audit Date:** November 17, 2025  
**Changes Applied:** 2 permission corrections  
**Result:** ✅ PASSED - All Endpoints Compliant


