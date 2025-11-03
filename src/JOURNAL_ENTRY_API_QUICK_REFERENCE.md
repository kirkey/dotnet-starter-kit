# Journal Entry API - Developer Quick Reference

## Overview
The Journal Entry API provides a complete double-entry bookkeeping system with approval workflows, posting to general ledger, and reversal capabilities.

---

## Key Features

✅ **Create journal entries with lines in a single transaction**
✅ **Automatic balance validation** (debits must equal credits)
✅ **Approval workflow** (Pending → Approved → Posted)
✅ **Post to General Ledger** (makes entry immutable)
✅ **Reverse posted entries** (creates offsetting entry)
✅ **Multi-tenant support** with data isolation
✅ **Comprehensive validation** at domain and application layers
✅ **Audit trail** with domain events

---

## Domain Entity: JournalEntry

### Properties
- `Date` - Transaction date
- `ReferenceNumber` - Unique reference (max 32 chars)
- `Source` - Source system/module (max 64 chars)
- `Description` - Entry description (max 1000 chars)
- `IsPosted` - Posted to GL flag
- `ApprovalStatus` - Pending/Approved/Rejected
- `Lines` - Collection of debit/credit lines
- `PeriodId` - Optional accounting period link
- `OriginalAmount` - Reference amount

### Key Methods
```csharp
// Add a line to the entry
journalEntry.AddLine(accountId, debitAmount, creditAmount, description, reference);

// Validate balance
journalEntry.ValidateBalance(); // Throws if not balanced

// Check if balanced
bool isBalanced = journalEntry.IsBalanced();

// Get totals
decimal debits = journalEntry.GetTotalDebits();
decimal credits = journalEntry.GetTotalCredits();
decimal diff = journalEntry.GetDifference();

// Workflow operations
journalEntry.Post();
journalEntry.Approve(approvedBy);
journalEntry.Reject(rejectedBy);
journalEntry.Reverse(reversalDate, reason);
```

---

## API Endpoints

### 1. Create Journal Entry with Lines
**POST** `/accounting/journal-entries`

**Request:**
```json
{
  "date": "2025-11-03T00:00:00Z",
  "referenceNumber": "JE-2025-001",
  "source": "ManualEntry",
  "description": "Monthly expense accrual",
  "periodId": "guid-optional",
  "originalAmount": 1000.00,
  "notes": "Optional notes",
  "lines": [
    {
      "accountId": "account-guid-1",
      "debitAmount": 1000.00,
      "creditAmount": 0,
      "description": "Rent expense",
      "reference": "INV-123"
    },
    {
      "accountId": "account-guid-2",
      "debitAmount": 0,
      "creditAmount": 1000.00,
      "description": "Accrued rent payable",
      "reference": "INV-123"
    }
  ]
}
```

**Validation Rules:**
- Minimum 2 lines required
- Debits must equal credits (within 0.01 tolerance)
- Each line must have either debit OR credit (not both)
- All amounts must be non-negative

---

### 2. Get Journal Entry
**GET** `/accounting/journal-entries/{id}`

**Response:**
```json
{
  "id": "guid",
  "date": "2025-11-03T00:00:00Z",
  "referenceNumber": "JE-2025-001",
  "source": "ManualEntry",
  "description": "Monthly expense accrual",
  "isPosted": false,
  "approvalStatus": "Pending",
  "lines": [
    {
      "id": "line-guid-1",
      "accountId": "account-guid-1",
      "debitAmount": 1000.00,
      "creditAmount": 0,
      "memo": "Rent expense"
    }
  ],
  "totalDebits": 1000.00,
  "totalCredits": 1000.00,
  "difference": 0,
  "isBalanced": true
}
```

---

### 3. Search Journal Entries
**GET** `/accounting/journal-entries?pageNumber=1&pageSize=10`

**Query Parameters:**
- `referenceNumber` - Filter by reference
- `source` - Filter by source
- `fromDate` - Filter by date range
- `toDate` - Filter by date range
- `isPosted` - Filter by posted status
- `approvalStatus` - Filter by approval status (Pending/Approved/Rejected)
- `periodId` - Filter by accounting period
- `orderBy` - Sort order

---

### 4. Update Journal Entry
**PUT** `/accounting/journal-entries/{id}`

**Note:** Only allowed for unposted entries

**Request:**
```json
{
  "id": "guid",
  "date": "2025-11-03T00:00:00Z",
  "referenceNumber": "JE-2025-001-REV",
  "description": "Updated description",
  "source": "ManualEntry",
  "periodId": "guid-optional",
  "originalAmount": 1000.00
}
```

---

### 5. Post to General Ledger
**POST** `/accounting/journal-entries/{id}/post`

**Request:**
```json
{
  "journalEntryId": "guid"
}
```

**Business Rules:**
- Entry must be balanced
- Entry must not already be posted
- Once posted, entry becomes immutable

---

### 6. Approve Journal Entry
**POST** `/accounting/journal-entries/{id}/approve`

**Request:**
```json
{
  "journalEntryId": "guid",
  "approvedBy": "user@example.com"
}
```

**Business Rules:**
- Entry must be balanced
- Cannot approve already approved entry
- Sets approval status to "Approved"

---

### 7. Reject Journal Entry
**POST** `/accounting/journal-entries/{id}/reject`

**Request:**
```json
{
  "journalEntryId": "guid",
  "rejectedBy": "user@example.com"
}
```

**Business Rules:**
- Cannot reject already rejected entry
- Sets approval status to "Rejected"

---

### 8. Reverse Journal Entry
**POST** `/accounting/journal-entries/{id}/reverse`

**Request:**
```json
{
  "journalEntryId": "guid",
  "reversalDate": "2025-11-03T00:00:00Z",
  "reversalReason": "Correction needed for incorrect amount"
}
```

**Business Rules:**
- Only posted entries can be reversed
- Creates new entry with debits/credits swapped
- New entry is automatically posted
- Reference number: `REV-{original-reference}`

---

## Working with Journal Entry Lines

### Create Line (Alternative to creating with entry)
**POST** `/accounting/journal-entry-lines`

```json
{
  "journalEntryId": "guid",
  "accountId": "account-guid",
  "debitAmount": 500.00,
  "creditAmount": 0,
  "memo": "Additional expense",
  "reference": "REF-123"
}
```

### Update Line
**PUT** `/accounting/journal-entry-lines/{id}`

**Note:** Only allowed if parent entry is not posted

### Delete Line
**DELETE** `/accounting/journal-entry-lines/{id}`

**Note:** Only allowed if parent entry is not posted

---

## Typical Workflows

### 1. Standard Journal Entry Flow
```
1. Create entry with lines → Status: Pending, Posted: false
2. Review and approve → Status: Approved, Posted: false
3. Post to GL → Status: Approved, Posted: true
4. Entry is now immutable
```

### 2. Entry Correction Flow
```
1. Posted entry found with error
2. Reverse the entry → Creates offsetting entry
3. Create corrected entry with proper amounts
4. Approve and post corrected entry
```

### 3. Draft Entry Flow
```
1. Create entry with lines → Status: Pending
2. Update if needed (while not posted)
3. Add/remove lines as needed
4. When ready: Approve → Post
```

---

## Exception Handling

### Common Exceptions
- `JournalEntryNotFoundException` - Entry not found (404)
- `JournalEntryNotBalancedException` - Debits ≠ Credits (403)
- `JournalEntryAlreadyPostedException` - Cannot modify posted entry (403)
- `JournalEntryCannotBeModifiedException` - Entry is immutable (403)

### Example Error Response
```json
{
  "status": 403,
  "title": "Forbidden",
  "detail": "journal entry with id {guid} is not balanced",
  "traceId": "..."
}
```

---

## Performance Tips

1. **Use search with filters** instead of loading all entries
2. **Lines are eagerly loaded** - no need for separate queries
3. **Caching is enabled** for Get operations
4. **Indexes exist** on Date, IsPosted, Source, ApprovalStatus
5. **Pagination** is supported for large result sets

---

## Security & Permissions

Required permissions:
- `Permissions.Accounting.View` - Read operations
- `Permissions.Accounting.Create` - Create entries
- `Permissions.Accounting.Update` - Update entries
- `Permissions.Accounting.Delete` - Delete entries
- `Permissions.Accounting.Post` - Post to GL
- `Permissions.Accounting.Approve` - Approve/Reject

---

## Best Practices

1. ✅ Always ensure debits = credits before posting
2. ✅ Use meaningful reference numbers (include year/month)
3. ✅ Provide descriptive entry descriptions
4. ✅ Add line-level memos for clarity
5. ✅ Use consistent source naming conventions
6. ✅ Link to accounting periods for reporting
7. ✅ Never manually modify posted entries
8. ✅ Use reversal feature for corrections
9. ✅ Implement approval workflow for control
10. ✅ Include original amount for audit trail

---

## Database Schema

### JournalEntries Table
```sql
Id (uniqueidentifier, PK)
Date (datetime2)
ReferenceNumber (nvarchar(32), UNIQUE)
Source (nvarchar(64))
Description (nvarchar(1000))
IsPosted (bit)
PeriodId (uniqueidentifier, nullable)
OriginalAmount (decimal(18,2))
ApprovalStatus (nvarchar(16))
ApprovedBy (nvarchar(256), nullable)
ApprovedDate (datetime2, nullable)
CreatedBy (nvarchar(256))
CreatedOn (datetime2)
LastModifiedBy (nvarchar(256))
LastModifiedOn (datetime2)
TenantId (nvarchar(64))
```

### JournalEntryLines Table
```sql
Id (uniqueidentifier, PK)
JournalEntryId (uniqueidentifier, FK)
AccountId (uniqueidentifier, FK)
DebitAmount (decimal(18,2))
CreditAmount (decimal(18,2))
Memo (nvarchar(500), nullable)
Reference (nvarchar(100), nullable)
CreatedBy (nvarchar(256))
CreatedOn (datetime2)
LastModifiedBy (nvarchar(256))
LastModifiedOn (datetime2)
TenantId (nvarchar(64))
```

---

## Testing Checklist

### Unit Tests
- [ ] Balance validation logic
- [ ] AddLine validation
- [ ] Post validation
- [ ] Approve validation
- [ ] Reverse logic

### Integration Tests
- [ ] Create with balanced lines
- [ ] Create with unbalanced lines (should fail)
- [ ] Post balanced entry
- [ ] Post unbalanced entry (should fail)
- [ ] Update unposted entry
- [ ] Update posted entry (should fail)
- [ ] Reverse posted entry
- [ ] Search with various filters
- [ ] Approval workflow

---

## Related Documentation

- [JOURNAL_ENTRY_API_REVIEW_COMPLETE.md](./JOURNAL_ENTRY_API_REVIEW_COMPLETE.md) - Full implementation review
- [JOURNAL_ENTRY_IMPLEMENTATION_COMPLETE.md](./JOURNAL_ENTRY_IMPLEMENTATION_COMPLETE.md) - Initial implementation
- [JOURNAL_ENTRY_PATTERN_REVIEW_COMPLETE.md](./JOURNAL_ENTRY_PATTERN_REVIEW_COMPLETE.md) - Pattern review

---

**Last Updated:** November 3, 2025
**Status:** ✅ Production Ready

