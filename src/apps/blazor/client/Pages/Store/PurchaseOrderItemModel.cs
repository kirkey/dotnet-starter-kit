namespace FSH.Starter.Blazor.Client.Pages.Store;

public class PurchaseOrderItemModel
{
    public DefaultIdType Id { get; set; } = DefaultIdType.NewGuid();
    public DefaultIdType PurchaseOrderId { get; set; }
    public DefaultIdType ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ItemSku { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal Total => Quantity * UnitPrice * (1 - DiscountPercentage / 100);
}
