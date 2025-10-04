namespace FSH.Starter.WebApi.Store.Application.StockLevels.Create.v1;

/// <summary>
/// Command to create a new stock level record.
/// </summary>
public sealed record CreateStockLevelCommand(
    string? Name,
    string? Description,
    string? Notes,
    DefaultIdType ItemId,
    DefaultIdType WarehouseId,
    DefaultIdType? WarehouseLocationId,
    DefaultIdType? BinId,
    DefaultIdType? LotNumberId,
    DefaultIdType? SerialNumberId,
    int QuantityOnHand
) : IRequest<CreateStockLevelResponse>;
