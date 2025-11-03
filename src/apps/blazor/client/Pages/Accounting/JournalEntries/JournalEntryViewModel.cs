using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.JournalEntries;

/// <summary>
/// View model for creating and editing journal entries.
/// Includes validation and calculated properties for balance checking.
/// </summary>
public class JournalEntryViewModel
{
    /// <summary>
    /// Journal entry identifier (null for new entries).
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Effective date of the journal entry transaction.
    /// </summary>
    [Required(ErrorMessage = "Date is required")]
    public DateTime? Date { get; set; } = DateTime.Today;

    /// <summary>
    /// External reference or document number.
    /// </summary>
    [Required(ErrorMessage = "Reference number is required")]
    [StringLength(50, ErrorMessage = "Reference number cannot exceed 50 characters")]
    public string ReferenceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Source system or module that created the entry.
    /// </summary>
    [Required(ErrorMessage = "Source is required")]
    public string Source { get; set; } = "ManualEntry";

    /// <summary>
    /// Description of the journal entry purpose.
    /// </summary>
    [Required(ErrorMessage = "Description is required")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Optional accounting period identifier.
    /// </summary>
    public DefaultIdType? PeriodId { get; set; }

    /// <summary>
    /// Original transaction amount for reference.
    /// </summary>
    public decimal OriginalAmount { get; set; }

    /// <summary>
    /// Additional notes for the journal entry.
    /// </summary>
    public string? Notes { get; set; }

    // Display properties
    /// <summary>
    /// Indicates whether the entry has been posted to GL.
    /// </summary>
    public bool IsPosted { get; set; }

    /// <summary>
    /// Approval status: Pending, Approved, or Rejected.
    /// </summary>
    public string ApprovalStatus { get; set; } = "Pending";

    /// <summary>
    /// User who approved or rejected the entry.
    /// </summary>
    public string? ApprovedBy { get; set; }

    /// <summary>
    /// Date/time when approved or rejected.
    /// </summary>
    public DateTime? ApprovedDate { get; set; }

    /// <summary>
    /// Collection of journal entry line items (debits and credits).
    /// </summary>
    [Required(ErrorMessage = "At least 2 lines are required")]
    [MinLength(2, ErrorMessage = "At least 2 lines are required for a balanced entry")]
    public List<JournalEntryLineViewModel> Lines { get; set; } = new();

    // Calculated properties
    /// <summary>
    /// Total of all debit amounts in the entry.
    /// </summary>
    public decimal TotalDebits => Lines.Sum(l => l.DebitAmount);

    /// <summary>
    /// Total of all credit amounts in the entry.
    /// </summary>
    public decimal TotalCredits => Lines.Sum(l => l.CreditAmount);

    /// <summary>
    /// Difference between total debits and credits.
    /// Should be zero for a balanced entry.
    /// </summary>
    public decimal Difference => TotalDebits - TotalCredits;

    /// <summary>
    /// Indicates whether the entry is balanced (debits = credits within tolerance).
    /// </summary>
    public bool IsBalanced => Math.Abs(Difference) < 0.01m;

    /// <summary>
    /// Indicates whether the entry is valid for posting.
    /// Requires at least 2 lines and balanced debits/credits.
    /// </summary>
    public bool IsValid => Lines.Count >= 2 && IsBalanced;
}

/// <summary>
/// View model for individual journal entry line items.
/// Represents a single debit or credit to an account.
/// </summary>
public class JournalEntryLineViewModel
{
    /// <summary>
    /// Line identifier (null for new lines).
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Chart of account identifier.
    /// </summary>
    [Required(ErrorMessage = "Account is required")]
    public DefaultIdType AccountId { get; set; }

    /// <summary>
    /// Account code for display purposes.
    /// </summary>
    public string AccountCode { get; set; } = string.Empty;

    /// <summary>
    /// Account name for display purposes.
    /// </summary>
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// Debit amount (zero if this is a credit line).
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Debit amount cannot be negative")]
    public decimal DebitAmount { get; set; }

    /// <summary>
    /// Credit amount (zero if this is a debit line).
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Credit amount cannot be negative")]
    public decimal CreditAmount { get; set; }

    /// <summary>
    /// Optional description for this line item.
    /// </summary>
    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    public string? Description { get; set; }

    // UI State properties
    /// <summary>
    /// Indicates whether this is a debit line.
    /// </summary>
    public bool IsDebit => DebitAmount > 0;

    /// <summary>
    /// Indicates whether this is a credit line.
    /// </summary>
    public bool IsCredit => CreditAmount > 0;
}

