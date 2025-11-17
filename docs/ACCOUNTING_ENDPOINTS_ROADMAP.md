# Accounting Endpoints - Implementation Roadmap for Remaining 38 Domains

**Purpose:** Quick reference guide for completing all remaining accounting endpoint permission fixes  
**Scope:** Banks through WriteOffs (38 domains, ~200+ endpoints)  
**Estimated Time:** 2-3 hours of focused work

---

## Quick Start Checklist for Each Domain

### For Each Domain (Template):

```
Domain: [Name]
Status: ⏳ TODO
Endpoints: [Count]
Expected Fixes: [2-5 typically]

Checklist:
- [ ] List all v1 endpoint files
- [ ] Read each endpoint file
- [ ] Extract HTTP method
- [ ] Extract current permission
- [ ] Determine expected FshAction
- [ ] Apply fixes (if needed)
- [ ] Verify compilation
- [ ] Create audit document
- [ ] Mark complete ✅
```

---

## Phase 1: Quick Win Domains (1-2 hours)

These domains likely need 0-2 fixes each (standard CRUD operations)

### 1. Banks (5 endpoints)
**Expected Fixes:** 0-1 (likely all correct)
```
BankCreateEndpoint → Create ✅ (verified)
BankDeleteEndpoint → Delete ✅ (verified)
BankGetEndpoint → View
BankSearchEndpoint → View
BankUpdateEndpoint → Update
```

### 2. Vendors (TBD endpoints)
**Expected Fixes:** 0-2 (likely standard CRUD)
**Status:** Quick review recommended

### 3. Payees (TBD endpoints)
**Expected Fixes:** 0-1
**Status:** Quick review recommended

### 4. TaxCodes (TBD endpoints)
**Expected Fixes:** 0-1
**Status:** Quick review recommended

### 5. Member (TBD endpoints)
**Expected Fixes:** 0-1
**Status:** Quick review recommended

---

## Phase 2: Standard Transaction Domains (2-3 hours)

These domains have workflow operations (Approve, Void, Post, etc.)

### 6. Bills (10 endpoints) ⭐ HIGH PRIORITY
**Known Endpoints:**
- ApproveBillEndpoint → Approve ✅
- BillCreateEndpoint → Create ✅
- BillUpdateEndpoint → Update ✅
- DeleteBillEndpoint → Delete ✅
- GetBillEndpoint → View ✅
- MarkBillAsPaidEndpoint → MarkAsPaid (verify)
- PostBillEndpoint → Post (verify)
- RejectBillEndpoint → Reject (verify)
- SearchBillsEndpoint → View ✅
- VoidBillEndpoint → Void (verify)

**Expected Fixes:** 3-4 (MarkAsPaid, Post, Reject, Void verification)

### 7. CostCenters (TBD endpoints)
**Expected Workflow:**
- Create, Get, Search, Update, Delete → Standard CRUD
- Activate/Deactivate → Process or custom
- UpdateBudget → Post or Update

**Expected Fixes:** 1-2

### 8. FixedAssets (TBD endpoints)
**Expected Workflow:**
- Create, Get, Search, Update, Delete → Standard CRUD
- Depreciate → Post (likely)
- Dispose → Void (likely)

**Expected Fixes:** 2-3

### 9. Invoices (TBD endpoints)
**Expected Workflow:**
- Create, Get, Search, Update, Delete → Standard CRUD
- Post → Post
- Approve → Approve
- Reject → Reject
- Void → Void

**Expected Fixes:** 4-5

### 10. Payments (TBD endpoints)
**Expected Workflow:**
- Create, Get, Search, Update, Delete → Standard CRUD
- Record → Post (verify)
- Allocate → Post (verify)
- Approve → Approve (verify)
- Void → Void (verify)
- MarkAsPaid → MarkAsPaid (verify)

**Expected Fixes:** 4-6

---

## Phase 3: CRITICAL Domains (1-2 hours)

### 11. JournalEntries ⭐⭐ CRITICAL
**Expected Workflow:**
- Create, Get, Search, Update, Delete → Standard CRUD
- Post → Post
- Approve → Approve
- Reject → Reject
- Void → Void
- Submit → Submit (verify)

**Expected Fixes:** 5-6
**Impact:** Core accounting transaction

### 12. JournalEntryLines ⭐⭐ CRITICAL
**Expected Workflow:**
- Create, Get, Search, Update, Delete → Standard CRUD
- Post → Post (if applicable)

**Expected Fixes:** 1-2
**Impact:** GL transaction details

### 13. ChartOfAccounts ⭐ HIGH
**Expected Workflow:**
- Create, Get, Search, Update, Delete → Standard CRUD
- Activate → Process (verify)
- Deactivate → Process (verify)
- Export → Export
- Import → Import
- UpdateBalance → Update

**Expected Fixes:** 3-4
**Impact:** Master GL account configuration

### 14. GeneralLedger (TBD endpoints)
**Expected Workflow:**
- Get, Search → View
- Post → Post (if applicable)

**Expected Fixes:** 0-2

### 15. TrialBalance (TBD endpoints)
**Expected Workflow:**
- Get, Search → View
- Generate → Generate
- Create → Create (for custom trial balances)
- Finalize → Complete (verify)
- Reopen → Process (verify)

**Expected Fixes:** 2-4

---

## Phase 4: Remaining Domains (1 hour)

20+ remaining lower-priority domains that likely need 0-2 fixes each:

- Billing, BudgetDetails, Budgets
- Checks, Consumptions
- CreditMemos, Customers
- DebitMemos, DeferredRevenues, DepreciationMethods
- FuelConsumption
- InterCompanyTransactions, InventoryItems
- Member, Meter
- Patronage, PaymentAllocations, PostingBatch, PrepaidExpenses, Projects
- RegulatoryReports, RecurringJournalEntries, RetainedEarnings
- SecurityDeposits
- WriteOffs

**Strategy:** Batch review for common patterns, apply similar fixes

---

## Systematic Fix Application

### For Each Endpoint Needing Fix:

```csharp
// BEFORE (e.g., wrong action)
.RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))

// AFTER (e.g., correct action)
.RequirePermission(FshPermission.NameFor(FshActions.Post, FshResources.Accounting))
```

### Using insert_edit_into_file:

```
Target: The permission line
Replace: Update → Post (or whatever change needed)
Verify: MapToApiVersion(1) present afterward
```

---

## Compilation Verification

After each batch of fixes:

```bash
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit
dotnet build src/api/server/server.csproj -c Debug
```

Expected result: ✅ No compilation errors related to authorization

---

## Documentation Pattern

For each domain fixed, create audit document:

**File:** `docs/[DOMAIN_NAME]_ENDPOINTS_AUDIT.md`

**Contents:**
1. Summary table (Endpoint | HTTP | Permission | Status)
2. Changes made (if any)
3. Verification checklist
4. FshActions reference table
5. Before/after comparison

**Example:** See `docs/ACCOUNTS_PAYABLE_ENDPOINTS_AUDIT.md`

---

## Success Criteria

✅ When complete, you should have:
- [ ] All 45+ domains audited
- [ ] All 250+ endpoints with proper permissions
- [ ] 0 compilation errors
- [ ] 45+ audit documents created
- [ ] Master comprehensive report updated
- [ ] All changes follow FshActions framework
- [ ] Proper RBAC enforcement across all operations

---

## Time Estimates

| Phase | Domains | Endpoints | Time | Effort |
|-------|---------|-----------|------|--------|
| Phase 1 | 5 | ~25 | 30 min | Easy |
| Phase 2 | 5 | ~50 | 1-1.5 hr | Medium |
| Phase 3 | 5 | ~40 | 1-1.5 hr | Complex |
| Phase 4 | 23 | ~85 | 1 hr | Mixed |
| **TOTAL** | **38** | **~200** | **3-4 hr** | **Systematic** |

---

## Priority Order for Execution

### Recommended Execution Order:
1. ✅ Phase 1 (Quick wins - 30 min)
2. ✅ Phase 3 CRITICAL (JournalEntries, ChartOfAccounts - 1.5 hr)
3. ✅ Phase 2 HIGH (Bills, Payments, Invoices - 1.5 hr)
4. ✅ Phase 4 (Remaining 20+ domains - 1 hr)

**Total: 3-4 hours of focused work**

---

## Next Session Kickoff Script

```bash
# 1. Navigate to project
cd /Users/kirkeypsalms/Projects/dotnet-starter-kit

# 2. For each pending domain:
#    a. List endpoints: ls src/api/modules/Accounting/Accounting.Infrastructure/Endpoints/[DOMAIN]/v1/
#    b. Review each endpoint file
#    c. Apply fixes
#    d. Compile check: dotnet build

# 3. Document: Update docs/ACCOUNTING_ENDPOINTS_COMPREHENSIVE_AUDIT.md

# 4. Final verification: dotnet build (should pass with 0 errors)
```

---

## Contact Points for Issues

If compilation errors occur after fixes:
1. Verify `using Shared.Authorization;` is present
2. Verify `FshResources.Accounting` is available
3. Verify `FshActions.[ActionName]` is valid per `FshActions.cs`
4. Check namespace consistency

---

**Status:** 🟢 Ready for execution  
**Last Updated:** November 17, 2025  
**Maintainer:** GitHub Copilot


