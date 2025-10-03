namespace FSH.Starter.WebApi.Store.Application.StockLevels.Allocate.v1;

/// <summary>
/// Command to allocate reserved quantity to a pick list.
/// </summary>
public sealed record AllocateStockCommand(
    DefaultIdType StockLevelId,
    int Quantity
) : IRequest<AllocateStockResponse>;
