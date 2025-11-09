namespace Accounting.Application.InventoryItems.AddStock.v1;

/// <summary>
/// Command to add stock to an inventory item.
/// </summary>
public sealed record AddStockCommand(
    DefaultIdType Id, 
    decimal Quantity) : IRequest<DefaultIdType>;
