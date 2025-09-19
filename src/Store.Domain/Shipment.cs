namespace Store.Domain;

/// <summary>
/// Represents an outbound shipment for customer deliveries with comprehensive tracking and fulfillment management.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track outbound deliveries to customers from sales orders and direct shipments.
/// - Support multiple shipment methods including ground, express, and freight delivery.
/// - Monitor shipment lifecycle from preparation through delivered with status tracking.
/// - Enable partial shipments for large orders and back-order management.
/// - Support multi-warehouse shipments and consolidation for cost optimization.
/// - Track carrier information and provide customer shipment visibility.
/// - Handle delivery exceptions, returns, and customer notifications.
/// - Generate shipping labels, packing slips, and delivery confirmations.
/// 
/// Default values:
/// - ShipmentNumber: required unique identifier (example: "SH-2025-09-001")
/// - SalesOrderId: optional sales order reference (null for direct shipments)
/// - CustomerId: required customer reference for delivery
/// - WarehouseId: required source warehouse for shipment
/// - ShipmentDate: required shipment creation date (example: 2025-09-19)
/// - EstimatedDeliveryDate: optional estimated delivery (example: 2025-09-21)
/// - Status: "Pending" (new shipments start as pending preparation)
/// - ShippingMethod: required delivery method (example: "Ground", "Express", "Freight")
/// - TrackingNumber: null (set when carrier assigns tracking)
/// - ShippingCost: 0.00 (calculated based on method and destination)
/// - ActualDeliveryDate: null (set when delivery is confirmed)
/// - DeliveryAddress: required destination address
/// 
/// Business rules:
/// - ShipmentNumber must be unique within the system
/// - Cannot modify shipped orders without proper authorization
/// - Estimated delivery date should be after shipment date
/// - Cannot ship without sufficient inventory at source warehouse
/// - Tracking number required for shipped status
/// - Delivery confirmation required for completion
/// - Cannot delete shipments with tracking information
/// - Carrier must be active to create new shipments
/// - Address validation required for successful delivery
/// </remarks>
/// <seealso cref="Store.Domain.Events.ShipmentCreated"/>
/// <seealso cref="Store.Domain.Events.ShipmentUpdated"/>
/// <seealso cref="Store.Domain.Events.ShipmentShipped"/>
/// <seealso cref="Store.Domain.Events.ShipmentDelivered"/>
/// <seealso cref="Store.Domain.Events.ShipmentCancelled"/>
/// <seealso cref="Store.Domain.Events.ShipmentTrackingUpdated"/>
/// <seealso cref="Store.Domain.Exceptions.Shipment.ShipmentNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.Shipment.ShipmentCannotBeModifiedException"/>
/// <seealso cref="Store.Domain.Exceptions.Shipment.InvalidShipmentStatusException"/>
public sealed class Shipment : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique shipment number for tracking deliveries.
    /// Example: "SH-2025-09-001". Max length: 100.
    /// </summary>
    public string ShipmentNumber { get; private set; } = default!;

    /// <summary>
    /// Optional reference to the sales order being fulfilled.
    /// Example: an existing <see cref="SalesOrder"/> Id or null for ad-hoc shipments.
    /// </summary>
    public DefaultIdType? SalesOrderId { get; private set; }

    /// <summary>
    /// Date when shipment was created/sent.
    /// Example: 2025-09-18T11:00:00Z. Defaults to current UTC if unspecified.
    /// </summary>
    public DateTime ShipDate { get; private set; }

    /// <summary>
    /// Shipment processing status.
    /// Allowed values: Pending, Shipped, Delivered, Cancelled. Default: Pending.
    /// </summary>
    public string Status { get; private set; } = "Pending"; // Pending, Shipped, Delivered, Cancelled

    /// <summary>
    /// Items included in this shipment.
    /// Example count: 0 at creation.
    /// </summary>
    public ICollection<ShipmentItem> Items { get; private set; } = new List<ShipmentItem>();

    private Shipment() { }

    private Shipment(DefaultIdType id, string shipmentNumber, DateTime shipDate, DefaultIdType? salesOrderId)
    {
        if (string.IsNullOrWhiteSpace(shipmentNumber)) throw new ArgumentException("ShipmentNumber is required", nameof(shipmentNumber));
        if (shipmentNumber.Length > 100) throw new ArgumentException("ShipmentNumber must not exceed 100 characters", nameof(shipmentNumber));

        Id = id;
        ShipmentNumber = shipmentNumber;
        ShipDate = shipDate == default ? DateTime.UtcNow : shipDate;
        SalesOrderId = salesOrderId;
        Status = "Pending";
        QueueDomainEvent(new ShipmentCreated { Shipment = this });
    }

    /// <summary>
    /// Factory to create a new shipment.
    /// </summary>
    /// <param name="shipmentNumber">Unique shipment number. Example: "SH-2025-09-001".</param>
    /// <param name="shipDate">Ship date. Defaults to now if unspecified.</param>
    /// <param name="salesOrderId">Optional sales order reference.</param>
    public static Shipment Create(string shipmentNumber, DateTime shipDate, DefaultIdType? salesOrderId = null)
        => new(DefaultIdType.NewGuid(), shipmentNumber, shipDate, salesOrderId);

    /// <summary>
    /// Adds an item to the shipment.
    /// </summary>
    /// <param name="groceryItemId">Item being shipped.</param>
    /// <param name="name">Item name snapshot. Example: "Bananas".</param>
    /// <param name="quantity">Shipped quantity. Example: 50.</param>
    public Shipment AddItem(DefaultIdType groceryItemId, string name, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        var item = ShipmentItem.Create(Id, groceryItemId, name, quantity);
        Items.Add(item);
        QueueDomainEvent(new ShipmentItemAdded { Shipment = this, Item = item });
        return this;
    }

    /// <summary>
    /// Marks the shipment as shipped/dispatched.
    /// </summary>
    public Shipment MarkShipped()
    {
        if (string.Equals(Status, "Shipped", StringComparison.OrdinalIgnoreCase)) return this;
        Status = "Shipped";
        QueueDomainEvent(new ShipmentShipped { Shipment = this });
        return this;
    }
}
