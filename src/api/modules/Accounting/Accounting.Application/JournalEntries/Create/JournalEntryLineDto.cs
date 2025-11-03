namespace Accounting.Application.JournalEntries.Create;

/// <summary>
/// Data transfer object for creating journal entry lines within a journal entry.
/// </summary>
public sealed record JournalEntryLineDto
{
    /// <summary>
    /// The chart of account identifier this line applies to.
    /// </summary>
    public DefaultIdType AccountId { get; init; }

    /// <summary>
    /// The debit amount (must be non-negative).
    /// </summary>
    public decimal DebitAmount { get; init; }

    /// <summary>
    /// The credit amount (must be non-negative).
    /// </summary>
    public decimal CreditAmount { get; init; }

    /// <summary>
    /// Optional description for this line item.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Optional reference for this line item.
    /// </summary>
    public string? Reference { get; init; }
}

