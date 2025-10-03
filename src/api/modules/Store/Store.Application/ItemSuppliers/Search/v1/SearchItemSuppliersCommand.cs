namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Search.v1;

/// <summary>
/// Request to search item-supplier relationships with filters and pagination.
/// </summary>
public class SearchItemSuppliersCommand : PaginationFilter, IRequest<PagedList<ItemSupplierResponse>>
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
    /// Filter by active status.
    /// </summary>
    public bool? IsActive { get; init; }

    /// <summary>
    /// Filter by preferred status.
    /// </summary>
    public bool? IsPreferred { get; init; }

    /// <summary>
    /// Filter by currency code.
    /// </summary>
    public string? CurrencyCode { get; init; }

    /// <summary>
    /// Filter by minimum reliability rating.
    /// </summary>
    public decimal? MinReliabilityRating { get; init; }
}
