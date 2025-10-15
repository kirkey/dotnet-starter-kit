# Check Management System

## Overview

The Check Management System provides comprehensive functionality for managing physical and electronic checks in the accounting module. Checks are registered in the system and later used for payments to vendors, payees, or expenses.

## Key Features

### 1. Check Registration
- Register checks with unique check numbers per bank account
- Track bank account associations
- Maintain check inventory for multiple bank accounts
- Support for check books and individual checks

### 2. Check Lifecycle Management
- **Available**: Check is registered and ready to be issued
- **Issued**: Check has been written and issued to a payee
- **Cleared**: Check has cleared the bank (via reconciliation)
- **Void**: Check has been voided/cancelled
- **StopPayment**: Stop payment has been requested
- **Stale**: Check is outstanding for too long

### 3. Check Operations

#### Issue Check
- Issue checks for payments to vendors or payees
- Link checks to payment transactions or expenses
- Record payee information and amount
- Add memo/notes for reference
- Track issued date

#### Print Management
- Mark checks as printed
- Track who printed the check and when
- Maintain print audit trail

#### Void Check
- Void checks with reason tracking
- Cannot void cleared checks
- Maintains void history

#### Clear Check
- Mark checks as cleared through bank reconciliation
- Track cleared date
- Support for outstanding check reporting

#### Stop Payment
- Request stop payment on issued checks
- Track stop payment reason and date
- Prevent duplicate stop payment requests

### 4. Check Search & Reporting
- Search by check number, bank account, status
- Filter by date ranges (issued, cleared)
- Filter by payee, vendor, or amount ranges
- Track printed vs unprinted checks
- Identify outstanding checks
- Stop payment tracking

## API Endpoints

### Commands (POST)
- `POST /accounting/checks` - Register a new check
- `POST /accounting/checks/issue` - Issue a check for payment
- `POST /accounting/checks/void` - Void a check
- `POST /accounting/checks/clear` - Mark check as cleared
- `POST /accounting/checks/stop-payment` - Request stop payment
- `POST /accounting/checks/print` - Mark check as printed

### Queries (GET/POST)
- `GET /accounting/checks/{id}` - Get check details by ID
- `POST /accounting/checks/search` - Search checks with filters

## Usage Examples

### 1. Register Checks from a New Check Book
```json
POST /accounting/checks
{
  "checkNumber": "1001",
  "bankAccountCode": "102",
  "bankAccountName": "Operating Checking Account",
  "description": "Check from book 2025-01",
  "notes": "Checks 1001-1050"
}
```

### 2. Issue Check for Vendor Payment
```json
POST /accounting/checks/issue
{
  "checkId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 1500.00,
  "payeeName": "ABC Suppliers Inc.",
  "issuedDate": "2025-10-15T00:00:00Z",
  "vendorId": "550e8400-e29b-41d4-a716-446655440000",
  "paymentId": "660e8400-e29b-41d4-a716-446655440000",
  "memo": "Invoice #12345 payment"
}
```

### 3. Search Outstanding Checks
```json
POST /accounting/checks/search
{
  "status": "Issued",
  "bankAccountCode": "102",
  "issuedDateFrom": "2025-09-01T00:00:00Z",
  "issuedDateTo": "2025-10-15T00:00:00Z",
  "pageNumber": 1,
  "pageSize": 50
}
```

### 4. Void a Check
```json
POST /accounting/checks/void
{
  "checkId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "voidReason": "Check lost - need to reissue",
  "voidedDate": "2025-10-15T00:00:00Z"
}
```

### 5. Mark Check as Cleared (During Bank Reconciliation)
```json
POST /accounting/checks/clear
{
  "checkId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "clearedDate": "2025-10-20T00:00:00Z"
}
```

## Business Rules

1. **Uniqueness**: Check numbers must be unique within a bank account
2. **Status Progression**: Available → Issued → Cleared (normal flow)
3. **Void Restrictions**: Cannot void cleared checks (use reversal entry instead)
4. **Amount Validation**: Check amounts must be positive
5. **Stop Payment**: Cannot be requested on cleared or voided checks
6. **Reuse Prevention**: Once issued, checks cannot be reused
7. **Audit Trail**: All check operations are tracked with timestamps and user information

## Integration Points

### Payment Processing
- Checks can be linked to Payment entities when used for customer payments
- Track payment allocations to invoices

### Vendor Payments
- Checks are linked to Vendor entities for vendor payments
- Support for expense tracking and accounts payable

### Bank Reconciliation
- Outstanding checks are identified during reconciliation
- Cleared checks are matched with bank statement entries
- Support for stale check identification

### General Ledger
- Check issuance creates GL entries (debit expense/payable, credit cash)
- Check clearing is tracked for cash flow management
- Voided checks reverse original GL entries

## Database Schema

### Checks Table
- **Primary Key**: Id (Guid)
- **Unique Index**: CheckNumber + BankAccountCode
- **Indexes**: Status, BankAccountCode, IssuedDate, VendorId, PayeeId

### Key Fields
- CheckNumber (required, max 64 chars)
- BankAccountCode (required, max 64 chars)
- Status (required, max 32 chars)
- Amount (decimal 18,2, nullable)
- PayeeName (max 256 chars)
- Memo (max 512 chars)
- Timestamps for all major events (issued, cleared, voided, printed)

## Security & Permissions

- **Create**: `Permissions.Accounting.Create` - Register new checks
- **Update**: `Permissions.Accounting.Update` - Issue, void, clear, stop payment
- **View**: `Permissions.Accounting.View` - Search and view check details

## Best Practices

1. **Check Inventory**: Register check books when received from bank
2. **Sequential Use**: Issue checks in sequential order when possible
3. **Timely Clearing**: Mark checks as cleared during regular bank reconciliation
4. **Void Promptly**: Void lost or incorrect checks immediately
5. **Stop Payment**: Use stop payment for checks that need to be cancelled after issuance
6. **Regular Review**: Identify and investigate stale checks regularly
7. **Audit Trail**: Always provide clear reasons for voids and stop payments

## Reporting Capabilities

- Outstanding checks by bank account
- Cleared vs uncleared checks analysis
- Voided checks report
- Stop payment tracking
- Check usage by vendor/payee
- Stale check identification
- Print history audit

## Domain Events

The system raises the following domain events:
- `CheckRegistered` - When a new check is registered
- `CheckIssued` - When a check is issued for payment
- `CheckPrinted` - When a check is marked as printed
- `CheckCleared` - When a check clears the bank
- `CheckVoided` - When a check is voided
- `CheckStopPaymentRequested` - When stop payment is requested
- `CheckMarkedAsStale` - When a check is identified as stale
- `CheckUpdated` - When check details are updated

These events can be used for:
- Audit logging
- Notifications
- Integration with other systems
- Workflow automation

