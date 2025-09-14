namespace Accounting.Application.Inventory.Commands;

public class CreateInventoryItemCommand : IRequest<DefaultIdType>
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Description { get; set; }
}

