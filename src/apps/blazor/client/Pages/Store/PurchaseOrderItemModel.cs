namespace FSH.Starter.Blazor.Client.Pages.Store;

public class PurchaseOrderItemModel
{
    public DefaultIdType Id { get; set; } = DefaultIdType.NewGuid();
    public DefaultIdType GroceryItemId { get; set; }
    public string GroceryItemName { get; set; } = string.Empty;
    public string GroceryItemSku { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal Total => Quantity * UnitPrice * (1 - DiscountPercentage / 100);
}
