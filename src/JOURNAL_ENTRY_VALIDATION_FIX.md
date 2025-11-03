# Journal Entry Validation Fix - Summary

## Issue
When creating a journal entry via the API, a validation error was occurring:
```
[ERR] FSH.Framework.Infrastructure.Exceptions.CustomExceptionHandler one or more validation errors occurred
```

The error was a 400 Bad Request, indicating validation failure.

## Root Cause
The `CreateJournalEntryRequestValidator` had issues handling the `Lines` collection:

1. **Null Reference Issue**: The `Lines` property in `CreateJournalEntryCommand` was not nullable, but when clients don't send it (or send null), it would cause validation issues.

2. **Incorrect Validation Logic**: The validator was using `.NotEmpty()` followed by `.Must(lines => lines.Count >= 2)`, which would fail on null collections.

3. **RuleForEach on Nullable Collection**: The `RuleForEach` was trying to iterate over a potentially null collection without proper null guards.

## Solution Applied

### 1. Made Lines Nullable in Command
**File**: `CreateJournalEntryCommand.cs`

```csharp
public sealed record CreateJournalEntryCommand(
    DateTime Date,
    string ReferenceNumber,
    string Source,
    string Description,
    List<JournalEntryLineDto>? Lines,  // Made nullable
    DefaultIdType? PeriodId = null,
    decimal OriginalAmount = 0,
    string? Notes = null
) : IRequest<CreateJournalEntryResponse>;
```

### 2. Updated Validator for Null Safety
**File**: `CreateJournalEntryRequestValidator.cs`

**Changes:**
- Changed `.NotEmpty()` to `.NotNull()` for clearer error messaging
- Added proper null checks before validating count
- Wrapped `RuleForEach` in a `When` clause to only execute when Lines is not null
- Updated `BeBalanced` method signature to accept nullable parameter

**Before:**
```csharp
RuleFor(x => x.Lines)
    .NotEmpty()
    .WithMessage("At least 2 lines are required for a balanced journal entry.")
    .Must(lines => lines.Count >= 2)
    .WithMessage("At least 2 lines are required for a balanced journal entry.");

RuleForEach(x => x.Lines)
    .ChildRules(line => { ... });
```

**After:**
```csharp
RuleFor(x => x.Lines)
    .NotNull()
    .WithMessage("Lines are required.")
    .Must(lines => lines != null && lines.Count >= 2)
    .WithMessage("At least 2 lines are required for a balanced journal entry.");

When(x => x.Lines != null, () =>
{
    RuleForEach(x => x.Lines!)
        .ChildRules(line => { ... });
});

RuleFor(x => x.Lines)
    .Must(BeBalanced)
    .When(x => x.Lines != null && x.Lines.Count >= 2)
    .WithMessage("The journal entry must be balanced (total debits must equal total credits).");
```

### 3. Updated Handler for Null Safety
**File**: `CreateJournalEntryHandler.cs`

Added null check before iterating lines:
```csharp
// Add lines to the journal entry (validation ensures Lines is not null and has at least 2 items)
if (request.Lines != null)
{
    foreach (var lineDto in request.Lines)
    {
        journalEntry.AddLine(
            lineDto.AccountId,
            lineDto.DebitAmount,
            lineDto.CreditAmount,
            lineDto.Description,
            lineDto.Reference);
    }
}
```

## Validation Rules Summary

### Entry Level:
1. ✅ **Date** - Required
2. ✅ **ReferenceNumber** - Required, max 32 chars
3. ✅ **Description** - Required, max 1000 chars
4. ✅ **Source** - Required, max 64 chars
5. ✅ **OriginalAmount** - Must be non-negative
6. ✅ **Lines** - Must not be null, must have at least 2 lines
7. ✅ **Balance** - Total debits must equal total credits (within 0.01 tolerance)

### Line Level (per line):
1. ✅ **AccountId** - Required
2. ✅ **DebitAmount** - Must be non-negative
3. ✅ **CreditAmount** - Must be non-negative
4. ✅ **Either/Or** - Must have either debit OR credit (not both, not neither)
5. ✅ **Description** - Optional, max 500 chars
6. ✅ **Reference** - Optional, max 100 chars

## Error Messages

Users will now receive clear error messages:

### If Lines is null or missing:
```json
{
  "errors": {
    "Lines": ["Lines are required."]
  }
}
```

### If Lines has fewer than 2 items:
```json
{
  "errors": {
    "Lines": ["At least 2 lines are required for a balanced journal entry."]
  }
}
```

### If entry is not balanced:
```json
{
  "errors": {
    "Lines": ["The journal entry must be balanced (total debits must equal total credits)."]
  }
}
```

### If a line has both debit and credit:
```json
{
  "errors": {
    "Lines[0]": ["A line cannot have both debit and credit amounts."]
  }
}
```

### If a line has neither debit nor credit:
```json
{
  "errors": {
    "Lines[0]": ["Each line must have either a debit or credit amount."]
  }
}
```

## Example Valid Request

```json
{
  "date": "2025-11-03T00:00:00Z",
  "referenceNumber": "JE-2025-001",
  "source": "ManualEntry",
  "description": "Office supplies expense",
  "periodId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "originalAmount": 500.00,
  "lines": [
    {
      "accountId": "account-guid-1",
      "debitAmount": 500.00,
      "creditAmount": 0,
      "description": "Office Supplies Expense"
    },
    {
      "accountId": "account-guid-2",
      "debitAmount": 0,
      "creditAmount": 500.00,
      "description": "Cash"
    }
  ]
}
```

## Testing Checklist

- [x] Null Lines collection validation
- [x] Empty Lines collection validation
- [x] Single line validation (should fail)
- [x] Two or more lines validation (should pass)
- [x] Unbalanced entry validation (should fail)
- [x] Balanced entry validation (should pass)
- [x] Line with both debit and credit (should fail)
- [x] Line with neither debit nor credit (should fail)
- [x] Proper error messages returned

## Result

✅ **Validation now works correctly** - The API will properly validate journal entry creation requests and return clear error messages when validation fails.

---

**Date Fixed**: November 3, 2025  
**Status**: ✅ Resolved

