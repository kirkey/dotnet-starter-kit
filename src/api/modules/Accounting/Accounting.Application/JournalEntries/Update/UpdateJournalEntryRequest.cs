namespace Accounting.Application.JournalEntries.Update;

/// <summary>
/// Command to update a JournalEntry's metadata (not allowed if posted).
/// </summary>
public sealed record UpdateJournalEntryCommand : IRequest<UpdateJournalEntryResponse>
{
    /// <summary>
    /// The ID of the journal entry to update.
    /// </summary>
    public DefaultIdType Id { get; init; }
    
    /// <summary>
    /// External reference number.
    /// </summary>
    public string? ReferenceNumber { get; init; }
    
    /// <summary>
    /// Transaction date.
    /// </summary>
    public DateTime? Date { get; init; }
    
    /// <summary>
    /// Source system or module.
    /// </summary>
    public string? Source { get; init; }
    
    /// <summary>
    /// Accounting period ID.
    /// </summary>
    public DefaultIdType? PeriodId { get; init; }
    
    /// <summary>
    /// Original transaction amount.
    /// </summary>
    public decimal? OriginalAmount { get; init; }
    
    /// <summary>
    /// Description of the journal entry.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; init; }
}
