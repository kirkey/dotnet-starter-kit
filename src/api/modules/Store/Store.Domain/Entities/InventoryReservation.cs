namespace Store.Domain.Entities;

/// <summary>
/// Represents a reservation of inventory for a specific purpose (order, transfer, production) to prevent overselling.
/// </summary>
/// <remarks>
/// Use cases:
/// - Reserve inventory for confirmed orders to prevent double-allocation.
/// - Hold stock for inter-warehouse transfers during transit.
/// - Reserve materials for production or assembly orders.
/// - Support soft allocation before hard allocation (pick list creation).
/// - Enable inventory availability checks considering reservations.
/// - Track reservation expiration for automatic release.
/// - Generate availability-to-promise (ATP) reports.
/// 
/// Default values:
/// - ReservationNumber: required unique identifier (example: "RES-2025-001")
/// - ItemId: required item being reserved
/// - WarehouseId: required source warehouse
/// - QuantityReserved: required quantity to reserve
/// - Status: "Active" (Active, Allocated, Released, Expired)
/// - ReservationType: required type (Order, Transfer, Production, Other)
/// - ExpirationDate: optional automatic release date
/// 
/// Business rules:
/// - ReservationNumber must be unique
/// - Quantity must be positive
/// - Cannot reserve more than available stock
/// - Expired reservations auto-release
/// - Allocated reservations convert to pick lists
/// - Released reservations return to available stock
/// </remarks>
/// <seealso cref="Store.Domain.Events.InventoryReservationCreated"/>
/// <seealso cref="Store.Domain.Events.InventoryReservationReleased"/>
/// <seealso cref="Store.Domain.Exceptions.InventoryReservation.InventoryReservationNotFoundException"/>
public sealed class InventoryReservation : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique reservation number.
    /// Example: "RES-2025-001", "RSV-WH1-001".
    /// Max length: 100.
    /// </summary>
    public string ReservationNumber { get; private set; } = default!;

    /// <summary>
    /// Item being reserved.
    /// </summary>
    public DefaultIdType ItemId { get; private set; }

    /// <summary>
    /// Warehouse where item is reserved.
    /// </summary>
    public DefaultIdType WarehouseId { get; private set; }

    /// <summary>
    /// Optional specific location within warehouse.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; private set; }

    /// <summary>
    /// Optional specific bin within location.
    /// </summary>
    public DefaultIdType? BinId { get; private set; }

    /// <summary>
    /// Optional lot number for lot-tracked items.
    /// </summary>
    public DefaultIdType? LotNumberId { get; private set; }

    /// <summary>
    /// Quantity reserved.
    /// </summary>
    public int QuantityReserved { get; private set; }

    /// <summary>
    /// Reservation type: Order, Transfer, Production, Assembly, Other.
    /// Max length: 50.
    /// </summary>
    public string ReservationType { get; private set; } = default!;

    /// <summary>
    /// Reservation status: Active, Allocated, Released, Expired, Cancelled.
    /// </summary>
    public string Status { get; private set; } = "Active";

    /// <summary>
    /// Reference to source document (order ID, transfer ID, etc.).
    /// Max length: 100.
    /// </summary>
    public string? ReferenceNumber { get; private set; }

    /// <summary>
    /// Date reservation was created.
    /// </summary>
    public DateTime ReservationDate { get; private set; }

    /// <summary>
    /// Optional expiration date for automatic release.
    /// </summary>
    public DateTime? ExpirationDate { get; private set; }

    /// <summary>
    /// Date reservation was released or allocated.
    /// </summary>
    public DateTime? CompletionDate { get; private set; }

    /// <summary>
    /// User who created the reservation.
    /// Max length: 100.
    /// </summary>
    public string? ReservedBy { get; private set; }

    /// <summary>
    /// Reason for release if status is Released or Cancelled.
    /// Max length: 500.
    /// </summary>
    public string? ReleaseReason { get; private set; }

    /// <summary>
    /// Navigation property to item.
    /// </summary>
    public Item Item { get; private set; } = default!;

    /// <summary>
    /// Navigation property to warehouse.
    /// </summary>
    public Warehouse Warehouse { get; private set; } = default!;

    /// <summary>
    /// Navigation property to warehouse location.
    /// </summary>
    public WarehouseLocation? WarehouseLocation { get; private set; }

    /// <summary>
    /// Navigation property to bin.
    /// </summary>
    public Bin? Bin { get; private set; }

    /// <summary>
    /// Navigation property to lot number.
    /// </summary>
    public LotNumber? LotNumber { get; private set; }

    private InventoryReservation() { }

    private InventoryReservation(
        DefaultIdType id,
        string reservationNumber,
        DefaultIdType itemId,
        DefaultIdType warehouseId,
        DefaultIdType? warehouseLocationId,
        DefaultIdType? binId,
        DefaultIdType? lotNumberId,
        int quantityReserved,
        string reservationType,
        string? referenceNumber,
        DateTime? expirationDate,
        string? reservedBy)
    {
        if (string.IsNullOrWhiteSpace(reservationNumber)) throw new ArgumentException("ReservationNumber is required", nameof(reservationNumber));
        if (reservationNumber.Length > 100) throw new ArgumentException("ReservationNumber must not exceed 100 characters", nameof(reservationNumber));

        if (itemId == Guid.Empty) throw new ArgumentException("ItemId is required", nameof(itemId));
        if (warehouseId == Guid.Empty) throw new ArgumentException("WarehouseId is required", nameof(warehouseId));

        if (quantityReserved <= 0) throw new ArgumentException("QuantityReserved must be positive", nameof(quantityReserved));

        if (string.IsNullOrWhiteSpace(reservationType)) throw new ArgumentException("ReservationType is required", nameof(reservationType));
        var validTypes = new[] { "Order", "Transfer", "Production", "Assembly", "Other" };
        if (!validTypes.Contains(reservationType, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException($"ReservationType must be one of: {string.Join(", ", validTypes)}", nameof(reservationType));

        if (referenceNumber is { Length: > 100 }) throw new ArgumentException("ReferenceNumber must not exceed 100 characters", nameof(referenceNumber));
        if (reservedBy is { Length: > 100 }) throw new ArgumentException("ReservedBy must not exceed 100 characters", nameof(reservedBy));

        Id = id;
        ReservationNumber = reservationNumber;
        ItemId = itemId;
        WarehouseId = warehouseId;
        WarehouseLocationId = warehouseLocationId;
        BinId = binId;
        LotNumberId = lotNumberId;
        QuantityReserved = quantityReserved;
        ReservationType = reservationType;
        ReferenceNumber = referenceNumber;
        ReservationDate = DateTime.UtcNow;
        ExpirationDate = expirationDate;
        ReservedBy = reservedBy;
        Status = "Active";

        QueueDomainEvent(new InventoryReservationCreated { InventoryReservation = this });
    }

    public static InventoryReservation Create(
        string reservationNumber,
        DefaultIdType itemId,
        DefaultIdType warehouseId,
        int quantityReserved,
        string reservationType,
        DefaultIdType? warehouseLocationId = null,
        DefaultIdType? binId = null,
        DefaultIdType? lotNumberId = null,
        string? referenceNumber = null,
        DateTime? expirationDate = null,
        string? reservedBy = null)
    {
        return new InventoryReservation(
            DefaultIdType.NewGuid(),
            reservationNumber,
            itemId,
            warehouseId,
            warehouseLocationId,
            binId,
            lotNumberId,
            quantityReserved,
            reservationType,
            referenceNumber,
            expirationDate,
            reservedBy);
    }

    public InventoryReservation Allocate()
    {
        if (Status != "Active") throw new InvalidOperationException($"Cannot allocate reservation in {Status} status");

        Status = "Allocated";
        CompletionDate = DateTime.UtcNow;

        QueueDomainEvent(new InventoryReservationAllocated { InventoryReservation = this });
        return this;
    }

    public InventoryReservation Release(string reason)
    {
        if (Status == "Released" || Status == "Allocated") throw new InvalidOperationException($"Cannot release reservation in {Status} status");

        Status = "Released";
        CompletionDate = DateTime.UtcNow;
        ReleaseReason = reason;

        QueueDomainEvent(new InventoryReservationReleased { InventoryReservation = this, Reason = reason });
        return this;
    }

    public InventoryReservation Cancel(string reason)
    {
        if (Status == "Cancelled" || Status == "Allocated") throw new InvalidOperationException($"Cannot cancel reservation in {Status} status");

        Status = "Cancelled";
        CompletionDate = DateTime.UtcNow;
        ReleaseReason = reason;

        QueueDomainEvent(new InventoryReservationCancelled { InventoryReservation = this, Reason = reason });
        return this;
    }

    public InventoryReservation MarkExpired()
    {
        if (Status != "Active") throw new InvalidOperationException($"Cannot expire reservation in {Status} status");

        Status = "Expired";
        CompletionDate = DateTime.UtcNow;
        ReleaseReason = "Automatic expiration";

        QueueDomainEvent(new InventoryReservationExpired { InventoryReservation = this });
        return this;
    }

    public bool IsExpired() => ExpirationDate.HasValue && ExpirationDate.Value <= DateTime.UtcNow && Status == "Active";

    public bool IsActive() => Status == "Active" && !IsExpired();
}
