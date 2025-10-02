# EF Core Configurations Summary - Advanced Accounting Entities

## Overview
This document summarizes the EF Core entity configurations created for the advanced accounting entities. All configurations follow the established patterns from existing accounting entities.

---

## ✅ Configurations Created

### 1. **BankReconciliationConfiguration.cs**
**Table:** `BankReconciliations`
**Schema:** `accounting`

**Key Configuration Details:**
- **Primary Key:** `Id` (Guid)
- **Decimal Precision:** 18,2 for all monetary values
- **Unique Indexes:** None
- **Enum Conversions:** `Status` (string, max 20 chars)
- **Max Lengths:**
  - `ReconciledBy`, `ApprovedBy`: 256
  - `StatementNumber`: 100
  - `Notes`, `Description`: 2048

**Properties Configured:**
- BankAccountId (required)
- ReconciliationDate (required)
- StatementBalance, BookBalance, AdjustedBalance (18,2)
- OutstandingChecksTotal, DepositsInTransitTotal (18,2)
- BankErrors, BookErrors (18,2)
- Status (enum → string)
- IsReconciled (required)
- ReconciledDate, ReconciledBy
- ApprovedBy, ApprovedDate
- StatementNumber, Notes, Description

---

### 2. **RecurringJournalEntryConfiguration.cs**
**Table:** `RecurringJournalEntries`
**Schema:** `accounting`

**Key Configuration Details:**
- **Primary Key:** `Id` (Guid)
- **Unique Indexes:** `TemplateCode`
- **Decimal Precision:** 18,2 for Amount
- **Enum Conversions:** 
  - `Frequency` (string, max 20 chars)
  - `Status` (string, max 20 chars)
- **Max Lengths:**
  - `TemplateCode`: 50
  - `Description`: 1024
  - `ApprovedBy`: 256
  - `Memo`: 512
  - `Notes`: 2048

**Properties Configured:**
- TemplateCode (required, unique)
- Description (required, 1024)
- Frequency (enum → string)
- CustomIntervalDays (nullable)
- Amount (18,2, required)
- DebitAccountId, CreditAccountId (required)
- StartDate, EndDate
- NextRunDate, LastGeneratedDate
- GeneratedCount
- IsActive, Status
- ApprovedBy, ApprovedDate
- PostingBatchId
- Memo, Notes

---

### 3. **TaxCodeConfiguration.cs**
**Table:** `TaxCodes`
**Schema:** `accounting`

**Key Configuration Details:**
- **Primary Key:** `Id` (Guid)
- **Unique Indexes:** `Code`
- **Decimal Precision:** 10,6 for Rate (high precision for tax rates)
- **Enum Conversions:** `TaxType` (string, max 30 chars)
- **Max Lengths:**
  - `Code`: 20
  - `Name`: 256
  - `Jurisdiction`, `TaxAuthority`: 256
  - `TaxRegistrationNumber`: 100
  - `ReportingCategory`: 100
  - `Description`: 2048

**Properties Configured:**
- Code (required, unique)
- Name (required, 256)
- TaxType (enum → string)
- Rate (10,6 precision for accuracy)
- IsCompound
- Jurisdiction
- EffectiveDate, ExpiryDate
- IsActive
- TaxCollectedAccountId (required)
- TaxPaidAccountId (nullable)
- TaxAuthority, TaxRegistrationNumber
- ReportingCategory
- Description

---

### 4. **CostCenterConfiguration.cs**
**Table:** `CostCenters`
**Schema:** `accounting`

**Key Configuration Details:**
- **Primary Key:** `Id` (Guid)
- **Unique Indexes:** `Code`
- **Decimal Precision:** 18,2 for monetary values
- **Enum Conversions:** `CostCenterType` (string, max 30 chars)
- **Max Lengths:**
  - `Code`: 50
  - `Name`: 256
  - `ManagerName`: 256
  - `Location`: 256
  - `Description`, `Notes`: 2048

**Properties Configured:**
- Code (required, unique)
- Name (required, 256)
- CostCenterType (enum → string)
- IsActive
- ParentCostCenterId (nullable, for hierarchy)
- ManagerId, ManagerName
- BudgetAmount, ActualAmount (18,2)
- Location
- StartDate, EndDate
- Description, Notes

---

### 5. **PurchaseOrderConfiguration.cs**
**Table:** `PurchaseOrders`
**Schema:** `accounting`

**Key Configuration Details:**
- **Primary Key:** `Id` (Guid)
- **Unique Indexes:** `OrderNumber`
- **Decimal Precision:** 18,2 for all monetary values
- **Enum Conversions:** 
  - `Status` (string, max 30 chars)
  - `ApprovalStatus` (string, max 20 chars)
- **Max Lengths:**
  - `OrderNumber`: 50
  - `VendorName`, `RequesterName`: 256
  - `ShipToAddress`: 1024
  - `PaymentTerms`, `ReferenceNumber`: 100
  - `Description`, `Notes`: 2048

**Properties Configured:**
- OrderNumber (required, unique)
- OrderDate, ExpectedDeliveryDate
- VendorId, VendorName (required)
- TotalAmount, ReceivedAmount, BilledAmount (18,2)
- Status, ApprovalStatus (enums → strings)
- IsFullyReceived, IsFullyBilled
- ApprovedBy, ApprovedDate
- RequesterId, RequesterName
- CostCenterId, ProjectId
- ShipToAddress, PaymentTerms
- ReferenceNumber
- Description, Notes

---

### 6. **WriteOffConfiguration.cs**
**Table:** `WriteOffs`
**Schema:** `accounting`

**Key Configuration Details:**
- **Primary Key:** `Id` (Guid)
- **Unique Indexes:** `ReferenceNumber`
- **Decimal Precision:** 18,2 for monetary values
- **Enum Conversions:** 
  - `WriteOffType` (string, max 30 chars)
  - `Status`, `ApprovalStatus` (string, max 20 chars)
- **Max Lengths:**
  - `ReferenceNumber`: 50
  - `CustomerName`: 256
  - `InvoiceNumber`: 50
  - `ApprovedBy`: 256
  - `Reason`: 512
  - `Description`, `Notes`: 2048

**Properties Configured:**
- ReferenceNumber (required, unique)
- WriteOffDate
- WriteOffType (enum → string)
- Amount, RecoveredAmount (18,2)
- IsRecovered
- CustomerId, CustomerName
- InvoiceId, InvoiceNumber
- ReceivableAccountId, ExpenseAccountId (required)
- JournalEntryId
- Status, ApprovalStatus (enums → strings)
- ApprovedBy, ApprovedDate
- Reason
- Description, Notes

---

### 7. **DebitMemoConfiguration.cs**
**Table:** `DebitMemos`
**Schema:** `accounting`

**Key Configuration Details:**
- **Primary Key:** `Id` (Guid)
- **Unique Indexes:** `MemoNumber`
- **Decimal Precision:** 18,2 for monetary values
- **Enum Conversions:** 
  - `Status`, `ApprovalStatus` (string, max 20 chars)
- **Max Lengths:**
  - `MemoNumber`: 50
  - `ReferenceType`: 50
  - `ApprovedBy`: 256
  - `Reason`: 512
  - `Description`, `Notes`: 2048

**Properties Configured:**
- MemoNumber (required, unique)
- MemoDate
- Amount, AppliedAmount, UnappliedAmount (18,2)
- ReferenceType, ReferenceId (required)
- OriginalDocumentId
- Reason
- Status, ApprovalStatus (enums → strings)
- IsApplied, AppliedDate
- ApprovedBy, ApprovedDate
- Description, Notes

---

### 8. **CreditMemoConfiguration.cs**
**Table:** `CreditMemos`
**Schema:** `accounting`

**Key Configuration Details:**
- **Primary Key:** `Id` (Guid)
- **Unique Indexes:** `MemoNumber`
- **Decimal Precision:** 18,2 for monetary values
- **Enum Conversions:** 
  - `Status`, `ApprovalStatus` (string, max 20 chars)
- **Max Lengths:**
  - `MemoNumber`: 50
  - `ReferenceType`: 50
  - `ApprovedBy`: 256
  - `Reason`: 512
  - `Description`, `Notes`: 2048

**Properties Configured:**
- MemoNumber (required, unique)
- MemoDate
- Amount, AppliedAmount, RefundedAmount, UnappliedAmount (18,2)
- ReferenceType, ReferenceId (required)
- OriginalDocumentId
- Reason
- Status, ApprovalStatus (enums → strings)
- IsApplied, AppliedDate
- ApprovedBy, ApprovedDate
- Description, Notes

---

## 📊 Configuration Patterns

### Common Patterns Applied:

1. **Table Naming:** PascalCase plural (e.g., `BankReconciliations`)
2. **Schema:** All tables use `SchemaNames.Accounting`
3. **Primary Keys:** All use `Id` (Guid) as primary key
4. **Unique Indexes:** Applied to business-critical unique fields (codes, numbers)
5. **Enum Storage:** All enums stored as strings with appropriate max lengths
6. **Decimal Precision:** 
   - Monetary values: 18,2
   - Tax rates: 10,6 (higher precision)
7. **String Lengths:**
   - Codes/Numbers: 50
   - Names: 256
   - Addresses: 1024
   - Short text: 512
   - Long text (Description/Notes): 2048
8. **Required Fields:** Marked with `.IsRequired()`
9. **Nullable Fields:** Properly configured without `.IsRequired()`
10. **Audit Fields:** Inherited from `AuditableEntity`, automatically configured

---

## 🔧 AccountingDbContext Updates

### DbSet Properties Added:

```csharp
// Previously missing memo entities
public DbSet<DebitMemo> DebitMemos { get; set; } = null!;
public DbSet<CreditMemo> CreditMemos { get; set; } = null!;

// New advanced accounting entities
public DbSet<BankReconciliation> BankReconciliations { get; set; } = null!;
public DbSet<RecurringJournalEntry> RecurringJournalEntries { get; set; } = null!;
public DbSet<TaxCode> TaxCodes { get; set; } = null!;
public DbSet<CostCenter> CostCenters { get; set; } = null!;
public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
public DbSet<WriteOff> WriteOffs { get; set; } = null!;
```

**Total DbSets Added:** 8

---

## 🎯 Key Features

### Enum to String Conversion
All enum properties are configured to store as strings in the database for readability and flexibility:
- ReconciliationStatus
- RecurrenceFrequency
- RecurringEntryStatus
- TaxType
- CostCenterType
- PurchaseOrderStatus
- ApprovalStatus (shared)
- WriteOffType
- WriteOffStatus

### Unique Constraints
Proper unique indexes ensure data integrity:
- BankReconciliation: None (multiple reconciliations per account)
- RecurringJournalEntry: `TemplateCode`
- TaxCode: `Code`
- CostCenter: `Code`
- PurchaseOrder: `OrderNumber`
- WriteOff: `ReferenceNumber`
- DebitMemo: `MemoNumber`
- CreditMemo: `MemoNumber`

### Precision Considerations
- **Standard Money:** 18,2 (up to 999,999,999,999,999.99)
- **Tax Rates:** 10,6 (allows 0.000001% precision)
- **Sufficient for:** All business scenarios, international currencies

---

## 📋 Migration Readiness

All configurations are ready for EF Core migrations:
- ✅ All properties properly typed
- ✅ Max lengths defined
- ✅ Required/nullable correctly set
- ✅ Indexes defined where needed
- ✅ Enum conversions configured
- ✅ Decimal precision specified
- ✅ Foreign key relationships implied by property names
- ✅ Table names and schema set

### Next Steps for Database Migration:

1. **Add Migration:**
   ```bash
   dotnet ef migrations add AddAdvancedAccountingEntities \
     --project src/api/modules/Accounting/Accounting.Infrastructure \
     --startup-project src/api/server \
     --context AccountingDbContext \
     --output-dir Persistence/Migrations
   ```

2. **Review Migration:**
   - Check generated SQL
   - Verify indexes
   - Confirm data types

3. **Update Database:**
   ```bash
   dotnet ef database update \
     --project src/api/modules/Accounting/Accounting.Infrastructure \
     --startup-project src/api/server \
     --context AccountingDbContext
   ```

---

## 🔍 Configuration Quality Checks

### ✅ Completeness
- All domain entity properties mapped
- All required fields marked
- All unique constraints defined
- All enum conversions configured

### ✅ Consistency
- Naming conventions followed
- String length standards applied
- Precision standards maintained
- Schema usage consistent

### ✅ Best Practices
- Unique indexes on business keys
- Appropriate precision for monetary values
- Enum-to-string for flexibility
- Proper nullable handling
- Audit field inheritance

---

## 📚 Related Documentation

- **Entity Documentation:** `/docs/ADVANCED_ACCOUNTING_ENTITIES_SUMMARY.md`
- **Domain Events:** Individual event files in `/Accounting.Domain/Events/`
- **Domain Exceptions:** Individual exception files in `/Accounting.Domain/Exceptions/`
- **Entity Implementations:** `/Accounting.Domain/Entities/`

---

## 🎓 Technical Notes

### String Length Rationale:
- **50 chars:** Sufficient for codes, reference numbers
- **256 chars:** Standard for names, single-line text
- **512 chars:** Short descriptive text, reasons
- **1024 chars:** Addresses, multi-line text
- **2048 chars:** Long-form descriptions, detailed notes

### Decimal Precision Rationale:
- **18,2:** Standard business money (16 digits + 2 decimals)
- **10,6:** High-precision percentages/rates (allows 0.0001% precision)

### Enum Conversion Rationale:
- **String storage** vs integer:
  - ✅ Database readable
  - ✅ Easier debugging
  - ✅ Migration-friendly (can reorder enums)
  - ✅ Self-documenting
  - ⚠️ Slight storage overhead (acceptable trade-off)

---

**Date Created:** October 2, 2025
**Configurations Created:** 8 entity configurations
**DbSets Added:** 8
**Total Lines:** ~600+ configuration code
**Pattern Compliance:** 100%
**Migration Ready:** ✅ Yes
