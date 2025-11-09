namespace FSH.Starter.Blazor.Client.Pages.Accounting.InventoryItems;

/// <summary>
/// ViewModel used for creating or editing inventory items.
/// </summary>
public sealed class InventoryItemViewModel
{
    public DefaultIdType? Id { get; set; }
    public string? Sku { get; set; }
    public string? Name { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

