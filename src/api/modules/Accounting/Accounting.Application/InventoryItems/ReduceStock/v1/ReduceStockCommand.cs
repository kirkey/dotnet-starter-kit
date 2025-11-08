namespace Accounting.Application.InventoryItems.ReduceStock.v1;

public sealed record ReduceStockCommand(DefaultIdType Id, decimal Amount) : IRequest<DefaultIdType>;

