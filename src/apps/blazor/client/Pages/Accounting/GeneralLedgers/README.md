# General Ledger UI Implementation

## Overview

The General Ledger UI provides comprehensive viewing and management capabilities for all financial transactions posted to the general ledger. This is a **critical** accounting feature that serves as the foundation for financial reporting and audit trail.

**Implementation Date:** November 9, 2025  
**Status:** ✅ Complete - Production Ready

---

## Features Implemented

### 🔍 Search & Filtering

Advanced search capabilities including:
- **Account filtering** - Filter by account code or select from chart of accounts
- **Date range filtering** - Search transactions within specific date ranges
- **Amount filtering** - Filter by debit/credit amount ranges
- **Reference number search** - Find transactions by reference numbers
- **Period filtering** - Filter by accounting period
- **USOA classification** - Filter by utility-specific account classes
- **Keyword search** - Full-text search across all fields

### 📊 Transaction Listing

Data grid with the following columns:
- Transaction Date
- Account ID (with lookup capability)
- Reference Number
- Debit Amount (formatted currency)
- Credit Amount (formatted currency)
- Memo
- USOA Class
- Posted Status (with visual indicator)

### 🔎 Drill-Down Capabilities

- **View Details** - Complete transaction details in a dialog
- **View Source Entry** - Navigate to the originating journal entry
- **Account Navigation** - Jump to chart of accounts for selected account
- **Audit Trail Display** - Created by, created on, posted by, posted on

### ✏️ Edit Operations

- **Update unposted entries** - Modify debit, credit, memo, USOA class, reference number
- **Immutable posted entries** - Posted entries cannot be edited (accounting best practice)
- **Validation** - Amount validation, required field checks

### 📤 Export Capabilities

- Export to Excel/CSV (via EntityTable framework)
- Filtered export - export only the displayed/filtered results

---

## File Structure

```
/Pages/Accounting/GeneralLedgers/
├── GeneralLedgers.razor              # Main page
├── GeneralLedgers.razor.cs           # Code-behind with business logic
├── GeneralLedgerViewModel.cs         # View model for UI binding
└── GeneralLedgerDetailsDialog.razor  # Details dialog component
```

---

## API Integration

### Endpoints Used

| Endpoint | Method | Purpose |
|----------|--------|---------|
| `/api/v1/general-ledger/search` | POST | Search general ledger entries |
| `/api/v1/general-ledger/{id}` | GET | Get entry details |
| `/api/v1/general-ledger/{id}` | PUT | Update entry (unposted only) |

### API Models

- **GeneralLedgerSearchQuery** - Search request with filters
- **GeneralLedgerSearchResponse** - Search results
- **GeneralLedgerGetResponse** - Detailed entry information
- **GeneralLedgerUpdateCommand** - Update command

---

## Usage Guide

### Accessing the Page

Navigate to: `/accounting/general-ledger`

Or from the navigation menu: **Accounting > General Ledger**

### Searching for Transactions

1. Use the **Search** button to open advanced search filters
2. Apply desired filters (date range, account, amounts, etc.)
3. Click **Search** to apply filters
4. Results are paginated (default 10 per page)

### Viewing Transaction Details

1. Click the **three-dot menu** on any transaction row
2. Select **View Details**
3. Details dialog shows:
   - Transaction information
   - Account information
   - Amounts (debit/credit)
   - Memo, description, notes
   - Complete audit trail
   - Source document information
   - Posted status and posting details

### Editing Unposted Entries

1. Click the **three-dot menu** on an unposted transaction
2. Select **Edit**
3. Modify allowed fields:
   - Debit amount
   - Credit amount
   - Memo
   - USOA class
   - Reference number
   - Description
   - Notes
4. Click **Save**

**Note:** Posted entries cannot be edited. Use reversing journal entries for corrections.

### Navigating to Source Documents

1. Click the **three-dot menu** on any transaction
2. Select **View Source Entry**
3. Navigate to the originating journal entry

---

## Business Rules

### Posting Rules

- ✅ Only unposted entries can be edited
- ❌ Posted entries are immutable (SOX compliance)
- ✅ Posted entries show posted date and user
- ✅ Complete audit trail maintained

### Amount Validation

- ✅ Debit and credit amounts must be non-negative
- ✅ Either debit OR credit must have an amount (not both on same entry)
- ✅ Amounts formatted as currency (2 decimal places)

### Security

- ✅ Permission required: `Permissions.Accounting.View`
- ✅ Update permission checked: `Permissions.Accounting.Update`
- ✅ No delete operation (accounting best practice - use reversing entries)

---

## Technical Details

### Dependencies

**NuGet Packages:**
- MudBlazor (UI framework)
- Mapster (object mapping)
- FSH.Framework (CQRS, Entity framework)

**Blazor Components:**
- EntityTable - Generic table component
- PageHeader - Page header component
- AutocompleteChartOfAccountId - Account selector
- AutocompleteAccountingPeriodId - Period selector

### State Management

- Server-side table context with pagination
- Dialog state managed locally
- Search filters maintained in component state
- No global state required

### Performance Considerations

- **Pagination** - Results are paginated (default 10, max 100 per page)
- **Server-side filtering** - All filtering done on the server
- **Lazy loading** - Details loaded only when requested
- **Efficient queries** - Uses specifications for optimized database queries

---

## Permissions Required

| Action | Permission |
|--------|-----------|
| View general ledger | `Permissions.Accounting.View` |
| Update entry | `Permissions.Accounting.Update` |
| Export data | `Permissions.Accounting.Export` |

---

## Integration Points

### Chart of Accounts

- Account selector uses autocomplete
- Click account to navigate to account details
- Account code and name displayed

### Journal Entries

- Direct navigation to source journal entry
- Entry ID linkage maintained
- Source transaction type displayed

### Accounting Periods

- Period filter available
- Period closed status affects edit capability
- Period-based reporting enabled

### Financial Statements

- GL entries are the data source for:
  - Balance Sheet
  - Income Statement
  - Cash Flow Statement
  - Trial Balance

---

## Future Enhancements

### Potential Improvements

1. **Account drill-down page** - Dedicated page showing all transactions for a specific account
2. **Running balance** - Display running balance for account-specific views
3. **Bulk operations** - Bulk update or export selected entries
4. **Advanced analytics** - Charts and graphs for transaction analysis
5. **Reconciliation tools** - GL to sub-ledger reconciliation interface
6. **Batch posting** - Post multiple unposted entries at once
7. **Transaction reversal** - Direct reversal interface from GL view
8. **Custom views** - Save and load custom filter configurations
9. **Real-time updates** - SignalR integration for real-time GL updates
10. **Mobile optimization** - Enhanced mobile responsive design

---

## Testing Checklist

### Functional Testing

- [x] Search with various filter combinations
- [x] View transaction details
- [x] Edit unposted entry successfully
- [x] Verify posted entries cannot be edited
- [x] Navigate to source journal entry
- [x] Export filtered results
- [x] Pagination works correctly
- [x] Sorting works on all columns

### UI/UX Testing

- [x] Responsive design (desktop, tablet, mobile)
- [x] Loading indicators display correctly
- [x] Error messages are clear and helpful
- [x] Success notifications display
- [x] Dialog opens and closes correctly
- [x] Filters are intuitive and easy to use

### Security Testing

- [x] Permission checks enforced
- [x] Update only allowed for unposted entries
- [x] Delete operation not available
- [x] Audit trail captured correctly

### Performance Testing

- [x] Page loads in < 2 seconds
- [x] Search returns results quickly
- [x] No memory leaks on repeated use
- [x] Large datasets handle properly

---

## Troubleshooting

### Common Issues

**Issue:** "General Ledger endpoints not found"
- **Solution:** Run NSwag regeneration script: `./apps/blazor/scripts/nswag-regen.sh`
- **Cause:** API client not generated after adding endpoints

**Issue:** "Cannot update posted entry"
- **Solution:** This is expected behavior. Use a reversing journal entry for corrections.
- **Cause:** Posted entries are immutable for audit compliance

**Issue:** "Account not found in autocomplete"
- **Solution:** Ensure Chart of Accounts is populated
- **Cause:** Empty or incomplete chart of accounts data

**Issue:** "Search returns no results"
- **Solution:** Check that GL has been posted with transactions
- **Cause:** No data in general ledger (need to post journal entries)

---

## Related Documentation

- [Accounting UI Implementation Gap Analysis](../../../ACCOUNTING_UI_IMPLEMENTATION_GAP_ANALYSIS.md)
- [Journal Entries Implementation](../JournalEntries/README.md) (if exists)
- [Chart of Accounts Implementation](../ChartOfAccounts/README.md) (if exists)
- [CQRS Implementation Checklist](../../../CQRS_IMPLEMENTATION_CHECKLIST.md)

---

## Changelog

### Version 1.0 (November 8, 2025)

- ✅ Initial implementation
- ✅ Search and filtering
- ✅ Details dialog
- ✅ Edit functionality
- ✅ Source navigation
- ✅ Complete audit trail
- ✅ API integration
- ✅ Documentation

---

## Support

For issues or questions:
1. Check this README first
2. Review the gap analysis document
3. Check existing Journal Entries and Banks pages for similar patterns
4. Review API endpoint documentation

---

**Document Version:** 1.0  
**Last Updated:** November 8, 2025  
**Implementation Status:** ✅ Complete

