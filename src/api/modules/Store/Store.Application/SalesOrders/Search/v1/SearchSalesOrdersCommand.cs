namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Search.v1;

public class SearchSalesOrdersCommand : PaginationFilter, IRequest<PagedList<Get.v1.GetSalesOrderResponse>>
{
    public string? OrderNumber { get; set; }
    public DefaultIdType? CustomerId { get; set; }
    public string? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public bool? IsUrgent { get; set; }
    public DefaultIdType? WarehouseId { get; set; }
}
