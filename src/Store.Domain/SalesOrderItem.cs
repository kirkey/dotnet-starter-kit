using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

namespace Store.Domain;

public sealed class SalesOrderItem : AuditableEntity, IAggregateRoot
{
    public DefaultIdType SalesOrderId { get; private set; }
    public DefaultIdType GroceryItemId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal TotalPrice { get; private set; }
    public int ShippedQuantity { get; private set; }
    
    public bool IsWholesaleItem { get; private set; }
    public decimal? WholesaleTierPrice { get; private set; }
    
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
        decimal? wholesaleTierPrice,
        string? notes)
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
        Notes = notes;
        
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
        decimal? wholesaleTierPrice = null,
        string? notes = null)
    {
        return new SalesOrderItem(
            DefaultIdType.NewGuid(),
            salesOrderId,
            groceryItemId,
            quantity,
            unitPrice,
            discountAmount ?? 0,
            isWholesaleItem,
            wholesaleTierPrice,
            notes);
    }

    public SalesOrderItem UpdateQuantity(int quantity)
    {
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
