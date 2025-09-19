namespace Store.Domain;

/// <summary>
/// Represents a single line within an outbound shipment.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track individual items and quantities being shipped.
/// - Support line-level tracking for partial shipments.
/// - Provide detailed shipment manifests.
/// </remarks>
/// <seealso cref="Store.Domain.Events.ShipmentItemAdded"/>
/// <seealso cref="Store.Domain.Exceptions.Shipment.ShipmentNotFoundException"/>
public sealed class ShipmentItem : AuditableEntity
{
    /// <summary>
    /// Parent shipment identifier.
    /// Example: a <see cref="Shipment"/> Id.
    /// </summary>
    public DefaultIdType ShipmentId { get; private set; }

    /// <summary>
    /// Grocery item being shipped.
    /// Example: an existing <see cref="GroceryItem"/> Id.
    /// </summary>
    public DefaultIdType GroceryItemId { get; private set; }

    /// <summary>
    /// Quantity being shipped. Must be positive.
    /// Example: 50.
    /// </summary>
    public int Quantity { get; private set; }

    private ShipmentItem() { }

    private ShipmentItem(DefaultIdType id, DefaultIdType shipmentId, DefaultIdType groceryItemId, string name, int quantity)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));

        Id = id;
        ShipmentId = shipmentId;
        GroceryItemId = groceryItemId;
        Name = name;
        Quantity = quantity;
    }

    /// <summary>
    /// Factory to create a shipment line item.
    /// </summary>
    /// <param name="shipmentId">Parent shipment id.</param>
    /// <param name="groceryItemId">Item being shipped.</param>
    /// <param name="name">Item name. Example: "Bananas".</param>
    /// <param name="quantity">Shipped quantity. Example: 50.</param>
    public static ShipmentItem Create(DefaultIdType shipmentId, DefaultIdType groceryItemId, string name, int quantity)
        => new(DefaultIdType.NewGuid(), shipmentId, groceryItemId, name, quantity);
}
