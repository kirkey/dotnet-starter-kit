namespace FSH.Starter.WebApi.Store.Application.PickLists.Search.v1;

public class SearchPickListsCommand : PaginationFilter, IRequest<PagedList<PickListResponse>>
{
    public string? PickListNumber { get; set; }
    public DefaultIdType? WarehouseId { get; set; }
    public string? Status { get; set; }
    public string? PickingType { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public DateTime? CompletedDateFrom { get; set; }
    public DateTime? CompletedDateTo { get; set; }
    public int? MinPriority { get; set; }
    public int? MaxPriority { get; set; }
}
