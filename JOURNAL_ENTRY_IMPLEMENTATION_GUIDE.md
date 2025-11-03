# Journal Entry Implementation Guide

## Executive Summary

This document provides comprehensive guidance for implementing the **Journal Entry** Blazor page and components. Based on analysis of the codebase, Journal Entries are designed to support **BOTH manual entry AND automated creation** from other accounting features.

**Generated**: November 2, 2025

---

## 1. Architecture Overview

### Current Implementation Status

| Component | Status | Details |
|-----------|--------|---------|
| **Domain Entity** | ✅ Complete | Full double-entry logic with validation |
| **API Endpoints** | ⚠️ Partial | Only Get & Search implemented |
| **Blazor Page** | ❌ Missing | No UI implementation |
| **Event Handlers** | ⚠️ Basic | Only logging, no GL posting |

### Key Finding: Dual Entry Approach

**Journal Entries support TWO modes:**

1. **Manual Entry** (Primary Need)
   - Direct journal entry creation by accounting staff
   - Full double-entry interface with debit/credit lines
   - Balance validation before posting
   - Approval workflow

2. **Automated Creation** (Future Enhancement)
   - System-generated from source documents (Invoices, Bills, Payments, etc.)
   - Background automation via event handlers
   - Currently NOT implemented in the codebase

---

## 2. Prerequisites & Dependencies

### Required Components (Already Implemented)

✅ **Chart of Accounts** - For account selection in journal lines
- Page: `/chart-of-accounts`
- Autocomplete component available
- API: Full CQRS

✅ **Accounting Periods** - For period assignment
- Page: `/accounting/periods`
- API: Full CQRS
- Used to control posting windows

✅ **General Ledger** - Target for posted entries
- API: Full CQRS (Search, Get, Create, Update, Delete)
- **Note**: GL posting NOT automated yet (needs event handler)

### Missing API Endpoints (Must Build First)

❌ **Journal Entry Create Endpoint** - Accepts entry with lines
❌ **Journal Entry Update Endpoint** - Modifies unposted entries
❌ **Journal Entry Delete Endpoint** - Removes unposted entries
❌ **Journal Entry Post Endpoint** - Posts to GL
❌ **Journal Entry Reverse Endpoint** - Creates reversal entry
❌ **Journal Entry Approve Endpoint** - Approval workflow
❌ **Journal Entry Reject Endpoint** - Rejection workflow

### Missing Event Handlers (Future Enhancement)

❌ **JournalEntryPostedEventHandler** - Creates GL entries
❌ **InvoicePostedEventHandler** - Auto-creates journal entry
❌ **BillPostedEventHandler** - Auto-creates journal entry
❌ **PaymentProcessedEventHandler** - Auto-creates journal entry
❌ **CheckClearedEventHandler** - Auto-creates journal entry

---

## 3. Implementation Strategy

### Phase 1: Manual Entry (Recommended First)

**Focus**: Build full manual journal entry capability

**Benefits:**
- Provides immediate accounting functionality
- Independent of other modules
- Follows established patterns (Check Management)
- Core accounting requirement

**Components Needed:**
1. Journal Entry page with line entry grid
2. Account autocomplete (reuse existing)
3. Balance calculation and validation
4. Post/Reverse/Approve operations
5. Journal Entry ViewModel
6. API endpoints (Create, Update, Delete, Post, Reverse, Approve, Reject)

### Phase 2: Automated Integration (Future)

**Focus**: Auto-generate journal entries from source transactions

**Depends On:**
- Invoices page implementation
- Bills page implementation
- Payments page implementation
- Event handler infrastructure

**Components Needed:**
1. Domain event handlers for transaction events
2. Journal entry template/mapping logic
3. Automatic posting rules
4. Source document reference tracking

---

## 4. Domain Model Analysis

### JournalEntry Entity

```csharp
public class JournalEntry : AuditableEntity, IAggregateRoot
{
    // Core Properties
    public DateTime Date { get; private set; }
    public string ReferenceNumber { get; private set; }
    public string Source { get; private set; }
    public bool IsPosted { get; private set; }
    public DefaultIdType? PeriodId { get; private set; }
    public decimal OriginalAmount { get; private set; }
    
    // Lines Collection
    private readonly List<JournalEntryLine> _lines = new();
    public IReadOnlyCollection<JournalEntryLine> Lines => _lines.AsReadOnly();
    
    // Approval Workflow
    public string ApprovalStatus { get; private set; } // Pending, Approved, Rejected
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovedDate { get; private set; }
    
    // Key Methods
    public static JournalEntry Create(...)
    public JournalEntry Update(...)
    public JournalEntry AddLine(DefaultIdType accountId, decimal debitAmount, decimal creditAmount, string? description = null)
    public JournalEntry Post()
    public JournalEntry Reverse(DateTime reversalDate, string reversalReason)
    public void Approve(string approvedBy)
    public void Reject(string rejectedBy)
}
```

### JournalEntryLine Entity

```csharp
public class JournalEntryLine : BaseEntity
{
    public DefaultIdType JournalEntryId { get; private set; }
    public DefaultIdType AccountId { get; private set; }
    public decimal DebitAmount { get; private set; }
    public decimal CreditAmount { get; private set; }
    public string? Description { get; private set; }
    
    // Validation: Either debit OR credit, not both, not neither
    public static JournalEntryLine Create(DefaultIdType journalEntryId, DefaultIdType accountId,
        decimal debitAmount, decimal creditAmount, string? description = null)
}
```

### Business Rules

1. **Balance Requirement**
   - Total Debits MUST equal Total Credits
   - Validation before posting
   - Tolerance: 0.01 cents

2. **Immutability After Posting**
   - Posted entries cannot be modified
   - Posted entries cannot be deleted
   - Must use Reverse() to correct

3. **Approval Workflow**
   - Status: Pending → Approved → Posted
   - Alternative: Pending → Rejected
   - Optional: Can skip approval and post directly

4. **Line Entry Rules**
   - Each line: Debit XOR Credit (exclusive or)
   - Amounts must be non-negative
   - Minimum 2 lines (debit and credit)

5. **Source Tracking**
   - `Source` field identifies entry origin
   - Examples: "ManualEntry", "InvoicePost", "PaymentProcessing"
   - `ReferenceNumber` links to source document

---

## 5. UI/UX Design Specification

### Page Layout

```
┌─────────────────────────────────────────────────────────────┐
│ Journal Entries                                              │
│ Manage double-entry journal entries                          │
├─────────────────────────────────────────────────────────────┤
│ [+ New Entry]  [🔍 Advanced Search]  [⚙️ Options] [📤 Export]│
├─────────────────────────────────────────────────────────────┤
│ Advanced Search Panel (Collapsible)                          │
│ ┌─────────────┬─────────────┬─────────────┬─────────────┐  │
│ │ Date From   │ Date To     │ Reference # │ Source      │  │
│ ├─────────────┼─────────────┼─────────────┼─────────────┤  │
│ │ Period      │ Status      │ Posted      │ Approved    │  │
│ └─────────────┴─────────────┴─────────────┴─────────────┘  │
├─────────────────────────────────────────────────────────────┤
│ Journal Entries Table                                        │
│ ┌────┬──────┬──────┬─────────┬─────┬────────┬─────┬─────┐ │
│ │ ID │ Date │ Ref# │ Source  │ Dr  │ Cr     │Status│ Actions│
│ ├────┼──────┼──────┼─────────┼─────┼────────┼─────┼─────┤ │
│ │1001│10/15 │INV001│Manual   │2,500│2,500   │Posted│ ⋮   │ │
│ │1002│10/16 │PAY002│Payment  │1,000│1,000   │Pending│ ⋮  │ │
│ │1003│10/17 │ADJ003│Manual   │500  │500     │Approved│⋮ │ │
│ └────┴──────┴──────┴─────────┴─────┴────────┴─────┴─────┘ │
│ Showing 1-3 of 150 entries                                   │
└─────────────────────────────────────────────────────────────┘
```

### Entry Dialog (Add/Edit)

```
┌──────────────────────────────────────────────────────────────┐
│ Create Journal Entry                                      [X] │
├──────────────────────────────────────────────────────────────┤
│ Header Information                                            │
│ ┌────────────┬────────────┬────────────┬──────────────────┐ │
│ │ Date*      │ Reference  │ Period     │ Original Amount  │ │
│ │ [10/15/25] │ [JE-1001]  │ [Oct-2025] │ [2,500.00]      │ │
│ └────────────┴────────────┴────────────┴──────────────────┘ │
│                                                               │
│ ┌────────────┬──────────────────────────────────────────────┐│
│ │ Source*    │ Description*                                 ││
│ │ [Manual▼]  │ [Adjusting entry for Oct revenue...]       ││
│ └────────────┴──────────────────────────────────────────────┘│
│                                                               │
│ Journal Entry Lines                           [+ Add Line]   │
│ ┌────┬──────────────┬──────────┬──────────┬──────────────┐ │
│ │ #  │ Account      │ Debit    │ Credit   │ Description  │ │
│ ├────┼──────────────┼──────────┼──────────┼──────────────┤ │
│ │ 1  │4000-Revenue  │          │ 2,500.00 │Oct services  │ │
│ │ 2  │1200-AR       │ 2,500.00 │          │Oct services  │ │
│ ├────┼──────────────┼──────────┼──────────┼──────────────┤ │
│ │    │ TOTALS       │ 2,500.00 │ 2,500.00 │ ✓ Balanced   │ │
│ └────┴──────────────┴──────────┴──────────┴──────────────┘ │
│                                                               │
│ Notes (Optional)                                              │
│ ┌─────────────────────────────────────────────────────────┐ │
│ │ Additional notes...                                      │ │
│ └─────────────────────────────────────────────────────────┘ │
│                                                               │
│                    [Cancel]  [Save Draft]  [Save & Post]     │
└──────────────────────────────────────────────────────────────┘
```

### Status Badges

- **Pending** - Yellow badge, awaiting approval
- **Approved** - Blue badge, approved but not posted
- **Posted** - Green badge, posted to GL
- **Rejected** - Red badge, rejected entry

### Context Actions (Based on Status)

**For Pending Entries:**
- ✏️ Edit
- ✓ Approve
- ✗ Reject
- 🗑️ Delete

**For Approved Entries:**
- 📮 Post to GL
- ✗ Reject
- 👁️ View

**For Posted Entries:**
- 👁️ View (read-only)
- 🔄 Reverse (creates new reversing entry)
- 📊 View GL Entries
- 🖨️ Print

---

## 6. Required API Endpoints

### Missing Endpoints to Implement

#### 1. Create Journal Entry
```csharp
// POST /api/accounting/journal-entries
public sealed record CreateJournalEntryCommand(
    DateTime Date,
    string ReferenceNumber,
    string Source,
    string Description,
    IReadOnlyCollection<JournalEntryLineDto> Lines,
    DefaultIdType? PeriodId = null,
    decimal OriginalAmount = 0,
    string? Notes = null
) : IRequest<DefaultIdType>;

public sealed record JournalEntryLineDto(
    DefaultIdType AccountId,
    decimal DebitAmount,
    decimal CreditAmount,
    string? Description = null
);
```

#### 2. Update Journal Entry
```csharp
// PUT /api/accounting/journal-entries/{id}
public sealed record UpdateJournalEntryCommand(
    DefaultIdType Id,
    DateTime? Date,
    string? ReferenceNumber,
    string? Source,
    string? Description,
    IReadOnlyCollection<JournalEntryLineDto>? Lines,
    DefaultIdType? PeriodId,
    decimal? OriginalAmount,
    string? Notes
) : IRequest<DefaultIdType>;
```

#### 3. Delete Journal Entry
```csharp
// DELETE /api/accounting/journal-entries/{id}
public sealed record DeleteJournalEntryCommand(DefaultIdType Id) : IRequest;
```

#### 4. Post Journal Entry
```csharp
// POST /api/accounting/journal-entries/{id}/post
public sealed record PostJournalEntryCommand(DefaultIdType Id) : IRequest;
```

#### 5. Reverse Journal Entry
```csharp
// POST /api/accounting/journal-entries/{id}/reverse
public sealed record ReverseJournalEntryCommand(
    DefaultIdType Id,
    DateTime ReversalDate,
    string ReversalReason
) : IRequest<DefaultIdType>; // Returns new reversing entry ID
```

#### 6. Approve Journal Entry
```csharp
// POST /api/accounting/journal-entries/{id}/approve
public sealed record ApproveJournalEntryCommand(
    DefaultIdType Id,
    string ApprovedBy
) : IRequest;
```

#### 7. Reject Journal Entry
```csharp
// POST /api/accounting/journal-entries/{id}/reject
public sealed record RejectJournalEntryCommand(
    DefaultIdType Id,
    string RejectedBy,
    string Reason
) : IRequest;
```

---

## 7. Blazor Implementation Guide

### File Structure

```
Pages/Accounting/JournalEntries/
├── JournalEntries.razor           # Main page
├── JournalEntries.razor.cs         # Code-behind
├── JournalEntryViewModel.cs        # View model
└── Components/
    ├── JournalEntryLineEditor.razor      # Line entry grid
    └── JournalEntryLineEditor.razor.cs
```

### JournalEntryViewModel.cs

```csharp
namespace FSH.Starter.Blazor.Client.Pages.Accounting.JournalEntries;

public class JournalEntryViewModel
{
    public DefaultIdType? Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public string ReferenceNumber { get; set; } = string.Empty;
    public string Source { get; set; } = "ManualEntry";
    public string Description { get; set; } = string.Empty;
    public DefaultIdType? PeriodId { get; set; }
    public decimal OriginalAmount { get; set; }
    public string? Notes { get; set; }
    
    // Display properties
    public bool IsPosted { get; set; }
    public string ApprovalStatus { get; set; } = "Pending";
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    
    // Lines
    public List<JournalEntryLineViewModel> Lines { get; set; } = new();
    
    // Calculated properties
    public decimal TotalDebits => Lines.Sum(l => l.DebitAmount);
    public decimal TotalCredits => Lines.Sum(l => l.CreditAmount);
    public decimal Difference => TotalDebits - TotalCredits;
    public bool IsBalanced => Math.Abs(Difference) < 0.01m;
    public bool IsValid => Lines.Count >= 2 && IsBalanced;
}

public class JournalEntryLineViewModel
{
    public DefaultIdType? Id { get; set; }
    public DefaultIdType AccountId { get; set; }
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public string? Description { get; set; }
    
    // UI State
    public bool IsDebit => DebitAmount > 0;
    public bool IsCredit => CreditAmount > 0;
}
```

### Key UI Features

#### 1. Line Entry Grid

**Features:**
- Inline editing with autocomplete for accounts
- Real-time balance calculation
- Color coding for debits (left) vs credits (right)
- Validation: One column filled, not both
- Add/Remove line buttons
- Drag-to-reorder lines (optional)

#### 2. Balance Indicator

```razor
<MudAlert Severity="@GetBalanceSeverity()" Dense="true">
    Total Debits: @model.TotalDebits.ToString("C")
    Total Credits: @model.TotalCredits.ToString("C")
    Difference: @model.Difference.ToString("C")
    @if (model.IsBalanced)
    {
        <MudIcon Icon="@Icons.Material.Filled.CheckCircle" Color="Color.Success" />
        <span>Balanced</span>
    }
    else
    {
        <MudIcon Icon="@Icons.Material.Filled.Warning" Color="Color.Warning" />
        <span>Out of Balance by @Math.Abs(model.Difference).ToString("C")</span>
    }
</MudAlert>
```

#### 3. Source Dropdown

```csharp
private readonly List<string> _sources = new()
{
    "ManualEntry",
    "InvoicePost",
    "BillPost",
    "PaymentProcessing",
    "CheckClearing",
    "BankReconciliation",
    "PeriodClose",
    "Depreciation",
    "AdjustingEntry",
    "CorrectingEntry"
};
```

#### 4. Action Buttons

```csharp
private async Task OnSaveDraft()
{
    // Save without posting
    model.ApprovalStatus = "Pending";
    await SaveJournalEntry();
}

private async Task OnSaveAndPost()
{
    // Validate balance
    if (!model.IsBalanced)
    {
        Snackbar.Add("Journal entry must be balanced before posting", Severity.Error);
        return;
    }
    
    // Save and post in one operation
    var id = await SaveJournalEntry();
    await PostJournalEntry(id);
}

private async Task OnPost(DefaultIdType id)
{
    // Post existing entry to GL
    await Client.PostJournalEntryAsync(id);
    await _table.ReloadDataAsync();
    Snackbar.Add("Journal entry posted to General Ledger", Severity.Success);
}

private async Task OnReverse(DefaultIdType id)
{
    // Show reversal dialog
    var parameters = new DialogParameters
    {
        { "JournalEntryId", id }
    };
    
    var dialog = await DialogService.ShowAsync<ReverseJournalEntryDialog>(
        "Reverse Journal Entry", parameters);
    
    var result = await dialog.Result;
    if (!result.Canceled)
    {
        await _table.ReloadDataAsync();
    }
}
```

---

## 8. Implementation Phases

### Phase 1A: API Endpoints (Week 1)

**Priority: CRITICAL**

1. ✅ Get & Search (Already done)
2. ❌ Create endpoint with lines
3. ❌ Update endpoint with lines
4. ❌ Delete endpoint
5. ❌ Post endpoint
6. ❌ Reverse endpoint
7. ❌ Approve endpoint
8. ❌ Reject endpoint

**Estimated**: 3-4 days

### Phase 1B: Blazor Page (Week 1-2)

**Priority: HIGH**

1. Basic page with EntityTable
2. ViewModel implementation
3. Line entry grid component
4. Balance calculation display
5. Create/Edit dialog
6. Status badges and filtering
7. Context actions based on status

**Estimated**: 4-5 days

### Phase 1C: Advanced Features (Week 2)

**Priority: MEDIUM**

1. Reverse entry dialog
2. Approval workflow UI
3. GL drill-down link
4. Period validation
5. Print functionality
6. Export to Excel

**Estimated**: 2-3 days

### Phase 2: Automated Integration (Future)

**Priority: LOW (Future enhancement)**

1. Invoice posted → Create JE
2. Bill posted → Create JE
3. Payment processed → Create JE
4. Check cleared → Create JE
5. Depreciation run → Create JE

**Estimated**: 1-2 weeks (depends on other features)

---

## 9. Testing Scenarios

### Manual Entry Testing

1. **Create Balanced Entry**
   - Add header information
   - Add 2+ lines with balanced debits/credits
   - Save and verify balance indicator
   - Post to GL

2. **Unbalanced Entry**
   - Try to post with debits ≠ credits
   - Verify error message
   - Correct balance and post

3. **Edit Before Posting**
   - Create entry
   - Edit lines
   - Verify balance recalculation
   - Save changes

4. **Cannot Edit After Posting**
   - Post an entry
   - Try to edit
   - Verify immutability

5. **Reversal**
   - Post an entry
   - Reverse it
   - Verify new reversing entry created
   - Verify opposite debits/credits

6. **Approval Workflow**
   - Create entry (Pending)
   - Approve
   - Post
   - Verify status progression

7. **Rejection**
   - Create entry
   - Reject
   - Verify cannot post rejected entry

### Integration Testing (Future)

1. **Invoice Posted**
   - Post invoice
   - Verify JE created automatically
   - Verify GL entries

2. **Payment Processed**
   - Process payment
   - Verify JE created
   - Verify cash and AR accounts

---

## 10. Best Practices

### UI/UX

1. **Real-time Balance Calculation**
   - Update totals on every line change
   - Show visual indicator (green checkmark / red warning)

2. **Account Autocomplete**
   - Show code and name
   - Filter by account type (optional)
   - Recent accounts suggestions

3. **Keyboard Navigation**
   - Tab through line entries
   - Enter to add new line
   - Ctrl+S to save

4. **Validation Feedback**
   - Inline validation for each line
   - Summary validation at top
   - Clear error messages

### Data Management

1. **Line Ordering**
   - Maintain user-specified order
   - Debits typically first, then credits
   - Allow reordering

2. **Defaults**
   - Date: Today
   - Source: "ManualEntry"
   - Period: Current open period

3. **Draft Saving**
   - Auto-save drafts
   - Allow save without posting
   - Resume incomplete entries

### Performance

1. **Line Entry Grid**
   - Virtualize for large entries (100+ lines)
   - Batch validation
   - Debounce balance calculation

2. **Account Lookup**
   - Cache active accounts
   - Lazy load on demand
   - Type-ahead search

---

## 11. Future Enhancements

### Short-term

1. **Recurring Journal Entry Integration**
   - Button to generate from template
   - Link to RecurringJournalEntry page

2. **Templates**
   - Save common entries as templates
   - Quick create from template

3. **Bulk Import**
   - Excel import for batch entries
   - CSV support

### Long-term

1. **Automated Posting**
   - Event-driven JE creation
   - Configurable posting rules
   - Integration with all transaction types

2. **AI-Assisted Entry**
   - Smart account suggestions
   - Pattern recognition
   - Anomaly detection

3. **Audit Trail**
   - Complete history of changes
   - Before/after comparison
   - Change reason tracking

---

## 12. Conclusion

### Recommended Approach: **Manual Entry First**

**Rationale:**
1. **Independent Implementation** - Doesn't depend on other features
2. **Immediate Value** - Core accounting need
3. **Proven Pattern** - Follows Check Management example
4. **Foundation for Automation** - Automated entries will use the same infrastructure

### Success Criteria

✅ Accountants can manually create balanced journal entries
✅ Entry validation prevents unbalanced postings
✅ Posted entries are immutable
✅ Reversal creates proper reversing entry
✅ Integration with General Ledger (manual at first, automated later)
✅ Approval workflow (optional but recommended)

### Next Steps

1. **Build API Endpoints** (Phase 1A) - 3-4 days
2. **Build Blazor Page** (Phase 1B) - 4-5 days
3. **Add Advanced Features** (Phase 1C) - 2-3 days
4. **Future: Automated Integration** (Phase 2) - When other features ready

**Total Estimated Time for Manual Entry: 10-12 days**

---

## Appendix A: Related Pages

Pages that will eventually integrate with Journal Entries:

- ✅ Chart of Accounts - Account selection
- ✅ Accounting Periods - Period validation
- 🔶 General Ledger - Target for postings (API exists, no page)
- 🔶 Invoices - Future: Auto-create JE on post
- 🔶 Bills - Future: Auto-create JE on post
- 🔶 Payments - Future: Auto-create JE on process
- ✅ Checks - Future: Auto-create JE on clear
- 🔶 Fixed Assets - Future: Auto-create JE on depreciation
- 🔶 Recurring Journal Entries - Template source

## Appendix B: Domain Events

**Currently Implemented:**
- `JournalEntryCreated`
- `JournalEntryUpdated`
- `JournalEntryPosted`
- `JournalEntryReversed`
- `JournalEntryApproved`
- `JournalEntryRejected`
- `JournalEntryLineAdded`

**Missing Handler:**
- `JournalEntryPostedEventHandler` - Should create GL entries

**Future Handlers:**
- Invoice/Bill/Payment posted → Create JE

---

**Document Version**: 1.0  
**Last Updated**: November 2, 2025  
**Status**: Ready for Implementation
