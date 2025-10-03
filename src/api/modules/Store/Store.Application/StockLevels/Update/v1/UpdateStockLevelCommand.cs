namespace FSH.Starter.WebApi.Store.Application.StockLevels.Update.v1;

/// <summary>
/// Command to update a stock level record.
/// </summary>
public sealed record UpdateStockLevelCommand(
    DefaultIdType Id,
    DefaultIdType? WarehouseLocationId,
    DefaultIdType? BinId,
    DefaultIdType? LotNumberId,
    DefaultIdType? SerialNumberId
) : IRequest<UpdateStockLevelResponse>;
