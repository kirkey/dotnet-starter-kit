namespace Accounting.Application.JournalEntries.Responses;

/// <summary>
/// Response model representing a journal entry line item.
/// Contains the detailed line information for each account affected by the journal entry.
/// </summary>
public class JournalEntryLineResponse(
    DefaultIdType id,
    DefaultIdType journalEntryId,
    DefaultIdType accountId,
    decimal debitAmount,
    decimal creditAmount,
    string? memo,
    string? reference)
{
    /// <summary>
    /// Unique identifier for the journal entry line.
    /// </summary>
    public DefaultIdType Id { get; set; } = id;
    
    /// <summary>
    /// Reference to the parent journal entry.
    /// </summary>
    public DefaultIdType JournalEntryId { get; set; } = journalEntryId;
    
    /// <summary>
    /// Account identifier for this line item.
    /// </summary>
    public DefaultIdType AccountId { get; set; } = accountId;
    
    /// <summary>
    /// Debit amount for this line (zero if credit entry).
    /// </summary>
    public decimal DebitAmount { get; set; } = debitAmount;
    
    /// <summary>
    /// Credit amount for this line (zero if debit entry).
    /// </summary>
    public decimal CreditAmount { get; set; } = creditAmount;
    
    /// <summary>
    /// Optional memo or description for this line item.
    /// </summary>
    public string? Memo { get; set; } = memo;
    
    /// <summary>
    /// Optional reference number or document reference for this line.
    /// </summary>
    public string? Reference { get; set; } = reference;
}
