# Journal Entries - User Quick Reference Guide

## Overview
The Journal Entries page allows you to create, manage, and post double-entry accounting transactions to the general ledger.

---

## Accessing Journal Entries
**Navigation:** Menu → Accounting → Journal Entries

---

## Creating a New Journal Entry

### Step 1: Click "Create" Button
Located at the top right of the page.

### Step 2: Fill in Entry Details
- **Date** *(Required)* - Transaction date
- **Reference Number** *(Required)* - Unique identifier (e.g., JE-2025-001)
- **Source** *(Required)* - Select from dropdown:
  - Manual Entry
  - Invoice Post
  - Bill Post
  - Payment Processing
  - Check Clearing
  - Bank Reconciliation
  - Period Close
  - Depreciation
  - Adjusting Entry
  - Correcting Entry
- **Description** *(Required)* - Describe the purpose of this entry
- **Accounting Period** *(Optional)* - Link to an accounting period
- **Original Amount** *(Optional)* - Reference amount

### Step 3: Add Journal Entry Lines
1. Click "Add Line" button
2. For each line:
   - Select **Account** from dropdown (start typing to search)
   - Enter either **Debit** OR **Credit** amount (not both)
   - Add optional **Description** for the line

### Step 4: Verify Balance
- Watch the balance summary at the bottom
- **Total Debits** must equal **Total Credits**
- Green checkmark = Balanced ✅
- Yellow warning = Out of balance ⚠️

### Step 5: Save
- Click "Save" button
- Entry is created with status "Pending"

---

## Viewing Journal Entries

### Search & Filter
Use the search panel to find entries:
- **Reference Number** - Search by reference
- **Source** - Filter by source system
- **Approval Status** - Pending, Approved, or Rejected
- **Date Range** - From/To dates
- **Posted Entries Only** - Toggle to show only posted entries

### View Details
1. Find the entry in the table
2. Click the **...** menu button
3. Select "View Details"
4. View complete entry information including all lines

---

## Editing Journal Entries

### Rules:
- ✅ Can edit entries with status "Pending" or "Rejected"
- ❌ Cannot edit entries that are "Posted"
- ❌ Cannot edit entries that are "Approved" (must unapprove first)

### Steps:
1. Find the entry in the table
2. Click the "Edit" icon or **...** → "Edit"
3. Make your changes
4. Ensure entry remains balanced
5. Click "Update"

---

## Approval Workflow

### Approving an Entry
1. Find a "Pending" entry
2. Click **...** → "Approve"
3. Confirm the approval
4. Status changes to "Approved"

**Note:** Only balanced entries can be approved.

### Rejecting an Entry
1. Find a "Pending" entry
2. Click **...** → "Reject"
3. Enter rejection reason (optional)
4. Click "Reject Entry"
5. Status changes to "Rejected"

---

## Posting to General Ledger

### Requirements:
- Entry must have status "Approved"
- Entry must be balanced
- Entry must not already be posted

### Steps:
1. Find an "Approved" entry
2. Click **...** → "Post to GL"
3. Confirm posting
4. Entry is posted to general ledger

**⚠️ Warning:** Once posted, entries become **immutable** (cannot be edited or deleted).

---

## Reversing Posted Entries

### When to Reverse:
- Correcting errors in posted entries
- Canceling transactions
- Period-end adjustments

### Steps:
1. Find a "Posted" entry
2. Click **...** → "Reverse Entry"
3. Enter:
   - **Reversal Date** - Date for the new entry
   - **Reversal Reason** - Explain why you're reversing
4. Click "Reverse Entry"

### What Happens:
- New entry is created with debits/credits swapped
- New entry reference: `REV-{original-reference}`
- New entry is automatically posted
- Original entry remains in system (marked as reversed)

---

## Understanding Entry Status

### Status Flow:
```
Pending → Approved → Posted
   ↓
Rejected
```

### Status Badges:
- 🟡 **Pending** - Awaiting approval
- 🔵 **Approved** - Ready to post
- 🔴 **Rejected** - Not approved
- 🟢 **Posted** - In general ledger (immutable)

---

## Common Scenarios

### Scenario 1: Simple Expense Entry
**Purpose:** Record $500 office supplies expense

**Lines:**
| Account | Debit | Credit |
|---------|-------|--------|
| Office Supplies Expense | 500.00 | - |
| Cash | - | 500.00 |

### Scenario 2: Accrual Entry
**Purpose:** Accrue $2,000 rent expense

**Lines:**
| Account | Debit | Credit |
|---------|-------|--------|
| Rent Expense | 2,000.00 | - |
| Accrued Rent Payable | - | 2,000.00 |

### Scenario 3: Revenue Recognition
**Purpose:** Record $10,000 sales revenue

**Lines:**
| Account | Debit | Credit |
|---------|-------|--------|
| Cash | 10,000.00 | - |
| Sales Revenue | - | 10,000.00 |

### Scenario 4: Complex Multi-Line Entry
**Purpose:** Record multiple expenses totaling $3,500

**Lines:**
| Account | Debit | Credit |
|---------|-------|--------|
| Supplies Expense | 1,000.00 | - |
| Utilities Expense | 1,200.00 | - |
| Maintenance Expense | 1,300.00 | - |
| Accounts Payable | - | 3,500.00 |

---

## Tips & Best Practices

### ✅ DO:
- Use consistent reference numbering (e.g., JE-YYYY-###)
- Add descriptive entry descriptions
- Add line-level descriptions for clarity
- Review entries before approving
- Verify balance before saving
- Use appropriate source types
- Link to accounting periods when applicable
- Get approval before posting

### ❌ DON'T:
- Post unbalanced entries
- Edit entries after posting
- Skip approval workflow
- Use vague descriptions
- Delete posted entries
- Forget to document the purpose

---

## Keyboard Shortcuts

- **Ctrl/Cmd + S** - Save form (when editing)
- **Esc** - Close dialog
- **Tab** - Navigate between fields
- **Enter** - Submit form (when on button)

---

## Balance Validation Rules

### Valid Entry:
```
Debits: $1,000.00
Credits: $1,000.00
Difference: $0.00
Status: ✅ Balanced
```

### Invalid Entry:
```
Debits: $1,000.00
Credits: $950.00
Difference: $50.00
Status: ⚠️ Out of Balance
```

**Tolerance:** Entries must balance within $0.01

---

## Troubleshooting

### Problem: Cannot save entry
**Solution:** 
- Check that entry is balanced
- Verify at least 2 lines exist
- Ensure required fields are filled

### Problem: Cannot edit entry
**Solution:** 
- Check entry status (cannot edit if posted)
- Verify you have edit permissions

### Problem: Cannot post entry
**Solution:** 
- Verify entry is approved
- Check that entry is balanced
- Ensure entry isn't already posted

### Problem: Account not found in dropdown
**Solution:** 
- Check that account is active
- Verify you have permission to use the account
- Contact administrator if account should exist

---

## Getting Help

### For Questions:
- Contact your accounting supervisor
- Review this guide
- Check system documentation

### For Technical Issues:
- Contact IT support
- Report bugs to system administrator
- Check system status page

---

## Glossary

**Debit** - Left side of accounting equation; increases assets and expenses

**Credit** - Right side of accounting equation; increases liabilities, equity, and revenue

**Journal Entry** - Record of debits and credits for a transaction

**General Ledger** - Master set of accounts recording all transactions

**Balanced Entry** - Entry where total debits equal total credits

**Posted Entry** - Entry recorded in the general ledger (immutable)

**Reversal** - Offsetting entry to cancel a previous entry

**Accrual** - Recording revenue or expense before cash changes hands

**Chart of Accounts** - List of all accounts used in accounting system

---

**Last Updated:** November 3, 2025  
**Version:** 1.0.0

