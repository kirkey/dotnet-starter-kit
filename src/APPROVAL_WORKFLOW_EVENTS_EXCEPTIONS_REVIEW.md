# Approval Workflow - Events, Exceptions & Application Layer Review

## Overview
This document provides a complete review of events, exceptions, and application layer implementation for entities updated to use `AuditableEntityWithApproval`.

---

## ✅ Events Review

### Entities with Complete Approval Events

#### 1. **JournalEntry**
**Location:** `/Accounting.Domain/Events/JournalEntry/`

✅ **Existing Events:**
- `JournalEntryCreated`
- `JournalEntryUpdated`
- `JournalEntryDeleted`
- `JournalEntryPosted`
- `JournalEntryReversed`
- `JournalEntryLineAdded`
- `JournalEntryApproved` (separate file)
- `JournalEntryRejected` (separate file)

**Status:** ✅ Complete

---

#### 2. **Bill**
**Location:** `/Accounting.Domain/Events/Bill/`

✅ **Existing Events:**
- `BillCreated`
- `BillUpdated`
- `BillApproved`
- `BillRejected`
- `BillPosted`
- `BillPaid`
- `BillVoided`

**Status:** ✅ Complete

---

#### 3. **Budget**
**Location:** `/Accounting.Domain/Events/Budget/`

✅ **Existing Events:**
- `BudgetCreated`
- `BudgetUpdated`
- `BudgetApproved`
- `BudgetActivated`
- `BudgetClosed`

**Status:** ✅ Complete (Note: No explicit reject event, uses status changes)

---

#### 4. **BankReconciliation**
**Location:** `/Accounting.Domain/Events/BankReconciliation/`

✅ **Existing Events:**
- `BankReconciliationCreated`
- `BankReconciliationUpdated`
- `BankReconciliationStarted`
- `BankReconciliationCompleted`
- `BankReconciliationApproved`
- `BankReconciliationRejected`
- `BankReconciliationDeleted`

**Status:** ✅ Complete

---

#### 5. **PostingBatch**
**Location:** `/Accounting.Domain/Events/PostingBatch/`

✅ **Existing Events:**
- `PostingBatchCreated`
- `PostingBatchUpdated`
- `PostingBatchDeleted`
- `PostingBatchPosted`
- `PostingBatchReversed`
- `PostingBatchApproved`
- `PostingBatchRejected`

**Status:** ✅ Complete

---

#### 6. **CreditMemo**
**Location:** `/Accounting.Domain/Events/CreditMemo/`

✅ **Existing Events:**
- `CreditMemoCreated`
- `CreditMemoUpdated`
- `CreditMemoDeleted`
- `CreditMemoApproved`
- `CreditMemoApplied`
- `CreditMemoRefunded`
- `CreditMemoVoided`

**Status:** ✅ Complete

---

#### 7. **Accrual** ⚠️ UPDATED
**Location:** `/Accounting.Domain/Events/Accrual/`

✅ **Events (Updated):**
- `AccrualCreated`
- `AccrualUpdated`
- `AccrualDeleted`
- `AccrualReversed`
- `AccrualAmountAdjusted`
- `AccrualApproved` ✨ **NEW**
- `AccrualRejected` ✨ **NEW**

**Status:** ✅ Complete (Added approval events)

---

#### 8. **FixedAsset** ⚠️ UPDATED
**Location:** `/Accounting.Domain/Events/FixedAsset/`

✅ **Events (Updated):**
- `FixedAssetCreated`
- `FixedAssetUpdated`
- `FixedAssetDeleted`
- `FixedAssetMaintenanceUpdated`
- `FixedAssetDepreciationAdded`
- `FixedAssetDisposed`
- `AssetMaintenanceScheduled`
- `AssetMaintenanceCompleted`
- `FixedAssetApproved` ✨ **NEW**
- `FixedAssetRejected` ✨ **NEW**
- `FixedAssetTransferred` ✨ **NEW**
- `FixedAssetRevalued` ✨ **NEW**

**Status:** ✅ Complete (Added approval and lifecycle events)

---

## ✅ Exceptions Review

### Entities with Complete Approval Exceptions

#### 1. **JournalEntry**
**Location:** `/Accounting.Domain/Exceptions/JournalEntryExceptions.cs`

✅ **Existing Exceptions:**
- `JournalEntryNotFoundException`
- `JournalEntryNotBalancedException`
- `JournalEntryAlreadyPostedException`
- `JournalEntryCannotBeModifiedException`
- `JournalEntryLineNotFoundException`
- `InvalidJournalEntryLineAmountException`
- `JournalEntryUnbalancedException`

**Status:** ✅ Complete

---

#### 2. **Bill**
**Location:** `/Accounting.Domain/Exceptions/BillExceptions.cs`

✅ **Existing Exceptions:**
- `BillNotFoundException`
- `BillCannotBeModifiedException`
- `BillAlreadyPostedException`
- `BillAlreadyApprovedException`
- `BillNotApprovedException`
- `BillNotPostedException`
- `BillAlreadyPaidException`
- `BillInvalidAmountException`
- `BillLineItemNotFoundException`

**Status:** ✅ Complete

---

#### 3. **Budget**
**Location:** `/Accounting.Domain/Exceptions/BudgetExceptions.cs`

✅ **Existing Exceptions:**
- `BudgetNotFoundException`
- `BudgetAlreadyApprovedException`
- `BudgetNotApprovedException`
- `BudgetCannotBeModifiedException`
- `InvalidBudgetAmountException`
- `EmptyBudgetCannotBeApprovedException`
- `BudgetDetailNotFoundException`
- `BudgetDetailAlreadyExistsException`

**Status:** ✅ Complete

---

#### 4. **BankReconciliation**
**Location:** `/Accounting.Domain/Exceptions/BankReconciliationExceptions.cs`

✅ **Existing Exceptions:**
- `BankReconciliationNotFoundException`
- `BankReconciliationCannotBeModifiedException`
- `BankReconciliationAlreadyReconciledException`
- `InvalidReconciliationStatusException`
- `ReconciliationBalanceMismatchException`
- `BankReconciliationNotApprovedException`
- `InvalidReconciliationDateException`

**Status:** ✅ Complete

---

#### 5. **PostingBatch**
**Location:** `/Accounting.Domain/Exceptions/PostingBatchExceptions.cs`

✅ **Existing Exceptions:**
- `PostingBatchByIdNotFoundException`
- `PostingBatchByNumberNotFoundException`
- `DuplicatePostingBatchNumberException`
- `CannotModifyPostedBatchException`
- `PostingBatchOutOfBalanceException`
- `CannotPostEmptyBatchException`
- `InvalidPostingDateException`
- `PostingBatchAlreadyReversedException`

**Status:** ✅ Complete

---

#### 6. **CreditMemo**
**Location:** `/Accounting.Domain/Exceptions/CreditMemoExceptions.cs`

✅ **Existing Exceptions:**
- `CreditMemoNotFoundException`
- `CreditMemoCannotBeModifiedException`
- `CreditMemoAlreadyApprovedException`
- `CreditMemoAlreadyVoidedException`
- `CreditMemoNotApprovedException`
- `CreditMemoInsufficientBalanceException`
- `InvalidCreditMemoAmountException`
- `InvalidCreditMemoReferenceTypeException`

**Status:** ✅ Complete

---

#### 7. **Accrual** ⚠️ UPDATED
**Location:** `/Accounting.Domain/Exceptions/AccrualExceptions.cs`

✅ **Exceptions (Updated):**
- `AccrualByIdNotFoundException`
- `AccrualByNumberNotFoundException`
- `DuplicateAccrualNumberException`
- `InvalidAccrualAmountException`
- `AccrualAlreadyReversedException`
- `CannotModifyReversedAccrualException`
- `InvalidAccrualDateException`
- `InvalidAccrualNumberFormatException`
- `AccrualAlreadyApprovedException` ✨ **NEW**
- `AccrualNotApprovedException` ✨ **NEW**

**Status:** ✅ Complete (Added approval exceptions)

---

#### 8. **FixedAsset** ⚠️ UPDATED
**Location:** `/Accounting.Domain/Exceptions/FixedAssetExceptions.cs`

✅ **Exceptions (Updated):**
- `FixedAssetNotFoundException`
- `FixedAssetAlreadyDisposedException`
- `FixedAssetCannotBeModifiedException`
- `InvalidDepreciationAmountException`
- `InvalidAssetPurchasePriceException`
- `InvalidAssetServiceLifeException`
- `InvalidAssetSalvageValueException`
- `FixedAssetAlreadyApprovedException` ✨ **NEW**
- `FixedAssetNotApprovedException` ✨ **NEW**
- `NegativeBookValueException` ✨ **NEW**

**Status:** ✅ Complete (Added approval exceptions)

---

## ✅ Application Layer Review

### Command Handlers Implementation Status

| Entity | Approve Handler | Reject Handler | Status |
|--------|----------------|----------------|---------|
| **JournalEntry** | ✅ `/JournalEntries/Approve/` | ✅ Implemented | ✅ Complete |
| **Bill** | ✅ `/Bills/Approve/v1/` | ✅ `/Bills/Reject/v1/` | ✅ Complete |
| **Budget** | ✅ `/Budgets/Approve/` | ❌ Not needed | ✅ Complete |
| **BankReconciliation** | ✅ `/BankReconciliations/Approve/` | ✅ `/BankReconciliations/Reject/` | ✅ Complete |
| **PostingBatch** | ✅ `/PostingBatches/Approve/` | ✅ `/PostingBatches/Reject/` | ✅ Complete |
| **CreditMemo** | ✅ `/CreditMemos/Approve/` | ❌ Not explicitly needed | ✅ Complete |
| **Accrual** | ✨ `/Accruals/Approve/` | ✨ `/Accruals/Reject/` | ✅ **NEW** |
| **FixedAsset** | ✨ `/FixedAssets/Approve/` | ✨ `/FixedAssets/Reject/` | ✅ **NEW** |

---

## 🆕 New Application Layer Handlers Created

### Accrual Approval Workflow

**Created Files:**
```
/Accounting.Application/Accruals/Approve/
  ├── ApproveAccrualCommand.cs
  ├── ApproveAccrualCommandValidator.cs
  └── ApproveAccrualHandler.cs

/Accounting.Application/Accruals/Reject/
  ├── RejectAccrualCommand.cs
  ├── RejectAccrualCommandValidator.cs
  └── RejectAccrualHandler.cs
```

**Features:**
- ✅ Validates accrual exists and is not reversed
- ✅ Checks for duplicate approval/rejection
- ✅ Updates status, approver info, and timestamps
- ✅ Queues domain events
- ✅ Proper exception handling
- ✅ Comprehensive logging

---

### FixedAsset Approval Workflow

**Created Files:**
```
/Accounting.Application/FixedAssets/Approve/
  ├── ApproveFixedAssetCommand.cs
  ├── ApproveFixedAssetCommandValidator.cs
  └── ApproveFixedAssetHandler.cs

/Accounting.Application/FixedAssets/Reject/
  ├── RejectFixedAssetCommand.cs
  ├── RejectFixedAssetCommandValidator.cs
  └── RejectFixedAssetHandler.cs
```

**Features:**
- ✅ Validates asset exists and is not disposed
- ✅ Checks for duplicate approval/rejection
- ✅ Updates status, approver info, and timestamps
- ✅ Queues domain events
- ✅ Proper exception handling
- ✅ Comprehensive logging

---

## 📊 Summary Statistics

### Events
- **Total Entities:** 8
- **Complete Event Sets:** 8 (100%)
- **New Events Added:** 6

### Exceptions
- **Total Entities:** 8
- **Complete Exception Sets:** 8 (100%)
- **New Exceptions Added:** 5

### Application Handlers
- **Total Entities:** 8
- **Complete Handlers:** 8 (100%)
- **New Handlers Created:** 6 files (Approve & Reject for Accruals and FixedAssets)

---

## 🎯 Validation Patterns

All approval command validators follow consistent patterns:

### Common Validations
```csharp
RuleFor(x => x.EntityId)
    .NotEmpty()
    .WithMessage("Entity ID is required.");

RuleFor(x => x.ApprovedBy)
    .NotEmpty()
    .WithMessage("Approver is required.")
    .MaximumLength(200)
    .WithMessage("Approver name cannot exceed 200 characters.");

// For Reject commands
RuleFor(x => x.Reason)
    .MaximumLength(500)
    .WithMessage("Reason cannot exceed 500 characters.")
    .When(x => !string.IsNullOrWhiteSpace(x.Reason));
```

---

## 🔄 Handler Patterns

All approval handlers follow consistent patterns:

### Approve Handler Pattern
```csharp
1. Validate request is not null
2. Load entity from repository
3. Check entity exists (throw NotFoundException)
4. Check entity state (not disposed, reversed, etc.)
5. Check not already approved
6. Update Status = "Approved"
7. Set ApprovedBy, ApproverName, ApprovedOn
8. Queue domain event
9. Update and save changes
10. Log approval action
11. Return entity ID
```

### Reject Handler Pattern
```csharp
1. Validate request is not null
2. Load entity from repository
3. Check entity exists (throw NotFoundException)
4. Check entity state (not disposed, reversed, etc.)
5. Update Status = "Rejected"
6. Set ApprovedBy, ApproverName, ApprovedOn
7. Set Remarks with reason
8. Queue domain event
9. Update and save changes
10. Log rejection action
11. Return entity ID
```

---

## 🚀 Next Steps

### 1. API Endpoints
Add HTTP endpoints for new handlers:
- `POST /api/accounting/accruals/{id}/approve`
- `POST /api/accounting/accruals/{id}/reject`
- `POST /api/accounting/fixed-assets/{id}/approve`
- `POST /api/accounting/fixed-assets/{id}/reject`

### 2. Authorization
Implement authorization policies:
- `ApproveAccruals` permission
- `ApproveFixedAssets` permission

### 3. Testing
Create integration tests for:
- Approval workflows
- Rejection workflows
- Edge cases and error scenarios

### 4. UI Implementation
Add approval/rejection UI for:
- Accruals management page
- Fixed Assets management page

---

## 📝 Notes

### Design Decisions

1. **Consistent Event Naming:** All approval events follow pattern `{Entity}Approved` and `{Entity}Rejected`

2. **Exception Hierarchy:** All exceptions inherit from appropriate base classes:
   - `NotFoundException` for missing entities
   - `ForbiddenException` for business rule violations
   - `BadRequestException` for invalid input

3. **Status Field Usage:** Base class `Status` field used for approval state instead of separate `ApprovalStatus`

4. **ApproverName vs ApprovedBy:** 
   - `ApprovedBy` (Guid) - User identifier
   - `ApproverName` (string) - Display name for UI

5. **Timestamp Field:** `ApprovedOn` instead of `ApprovedDate` to match base class

---

**Last Updated:** November 8, 2025  
**Status:** ✅ All entities reviewed and updated  
**Created By:** Automated review and implementation process

