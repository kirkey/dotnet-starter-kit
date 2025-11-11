namespace FSH.Starter.WebApi.Store.Application.SalesImports.Search.v1;

/// <summary>
/// Response model for sales import search results.
/// Represents a batch import of sales data from external Point of Sale systems.
/// </summary>
public sealed record SalesImportResponse(
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
    string? Notes);

