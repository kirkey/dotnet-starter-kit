namespace Accounting.Application.Payments.Search.v1;

/// <summary>
/// Request to search payments with filtering and pagination.
/// </summary>
public class PaymentSearchRequest : PaginationFilter, IRequest<PagedList<PaymentSearchResponse>>
{
    /// <summary>
    /// Optional payment number filter.
    /// </summary>
    public string? PaymentNumber { get; init; }

    /// <summary>
    /// Optional member ID filter.
    /// </summary>
    public DefaultIdType? MemberId { get; init; }

    /// <summary>
    /// Optional payment method filter.
    /// </summary>
    public string? PaymentMethod { get; init; }

    /// <summary>
    /// Optional start date for payment date range.
    /// </summary>
    public DateTime? StartDate { get; init; }

    /// <summary>
    /// Optional end date for payment date range.
    /// </summary>
    public DateTime? EndDate { get; init; }

    /// <summary>
    /// Optional minimum amount filter.
    /// </summary>
    public decimal? MinAmount { get; init; }

    /// <summary>
    /// Optional maximum amount filter.
    /// </summary>
    public decimal? MaxAmount { get; init; }

    /// <summary>
    /// Filter for payments with unapplied amounts.
    /// </summary>
    public bool? HasUnappliedAmount { get; init; }
}
