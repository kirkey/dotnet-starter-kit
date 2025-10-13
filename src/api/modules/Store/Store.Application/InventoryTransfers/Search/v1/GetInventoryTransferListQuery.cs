namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Search.v1;

/// <summary>
/// Command for searching inventory transfers with pagination and filtering support.
/// </summary>
public class SearchInventoryTransfersCommand : PaginationFilter, IRequest<PagedList<GetInventoryTransferListResponse>>
{
    /// <summary>
    /// Search term to filter by transfer number, transport method, tracking number, or requested by.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Filter by source warehouse ID.
    /// </summary>
    public DefaultIdType? FromWarehouseId { get; set; }

    /// <summary>
    /// Filter by destination warehouse ID.
    /// </summary>
    public DefaultIdType? ToWarehouseId { get; set; }

    /// <summary>
    /// Filter by transfer status.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filter by transfer date from this date onwards.
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Filter by transfer date up to this date.
    /// </summary>
    public DateTime? ToDate { get; set; }
}
