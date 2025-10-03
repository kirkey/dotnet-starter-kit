namespace FSH.Starter.WebApi.Store.Application.StockLevels.Create.v1;

/// <summary>
/// Command to create a new stock level record.
/// </summary>
public sealed record CreateStockLevelCommand(
    DefaultIdType ItemId,
    DefaultIdType WarehouseId,
    DefaultIdType? WarehouseLocationId,
    DefaultIdType? BinId,
    DefaultIdType? LotNumberId,
    DefaultIdType? SerialNumberId,
    int QuantityOnHand
) : IRequest<CreateStockLevelResponse>;
