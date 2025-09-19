namespace Accounting.Application.JournalEntries.Responses;

/// <summary>
/// Response model representing a journal entry.
/// Contains the main journal entry information including date, reference, and posting status.
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
}
