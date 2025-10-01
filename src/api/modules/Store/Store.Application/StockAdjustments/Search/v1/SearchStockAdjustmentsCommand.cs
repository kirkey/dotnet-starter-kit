using FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Search.v1;

public class SearchStockAdjustmentsCommand : PaginationFilter, IRequest<PagedList<StockAdjustmentResponse>>
{
    public DefaultIdType? GroceryItemId { get; set; }
    public DefaultIdType? WarehouseLocationId { get; set; }
    public string? AdjustmentType { get; set; }
    public string? Reason { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
}
