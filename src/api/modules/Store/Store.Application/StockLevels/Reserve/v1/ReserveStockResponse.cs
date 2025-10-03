namespace FSH.Starter.WebApi.Store.Application.StockLevels.Reserve.v1;

/// <summary>
/// Response after reserving stock.
/// </summary>
public sealed record ReserveStockResponse(
    DefaultIdType StockLevelId,
    int ReservedQuantity,
    int RemainingAvailable
);
