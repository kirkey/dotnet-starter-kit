namespace FSH.Starter.WebApi.Store.Application.StockLevels.Reserve.v1;

/// <summary>
/// Command to reserve quantity from available stock.
/// </summary>
public sealed record ReserveStockCommand(
    DefaultIdType StockLevelId,
    int Quantity
) : IRequest<ReserveStockResponse>;
