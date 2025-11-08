# Approval Workflow Implementation - Accounting Module

## Overview
Updated accounting entities to use `AuditableEntityWithApproval` base class for entities requiring approval workflows. This provides a consistent approach to tracking approval, prepared by, reviewed by, and recommended by information across all entities.

## Changes Summary

### Entities Updated to Use `AuditableEntityWithApproval`

The following entities now inherit from `AuditableEntityWithApproval` instead of `AuditableEntity`:

#### 1. **JournalEntry** (`Accounting.Domain.Entities`)
- **Reason**: Manual journal entries require approval before posting to general ledger
- **Changes**:
  - Removed duplicate `ApprovalStatus`, `ApprovedBy`, and `ApprovedDate` properties
  - Updated `Approve()` and `Reject()` methods to use base class properties (`Status`, `ApprovedBy`, `ApproverName`, `ApprovedOn`)
  - Base class provides full approval workflow tracking with PreparedBy, ReviewedBy, RecommendedBy, and ApprovedBy

#### 2. **Bill** (`Accounting.Domain.Entities`)
- **Reason**: Vendor bills require approval before payment processing
- **Changes**:
  - Removed duplicate `ApprovalStatus`, `ApprovedBy`, and `ApprovedDate` properties
  - Updated `Approve()` and `Reject()` methods to use base class properties
  - Updated `Post()` method to check `Status` field instead of `ApprovalStatus`
  - Added `Remarks` field population in `Reject()` method

#### 3. **Budget** (`Accounting.Domain.Entities`)
- **Reason**: Budgets require approval before activation
- **Changes**:
  - Removed duplicate `ApprovedDate` and `ApprovedBy` properties
  - Updated `Approve()` method to use base class properties (`ApprovedOn`, `ApprovedBy`, `ApproverName`)
  - Status field from base class tracks: Draft, Approved, Active, Closed

#### 4. **BankReconciliation** (`Accounting.Domain.Entities`)
- **Reason**: Bank reconciliations require approval before finalizing
- **Changes**:
  - Inherited from `AuditableEntityWithApproval`
  - Provides approval workflow for monthly/periodic reconciliation processes

#### 5. **PostingBatch** (`Accounting.Domain.Entities`)
- **Reason**: Batch postings require approval before posting to general ledger
- **Changes**:
  - Removed duplicate `ApprovalStatus`, `ApprovedBy`, and `ApprovedDate` properties
  - Removed local `Status` property declaration (uses base class property)
  - Updated `Approve()` and `Reject()` methods to use base class properties
  - Updated `Post()` method to check approval status correctly
  - Fixed constructor to emit `PostingBatchCreated` event with Description parameter

#### 6. **CreditMemo** (`Accounting.Domain.Entities`)
- **Reason**: Credit memos require approval before applying to customer/vendor accounts
- **Changes**:
  - Inherited from `AuditableEntityWithApproval`
  - Provides approval workflow for billing corrections and adjustments

#### 7. **Accrual** (`Accounting.Domain.Entities`)
- **Reason**: Period-end accruals require approval for accurate financial reporting
- **Changes**:
  - Inherited from `AuditableEntityWithApproval`
  - Provides approval workflow for accrual adjustments

#### 8. **FixedAsset** (`Accounting.Domain.Entities`)
- **Reason**: Capital asset purchases typically require approval
- **Changes**:
  - Inherited from `AuditableEntityWithApproval`
  - Provides approval workflow for asset acquisition and disposal

### Entities Remaining with `AuditableEntity`

The following entities do NOT require approval workflows and remain with `AuditableEntity`:

- **GeneralLedger** - Read-only, generated from approved journal entries
- **Invoice** - Customer invoices generated from billing system
- **Check** - Check registration; approval is at payment level
- **PrepaidExpense** - Informational tracking
- **Master Data Entities** - ChartOfAccount, Vendor, Customer, TaxCode, Meter, DepreciationMethod, etc.

### Application Layer Updates

Updated application layer to use base class property names:

#### PostingBatches
- **Files Updated**:
  - `PostingBatches/Queries/SearchPostingBatchesSpec.cs` - Changed `ApprovalStatus` to `Status`
  - `PostingBatches/Get/v1/PostingBatchGetHandler.cs` - Changed to use `Status`, `ApproverName`, `ApprovedOn`
  - `PostingBatches/Search/v1/PostingBatchSearchResponse.cs` - Changed to use `Status`
  - `PostingBatches/Search/v1/PostingBatchSearchSpec.cs` - Changed to use `Status`
  - `PostingBatches/Create/v1/PostingBatchCreateHandler.cs` - Changed to use `Status`
  - `PostingBatches/Handlers/GetPostingBatchByIdHandler.cs` - Changed to use `Status`, `ApproverName`, `ApprovedOn`

#### Bills
- **Files Updated**:
  - `Bills/Queries/BillSpecs.cs` - Changed `ApprovalStatus` to `Status`
  - `Bills/Search/v1/SearchBillsSpec.cs` - Changed to use `Status`

#### JournalEntries
- **Files Updated**:
  - `JournalEntries/Search/SearchJournalEntriesSpec.cs` - Changed `ApprovalStatus` to `Status`

#### Budgets
- **Files Updated**:
  - `Budgets/Responses/BudgetResponse.cs` - Changed `ApprovedDate` to `ApprovedOn` and `ApprovedBy` to `ApproverName`

## Base Class Properties Available

The `AuditableEntityWithApproval` base class provides the following properties for approval workflows:

### Workflow Status
- `Status` (string) - Current workflow status (Pending, Approved, Rejected, etc.)
- `Request` (string?) - Optional request payload
- `Feedback` (string?) - Short feedback from reviewers
- `Remarks` (string?) - Optional remarks about approval decision

### Preparation Tracking
- `PreparedBy` (DefaultIdType?) - User who prepared the request
- `PreparerName` (string?) - Readable name of preparer
- `PreparedOn` (DateTime?) - When prepared

### Review Tracking
- `ReviewedBy` (DefaultIdType?) - User who reviewed
- `ReviewerName` (string?) - Readable name of reviewer
- `ReviewedOn` (DateTime?) - When reviewed

### Recommendation Tracking
- `RecommendedBy` (DefaultIdType?) - User who recommended
- `RecommenderName` (string?) - Readable name of recommender
- `RecommendedOn` (DateTime?) - When recommended

### Approval Tracking
- `ApprovedBy` (DefaultIdType?) - User who approved
- `ApproverName` (string?) - Readable name of approver
- `ApprovedOn` (DateTime?) - When approved

## Benefits

1. **Consistency**: All approval workflows follow the same pattern
2. **Audit Trail**: Complete tracking of who prepared, reviewed, recommended, and approved
3. **Flexibility**: Support for multi-level approval workflows (prepare → review → recommend → approve)
4. **Extensibility**: Easy to add new entities requiring approval
5. **Database Migration**: Will require migration to add new approval columns to existing tables

## Next Steps

1. **Database Migration**: Generate and apply migrations for the new approval columns
2. **Update UI**: Modify Blazor components to show and allow editing of approval workflow fields
3. **Permissions**: Implement authorization policies for different approval levels
4. **Workflows**: Implement business logic for approval routing and notifications
5. **Testing**: Add unit and integration tests for approval workflows

## Migration Notes

When generating migrations, the following new columns will be added to tables for entities using `AuditableEntityWithApproval`:

- `Request` (VARCHAR 1024, nullable)
- `Feedback` (VARCHAR 32, nullable)
- `Status` (VARCHAR 32, nullable) - replaces or extends existing status columns
- `Remarks` (VARCHAR 1024, nullable)
- `PreparedBy` (Guid, nullable)
- `PreparerName` (VARCHAR 1024, nullable)
- `PreparedOn` (DateTime, nullable)
- `ReviewedBy` (Guid, nullable)
- `ReviewerName` (VARCHAR 1024, nullable)
- `ReviewedOn` (DateTime, nullable)
- `RecommendedBy` (Guid, nullable)
- `RecommenderName` (VARCHAR 1024, nullable)
- `RecommendedOn` (DateTime, nullable)
- `ApprovedBy` (Guid, nullable) - replaces existing string ApprovedBy
- `ApproverName` (VARCHAR 1024, nullable)
- `ApprovedOn` (DateTime, nullable) - replaces existing ApprovedDate

**Note**: Some entities already had `ApprovedBy` (string) and `ApprovedDate` (DateTime) fields that will be replaced by the base class fields with different types.

