namespace Accounting.Application.GeneralLedgers.Search.v1;

/// <summary>
/// Request to search general ledger entries with filtering and pagination.
/// </summary>
public sealed class GeneralLedgerSearchRequest : PaginationFilter, IRequest<PagedList<GeneralLedgerSearchResponse>>
{
    /// <summary>
    /// Optional journal entry ID filter.
    /// </summary>
    public DefaultIdType? EntryId { get; init; }

    /// <summary>
    /// Optional account ID filter.
    /// </summary>
    public DefaultIdType? AccountId { get; init; }

    /// <summary>
    /// Optional period ID filter.
    /// </summary>
    public DefaultIdType? PeriodId { get; init; }

    /// <summary>
    /// Optional USOA class filter.
    /// </summary>
    public string? UsoaClass { get; init; }

    /// <summary>
    /// Optional start date for transaction date range.
    /// </summary>
    public DateTime? StartDate { get; init; }

    /// <summary>
    /// Optional end date for transaction date range.
    /// </summary>
    public DateTime? EndDate { get; init; }

    /// <summary>
    /// Optional minimum debit amount filter.
    /// </summary>
    public decimal? MinDebit { get; init; }

    /// <summary>
    /// Optional maximum debit amount filter.
    /// </summary>
    public decimal? MaxDebit { get; init; }

    /// <summary>
    /// Optional minimum credit amount filter.
    /// </summary>
    public decimal? MinCredit { get; init; }

    /// <summary>
    /// Optional maximum credit amount filter.
    /// </summary>
    public decimal? MaxCredit { get; init; }

    /// <summary>
    /// Optional reference number filter (partial match).
    /// </summary>
    public string? ReferenceNumber { get; init; }
}
