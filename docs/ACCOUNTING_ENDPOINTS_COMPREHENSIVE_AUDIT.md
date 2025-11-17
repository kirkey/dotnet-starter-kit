# Comprehensive Accounting Endpoints Audit & Fix Report

**Date:** November 17, 2025  
**Status:** 🔄 IN PROGRESS  
**Scope:** All 45+ accounting endpoint domains (250+ endpoints)  
**Completed:** 7 domains (Accruals, AccountReconciliation*, AccountingPeriods, AP Accounts, AR Accounts, BankReconciliations)  
**Remaining:** 38 domains

---

## Executive Summary

This document tracks the systematic audit and correction of **all 45+ accounting endpoint domains** to ensure permissions align with business workflows using `FshActions` from the authorization framework.

### Completed Domains (✅ 7 Total)
1. ✅ **Accruals** - 8 endpoints (Approve, Reject endpoints fixed with missing permissions)
2. ✅ **AccountReconciliation** - 1 endpoint (Reconcile uses Update)
3. ✅ **AccountReconciliations** - 6 endpoints (All CRUD + Approve aligned)
4. ✅ **AccountingPeriods** - 7 endpoints (Close/Reopen fixed from Delete to Post)
5. ✅ **AccountsPayableAccounts** - 9 endpoints (RecordDiscountLost, RecordPayment fixed to Post)
6. ✅ **AccountsReceivableAccounts** - 8 endpoints (RecordCollection→Receive, RecordWriteOff→Post)
7. ✅ **BankReconciliations** - 9 endpoints (Start→Process, Complete→Complete, Reject→Reject)

### Pending Domains (⏳ 38 Total)

| # | Domain | Endpoints | Priority | Status |
|----|--------|-----------|----------|--------|
| 1 | Banks | 5 | HIGH | ⏳ Review |
| 2 | Billing | TBD | MEDIUM | ⏳ Review |
| 3 | Bills | 10 | HIGH | ⏳ Review |
| 4 | BudgetDetails | TBD | MEDIUM | ⏳ Review |
| 5 | Budgets | TBD | MEDIUM | ⏳ Review |
| 6 | ChartOfAccounts | 10 | CRITICAL | ⏳ Review |
| 7 | Checks | TBD | MEDIUM | ⏳ Review |
| 8 | Consumptions | TBD | LOW | ⏳ Review |
| 9 | CostCenters | TBD | HIGH | ⏳ Review |
| 10 | CreditMemos | TBD | MEDIUM | ⏳ Review |
| 11 | Customers | TBD | MEDIUM | ⏳ Review |
| 12 | DebitMemos | TBD | MEDIUM | ⏳ Review |
| 13 | DeferredRevenues | TBD | HIGH | ⏳ Review |
| 14 | DepreciationMethods | TBD | LOW | ⏳ Review |
| 15 | FinancialStatements | TBD | HIGH | ⏳ Review |
| 16 | FiscalPeriodCloses | TBD | HIGH | ⏳ Review |
| 17 | FixedAssets | TBD | HIGH | ⏳ Review |
| 18 | FuelConsumption | TBD | LOW | ⏳ Review |
| 19 | GeneralLedger | TBD | HIGH | ⏳ Review |
| 20 | InterCompanyTransactions | TBD | MEDIUM | ⏳ Review |
| 21 | InventoryItems | TBD | HIGH | ⏳ Review |
| 22 | Invoices | TBD | HIGH | ⏳ Review |
| 23 | JournalEntries | TBD | CRITICAL | ⏳ Review |
| 24 | JournalEntryLines | TBD | CRITICAL | ⏳ Review |
| 25 | Member | TBD | MEDIUM | ⏳ Review |
| 26 | Meter | TBD | LOW | ⏳ Review |
| 27 | Patronage | TBD | LOW | ⏳ Review |
| 28 | Payees | TBD | LOW | ⏳ Review |
| 29 | PaymentAllocations | TBD | MEDIUM | ⏳ Review |
| 30 | Payments | TBD | HIGH | ⏳ Review |
| 31 | PostingBatch | TBD | MEDIUM | ⏳ Review |
| 32 | PrepaidExpenses | TBD | MEDIUM | ⏳ Review |
| 33 | Projects | TBD | MEDIUM | ⏳ Review |
| 34 | RecurringJournalEntries | TBD | MEDIUM | ⏳ Review |
| 35 | RegulatoryReports | TBD | LOW | ⏳ Review |
| 36 | RetainedEarnings | TBD | MEDIUM | ⏳ Review |
| 37 | SecurityDeposits | TBD | MEDIUM | ⏳ Review |
| 38 | TaxCodes | TBD | MEDIUM | ⏳ Review |
| 39 | TrialBalance | TBD | HIGH | ⏳ Review |
| 40 | Vendors | TBD | MEDIUM | ⏳ Review |
| 41 | WriteOffs | TBD | MEDIUM | ⏳ Review |

---

## Permission Alignment Framework

### FshActions Available (from Shared.Authorization.FshActions)

```csharp
// Core CRUD
View, Create, Update, Delete

// Workflow State Transitions
Approve, Reject, Submit, Process, Complete, Cancel

// Accounting-Specific  
Post, Receive, Send, MarkAsPaid, Accrue, Void

// Data Operations
Export, Import, Generate, Clean

// Other
Assign, Manage, Regularize, Terminate, Acknowledge, UpgradeSubscription
```

### HTTP Method → FshAction Mapping

| HTTP Method | Pattern | Expected FshAction | Workflow Purpose |
|------------|---------|------------------|------------------|
| GET | Any | `View` | Read data |
| POST | /search | `View` | List/search data |
| POST | / | `Create` | Create new entity |
| POST | /{id}/start | `Process` | Initiate workflow |
| POST | /{id}/process | `Process` | Process workflow step |
| POST | /{id}/complete | `Complete` | Complete workflow |
| POST | /{id}/approve | `Approve` | Approve state change |
| POST | /{id}/reject | `Reject` | Reject state change |
| POST | /{id}/void | `Void` | Void/cancel operation |
| POST | /{id}/reconcile | `Update` | Reconcile state |
| POST | /{id}/(post\|send) | `Post` | Post transaction |
| POST | /{id}/(receive\|collection) | `Receive` | Receive payment |
| POST | /{id}/(record...) | `Post` | Record entry |
| PUT | /{id} | `Update` | Update entity |
| PUT | /{id}/reverse | `Update` | Reverse/undo |
| DELETE | /{id} | `Delete` | Delete entity |

---

## High-Priority Domains to Fix

### 1. ChartOfAccounts (CRITICAL - 10 endpoints)
**Concern:** Master data for all GL accounts; incorrect permissions affect core accounting operations  
**Expected fixes:**
- Activate/Deactivate → might need `Process` or custom action
- Export/Import → `Export`, `Import`
- UpdateBalance → `Update`

### 2. JournalEntries (CRITICAL - Multiple endpoints)
**Concern:** Core accounting transaction entry point  
**Expected fixes:**
- Post → `Post`
- Approve → `Approve`
- Reject → `Reject`
- Void → `Void`

### 3. Payments (HIGH - Multiple endpoints)
**Concern:** Critical cash management workflow  
**Expected fixes:**
- Record → `Post`  
- Approve → `Approve`
- Void → `Void`
- Allocate → `Post`

### 4. Invoices (HIGH - Multiple endpoints)
**Concern:** AR workflow  
**Expected fixes:**
- Post → `Post`
- Approve → `Approve`
- Void → `Void`

### 5. Bills (HIGH - 10 endpoints)
**Status:** Appears well-configured already (verified Create, Approve)  
**Quick check needed** for: MarkAsPaid, Post, Void, Reject endpoints

---

## Audit Pattern Identified

From reviewed completed domains, common issues found:

1. **Missing RequirePermission** (Accruals: Approve, Reject endpoints)
2. **Incorrect workflow action** (AccountingPeriods: Close using Delete instead of Post)
3. **Non-semantic action mapping** (BankReconciliations: Start/Complete using Update)
4. **Transaction recording mapped to Create** (AP/AR: RecordPayment/RecordCollection using Create)

---

## Implementation Strategy

### Phase 1: High-Priority Domains (This session)
- [ ] Banks (5 endpoints)
- [ ] Bills (10 endpoints)  
- [ ] ChartOfAccounts (10 endpoints)
- [ ] CostCenters (TBD)
- [ ] FixedAssets (TBD)
- [ ] GeneralLedger (TBD)
- [ ] Payments (TBD)

### Phase 2: Critical Domains (Next session)
- [ ] JournalEntries (CRITICAL)
- [ ] JournalEntryLines (CRITICAL)
- [ ] Invoices (HIGH)
- [ ] TrialBalance (HIGH)

### Phase 3: Remaining Domains (Follow-up)
- [ ] All 20+ remaining domains with lower priority

---

## Verification Checklist

After each domain fix:
- [ ] All endpoints have `RequirePermission` attribute
- [ ] Permissions match workflow semantics
- [ ] All use `FshResources.Accounting`
- [ ] `MapToApiVersion(1)` or equivalent present
- [ ] No compilation errors
- [ ] Documentation updated

---

## Success Metrics

- ✅ **100% of 250+ endpoints** have explicit permission attributes
- ✅ **0 permission misalignments** with workflow semantics
- ✅ **Audit document** tracking all changes
- ✅ **Zero compilation errors**
- ✅ **Proper RBAC** enforcement across all operations

---

**Next Steps:**
1. Start with Phase 1 domains (Banks, Bills, ChartOfAccounts)
2. Apply fixes systematically
3. Document changes
4. Proceed to Phase 2 (JournalEntries, Invoices, etc.)
5. Complete remaining 20+ domains


