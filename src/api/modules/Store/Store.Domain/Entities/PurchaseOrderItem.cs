using Store.Domain.Exceptions.PurchaseOrderItem;

namespace Store.Domain;

/// <summary>
/// Line item on a purchase order representing a requested quantity from a supplier.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track ordered vs received quantities.
/// - Apply per-line discounts and compute totals.
/// </remarks>
public sealed class PurchaseOrderItem : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Parent purchase order id.
    /// </summary>
    public DefaultIdType PurchaseOrderId { get; private set; }

    /// <summary>
    /// Grocery item id ordered from the supplier.
    /// </summary>
    public DefaultIdType GroceryItemId { get; private set; }

    /// <summary>
    /// Quantity ordered. Must be > 0.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Unit price agreed with the supplier.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Discount amount applied to this line item.
    /// </summary>
    public decimal DiscountAmount { get; private set; }

    /// <summary>
    /// Quantity that has been received so far.
    /// </summary>
    public int ReceivedQuantity { get; private set; }

    /// <summary>
    /// Computed total price for this line (Quantity * UnitPrice) - DiscountAmount.
    /// </summary>
    public decimal TotalPrice { get; private set; }

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
        if (quantity <= 0) throw new InvalidPurchaseOrderItemQuantityException();
        if (unitPrice < 0m) throw new InvalidPurchaseOrderItemCostException();
        if (discountAmount < 0m) throw new InvalidPurchaseOrderItemCostException();
        if (discountAmount > quantity * unitPrice) throw new DiscountExceedsLineTotalException(id);

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

    /// <summary>
    /// Factory method to create a new <see cref="PurchaseOrderItem"/>.
    /// Throws domain exceptions when inputs are invalid.
    /// </summary>
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

    /// <summary>
    /// Updates the ordered quantity for this line item.
    /// Quantity must be positive and cannot be less than already received quantity.
    /// Emits a <see cref="Store.Domain.Events.PurchaseOrderItemQuantityUpdated"/> domain event when changed.
    /// </summary>
    /// <exception cref="InvalidPurchaseOrderItemQuantityException">Thrown when the quantity is not positive.</exception>
    /// <exception cref="CannotReduceQuantityBelowReceivedException">Thrown when attempting to set quantity less than already received.</exception>
    public PurchaseOrderItem UpdateQuantity(int quantity)
    {
        if (quantity <= 0) throw new InvalidPurchaseOrderItemQuantityException();
        if (quantity < ReceivedQuantity)
            throw new CannotReduceQuantityBelowReceivedException(Id);

        if (Quantity != quantity)
        {
            Quantity = quantity;
            // Ensure discount still valid under new quantity
            if (DiscountAmount > Quantity * UnitPrice)
                throw new DiscountExceedsLineTotalException(Id);
            CalculateTotalPrice();
            QueueDomainEvent(new PurchaseOrderItemQuantityUpdated { PurchaseOrderItem = this });
        }

        return this;
    }

    /// <summary>
    /// Updates the unit price and optionally the discount for this line item.
    /// Unit price and discount must be non-negative and discount cannot exceed the line total.
    /// Emits a <see cref="Store.Domain.Events.PurchaseOrderItemPriceUpdated"/> domain event when changed.
    /// </summary>
    public PurchaseOrderItem UpdatePrice(decimal unitPrice, decimal? discountAmount = null)
    {
        if (unitPrice < 0m) throw new InvalidPurchaseOrderItemCostException();
        if (discountAmount is < 0m) throw new InvalidPurchaseOrderItemCostException();

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

        // Validate discount vs current quantity and unit price
        if (DiscountAmount > Quantity * UnitPrice)
            throw new DiscountExceedsLineTotalException(Id);

        if (isUpdated)
        {
            CalculateTotalPrice();
            QueueDomainEvent(new PurchaseOrderItemPriceUpdated { PurchaseOrderItem = this });
        }

        return this;
    }

    /// <summary>
    /// Sets the received quantity for this line item. Received quantity cannot be negative or exceed the ordered quantity.
    /// Emits a <see cref="Store.Domain.Events.PurchaseOrderItemReceived"/> domain event when changed.
    /// </summary>
    /// <exception cref="InvalidPurchaseOrderItemQuantityException">Thrown when received quantity is negative.</exception>
    /// <exception cref="ReceivedQuantityExceedsOrderedException">Thrown when received quantity exceeds ordered quantity.</exception>
    public PurchaseOrderItem ReceiveQuantity(int receivedQuantity)
    {
        if (receivedQuantity < 0) throw new InvalidPurchaseOrderItemQuantityException();
        if (receivedQuantity > Quantity)
        {
            throw new ReceivedQuantityExceedsOrderedException(Id);
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
