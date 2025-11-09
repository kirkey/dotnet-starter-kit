namespace Accounting.Application.GeneralLedgers.Search.v1;

/// <summary>
/// Response for general ledger search results.
/// </summary>
public sealed record GeneralLedgerSearchResponse
{
    /// <summary>
    /// The general ledger entry ID.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// The journal entry ID.
    /// </summary>
    public DefaultIdType EntryId { get; init; }

    /// <summary>
    /// The account ID.
    /// </summary>
    public DefaultIdType AccountId { get; init; }

    /// <summary>
    /// Debit amount.
    /// </summary>
    public decimal Debit { get; init; }

    /// <summary>
    /// Credit amount.
    /// </summary>
    public decimal Credit { get; init; }

    /// <summary>
    /// Transaction memo.
    /// </summary>
    public string? Memo { get; init; }

    /// <summary>
    /// USOA classification.
    /// </summary>
    public string UsoaClass { get; init; } = string.Empty;

    /// <summary>
    /// Transaction date.
    /// </summary>
    public DateTime TransactionDate { get; init; }

    /// <summary>
    /// Reference number.
    /// </summary>
    public string? ReferenceNumber { get; init; }

    /// <summary>
    /// Period ID.
    /// </summary>
    public DefaultIdType? PeriodId { get; init; }

    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// When created (UTC).
    /// </summary>
    public DateTimeOffset CreatedOn { get; init; }

    /// <summary>
    /// Indicates whether this entry has been posted to the general ledger.
    /// </summary>
    public bool IsPosted { get; init; }

    /// <summary>
    /// Source type of the transaction (JournalEntry, Invoice, Bill, Payment, etc.).
    /// </summary>
    public string? Source { get; init; }

    /// <summary>
    /// Source document identifier for complete audit trail.
    /// </summary>
    public DefaultIdType? SourceId { get; init; }
}

