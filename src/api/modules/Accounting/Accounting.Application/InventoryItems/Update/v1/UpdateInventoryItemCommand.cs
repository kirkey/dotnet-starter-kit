namespace Accounting.Application.InventoryItems.Update.v1;

public sealed record UpdateInventoryItemCommand(
    DefaultIdType Id,
    string? Sku = null,
    string? Name = null,
    decimal? Quantity = null,
    decimal? UnitPrice = null,
    string? Description = null
) : IRequest<DefaultIdType>;

