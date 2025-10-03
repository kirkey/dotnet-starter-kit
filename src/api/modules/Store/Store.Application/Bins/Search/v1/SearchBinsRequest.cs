using FSH.Starter.WebApi.Store.Application.Bins.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.Bins.Search.v1;

public class SearchBinsRequest : PaginationFilter, IRequest<PagedList<BinResponse>>
{
    public DefaultIdType? WarehouseLocationId { get; set; }
    public string? BinType { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsPickable { get; set; }
    public bool? IsPutable { get; set; }
}
