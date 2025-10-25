namespace FSH.Starter.Blazor.Client.Pages.Store;

/// <summary>
/// Model for managing purchase order items in the dialog form.
/// Used for both creating and editing purchase order items.
/// </summary>
public class PurchaseOrderItemModel
{
    /// <summary>
    /// Gets or sets the purchase order item identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the grocery item identifier.
    /// </summary>
    public DefaultIdType ItemId { get; set; }

    /// <summary>
    /// Gets or sets the quantity ordered.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price for the item.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount amount applied to this item.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Gets or sets the quantity received (for tracking deliveries).
    /// </summary>
    public int ReceivedQuantity { get; set; }

    /// <summary>
    /// Gets or sets any notes or comments about this item.
    /// </summary>
    public string? Notes { get; set; }
}

