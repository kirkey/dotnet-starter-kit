namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Update.v1;

public sealed record UpdateStockAdjustmentCommand(
    DefaultIdType Id,
    DefaultIdType GroceryItemId,
    DefaultIdType WarehouseLocationId,
    string AdjustmentType,
    int QuantityAdjusted,
    string Reason,
    string? Notes = null) : IRequest<UpdateStockAdjustmentResponse>;
