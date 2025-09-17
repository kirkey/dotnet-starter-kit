namespace Store.Domain;

/// <summary>
/// Line item inside a sales order. Holds quantity, unit price, discount and shipped quantities.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track ordered vs shipped quantities.
/// - Compute line totals used in order totals.
/// </remarks>
public sealed class SalesOrderItem : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Parent sales order id.
    /// </summary>
    public DefaultIdType SalesOrderId { get; private set; }

    /// <summary>
    /// Grocery item being ordered.
    /// </summary>
    public DefaultIdType GroceryItemId { get; private set; }

    /// <summary>
    /// Quantity ordered. Must be > 0.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Unit price applied to this line.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Discount amount applied to this line.
    /// </summary>
    public decimal DiscountAmount { get; private set; }

    /// <summary>
    /// Quantity that has already been shipped for this line.
    /// </summary>
    public int ShippedQuantity { get; private set; }
    
    public bool IsWholesaleItem { get; private set; }
    public decimal? WholesaleTierPrice { get; private set; }

    /// <summary>
    /// Computed total price for this line (Quantity * effective price) minus discount.
    /// </summary>
    public decimal TotalPrice { get; private set; }

    public SalesOrder SalesOrder { get; private set; } = default!;
    public GroceryItem GroceryItem { get; private set; } = default!;

    private SalesOrderItem() { }

    private SalesOrderItem(
        DefaultIdType id,
        DefaultIdType salesOrderId,
        DefaultIdType groceryItemId,
        int quantity,
        decimal unitPrice,
        decimal discountAmount,
        bool isWholesaleItem,
        decimal? wholesaleTierPrice)
    {
        Id = id;
        SalesOrderId = salesOrderId;
        GroceryItemId = groceryItemId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountAmount = discountAmount;
        ShippedQuantity = 0;
        IsWholesaleItem = isWholesaleItem;
        WholesaleTierPrice = wholesaleTierPrice;
        
        CalculateTotalPrice();

        QueueDomainEvent(new SalesOrderItemCreated { SalesOrderItem = this });
    }

    public static SalesOrderItem Create(
        DefaultIdType salesOrderId,
        DefaultIdType groceryItemId,
        int quantity,
        decimal unitPrice,
        decimal? discountAmount = null,
        bool isWholesaleItem = false,
        decimal? wholesaleTierPrice = null)
    {
        // domain level validations to avoid invalid state
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        if (unitPrice < 0m)
            throw new ArgumentException("Unit price must be non-negative", nameof(unitPrice));

        if (discountAmount is < 0m)
            throw new ArgumentException("Discount must be non-negative", nameof(discountAmount));

        if (wholesaleTierPrice is < 0m)
            throw new ArgumentException("Wholesale tier price must be non-negative", nameof(wholesaleTierPrice));

        var discount = discountAmount ?? 0m;
        var max = quantity * unitPrice;
        if (discount > max)
            throw new ArgumentException("Discount cannot exceed line total (quantity * unit price)", nameof(discountAmount));

        return new SalesOrderItem(
            DefaultIdType.NewGuid(),
            salesOrderId,
            groceryItemId,
            quantity,
            unitPrice,
            discount,
            isWholesaleItem,
            wholesaleTierPrice);
    }

    public SalesOrderItem UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

        if (Quantity != quantity)
        {
            Quantity = quantity;
            CalculateTotalPrice();
            QueueDomainEvent(new SalesOrderItemQuantityUpdated { SalesOrderItem = this });
        }

        return this;
    }

    public SalesOrderItem ShipQuantity(int shippedQuantity)
    {
        if (shippedQuantity < 0 || shippedQuantity > Quantity)
        {
            throw new ArgumentException("Shipped quantity cannot be negative or exceed ordered quantity.", nameof(shippedQuantity));
        }

        if (ShippedQuantity != shippedQuantity)
        {
            var previousShipped = ShippedQuantity;
            ShippedQuantity = shippedQuantity;
            
            QueueDomainEvent(new SalesOrderItemShipped 
            { 
                SalesOrderItem = this, 
                PreviousShippedQuantity = previousShipped,
                NewShippedQuantity = shippedQuantity
            });
        }

        return this;
    }

    private void CalculateTotalPrice()
    {
        var effectivePrice = IsWholesaleItem && WholesaleTierPrice.HasValue ? WholesaleTierPrice.Value : UnitPrice;
        TotalPrice = (Quantity * effectivePrice) - DiscountAmount;
    }

    public bool IsFullyShipped() => ShippedQuantity >= Quantity;
    public bool IsPartiallyShipped() => ShippedQuantity > 0 && ShippedQuantity < Quantity;
    public int GetPendingShipmentQuantity() => Math.Max(0, Quantity - ShippedQuantity);
    public decimal GetDiscountPercentage() => UnitPrice > 0 ? (DiscountAmount / (Quantity * UnitPrice)) * 100 : 0;
}
