namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;

/// <summary>
/// Response for stock adjustment operations.
/// </summary>
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
    DefaultIdType? CreatedBy,
    bool IsApproved,
    string? ApprovedBy,
    DateTime? ApprovalDate);
