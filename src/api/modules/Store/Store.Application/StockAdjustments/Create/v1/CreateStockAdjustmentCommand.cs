namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Create.v1;

public record CreateStockAdjustmentCommand(
    [property: DefaultValue("Stock Count Adjustment")] string Name,
    [property: DefaultValue("Inventory count adjustment")] string? Description,
    [property: DefaultValue("ADJ001")] string AdjustmentNumber,
    DefaultIdType ItemId,
    DefaultIdType WarehouseId,
    DefaultIdType? WarehouseLocationId,
    [property: DefaultValue("2024-01-01")] DateTime AdjustmentDate,
    [property: DefaultValue("Physical Count")] string AdjustmentType,
    [property: DefaultValue("Pending")] string Status,
    [property: DefaultValue("Monthly inventory count")] string Reason,
    [property: DefaultValue(0)] int QuantityBefore,
    [property: DefaultValue(0)] int AdjustmentQuantity,
    [property: DefaultValue(0.0)] decimal UnitCost,
    [property: DefaultValue(null)] string? Reference = null,
    [property: DefaultValue(null)] string? Notes = null,
    [property: DefaultValue(null)] string? AdjustedBy = null,
    [property: DefaultValue(null)] string? BatchNumber = null,
    [property: DefaultValue(null)] DateTime? ExpiryDate = null) : IRequest<CreateStockAdjustmentResponse>;
