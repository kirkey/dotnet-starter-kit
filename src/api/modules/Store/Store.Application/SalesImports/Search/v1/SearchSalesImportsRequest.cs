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
    public string? Status { get; set; }
    public bool? IsReversed { get; set; }
}

/// <summary>
/// Response model for sales import search results.
/// </summary>
public class SalesImportResponse
{
    public DefaultIdType Id { get; set; }
    public string ImportNumber { get; set; } = default!;
    public DateTime ImportDate { get; set; }
    public DateTime SalesPeriodFrom { get; set; }
    public DateTime SalesPeriodTo { get; set; }
    public DefaultIdType WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public string FileName { get; set; } = default!;
    public int TotalRecords { get; set; }
    public int ProcessedRecords { get; set; }
    public int ErrorRecords { get; set; }
    public int TotalQuantity { get; set; }
    public decimal? TotalValue { get; set; }
    public string Status { get; set; } = default!;
    public bool IsReversed { get; set; }
    public DateTime? ReversedDate { get; set; }
    public string? ReversedBy { get; set; }
    public string? ReversalReason { get; set; }
    public string? ProcessedBy { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedOn { get; set; }
}

