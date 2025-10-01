using FSH.Starter.WebApi.Store.Application.CycleCounts.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Search.v1;

public class SearchCycleCountsCommand : PaginationFilter, IRequest<PagedList<CycleCountResponse>>
{
    public string? CountNumber { get; set; }
    public string? Status { get; set; }
    public DefaultIdType? WarehouseId { get; set; }
}

