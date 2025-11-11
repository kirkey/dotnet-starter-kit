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
public sealed record SalesImportDetailResponse(
    DefaultIdType Id,
    string ImportNumber,
    DateTime ImportDate,
    DateTime SalesPeriodFrom,
    DateTime SalesPeriodTo,
    DefaultIdType WarehouseId,
    string? WarehouseName,
    string FileName,
    int TotalRecords,
    int ProcessedRecords,
    int ErrorRecords,
    int TotalQuantity,
    decimal? TotalValue,
    string Status,
    bool IsReversed,
    DateTime? ReversedDate,
    string? ReversedBy,
    string? ReversalReason,
    string? ProcessedBy,
    string? ErrorMessage,
    string? Notes,
    IReadOnlyCollection<SalesImportItemResponse> Items);

/// <summary>
/// Response model for individual sales import items.
/// </summary>
public sealed record SalesImportItemResponse(
    DefaultIdType Id,
    int LineNumber,
    DateTime SaleDate,
    string Barcode,
    string? ItemName,
    int QuantitySold,
    decimal? UnitPrice,
    decimal? TotalAmount,
    DefaultIdType? ItemId,
    string? ItemSKU,
    DefaultIdType? InventoryTransactionId,
    string? TransactionNumber,
    bool IsProcessed,
    bool HasError,
    string? ErrorMessage);

