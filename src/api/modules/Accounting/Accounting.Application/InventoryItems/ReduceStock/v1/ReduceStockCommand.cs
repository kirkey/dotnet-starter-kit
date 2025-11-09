namespace Accounting.Application.InventoryItems.ReduceStock.v1;

/// <summary>
/// Command to reduce stock from an inventory item.
/// </summary>
public sealed record ReduceStockCommand(
    DefaultIdType Id, 
    decimal Quantity) : IRequest<DefaultIdType>;
