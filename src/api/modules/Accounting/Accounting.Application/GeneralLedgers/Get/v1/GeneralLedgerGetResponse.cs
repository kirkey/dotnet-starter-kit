namespace Accounting.Application.GeneralLedgers.Get.v1;

/// <summary>
/// Response containing general ledger entry details.
/// </summary>
public sealed record GeneralLedgerGetResponse
{
    /// <summary>
    /// The unique identifier of the general ledger entry.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// The journal entry ID that created this GL entry.
    /// </summary>
    public DefaultIdType EntryId { get; init; }

    /// <summary>
    /// The chart of account ID.
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
    /// Transaction memo/description.
    /// </summary>
    public string? Memo { get; init; }

    /// <summary>
    /// USOA classification (Generation, Transmission, Distribution, etc.).
    /// </summary>
    public string UsoaClass { get; init; } = string.Empty;

    /// <summary>
    /// Transaction effective date.
    /// </summary>
    public DateTime TransactionDate { get; init; }

    /// <summary>
    /// Reference number (invoice, check, etc.).
    /// </summary>
    public string? ReferenceNumber { get; init; }

    /// <summary>
    /// Accounting period ID.
    /// </summary>
    public DefaultIdType? PeriodId { get; init; }

    /// <summary>
    /// Additional description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; init; }

    /// <summary>
    /// When the entry was created.
    /// </summary>
    public DateTime CreatedOn { get; init; }

    /// <summary>
    /// Who created the entry.
    /// </summary>
    public string? CreatedByUserName { get; init; }
}

