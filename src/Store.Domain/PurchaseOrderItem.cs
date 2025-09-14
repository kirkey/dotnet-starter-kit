namespace Store.Domain;

public sealed class PurchaseOrderItem : AuditableEntity, IAggregateRoot
{
    public DefaultIdType PurchaseOrderId { get; private set; }
    public DefaultIdType GroceryItemId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal TotalPrice { get; private set; }
    public int ReceivedQuantity { get; private set; }
    
    public PurchaseOrder PurchaseOrder { get; private set; } = default!;
    public GroceryItem GroceryItem { get; private set; } = default!;

    private PurchaseOrderItem() { }

    private PurchaseOrderItem(
        DefaultIdType id,
        DefaultIdType purchaseOrderId,
        DefaultIdType groceryItemId,
        int quantity,
        decimal unitPrice,
        decimal discountAmount,
        string? notes)
    {
        // validations
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        if (unitPrice < 0m) throw new ArgumentException("UnitPrice must be zero or greater", nameof(unitPrice));
        if (discountAmount < 0m) throw new ArgumentException("DiscountAmount must be zero or greater", nameof(discountAmount));
        if (discountAmount > quantity * unitPrice) throw new ArgumentException("Discount cannot exceed line total", nameof(discountAmount));

        Id = id;
        PurchaseOrderId = purchaseOrderId;
        GroceryItemId = groceryItemId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        DiscountAmount = discountAmount;
        ReceivedQuantity = 0;
        Notes = notes;
        
        CalculateTotalPrice();

        QueueDomainEvent(new PurchaseOrderItemCreated { PurchaseOrderItem = this });
    }

    public static PurchaseOrderItem Create(
        DefaultIdType purchaseOrderId,
        DefaultIdType groceryItemId,
        int quantity,
        decimal unitPrice,
        decimal? discountAmount = null,
        string? notes = null)
    {
        return new PurchaseOrderItem(
            DefaultIdType.NewGuid(),
            purchaseOrderId,
            groceryItemId,
            quantity,
            unitPrice,
            discountAmount ?? 0,
            notes);
    }

    public PurchaseOrderItem UpdateQuantity(int quantity)
    {
        if (Quantity != quantity)
        {
            Quantity = quantity;
            CalculateTotalPrice();
            QueueDomainEvent(new PurchaseOrderItemQuantityUpdated { PurchaseOrderItem = this });
        }

        return this;
    }

    public PurchaseOrderItem UpdatePrice(decimal unitPrice, decimal? discountAmount = null)
    {
        bool isUpdated = false;

        if (UnitPrice != unitPrice)
        {
            UnitPrice = unitPrice;
            isUpdated = true;
        }

        if (discountAmount.HasValue && DiscountAmount != discountAmount.Value)
        {
            DiscountAmount = discountAmount.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            CalculateTotalPrice();
            QueueDomainEvent(new PurchaseOrderItemPriceUpdated { PurchaseOrderItem = this });
        }

        return this;
    }

    public PurchaseOrderItem ReceiveQuantity(int receivedQuantity)
    {
        if (receivedQuantity < 0 || receivedQuantity > Quantity)
        {
            throw new ArgumentException("Received quantity cannot be negative or exceed ordered quantity.", nameof(receivedQuantity));
        }

        if (ReceivedQuantity != receivedQuantity)
        {
            var previousReceived = ReceivedQuantity;
            ReceivedQuantity = receivedQuantity;
            
            QueueDomainEvent(new PurchaseOrderItemReceived 
            { 
                PurchaseOrderItem = this, 
                PreviousReceivedQuantity = previousReceived,
                NewReceivedQuantity = receivedQuantity
            });
        }

        return this;
    }

    private void CalculateTotalPrice()
    {
        TotalPrice = (Quantity * UnitPrice) - DiscountAmount;
    }

    public bool IsFullyReceived() => ReceivedQuantity >= Quantity;
    public bool IsPartiallyReceived() => ReceivedQuantity > 0 && ReceivedQuantity < Quantity;
    public bool IsNotReceived() => ReceivedQuantity == 0;
    public int GetPendingQuantity() => Math.Max(0, Quantity - ReceivedQuantity);
    public decimal GetDiscountPercentage() => UnitPrice > 0 ? (DiscountAmount / (Quantity * UnitPrice)) * 100 : 0;
}
