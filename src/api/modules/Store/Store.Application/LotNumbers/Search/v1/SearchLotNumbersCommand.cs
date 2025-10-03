namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Search.v1;

/// <summary>
/// Command to search lot numbers with filters and pagination.
/// </summary>
public sealed class SearchLotNumbersCommand : PaginationFilter, IRequest<PagedList<LotNumberResponse>>
{
    /// <summary>
    /// Filter by item ID.
    /// </summary>
    public DefaultIdType? ItemId { get; init; }

    /// <summary>
    /// Filter by supplier ID.
    /// </summary>
    public DefaultIdType? SupplierId { get; init; }

    /// <summary>
    /// Filter by lot code (partial match).
    /// </summary>
    public string? LotCode { get; init; }

    /// <summary>
    /// Filter by status (Active, Expired, Quarantine, Recalled).
    /// </summary>
    public string? Status { get; init; }

    /// <summary>
    /// Filter by expiration date range start.
    /// </summary>
    public DateTime? ExpirationDateFrom { get; init; }

    /// <summary>
    /// Filter by expiration date range end.
    /// </summary>
    public DateTime? ExpirationDateTo { get; init; }

    /// <summary>
    /// Filter by expiring within specified days.
    /// </summary>
    public int? ExpiringWithinDays { get; init; }

    /// <summary>
    /// Filter to show only expired lots.
    /// </summary>
    public bool? IsExpired { get; init; }

    /// <summary>
    /// Filter by minimum remaining quantity.
    /// </summary>
    public int? MinQuantityRemaining { get; init; }
}
