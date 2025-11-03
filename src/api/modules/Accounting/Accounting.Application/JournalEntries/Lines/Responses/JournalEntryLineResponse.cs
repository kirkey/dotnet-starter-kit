namespace Accounting.Application.JournalEntries.Lines.Responses;

/// <summary>
/// Response model for a journal entry line.
/// </summary>
public sealed record JournalEntryLineResponse
{
    /// <summary>
    /// The unique identifier for this line.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// The parent journal entry identifier.
    /// </summary>
    public DefaultIdType JournalEntryId { get; init; }

    /// <summary>
    /// The chart of account identifier.
    /// </summary>
    public DefaultIdType AccountId { get; init; }

    /// <summary>
    /// The account code for display purposes.
    /// </summary>
    public string? AccountCode { get; init; }

    /// <summary>
    /// The account name for display purposes.
    /// </summary>
    public string? AccountName { get; init; }

    /// <summary>
    /// Debit amount for this line.
    /// </summary>
    public decimal DebitAmount { get; init; }

    /// <summary>
    /// Credit amount for this line.
    /// </summary>
    public decimal CreditAmount { get; init; }

    /// <summary>
    /// Optional memo/description.
    /// </summary>
    public string? Memo { get; init; }

    /// <summary>
    /// Optional reference number.
    /// </summary>
    public string? Reference { get; init; }
}

