# Journal Entry API - Required & Optional Fields Reference

## Overview
This document lists all required and optional fields when creating a journal entry, along with validation rules.

---

## 📋 Journal Entry Fields

### ✅ **REQUIRED Fields**

#### 1. **Date** (DateTime)
- **Type:** `DateTime`
- **Required:** ✅ Yes (`.NotEmpty()`)
- **Nullable:** ❌ No
- **Validation:** Must not be empty
- **Example:** `"2025-11-03T00:00:00Z"`
- **Error if missing:** "Date is required."

#### 2. **ReferenceNumber** (string)
- **Type:** `string`
- **Required:** ✅ Yes (`.NotEmpty()`)
- **Nullable:** ❌ No
- **Max Length:** 32 characters
- **Validation:** 
  - Must not be empty
  - Cannot exceed 32 characters
- **Example:** `"JE-2025-001"`
- **Errors:**
  - If missing: "Reference number is required."
  - If too long: "Reference number cannot exceed 32 characters."

#### 3. **Source** (string)
- **Type:** `string`
- **Required:** ✅ Yes (`.NotEmpty()`)
- **Nullable:** ❌ No
- **Max Length:** 64 characters
- **Validation:**
  - Must not be empty
  - Cannot exceed 64 characters
- **Example:** `"ManualEntry"`
- **Common Values:**
  - `"ManualEntry"`
  - `"InvoicePost"`
  - `"BillPost"`
  - `"PaymentProcessing"`
  - `"CheckClearing"`
  - `"BankReconciliation"`
  - `"PeriodClose"`
  - `"Depreciation"`
  - `"AdjustingEntry"`
  - `"CorrectingEntry"`
- **Errors:**
  - If missing: "Source is required."
  - If too long: "Source cannot exceed 64 characters."

#### 4. **Description** (string)
- **Type:** `string`
- **Required:** ✅ Yes (`.NotEmpty()`)
- **Nullable:** ❌ No
- **Max Length:** 1000 characters
- **Validation:**
  - Must not be empty
  - Cannot exceed 1000 characters
- **Example:** `"Office supplies expense for November 2025"`
- **Errors:**
  - If missing: "Description is required."
  - If too long: "Description cannot exceed 1000 characters."

#### 5. **Lines** (List of JournalEntryLineDto)
- **Type:** `List<JournalEntryLineDto>?`
- **Required:** ✅ Yes (`.NotNull()`)
- **Nullable:** ⚠️ Yes in schema, but validated as required
- **Min Count:** 2 lines
- **Validation:**
  - Must not be null
  - Must have at least 2 lines
  - Must be balanced (total debits = total credits)
- **Example:** See "Journal Entry Line Fields" section below
- **Errors:**
  - If null: "Lines are required."
  - If < 2 lines: "At least 2 lines are required for a balanced journal entry."
  - If not balanced: "The journal entry must be balanced (total debits must equal total credits)."

---

### 🔷 **OPTIONAL Fields**

#### 6. **PeriodId** (Guid?)
- **Type:** `DefaultIdType?` (Nullable Guid)
- **Required:** ❌ No
- **Nullable:** ✅ Yes
- **Default:** `null`
- **Validation:** None
- **Example:** `"3fa85f64-5717-4562-b3fc-2c963f66afa6"`
- **Purpose:** Links entry to an accounting period

#### 7. **OriginalAmount** (decimal)
- **Type:** `decimal`
- **Required:** ❌ No
- **Nullable:** ❌ No (but has default)
- **Default:** `0`
- **Validation:** Must be >= 0 (`.GreaterThanOrEqualTo(0)`)
- **Example:** `1000.00`
- **Error:** "Original amount must be non-negative."
- **Purpose:** Reference amount for control purposes

#### 8. **Notes** (string?)
- **Type:** `string?`
- **Required:** ❌ No
- **Nullable:** ✅ Yes
- **Default:** `null`
- **Validation:** None
- **Example:** `"Approved by finance manager"`
- **Purpose:** Additional notes/comments

---

## 📋 Journal Entry Line Fields (JournalEntryLineDto)

Each line in the `Lines` array must have:

### ✅ **REQUIRED Fields**

#### 1. **AccountId** (Guid)
- **Type:** `DefaultIdType` (Guid)
- **Required:** ✅ Yes (`.NotEmpty()`)
- **Nullable:** ❌ No
- **Validation:** Must not be empty
- **Example:** `"a3b5c7d9-1234-5678-9abc-def012345678"`
- **Error:** "Account ID is required for each line."
- **Purpose:** Chart of Account identifier

#### 2. **DebitAmount** (decimal)
- **Type:** `decimal`
- **Required:** ✅ Yes (but can be 0)
- **Nullable:** ❌ No
- **Validation:**
  - Must be >= 0 (`.GreaterThanOrEqualTo(0)`)
  - Must have either debit OR credit > 0 (not both, not neither)
  - Cannot be > 0 if CreditAmount > 0
- **Example:** `500.00` or `0`
- **Errors:**
  - If negative: "Debit amount must be non-negative."
  - If both debit and credit > 0: "A line cannot have both debit and credit amounts."
  - If both are 0: "Each line must have either a debit or credit amount."

#### 3. **CreditAmount** (decimal)
- **Type:** `decimal`
- **Required:** ✅ Yes (but can be 0)
- **Nullable:** ❌ No
- **Validation:**
  - Must be >= 0 (`.GreaterThanOrEqualTo(0)`)
  - Must have either debit OR credit > 0 (not both, not neither)
  - Cannot be > 0 if DebitAmount > 0
- **Example:** `500.00` or `0`
- **Errors:**
  - If negative: "Credit amount must be non-negative."
  - If both debit and credit > 0: "A line cannot have both debit and credit amounts."
  - If both are 0: "Each line must have either a debit or credit amount."

### 🔷 **OPTIONAL Fields**

#### 4. **Description** (string?)
- **Type:** `string?`
- **Required:** ❌ No
- **Nullable:** ✅ Yes
- **Max Length:** 500 characters
- **Validation:** Cannot exceed 500 characters (if provided)
- **Example:** `"Payment for office supplies"`
- **Error:** "Line description cannot exceed 500 characters."

#### 5. **Reference** (string?)
- **Type:** `string?`
- **Required:** ❌ No
- **Nullable:** ✅ Yes
- **Max Length:** 100 characters
- **Validation:** Cannot exceed 100 characters (if provided)
- **Example:** `"INV-2025-001"`
- **Error:** "Line reference cannot exceed 100 characters."

---

## 📊 Special Validation Rules

### Balance Validation
- **Rule:** Total Debits MUST equal Total Credits
- **Tolerance:** 0.01 (allows small rounding differences)
- **Formula:** `Math.Abs(TotalDebits - TotalCredits) < 0.01`
- **Error:** "The journal entry must be balanced (total debits must equal total credits)."

### Minimum Lines
- **Rule:** Must have at least 2 lines
- **Reason:** Double-entry bookkeeping requires at least one debit and one credit
- **Error:** "At least 2 lines are required for a balanced journal entry."

### Debit/Credit Mutual Exclusivity
- **Rule:** Each line must have EITHER debit OR credit (not both, not neither)
- **Valid:**
  - Debit = 100, Credit = 0 ✅
  - Debit = 0, Credit = 100 ✅
- **Invalid:**
  - Debit = 100, Credit = 100 ❌
  - Debit = 0, Credit = 0 ❌

---

## 📝 Complete Example Request

### ✅ Valid Request (Minimal Required Fields)

```json
{
  "date": "2025-11-03T00:00:00Z",
  "referenceNumber": "JE-2025-001",
  "source": "ManualEntry",
  "description": "Office supplies expense",
  "lines": [
    {
      "accountId": "a3b5c7d9-1234-5678-9abc-def012345678",
      "debitAmount": 500.00,
      "creditAmount": 0
    },
    {
      "accountId": "b4c6d8e0-2345-6789-abcd-ef0123456789",
      "debitAmount": 0,
      "creditAmount": 500.00
    }
  ]
}
```

### ✅ Valid Request (All Fields)

```json
{
  "date": "2025-11-03T00:00:00Z",
  "referenceNumber": "JE-2025-001",
  "source": "ManualEntry",
  "description": "Office supplies expense for November 2025",
  "periodId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "originalAmount": 500.00,
  "notes": "Approved by finance manager",
  "lines": [
    {
      "accountId": "a3b5c7d9-1234-5678-9abc-def012345678",
      "debitAmount": 500.00,
      "creditAmount": 0,
      "description": "Office Supplies Expense",
      "reference": "INV-2025-001"
    },
    {
      "accountId": "b4c6d8e0-2345-6789-abcd-ef0123456789",
      "debitAmount": 0,
      "creditAmount": 500.00,
      "description": "Cash Account",
      "reference": "INV-2025-001"
    }
  ]
}
```

---

## ❌ Common Validation Errors

### 1. Missing Required Field
```json
{
  "errors": {
    "ReferenceNumber": ["Reference number is required."]
  }
}
```

### 2. Lines is Null
```json
{
  "errors": {
    "Lines": ["Lines are required."]
  }
}
```

### 3. Not Enough Lines
```json
{
  "errors": {
    "Lines": ["At least 2 lines are required for a balanced journal entry."]
  }
}
```

### 4. Unbalanced Entry
```json
{
  "errors": {
    "Lines": ["The journal entry must be balanced (total debits must equal total credits)."]
  }
}
```

### 5. Line with Both Debit and Credit
```json
{
  "errors": {
    "Lines[0]": ["A line cannot have both debit and credit amounts."]
  }
}
```

### 6. Line with Neither Debit nor Credit
```json
{
  "errors": {
    "Lines[0]": ["Each line must have either a debit or credit amount."]
  }
}
```

### 7. String Too Long
```json
{
  "errors": {
    "ReferenceNumber": ["Reference number cannot exceed 32 characters."],
    "Lines[0].Description": ["Line description cannot exceed 500 characters."]
  }
}
```

---

## 📋 Quick Reference Table

### Journal Entry Level

| Field | Type | Required | Nullable | Default | Max Length | Validation |
|-------|------|----------|----------|---------|------------|------------|
| Date | DateTime | ✅ Yes | ❌ No | - | - | Not empty |
| ReferenceNumber | string | ✅ Yes | ❌ No | - | 32 | Not empty |
| Source | string | ✅ Yes | ❌ No | - | 64 | Not empty |
| Description | string | ✅ Yes | ❌ No | - | 1000 | Not empty |
| Lines | List | ✅ Yes | ⚠️ Yes* | - | - | Not null, >= 2 items, balanced |
| PeriodId | Guid? | ❌ No | ✅ Yes | null | - | None |
| OriginalAmount | decimal | ❌ No | ❌ No | 0 | - | >= 0 |
| Notes | string? | ❌ No | ✅ Yes | null | - | None |

*Lines is nullable in schema but validated as required

### Journal Entry Line Level

| Field | Type | Required | Nullable | Default | Max Length | Validation |
|-------|------|----------|----------|---------|------------|------------|
| AccountId | Guid | ✅ Yes | ❌ No | - | - | Not empty |
| DebitAmount | decimal | ✅ Yes | ❌ No | 0 | - | >= 0, either/or with credit |
| CreditAmount | decimal | ✅ Yes | ❌ No | 0 | - | >= 0, either/or with debit |
| Description | string? | ❌ No | ✅ Yes | null | 500 | Optional |
| Reference | string? | ❌ No | ✅ Yes | null | 100 | Optional |

---

## 🔍 Debugging Tips

### To see detailed validation errors:
1. Check the response body for the `errors` object
2. Each error includes the field path and message
3. Array items are indexed: `Lines[0]`, `Lines[1]`, etc.

### Common issues:
- ✅ Make sure `Lines` is not null
- ✅ Ensure at least 2 lines exist
- ✅ Verify total debits = total credits
- ✅ Each line should have either debit OR credit (not both)
- ✅ All required string fields must not be empty
- ✅ Check string length limits

---

**Last Updated:** November 3, 2025  
**API Version:** v1  
**Status:** ✅ Current

