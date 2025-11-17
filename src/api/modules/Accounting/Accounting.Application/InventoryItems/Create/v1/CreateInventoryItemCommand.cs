namespace Accounting.Application.InventoryItems.Create.v1;

public sealed record CreateInventoryItemCommand(
    string Sku,
    string Name,
    decimal Quantity,
    decimal UnitPrice,
    string? Description = null,
    string? ImageUrl = null
) : IRequest<DefaultIdType>;
