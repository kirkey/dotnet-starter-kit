using FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Search.v1;

/// <summary>
/// Request to search cycle counts with filters and pagination.
/// </summary>
public class SearchCycleCountsRequest : PaginationFilter, IRequest<PagedList<CycleCountResponse>>
{
    /// <summary>
    /// Filter by count number.
    /// </summary>
    public string? CountNumber { get; set; }
    
    /// <summary>
    /// Filter by status (Scheduled, InProgress, Completed, Cancelled).
    /// </summary>
    public string? Status { get; set; }
    
    /// <summary>
    /// Filter by warehouse.
    /// </summary>
    public DefaultIdType? WarehouseId { get; set; }
    
    /// <summary>
    /// Filter by date range start.
    /// </summary>
    public DateTime? CountDateFrom { get; set; }
    
    /// <summary>
    /// Filter by date range end.
    /// </summary>
    public DateTime? CountDateTo { get; set; }
}

