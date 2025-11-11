namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;

public sealed record StockAdjustmentResponse(
    DefaultIdType? Id,
    DefaultIdType ItemId,
    string? ItemName,
    DefaultIdType WarehouseLocationId,
    string? WarehouseLocationName,
    string AdjustmentType,
    int QuantityAdjusted,
    string Reason,
    DateTime AdjustmentDate,
    string? Notes,
    DefaultIdType? CreatedBy);
