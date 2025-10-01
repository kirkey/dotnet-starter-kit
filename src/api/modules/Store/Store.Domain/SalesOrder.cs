namespace Store.Domain;

/// <summary>
/// Represents a sales order placed by a customer with comprehensive order lifecycle and fulfillment management.
/// </summary>
/// <remarks>
/// Use cases:
/// - Process customer orders from retail, wholesale, and e-commerce channels.
/// - Track order fulfillment from creation through delivery with status management.
/// - Calculate order totals including taxes, discounts, shipping, and promotional pricing.
/// - Support partial shipments and back-order management for out-of-stock items.
/// - Enable order modification and cancellation with proper business rules.
/// - Track payment status and integrate with payment processing systems.
/// - Support multiple delivery options including pickup, delivery, and drop-shipping.
/// - Generate pick lists and shipping labels for warehouse operations.
/// 
/// Default values:
/// - OrderNumber: required unique identifier (example: "SO-2025-09-001")
/// - CustomerId: required customer reference for order attribution
/// - OrderDate: required order creation date (example: 2025-09-19)
/// - DeliveryAddress: optional delivery location (defaults to customer address)
/// - Status: "Draft" (new orders start as draft until confirmed)
/// - PaymentStatus: "Pending" (awaiting payment processing)
/// - FulfillmentStatus: "Pending" (awaiting warehouse processing)
/// - TotalAmount: 0.00 (calculated from order items)
/// - TaxAmount: 0.00 (calculated based on tax rates and location)
/// - DiscountAmount: 0.00 (applied promotions and customer discounts)
/// - ShippingCost: 0.00 (delivery charges based on method and distance)
/// - RequestedDeliveryDate: null (optional customer preference)
/// - ShippedDate: null (set when order ships)
/// - DeliveredDate: null (set when delivery is confirmed)
/// 
/// Business rules:
/// - OrderNumber must be unique within the system
/// - Cannot modify confirmed orders without proper authorization
/// - Requested delivery date must be in the future
/// - Total amount must equal sum of line items plus tax and shipping minus discounts
/// - Cannot delete orders with payment transactions or shipped items
/// - Order confirmation requires inventory availability check
/// - Customer must be active to place new orders
/// - Payment authorization required before order fulfillment
/// - Cancellation refund processing follows business policies
/// </remarks>
/// <seealso cref="Store.Domain.Events.SalesOrderCreated"/>
/// <seealso cref="Store.Domain.Events.SalesOrderUpdated"/>
/// <seealso cref="Store.Domain.Events.SalesOrderConfirmed"/>
/// <seealso cref="Store.Domain.Events.SalesOrderShipped"/>
/// <seealso cref="Store.Domain.Events.SalesOrderDelivered"/>
/// <seealso cref="Store.Domain.Events.SalesOrderCancelled"/>
/// <seealso cref="Store.Domain.Events.SalesOrderPaymentReceived"/>
/// <seealso cref="Store.Domain.Exceptions.SalesOrder.SalesOrderNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.SalesOrder.SalesOrderCannotBeModifiedException"/>
/// <seealso cref="Store.Domain.Exceptions.SalesOrder.InvalidSalesOrderStatusException"/>
public sealed class SalesOrder : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Public order identifier. Example: "SO-2025-001".
    /// </summary>
    public string OrderNumber { get; private set; } = default!;

    /// <summary>
    /// Customer who placed the order.
    /// </summary>
    public DefaultIdType CustomerId { get; private set; }

    /// <summary>
    /// Order creation date.
    /// </summary>
    public DateTime OrderDate { get; private set; }

    /// <summary>
    /// Delivery address if different from customer address (optional).
    /// </summary>
    public string? DeliveryAddress { get; private set; }

    /// <summary>
    /// Delivery date (optional) for scheduled shipments.
    /// </summary>
    public DateTime? DeliveryDate { get; private set; }

    /// <summary>
    /// Optional order notes set by sales or customer service.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Order, payment and fulfillment related small enums/strings.
    /// </summary>
    public string Status { get; private set; } = default!;
    public string OrderType { get; private set; } = default!;
    public string PaymentStatus { get; private set; } = default!;
    public string PaymentMethod { get; private set; } = default!;

    /// <summary>
    /// Totals and amounts.
    /// </summary>
    public decimal SubTotal { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal ShippingAmount { get; private set; }
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Flags and references.
    /// </summary>
    public bool IsUrgent { get; private set; }
    public string? SalesPersonId { get; private set; }
    public DefaultIdType? WarehouseId { get; private set; }

    /// <summary>
    /// Line items contained in the order.
    /// </summary>
    public ICollection<SalesOrderItem> Items { get; private set; } = new List<SalesOrderItem>();

    // The parameterless constructor and private setters are required by EF Core for materialization.
    // They may look unused in code, but they are used by the ORM at runtime.
    private SalesOrder() { }

    private SalesOrder(
        DefaultIdType id,
        string orderNumber,
        DefaultIdType customerId,
        DateTime orderDate,
        string status,
        string orderType,
        string paymentStatus,
        string paymentMethod,
        string? deliveryAddress,
        string? notes,
        bool isUrgent,
        string? salesPersonId,
        DefaultIdType? warehouseId)
    {
        // domain validations
        if (string.IsNullOrWhiteSpace(orderNumber)) throw new ArgumentException("OrderNumber is required", nameof(orderNumber));
        if (orderNumber.Length > 100) throw new ArgumentException("OrderNumber must not exceed 100 characters", nameof(orderNumber));

        if (customerId == default) throw new ArgumentException("CustomerId is required", nameof(customerId));

        if (orderDate == default) throw new ArgumentException("OrderDate is required", nameof(orderDate));

        if (status is { Length: > 50 }) throw new ArgumentException("Status must not exceed 50 characters", nameof(status));
        if (orderType is { Length: > 50 }) throw new ArgumentException("OrderType must not exceed 50 characters", nameof(orderType));
        if (paymentStatus is { Length: > 50 }) throw new ArgumentException("PaymentStatus must not exceed 50 characters", nameof(paymentStatus));
        if (paymentMethod is { Length: > 50 }) throw new ArgumentException("PaymentMethod must not exceed 50 characters", nameof(paymentMethod));
        if (deliveryAddress is { Length: > 500 }) throw new ArgumentException("DeliveryAddress must not exceed 500 characters", nameof(deliveryAddress));
        if (salesPersonId is { Length: > 100 }) throw new ArgumentException("SalesPersonId must not exceed 100 characters", nameof(salesPersonId));

        Id = id;
        OrderNumber = orderNumber;
        CustomerId = customerId;
        OrderDate = orderDate;
        Status = status;
        OrderType = orderType;
        PaymentStatus = paymentStatus;
        PaymentMethod = paymentMethod;
        DeliveryAddress = deliveryAddress;
        Notes = notes;
        IsUrgent = isUrgent;
        SalesPersonId = salesPersonId;
        WarehouseId = warehouseId;
        SubTotal = 0;
        TaxAmount = 0;
        DiscountAmount = 0;
        ShippingAmount = 0;
        TotalAmount = 0;

        QueueDomainEvent(new SalesOrderCreated { SalesOrder = this });
    }

    public static SalesOrder Create(
        string orderNumber,
        DefaultIdType customerId,
        DateTime orderDate,
        string status = "Draft",
        string orderType = "Retail",
        string paymentStatus = "Pending",
        string paymentMethod = "Cash",
        string? deliveryAddress = null,
        string? notes = null,
        bool isUrgent = false,
        string? salesPersonId = null,
        DefaultIdType? warehouseId = null)
    {
        return new SalesOrder(
            DefaultIdType.NewGuid(),
            orderNumber,
            customerId,
            orderDate,
            status,
            orderType,
            paymentStatus,
            paymentMethod,
            deliveryAddress,
            notes,
            isUrgent,
            salesPersonId,
            warehouseId);
    }

    public SalesOrder AddItem(DefaultIdType groceryItemId, int quantity, decimal unitPrice, decimal? discount = null)
    {
        var existingItem = Items.FirstOrDefault(i => i.GroceryItemId == groceryItemId);
        
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
            // queue quantity-updated event for the existing item
            QueueDomainEvent(new SalesOrderItemQuantityUpdated { SalesOrderItem = existingItem });
        }
        else
        {
            var newItem = SalesOrderItem.Create(Id, groceryItemId, quantity, unitPrice, discount);
            Items.Add(newItem);
            // queue created event for the new item
            QueueDomainEvent(new SalesOrderItemCreated { SalesOrderItem = newItem });
        }

        RecalculateTotals();
        // keep a SalesOrderUpdated event as well to indicate order level change
        QueueDomainEvent(new SalesOrderUpdated { SalesOrder = this });
        
        return this;
    }

    // Remove a line item by grocery item id
    public SalesOrder RemoveItem(DefaultIdType groceryItemId)
    {
        var item = Items.FirstOrDefault(i => i.GroceryItemId == groceryItemId);
        if (item != null)
        {
            Items.Remove(item);
            RecalculateTotals();
            QueueDomainEvent(new SalesOrderItemRemoved { SalesOrder = this, GroceryItemId = groceryItemId });
            QueueDomainEvent(new SalesOrderUpdated { SalesOrder = this });
        }
        return this;
    }

    public SalesOrder UpdateStatus(string status)
    {
        if (!string.Equals(Status, status, StringComparison.OrdinalIgnoreCase))
        {
            var previousStatus = Status;
            Status = status;
            
            QueueDomainEvent(new SalesOrderStatusChanged 
            { 
                SalesOrder = this, 
                PreviousStatus = previousStatus, 
                NewStatus = status 
            });
        }

        return this;
    }

    public SalesOrder UpdatePaymentStatus(string paymentStatus)
    {
        if (!string.Equals(PaymentStatus, paymentStatus, StringComparison.OrdinalIgnoreCase))
        {
            var previousStatus = PaymentStatus;
            PaymentStatus = paymentStatus;
            
            QueueDomainEvent(new SalesOrderPaymentStatusChanged 
            { 
                SalesOrder = this, 
                PreviousStatus = previousStatus, 
                NewStatus = paymentStatus 
            });
        }

        return this;
    }

    public SalesOrder ApplyDiscount(decimal discountAmount)
    {
        DiscountAmount = discountAmount;
        RecalculateTotals();
        QueueDomainEvent(new SalesOrderDiscountApplied { SalesOrder = this, DiscountAmount = discountAmount });
        return this;
    }

    public SalesOrder SetDeliveryDate(DateTime deliveryDate)
    {
        DeliveryDate = deliveryDate;
        QueueDomainEvent(new SalesOrderDeliveryScheduled { SalesOrder = this });
        return this;
    }

    private void RecalculateTotals()
    {
        SubTotal = Items.Sum(i => i.TotalPrice);
        TotalAmount = SubTotal + TaxAmount + ShippingAmount - DiscountAmount;
    }

    public bool IsWholesaleOrder() => OrderType.Equals("Wholesale", StringComparison.OrdinalIgnoreCase);
    public bool IsRetailOrder() => OrderType.Equals("Retail", StringComparison.OrdinalIgnoreCase);
    public bool IsOverdue() => PaymentStatus == "Overdue";
    public bool IsPaid() => PaymentStatus == "Paid";
    public bool IsDelivered() => Status == "Delivered";

    // Update method to apply editable fields from application layer
    public SalesOrder Update(
        string orderNumber,
        DefaultIdType customerId,
        DateTime orderDate,
        DateTime? deliveryDate,
        string status,
        string orderType,
        string paymentStatus,
        string paymentMethod,
        string? deliveryAddress,
        bool isUrgent,
        string? salesPersonId,
        DefaultIdType? warehouseId)
    {
        var changed = false;

        if (OrderNumber != orderNumber)
        {
            OrderNumber = orderNumber;
            changed = true;
        }

        if (CustomerId != customerId)
        {
            CustomerId = customerId;
            changed = true;
        }

        if (OrderDate != orderDate)
        {
            OrderDate = orderDate;
            changed = true;
        }

        if (DeliveryDate != deliveryDate)
        {
            DeliveryDate = deliveryDate;
            changed = true;
        }

        if (Status != status)
        {
            Status = status;
            changed = true;
        }

        if (OrderType != orderType)
        {
            OrderType = orderType;
            changed = true;
        }

        if (PaymentStatus != paymentStatus)
        {
            PaymentStatus = paymentStatus;
            changed = true;
        }

        if (PaymentMethod != paymentMethod)
        {
            PaymentMethod = paymentMethod;
            changed = true;
        }

        if (DeliveryAddress != deliveryAddress)
        {
            DeliveryAddress = deliveryAddress;
            changed = true;
        }

        if (IsUrgent != isUrgent)
        {
            IsUrgent = isUrgent;
            changed = true;
        }

        if (SalesPersonId != salesPersonId)
        {
            SalesPersonId = salesPersonId;
            changed = true;
        }

        if (WarehouseId != warehouseId)
        {
            WarehouseId = warehouseId;
            changed = true;
        }

        if (changed)
        {
            QueueDomainEvent(new SalesOrderUpdated { SalesOrder = this });
        }

        return this;
    }

    // Update a child item's quantity and recalculate totals
    public SalesOrder UpdateItemQuantity(DefaultIdType salesOrderItemId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.Id == salesOrderItemId);
        if (item is null)
            throw new Store.Domain.Exceptions.SalesOrder.SalesOrderItemNotFoundException(salesOrderItemId);

        item.UpdateQuantity(quantity);
        RecalculateTotals();
        QueueDomainEvent(new SalesOrderItemQuantityUpdated { SalesOrderItem = item });
        QueueDomainEvent(new SalesOrderUpdated { SalesOrder = this });
        return this;
    }

    // Ship (set shipped quantity) for a child item and recalc totals
    public SalesOrder ShipItemQuantity(DefaultIdType salesOrderItemId, int shippedQuantity)
    {
        var item = Items.FirstOrDefault(i => i.Id == salesOrderItemId);
        if (item is null)
            throw new Store.Domain.Exceptions.SalesOrder.SalesOrderItemNotFoundException(salesOrderItemId);

        item.ShipQuantity(shippedQuantity);
        RecalculateTotals();
        QueueDomainEvent(new SalesOrderItemShipped { SalesOrderItem = item, PreviousShippedQuantity = 0, NewShippedQuantity = shippedQuantity });
        QueueDomainEvent(new SalesOrderUpdated { SalesOrder = this });
        return this;
    }
}
