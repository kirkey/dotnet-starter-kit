# Accounting Module - Command Naming Analysis

**Date:** November 8, 2025  
**Scope:** All Accounting Application Commands

---

## Commands That Need Renaming

After scanning all 400+ commands in the Accounting module, here are the commands that could benefit from more specific naming:

### 1. ❌ AccountsReceivableAccounts - Ambiguous Commands

**Location:** `Accounting.Application/AccountsReceivableAccounts/`

| Current Name | Issue | Suggested Name |
|--------------|-------|----------------|
| `RecordCollectionCommand` | Doesn't specify AR context | `RecordARCollectionCommand` |
| `RecordWriteOffCommand` | Doesn't specify AR context | `RecordARWriteOffCommand` |
| `ReconcileArAccountCommand` | ✅ Already has AR | No change needed |
| `UpdateArBalanceCommand` | ✅ Already has AR | No change needed |
| `UpdateARAllowanceCommand` | ✅ Already has AR | No change needed |

### 2. ❌ AccountsPayableAccounts - Ambiguous Commands

**Location:** `Accounting.Application/AccountsPayableAccounts/`

| Current Name | Issue | Suggested Name |
|--------------|-------|----------------|
| `RecordDiscountLostCommand` | Doesn't specify AP context | `RecordAPDiscountLostCommand` |
| `RecordAPPaymentCommand` | ✅ Already has AP | No change needed |
| `ReconcileAPAccountCommand` | ✅ Already has AP | No change needed |
| `UpdateAPBalanceCommand` | ✅ Already has AP | No change needed |

### 3. ❌ CostCenters - Ambiguous Commands

**Location:** `Accounting.Application/CostCenters/`

| Current Name | Issue | Suggested Name |
|--------------|-------|----------------|
| `RecordActualCommand` | Too generic | `RecordCostCenterActualCommand` |
| `UpdateBudgetCommand` | Conflicts with Budgets.UpdateBudgetCommand | `UpdateCostCenterBudgetCommand` |

### 4. ❌ AccountReconciliations - Generic Command

**Location:** `Accounting.Application/AccountReconciliations/`

| Current Name | Issue | Suggested Name |
|--------------|-------|----------------|
| `ReconcileAccountCommand` | Too generic | `ReconcileGeneralLedgerAccountCommand` |

### 5. ✅ FiscalPeriodCloses - Recently Fixed

**Location:** `Accounting.Application/FiscalPeriodCloses/Commands/v1/`

| Current Name | Status |
|--------------|--------|
| `CompleteFiscalPeriodTaskCommand` | ✅ Correctly named |
| `AddFiscalPeriodCloseValidationIssueCommand` | ✅ Correctly named |
| `ResolveFiscalPeriodCloseValidationIssueCommand` | ✅ Correctly named |

---

## Commands That Are Correctly Named

### ✅ Well-Named Commands (Examples)

These commands clearly indicate their feature context:

- `ApproveBankReconciliationCommand` - Clear (Bank Reconciliation)
- `PostJournalEntryCommand` - Clear (Journal Entry)
- `RecordAPPaymentCommand` - Clear (AP Account)
- `UpdateARAllowanceCommand` - Clear (AR Account)
- `DepreciateFixedAssetCommand` - Clear (Fixed Asset)
- `RecognizeDeferredRevenueCommand` - Clear (Deferred Revenue)
- `RecordAmortizationCommand` - Clear (Prepaid Expense)
- `CompleteFiscalPeriodCloseCommand` - Clear (Fiscal Period Close)

---

## Priority Ranking

### 🔥 HIGH Priority (Namespace Conflicts)

1. **CostCenters.UpdateBudgetCommand**
   - Conflicts with `Budgets.UpdateBudgetCommand`
   - Both commands exist in the same module
   - **HIGH risk of confusion**

### 🔶 MEDIUM Priority (Ambiguous Context)

2. **RecordCollectionCommand** (AR)
3. **RecordWriteOffCommand** (AR)
4. **RecordDiscountLostCommand** (AP)
5. **RecordActualCommand** (Cost Center)
6. **ReconcileAccountCommand** (Account Reconciliation)

---

## Recommended Changes

### Change #1: CostCenters.UpdateBudgetCommand

**File:** `Accounting.Application/CostCenters/UpdateBudget/v1/UpdateBudgetCommand.cs`

```csharp
// Current
public sealed record UpdateBudgetCommand(
    DefaultIdType Id, 
    decimal BudgetAmount
) : IRequest<DefaultIdType>;

// Rename to
public sealed record UpdateCostCenterBudgetCommand(
    DefaultIdType Id, 
    decimal BudgetAmount
) : IRequest<DefaultIdType>;
```

**Also update:**
- `UpdateBudgetHandler.cs` → `UpdateCostCenterBudgetHandler.cs`
- `UpdateBudgetCommandValidator.cs` → `UpdateCostCenterBudgetCommandValidator.cs`
- Related endpoint

---

### Change #2: CostCenters.RecordActualCommand

**File:** `Accounting.Application/CostCenters/RecordActual/v1/RecordActualCommand.cs`

```csharp
// Current
public sealed record RecordActualCommand(
    DefaultIdType Id, 
    decimal Amount
) : IRequest<DefaultIdType>;

// Rename to
public sealed record RecordCostCenterActualCommand(
    DefaultIdType Id, 
    decimal Amount
) : IRequest<DefaultIdType>;
```

**Also update:**
- `RecordActualHandler.cs` → `RecordCostCenterActualHandler.cs`
- `RecordActualCommandValidator.cs` → `RecordCostCenterActualCommandValidator.cs`
- Related endpoint

---

### Change #3: AR.RecordCollectionCommand

**File:** `Accounting.Application/AccountsReceivableAccounts/RecordCollection/v1/RecordCollectionCommand.cs`

```csharp
// Current
public sealed record RecordCollectionCommand(
    DefaultIdType Id, 
    decimal Amount
) : IRequest<DefaultIdType>;

// Rename to
public sealed record RecordARCollectionCommand(
    DefaultIdType Id, 
    decimal Amount
) : IRequest<DefaultIdType>;
```

**Also update:**
- `RecordCollectionHandler.cs` → `RecordARCollectionHandler.cs`
- `RecordCollectionCommandValidator.cs` → `RecordARCollectionCommandValidator.cs`
- Related endpoint

---

### Change #4: AR.RecordWriteOffCommand

**File:** `Accounting.Application/AccountsReceivableAccounts/RecordWriteOff/v1/RecordWriteOffCommand.cs`

```csharp
// Current
public sealed record RecordWriteOffCommand(
    DefaultIdType Id, 
    decimal Amount
) : IRequest<DefaultIdType>;

// Rename to
public sealed record RecordARWriteOffCommand(
    DefaultIdType Id, 
    decimal Amount
) : IRequest<DefaultIdType>;
```

**Also update:**
- `RecordWriteOffHandler.cs` → `RecordARWriteOffHandler.cs`
- `RecordWriteOffCommandValidator.cs` → `RecordARWriteOffCommandValidator.cs`
- Related endpoint

---

### Change #5: AP.RecordDiscountLostCommand

**File:** `Accounting.Application/AccountsPayableAccounts/RecordDiscountLost/v1/RecordDiscountLostCommand.cs`

```csharp
// Current
public sealed record RecordDiscountLostCommand(
    DefaultIdType Id, 
    decimal DiscountAmount
) : IRequest<DefaultIdType>;

// Rename to
public sealed record RecordAPDiscountLostCommand(
    DefaultIdType Id, 
    decimal DiscountAmount
) : IRequest<DefaultIdType>;
```

**Also update:**
- `RecordDiscountLostHandler.cs` → `RecordAPDiscountLostHandler.cs`
- `RecordDiscountLostCommandValidator.cs` → `RecordAPDiscountLostCommandValidator.cs`
- Related endpoint

---

### Change #6: AccountReconciliations.ReconcileAccountCommand

**File:** `Accounting.Application/AccountReconciliations/Commands/ReconcileAccount/v1/ReconcileAccountCommand.cs`

```csharp
// Current
public class ReconcileAccountCommand : BaseRequest, IRequest<DefaultIdType>

// Rename to
public class ReconcileGeneralLedgerAccountCommand : BaseRequest, IRequest<DefaultIdType>
```

**Also update:**
- `ReconcileAccountCommandHandler.cs` → `ReconcileGeneralLedgerAccountCommandHandler.cs`
- Related endpoint

---

## Summary of Changes Needed

| Feature | Commands to Rename | Priority |
|---------|-------------------|----------|
| **Cost Centers** | 2 commands | 🔥 HIGH |
| **AR Accounts** | 2 commands | 🔶 MEDIUM |
| **AP Accounts** | 1 command | 🔶 MEDIUM |
| **Account Reconciliations** | 1 command | 🔶 MEDIUM |
| **Total** | **6 commands** | |

---

## Files That Will Be Modified

### Per Command (6 commands × 3-4 files each)

For each command, we need to rename:
1. Command file (.cs)
2. Handler file (.cs)
3. Validator file (.cs) - if exists
4. Endpoint file (.cs) - if exists

**Estimated total files:** 18-24 files

---

## Benefits of Renaming

### 1. ✅ Eliminates Namespace Conflicts
- `CostCenters.UpdateBudgetCommand` vs `Budgets.UpdateBudgetCommand` resolved

### 2. ✅ Clear Feature Context
- `RecordCollectionCommand` → `RecordARCollectionCommand` (now obvious it's AR)
- `RecordActualCommand` → `RecordCostCenterActualCommand` (now obvious it's Cost Center)

### 3. ✅ Better IntelliSense
- When searching for AR commands, all will show together
- When searching for AP commands, all will show together
- When searching for Cost Center commands, all will show together

### 4. ✅ Prevents Developer Confusion
- No ambiguity about which feature a command belongs to
- Reduces time spent searching for the right command

### 5. ✅ Consistent with Recent Changes
- Follows the same pattern we just applied to FiscalPeriodClose commands

---

## No Changes Needed

The following are already well-named:

### ✅ Commands with Clear Context
- All Bank commands (BankCreateCommand, BankUpdateCommand, etc.)
- All Bill commands (BillCreateCommand, ApproveBillCommand, etc.)
- All Invoice commands (CreateInvoiceCommand, ApplyInvoicePaymentCommand, etc.)
- All Journal Entry commands (PostJournalEntryCommand, ApproveJournalEntryCommand, etc.)
- All Bank Reconciliation commands (CompleteBankReconciliationCommand, etc.)
- All Fixed Asset commands (DepreciateFixedAssetCommand, etc.)
- All Prepaid Expense commands (RecordAmortizationCommand, ClosePrepaidExpenseCommand, etc.)
- All Deferred Revenue commands (RecognizeDeferredRevenueCommand, etc.)
- All Trial Balance commands (TrialBalanceCreateCommand, TrialBalanceFinalizeCommand, etc.)
- All Fiscal Period Close commands (CompleteFiscalPeriodCloseCommand, etc.)

**These commands are clear and don't need changes!** ✅

---

## Conclusion

Out of 400+ commands in the Accounting module, only **6 commands** need renaming for better clarity:

1. 🔥 `UpdateBudgetCommand` → `UpdateCostCenterBudgetCommand` (HIGH - conflict)
2. 🔶 `RecordActualCommand` → `RecordCostCenterActualCommand`
3. 🔶 `RecordCollectionCommand` → `RecordARCollectionCommand`
4. 🔶 `RecordWriteOffCommand` → `RecordARWriteOffCommand`
5. 🔶 `RecordDiscountLostCommand` → `RecordAPDiscountLostCommand`
6. 🔶 `ReconcileAccountCommand` → `ReconcileGeneralLedgerAccountCommand`

The vast majority of commands (99%) are already well-named and follow good conventions!

---

**Analysis Date:** November 8, 2025  
**Total Commands Analyzed:** 400+  
**Commands Needing Rename:** 6 (1.5%)  
**Recommendation:** Proceed with renaming the 6 identified commands

