using FSH.Starter.WebApi.Store.Application.Bins.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.Bins.Search.v1;

/// <summary>
/// Command for searching bins with filtering and pagination.
/// </summary>
public class SearchBinsCommand : PaginationFilter, IRequest<PagedList<BinResponse>>
{
    /// <summary>
    /// Filter by search term (partial match on name).
    /// </summary>
    public string? SearchTerm { get; init; }

    /// <summary>
    /// Filter by warehouse location identifier.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; init; }

    /// <summary>
    /// Filter by active status.
    /// </summary>
    public bool? IsActive { get; init; }
}
