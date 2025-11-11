using FSH.Starter.WebApi.Store.Application.SalesImports.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.SalesImports.Get.v1;

/// <summary>
/// Request to get detailed information about a sales import including all items.
/// </summary>
public class GetSalesImportRequest(DefaultIdType id) : IRequest<SalesImportDetailResponse>
{
    public DefaultIdType Id { get; set; } = id;
}

/// <summary>
/// Detailed response for a sales import including all import items.
/// </summary>
public class SalesImportDetailResponse : SalesImportResponse
{
    public List<SalesImportItemResponse> Items { get; set; } = new();
}

/// <summary>
/// Response model for individual sales import items.
/// </summary>
public class SalesImportItemResponse
{
    public DefaultIdType Id { get; set; }
    public int LineNumber { get; set; }
    public DateTime SaleDate { get; set; }
    public string Barcode { get; set; } = default!;
    public string? ItemName { get; set; }
    public int QuantitySold { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? TotalAmount { get; set; }
    public DefaultIdType? ItemId { get; set; }
    public string? ItemSKU { get; set; }
    public DefaultIdType? InventoryTransactionId { get; set; }
    public string? TransactionNumber { get; set; }
    public bool IsProcessed { get; set; }
    public bool HasError { get; set; }
    public string? ErrorMessage { get; set; }
}

