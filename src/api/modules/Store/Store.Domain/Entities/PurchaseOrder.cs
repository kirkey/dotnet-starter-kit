using Store.Domain.Exceptions.PurchaseOrder;
using Store.Domain.Exceptions.PurchaseOrderItem;

namespace Store.Domain.Entities;

/// <summary>
/// Represents a purchase order sent to a supplier to procure inventory with comprehensive order lifecycle management.
/// </summary>
/// <remarks>
/// Use cases:
/// - Create purchase orders to suppliers for inventory replenishment and new product procurement.
/// - Track order status from draft through delivered with approval workflows.
/// - Manage expected vs actual delivery dates for supply chain planning.
/// - Calculate order totals including taxes, discounts, and shipping costs.
/// - Support partial deliveries and back-order management for incomplete shipments.
/// - Enable three-way matching (PO, receipt, invoice) for accounts payable processing.
/// - Track supplier performance metrics including on-time delivery and quality.
/// - Support drop-shipping and direct-to-customer fulfillment scenarios.
/// 
/// Default values:
/// - OrderNumber: required unique identifier (example: "PO-2025-09-001")
/// - SupplierId: required supplier reference for procurement
/// - OrderDate: required order placement date (example: 2025-09-19)
/// - ExpectedDeliveryDate: optional expected delivery (example: 2025-09-26)
/// - Status: "Draft" (new orders start as draft until submitted)
/// - TotalAmount: 0.00 (calculated from order items)
/// - TaxAmount: 0.00 (calculated based on tax rates)
/// - DiscountAmount: 0.00 (applied discounts and rebates)
/// - ShippingCost: 0.00 (transportation and handling charges)
/// - DeliveredDate: null (set when order is received)
/// - IsFullyDelivered: false (true when all items received)
/// 
/// Business rules:
/// - OrderNumber must be unique within the system
/// - Cannot modify submitted orders without proper authorization
/// - Expected delivery date should be after order date
/// - Total amount must equal sum of line items plus tax and shipping
/// - Cannot delete orders with associated receipts or invoices
/// - Partial deliveries update item received quantities
/// - Supplier must be active to create new orders
/// - Order approval required above specified dollar thresholds
/// </remarks>
/// <seealso cref="Store.Domain.Events.PurchaseOrderCreated"/>
/// <seealso cref="Store.Domain.Events.PurchaseOrderUpdated"/>
/// <seealso cref="Store.Domain.Events.PurchaseOrderSubmitted"/>
/// <seealso cref="Store.Domain.Events.PurchaseOrderApproved"/>
/// <seealso cref="Store.Domain.Events.PurchaseOrderDelivered"/>
/// <seealso cref="Store.Domain.Events.PurchaseOrderCancelled"/>
/// <seealso cref="Store.Domain.Exceptions.PurchaseOrder.PurchaseOrderNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.PurchaseOrder.PurchaseOrderCannotBeModifiedException"/>
/// <seealso cref="Store.Domain.Exceptions.PurchaseOrder.InvalidPurchaseOrderStatusException"/>
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
    /// Order status: Draft, Submitted, Approved, Sent, Received, Cancelled.
    /// </summary>
    public string Status { get; private set; } = default!;

    /// <summary>
    /// Computed totals: TotalAmount includes line totals, NetAmount accounts for taxes/discounts.
    /// </summary>
    public decimal TotalAmount { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal ShippingCost { get; private set; }
    public decimal NetAmount { get; private set; }
    public string? DeliveryAddress { get; private set; }
    public string? ContactPerson { get; private set; }
    public string? ContactPhone { get; private set; }
    public bool IsUrgent { get; private set; }
    
    public Supplier Supplier { get; private set; } = default!;
    
    private readonly List<PurchaseOrderItem> _items = new();
    /// <summary>
    /// Collection of purchase order items, each representing a requested quantity from the supplier.
    /// Read-only to enforce proper aggregate management.
    /// </summary>
    public IReadOnlyCollection<PurchaseOrderItem> Items => _items.AsReadOnly();

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

        if (expectedDeliveryDate.HasValue && expectedDeliveryDate.Value.Date < orderDate.Date)
            throw new ArgumentException("Expected delivery date must be on or after the order date", nameof(expectedDeliveryDate));

        if (!PurchaseOrderStatus.IsAllowed(status))
            throw new InvalidPurchaseOrderStatusException(status);

        if (deliveryAddress is { Length: > 500 }) throw new ArgumentException("DeliveryAddress must not exceed 500 characters", nameof(deliveryAddress));
        if (contactPerson is { Length: > 100 }) throw new ArgumentException("ContactPerson must not exceed 100 characters", nameof(contactPerson));
        if (contactPhone is { Length: > 50 }) throw new ArgumentException("ContactPhone must not exceed 50 characters", nameof(contactPhone));

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
        string status = PurchaseOrderStatus.Draft,
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

    private void EnsureModifiable()
    {
        if (!PurchaseOrderStatus.IsModifiable(Status))
            throw new PurchaseOrderCannotBeModifiedException(Id, Status);
    }

    public PurchaseOrder UpdateStatus(string status)
    {
        if (!PurchaseOrderStatus.IsAllowed(status))
            throw new InvalidPurchaseOrderStatusException(status);

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

            // Emit explicit lifecycle events for certain statuses
            if (string.Equals(status, PurchaseOrderStatus.Submitted, StringComparison.OrdinalIgnoreCase))
                QueueDomainEvent(new PurchaseOrderSubmitted { PurchaseOrder = this });
            else if (string.Equals(status, PurchaseOrderStatus.Approved, StringComparison.OrdinalIgnoreCase))
                QueueDomainEvent(new PurchaseOrderApproved { PurchaseOrder = this });
            else if (string.Equals(status, PurchaseOrderStatus.Cancelled, StringComparison.OrdinalIgnoreCase))
                QueueDomainEvent(new PurchaseOrderCancelled { PurchaseOrder = this });
            else if (string.Equals(status, PurchaseOrderStatus.Sent, StringComparison.OrdinalIgnoreCase))
                QueueDomainEvent(new PurchaseOrderSentEvent { PurchaseOrder = this });
        }

        return this;
    }

    public PurchaseOrder AddItem(DefaultIdType itemId, int quantity, decimal unitPrice, decimal? discount = null)
    {
        EnsureModifiable();

        var existingItem = Items.FirstOrDefault(i => i.ItemId == itemId);
        
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            var newItem = PurchaseOrderItem.Create(Id, itemId, quantity, unitPrice, discount);
            _items.Add(newItem);
        }

        RecalculateTotals();
        QueueDomainEvent(new PurchaseOrderItemAdded { PurchaseOrder = this, ItemId = itemId });
        
        return this;
    }

    public PurchaseOrder RemoveItem(DefaultIdType itemId)
    {
        EnsureModifiable();

        var item = Items.FirstOrDefault(i => i.ItemId == itemId);
        if (item != null)
        {
            if (item.ReceivedQuantity > 0)
                throw new CannotRemoveReceivedPurchaseOrderItemException(item.Id);

            _items.Remove(item);
            RecalculateTotals();
            QueueDomainEvent(new PurchaseOrderItemRemoved { PurchaseOrder = this, ItemId = itemId });
        }

        return this;
    }

    // Update a child item's quantity and recalculate totals
    public PurchaseOrder UpdateItemQuantity(DefaultIdType purchaseOrderItemId, int quantity)
    {
        EnsureModifiable();

        var item = Items.FirstOrDefault(i => i.Id == purchaseOrderItemId);
        if (item is null)
            throw new PurchaseOrderItemNotFoundException(purchaseOrderItemId);

        item.UpdateQuantity(quantity);
        RecalculateTotals();
        QueueDomainEvent(new PurchaseOrderUpdated { PurchaseOrder = this });
        return this;
    }

    // Update a child item's price and optionally discount, then recalc totals
    public PurchaseOrder UpdateItemPrice(DefaultIdType purchaseOrderItemId, decimal unitPrice, decimal? discountAmount = null)
    {
        EnsureModifiable();

        var item = Items.FirstOrDefault(i => i.Id == purchaseOrderItemId);
        if (item is null)
            throw new PurchaseOrderItemNotFoundException(purchaseOrderItemId);

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
            throw new PurchaseOrderItemNotFoundException(purchaseOrderItemId);

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
                // When delivery date is set, consider the order received
                var previousStatus = Status;
                Status = PurchaseOrderStatus.Received;
                QueueDomainEvent(new PurchaseOrderDelivered { PurchaseOrder = this });
                QueueDomainEvent(new PurchaseOrderStatusChanged { PurchaseOrder = this, PreviousStatus = previousStatus, NewStatus = PurchaseOrderStatus.Received });
            }
        }

        return this;
    }

    public PurchaseOrder ApplyDiscount(decimal discountAmount)
    {
        EnsureModifiable();

        if (discountAmount < 0m) throw new ArgumentException("Discount must be zero or greater", nameof(discountAmount));
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
        EnsureModifiable();

        // validate inputs similar to constructor
        if (string.IsNullOrWhiteSpace(orderNumber)) throw new ArgumentException("Order number is required", nameof(orderNumber));
        if (orderNumber.Length > 100) throw new ArgumentException("Order number must not exceed 100 characters", nameof(orderNumber));
        if (supplierId == default) throw new ArgumentException("SupplierId is required", nameof(supplierId));
        if (orderDate == default) throw new ArgumentException("OrderDate is required", nameof(orderDate));
        if (expectedDeliveryDate.HasValue && expectedDeliveryDate.Value.Date < orderDate.Date)
            throw new ArgumentException("Expected delivery date must be on or after the order date", nameof(expectedDeliveryDate));
        if (!PurchaseOrderStatus.IsAllowed(status))
            throw new InvalidPurchaseOrderStatusException(status);
        if (deliveryAddress is { Length: > 500 }) throw new ArgumentException("DeliveryAddress must not exceed 500 characters", nameof(deliveryAddress));
        if (contactPerson is { Length: > 100 }) throw new ArgumentException("ContactPerson must not exceed 100 characters", nameof(contactPerson));
        if (contactPhone is { Length: > 50 }) throw new ArgumentException("ContactPhone must not exceed 50 characters", nameof(contactPhone));

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

        if (!string.Equals(Status, status, StringComparison.OrdinalIgnoreCase))
        {
            var previousStatus = Status;
            Status = status;
            changed = true;
            QueueDomainEvent(new PurchaseOrderStatusChanged { PurchaseOrder = this, PreviousStatus = previousStatus, NewStatus = status });
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
        NetAmount = TotalAmount + TaxAmount + ShippingCost - DiscountAmount;
    }

    /// <summary>
    /// Update the aggregate totals after items have changed. Intended to be called by application handlers
    /// managing <see cref="PurchaseOrderItem"/> entities independently.
    /// </summary>
    /// <param name="totalAmount">New total of all TotalPrice values for items belonging to this purchase order.</param>
    public PurchaseOrder UpdateTotals(decimal totalAmount)
    {
        if (totalAmount < 0)
            throw new ArgumentException("Total amount cannot be negative", nameof(totalAmount));

        TotalAmount = totalAmount;
        NetAmount = TotalAmount + TaxAmount + ShippingCost - DiscountAmount;
        QueueDomainEvent(new PurchaseOrderUpdated { PurchaseOrder = this });
        return this;
    }

    /// <summary>
    /// Updates the tax amount for the purchase order and recalculates net amount.
    /// </summary>
    public PurchaseOrder UpdateTaxAmount(decimal taxAmount)
    {
        if (taxAmount < 0m) throw new ArgumentException("Tax amount must be zero or greater", nameof(taxAmount));
        
        if (TaxAmount != taxAmount)
        {
            TaxAmount = taxAmount;
            RecalculateTotals();
            QueueDomainEvent(new PurchaseOrderUpdated { PurchaseOrder = this });
        }

        return this;
    }

    /// <summary>
    /// Updates the shipping cost for the purchase order and recalculates net amount.
    /// </summary>
    public PurchaseOrder UpdateShippingCost(decimal shippingCost)
    {
        if (shippingCost < 0m) throw new ArgumentException("Shipping cost must be zero or greater", nameof(shippingCost));
        
        if (ShippingCost != shippingCost)
        {
            ShippingCost = shippingCost;
            RecalculateTotals();
            QueueDomainEvent(new PurchaseOrderUpdated { PurchaseOrder = this });
        }

        QueueDomainEvent(new PurchaseOrderUpdated { PurchaseOrder = this });
        return this;
    }

    public bool IsOverdue() => 
        ExpectedDeliveryDate is { } ed &&
        ed < DateTime.UtcNow &&
        !ActualDeliveryDate.HasValue &&
        !string.Equals(Status, PurchaseOrderStatus.Cancelled, StringComparison.OrdinalIgnoreCase);

    public bool IsDelivered() => ActualDeliveryDate.HasValue;

    public int GetDaysUntilExpectedDelivery() =>
        ExpectedDeliveryDate is { } ed
            ? (ed - DateTime.UtcNow).Days
            : 0;
}
