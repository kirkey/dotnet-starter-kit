namespace FSH.Starter.WebApi.Store.Application.SalesImports.Search.v1;

/// <summary>
/// Request to search sales imports with filtering and pagination.
/// </summary>
public class SearchSalesImportsRequest : PaginationFilter, IRequest<PagedList<SalesImportResponse>>
{
    public string? ImportNumber { get; set; }
    public DefaultIdType? WarehouseId { get; set; }
    public DateTime? SalesPeriodFrom { get; set; }
    public DateTime? SalesPeriodTo { get; set; }
    public DateTime? ImportDateFrom { get; set; }
    public DateTime? ImportDateTo { get; set; }
    public string? Status { get; set; }
    public bool? IsReversed { get; set; }
}
