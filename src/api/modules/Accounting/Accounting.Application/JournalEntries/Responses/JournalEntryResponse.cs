using Accounting.Application.JournalEntries.Lines.Responses;

namespace Accounting.Application.JournalEntries.Responses;

/// <summary>
/// Response model representing a journal entry.
/// Contains the main journal entry information including date, reference, posting status, and approval information.
/// </summary>
public class JournalEntryResponse : BaseDto
{
    /// <summary>
    /// Date of the journal entry transaction.
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Unique reference number for the journal entry.
    /// </summary>
    public string ReferenceNumber { get; set; } = null!;
    
    /// <summary>
    /// Source system or module that created this journal entry.
    /// </summary>
    public string Source { get; set; } = null!;
    
    /// <summary>
    /// Indicates whether the journal entry has been posted to the general ledger.
    /// </summary>
    public bool IsPosted { get; set; }
    
    /// <summary>
    /// Accounting period identifier for this journal entry.
    /// </summary>
    public DefaultIdType? PeriodId { get; set; }
    
    /// <summary>
    /// Original transaction amount before any adjustments.
    /// </summary>
    public decimal OriginalAmount { get; set; }
    
    /// <summary>
    /// Approval status: Pending, Approved, or Rejected.
    /// </summary>
    public string ApprovalStatus { get; set; } = "Pending";
    
    /// <summary>
    /// User who approved or rejected the journal entry.
    /// </summary>
    public string? ApprovedBy { get; set; }
    
    /// <summary>
    /// Date and time when the entry was approved or rejected.
    /// </summary>
    public DateTime? ApprovedDate { get; set; }
    
    /// <summary>
    /// Collection of journal entry line items (debits and credits).
    /// </summary>
    public List<JournalEntryLineResponse> Lines { get; set; } = new();
    
    /// <summary>
    /// Calculated total of all debit amounts.
    /// </summary>
    public decimal TotalDebits => Lines.Sum(l => l.DebitAmount);
    
    /// <summary>
    /// Calculated total of all credit amounts.
    /// </summary>
    public decimal TotalCredits => Lines.Sum(l => l.CreditAmount);
    
    /// <summary>
    /// Difference between debits and credits (should be zero for balanced entries).
    /// </summary>
    public decimal Difference => TotalDebits - TotalCredits;
    
    /// <summary>
    /// Indicates whether the entry is balanced (debits = credits within tolerance).
    /// </summary>
    public bool IsBalanced => Math.Abs(Difference) < 0.01m;
}
