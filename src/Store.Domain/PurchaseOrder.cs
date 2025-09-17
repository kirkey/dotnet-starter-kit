namespace Store.Domain;

/// <summary>
/// Represents a purchase order sent to a supplier to procure stock.
/// Tracks items, delivery dates, and totals.
/// </summary>
/// <remarks>
/// Use cases:
/// - Create orders to suppliers and track receipt of goods.
/// - Compute totals, taxes, discounts, and delivery status.
/// </remarks>
public sealed class PurchaseOrder : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Purchase order number. Example: "PO-2025-001".
    /// </summary>
    public string OrderNumber { get; private set; } = default!;

    /// <summary>
    /// Supplier id the order is sent to.
    /// </summary>
    public DefaultIdType SupplierId { get; private set; }

    /// <summary>
    /// Date the order was placed.
    /// </summary>
    public DateTime OrderDate { get; private set; }

    /// <summary>
    /// Expected delivery date (optional).
    /// </summary>
    public DateTime? ExpectedDeliveryDate { get; private set; }

    /// <summary>
    /// Actual delivery date when goods were received (optional).
    /// Set by UpdateDeliveryDate when a delivery is recorded.
    /// </summary>
    public DateTime? ActualDeliveryDate { get; private set; }

    /// <summary>
    /// Order status: Draft, Sent, Confirmed, Received, Cancelled.
    /// </summary>
    public string Status { get; private set; } = default!;

    /// <summary>
    /// Computed totals: TotalAmount includes line totals, NetAmount accounts for taxes/discounts.
    /// </summary>
    public decimal TotalAmount { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal NetAmount { get; private set; }
    public string? DeliveryAddress { get; private set; }
    public string? ContactPerson { get; private set; }
    public string? ContactPhone { get; private set; }
    public bool IsUrgent { get; private set; }
    
    public Supplier Supplier { get; private set; } = default!;
    public ICollection<PurchaseOrderItem> Items { get; private set; } = new List<PurchaseOrderItem>();

    // The parameterless constructor is required by EF Core for entity materialization.
    // It may appear unused to static analyzers but must remain.
    private PurchaseOrder() { }

    private PurchaseOrder(
        DefaultIdType id,
        string orderNumber,
        DefaultIdType supplierId,
        DateTime orderDate,
        DateTime? expectedDeliveryDate,
        string status,
        string? notes,
        string? deliveryAddress,
        string? contactPerson,
        string? contactPhone,
        bool isUrgent)
    {
        // domain validations
        if (string.IsNullOrWhiteSpace(orderNumber)) throw new ArgumentException("Order number is required", nameof(orderNumber));
        if (orderNumber.Length > 100) throw new ArgumentException("Order number must not exceed 100 characters", nameof(orderNumber));

        if (supplierId == default) throw new ArgumentException("SupplierId is required", nameof(supplierId));

        if (orderDate == default) throw new ArgumentException("OrderDate is required", nameof(orderDate));

        if (status is { Length: > 50 }) throw new ArgumentException("Status must not exceed 50 characters", nameof(status));

        if (deliveryAddress is { Length: > 500 }) throw new ArgumentException("DeliveryAddress must not exceed 500 characters", nameof(deliveryAddress));

        Id = id;
        OrderNumber = orderNumber;
        SupplierId = supplierId;
        OrderDate = orderDate;
        ExpectedDeliveryDate = expectedDeliveryDate;
        Status = status;
        Notes = notes;
        DeliveryAddress = deliveryAddress;
        ContactPerson = contactPerson;
        ContactPhone = contactPhone;
        IsUrgent = isUrgent;
        TotalAmount = 0;
        TaxAmount = 0;
        DiscountAmount = 0;
        NetAmount = 0;

        QueueDomainEvent(new PurchaseOrderCreated { PurchaseOrder = this });
    }

    public static PurchaseOrder Create(
        string orderNumber,
        DefaultIdType supplierId,
        DateTime orderDate,
        DateTime? expectedDeliveryDate = null,
        string status = "Draft",
        string? notes = null,
        string? deliveryAddress = null,
        string? contactPerson = null,
        string? contactPhone = null,
        bool isUrgent = false)
    {
        return new PurchaseOrder(
            DefaultIdType.NewGuid(),
            orderNumber,
            supplierId,
            orderDate,
            expectedDeliveryDate,
            status,
            notes,
            deliveryAddress,
            contactPerson,
            contactPhone,
            isUrgent);
    }

    public PurchaseOrder UpdateStatus(string status)
    {
        if (!string.Equals(Status, status, StringComparison.OrdinalIgnoreCase))
        {
            var previousStatus = Status;
            Status = status;
            
            QueueDomainEvent(new PurchaseOrderStatusChanged 
            { 
                PurchaseOrder = this, 
                PreviousStatus = previousStatus, 
                NewStatus = status 
            });
        }

        return this;
    }

    public PurchaseOrder AddItem(DefaultIdType groceryItemId, int quantity, decimal unitPrice, decimal? discount = null)
    {
        var existingItem = Items.FirstOrDefault(i => i.GroceryItemId == groceryItemId);
        
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var newItem = PurchaseOrderItem.Create(Id, groceryItemId, quantity, unitPrice, discount);
            Items.Add(newItem);
        }

        RecalculateTotals();
        QueueDomainEvent(new PurchaseOrderItemAdded { PurchaseOrder = this, GroceryItemId = groceryItemId });
        
        return this;
    }

    public PurchaseOrder RemoveItem(DefaultIdType groceryItemId)
    {
        var item = Items.FirstOrDefault(i => i.GroceryItemId == groceryItemId);
        if (item != null)
        {
            Items.Remove(item);
            RecalculateTotals();
            QueueDomainEvent(new PurchaseOrderItemRemoved { PurchaseOrder = this, GroceryItemId = groceryItemId });
        }

        return this;
    }

    // Update a child item's quantity and recalculate totals
    public PurchaseOrder UpdateItemQuantity(DefaultIdType purchaseOrderItemId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.Id == purchaseOrderItemId);
        if (item is null)
            throw new Store.Domain.Exceptions.PurchaseOrder.PurchaseOrderItemNotFoundException(purchaseOrderItemId);

        item.UpdateQuantity(quantity);
        RecalculateTotals();
        QueueDomainEvent(new PurchaseOrderUpdated { PurchaseOrder = this });
        return this;
    }

    // Update a child item's price and optionally discount, then recalc totals
    public PurchaseOrder UpdateItemPrice(DefaultIdType purchaseOrderItemId, decimal unitPrice, decimal? discountAmount = null)
    {
        var item = Items.FirstOrDefault(i => i.Id == purchaseOrderItemId);
        if (item is null)
            throw new Store.Domain.Exceptions.PurchaseOrder.PurchaseOrderItemNotFoundException(purchaseOrderItemId);

        item.UpdatePrice(unitPrice, discountAmount);
        RecalculateTotals();
        QueueDomainEvent(new PurchaseOrderUpdated { PurchaseOrder = this });
        return this;
    }

    // Receive quantity for a child item and recalc totals
    public PurchaseOrder ReceiveItemQuantity(DefaultIdType purchaseOrderItemId, int receivedQuantity)
    {
        var item = Items.FirstOrDefault(i => i.Id == purchaseOrderItemId);
        if (item is null)
            throw new Store.Domain.Exceptions.PurchaseOrder.PurchaseOrderItemNotFoundException(purchaseOrderItemId);

        item.ReceiveQuantity(receivedQuantity);
        RecalculateTotals();
        QueueDomainEvent(new PurchaseOrderUpdated { PurchaseOrder = this });
        return this;
    }

    public PurchaseOrder UpdateDeliveryDate(DateTime? actualDeliveryDate)
    {
        if (ActualDeliveryDate != actualDeliveryDate)
        {
            ActualDeliveryDate = actualDeliveryDate;
            
            if (actualDeliveryDate.HasValue)
            {
                QueueDomainEvent(new PurchaseOrderDelivered { PurchaseOrder = this });
            }
        }

        return this;
    }

    public PurchaseOrder ApplyDiscount(decimal discountAmount)
    {
        if (DiscountAmount != discountAmount)
        {
            DiscountAmount = discountAmount;
            RecalculateTotals();
            QueueDomainEvent(new PurchaseOrderDiscountApplied { PurchaseOrder = this, DiscountAmount = discountAmount });
        }

        return this;
    }

    // Update method to apply editable fields and emit PurchaseOrderUpdated when there are changes
    public PurchaseOrder Update(
        string orderNumber,
        DefaultIdType supplierId,
        DateTime orderDate,
        DateTime? expectedDeliveryDate,
        string status,
        string? notes,
        string? deliveryAddress,
        string? contactPerson,
        string? contactPhone,
        bool isUrgent)
    {
        var changed = false;

        if (OrderNumber != orderNumber)
        {
            OrderNumber = orderNumber;
            changed = true;
        }

        if (SupplierId != supplierId)
        {
            SupplierId = supplierId;
            changed = true;
        }

        if (OrderDate != orderDate)
        {
            OrderDate = orderDate;
            changed = true;
        }

        if (ExpectedDeliveryDate != expectedDeliveryDate)
        {
            ExpectedDeliveryDate = expectedDeliveryDate;
            changed = true;
        }

        if (Status != status)
        {
            Status = status;
            changed = true;
        }

        if (Notes != notes)
        {
            Notes = notes;
            changed = true;
        }

        if (DeliveryAddress != deliveryAddress)
        {
            DeliveryAddress = deliveryAddress;
            changed = true;
        }

        if (ContactPerson != contactPerson)
        {
            ContactPerson = contactPerson;
            changed = true;
        }

        if (ContactPhone != contactPhone)
        {
            ContactPhone = contactPhone;
            changed = true;
        }

        if (IsUrgent != isUrgent)
        {
            IsUrgent = isUrgent;
            changed = true;
        }

        if (changed)
        {
            QueueDomainEvent(new PurchaseOrderUpdated { PurchaseOrder = this });
        }

        return this;
    }

    private void RecalculateTotals()
    {
        TotalAmount = Items.Sum(i => i.TotalPrice);
        NetAmount = TotalAmount + TaxAmount - DiscountAmount;
    }

    public bool IsOverdue() => 
        ExpectedDeliveryDate is { } ed &&
        ed < DateTime.UtcNow &&
        !ActualDeliveryDate.HasValue &&
        Status != "Cancelled";

    public bool IsDelivered() => ActualDeliveryDate.HasValue;

    public int GetDaysUntilExpectedDelivery() =>
        ExpectedDeliveryDate is { } ed
            ? (ed - DateTime.UtcNow).Days
            : 0;
}
