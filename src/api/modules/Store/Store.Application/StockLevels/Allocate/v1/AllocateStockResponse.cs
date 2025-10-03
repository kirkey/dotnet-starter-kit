namespace FSH.Starter.WebApi.Store.Application.StockLevels.Allocate.v1;

/// <summary>
/// Response after allocating stock.
/// </summary>
public sealed record AllocateStockResponse(
    DefaultIdType StockLevelId,
    int AllocatedQuantity,
    int RemainingReserved
);
