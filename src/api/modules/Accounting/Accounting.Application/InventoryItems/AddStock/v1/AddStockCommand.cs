namespace Accounting.Application.InventoryItems.AddStock.v1;

public sealed record AddStockCommand(DefaultIdType Id, decimal Amount) : IRequest<DefaultIdType>;

