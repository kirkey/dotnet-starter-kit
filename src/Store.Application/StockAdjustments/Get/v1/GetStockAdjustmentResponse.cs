namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;

public record GetStockAdjustmentResponse(
    DefaultIdType Id,
    string Name,
    string? Description,
    string AdjustmentNumber,
    DefaultIdType WarehouseId,
    string WarehouseName,
    DateTime AdjustmentDate,
    string AdjustmentType,
    string Status,
    string Reason,
    string? Notes,
    DateTime CreatedOn,
    DateTime? LastModifiedOn);
