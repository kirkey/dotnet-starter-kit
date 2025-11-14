namespace Store.Domain.Entities;

/// <summary>
/// Represents real-time stock levels for an item at a specific warehouse, location, and bin with lot/serial tracking.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track current stock quantities by warehouse, location, and bin for accurate inventory visibility.
/// - Support multi-location inventory with bin-level precision for efficient picking and put-away.
/// - Enable real-time stock queries for availability checks and allocation decisions.
/// - Track reserved quantities for pending orders and transfers separately from available stock.
/// - Support lot number and serial number tracking for traceability and expiration management.
/// - Generate stock reports by location, zone, or warehouse for inventory analysis.
/// - Enable automatic replenishment triggers based on location-specific min/max levels.
/// - Support FIFO/FEFO picking strategies based on lot dates and expiration.
/// 
/// Default values:
/// - ItemId: required item reference
/// - WarehouseId: required warehouse reference
/// - WarehouseLocationId: optional location within warehouse
/// - BinId: optional bin within location
/// - QuantityOnHand: 0 (total physical quantity)
/// - QuantityAvailable: 0 (quantity available for allocation)
/// - QuantityReserved: 0 (quantity reserved for orders/transfers)
/// - QuantityAllocated: 0 (quantity allocated but not yet picked)
/// - LotNumberId: optional lot assignment
/// - SerialNumberId: optional serial number (typically 1 unit per serial)
/// - LastCountDate: null (last physical count date)
/// - LastMovementDate: null (last transaction date)
/// 
/// Business rules:
/// - QuantityOnHand = QuantityAvailable + QuantityReserved + QuantityAllocated
/// - QuantityAvailable cannot be negative
/// - Serial tracked items have quantity of 1 per stock level record
/// - Lot tracked items group by lot number
/// - Reserved quantity reduces available quantity
/// - Location and bin must belong to the specified warehouse
/// - Cannot delete stock levels with positive quantities
/// </remarks>
/// <seealso cref="Store.Domain.Events.StockLevelCreated"/>
/// <seealso cref="Store.Domain.Events.StockLevelUpdated"/>
/// <seealso cref="Store.Domain.Events.StockLevelReserved"/>
/// <seealso cref="Store.Domain.Events.StockLevelAllocated"/>
/// <seealso cref="Store.Domain.Exceptions.StockLevel.StockLevelNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.StockLevel.InsufficientStockException"/>
public sealed class StockLevel : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Item identifier for this stock level.
    /// </summary>
    public DefaultIdType ItemId { get; private set; }

    /// <summary>
    /// Warehouse identifier where stock is located.
    /// </summary>
    public DefaultIdType WarehouseId { get; private set; }

    /// <summary>
    /// Optional warehouse location (aisle, zone, area) within the warehouse.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; private set; }

    /// <summary>
    /// Optional bin (shelf, pallet, container) within the location.
    /// </summary>
    public DefaultIdType? BinId { get; private set; }

    /// <summary>
    /// Optional lot number for lot-tracked items.
    /// </summary>
    public DefaultIdType? LotNumberId { get; private set; }

    /// <summary>
    /// Optional serial number for serial-tracked items.
    /// </summary>
    public DefaultIdType? SerialNumberId { get; private set; }

    /// <summary>
    /// Total quantity physically on hand at this location.
    /// Example: 100 units available in this bin.
    /// </summary>
    public int QuantityOnHand { get; private set; }

    /// <summary>
    /// Quantity available for allocation (not reserved or allocated).
    /// Example: 75 units available for new orders.
    /// </summary>
    public int QuantityAvailable { get; private set; }

    /// <summary>
    /// Quantity reserved for specific orders or transfers (soft allocation).
    /// Example: 20 units reserved for customer orders.
    /// </summary>
    public int QuantityReserved { get; private set; }

    /// <summary>
    /// Quantity allocated to pick lists (hard allocation).
    /// Example: 5 units on active pick lists.
    /// </summary>
    public int QuantityAllocated { get; private set; }

    /// <summary>
    /// Date of last physical count for this stock level.
    /// Example: 2025-09-15 during cycle count.
    /// </summary>
    public DateTime? LastCountDate { get; private set; }

    /// <summary>
    /// Date of last inventory movement affecting this stock level.
    /// Example: 2025-10-01 when goods were received.
    /// </summary>
    public DateTime? LastMovementDate { get; private set; }

    /// <summary>
    /// Navigation property to the item.
    /// </summary>
    public Item Item { get; private set; } = null!;

    /// <summary>
    /// Navigation property to the warehouse.
    /// </summary>
    public Warehouse Warehouse { get; private set; } = null!;

    /// <summary>
    /// Navigation property to the warehouse location.
    /// </summary>
    public WarehouseLocation? WarehouseLocation { get; private set; }

    /// <summary>
    /// Navigation property to the bin.
    /// </summary>
    public Bin? Bin { get; private set; }

    /// <summary>
    /// Navigation property to the lot number.
    /// </summary>
    public LotNumber? LotNumber { get; private set; }

    /// <summary>
    /// Navigation property to the serial number.
    /// </summary>
    public SerialNumber? SerialNumber { get; private set; }

    private StockLevel() { }

    private StockLevel(
        DefaultIdType id,
        DefaultIdType itemId,
        DefaultIdType warehouseId,
        DefaultIdType? warehouseLocationId,
        DefaultIdType? binId,
        DefaultIdType? lotNumberId,
        DefaultIdType? serialNumberId,
        int quantityOnHand)
    {
        if (itemId == DefaultIdType.Empty) throw new ArgumentException("ItemId is required", nameof(itemId));
        if (warehouseId == DefaultIdType.Empty) throw new ArgumentException("WarehouseId is required", nameof(warehouseId));
        if (quantityOnHand < 0) throw new ArgumentException("QuantityOnHand cannot be negative", nameof(quantityOnHand));

        Id = id;
        ItemId = itemId;
        WarehouseId = warehouseId;
        WarehouseLocationId = warehouseLocationId;
        BinId = binId;
        LotNumberId = lotNumberId;
        SerialNumberId = serialNumberId;
        QuantityOnHand = quantityOnHand;
        QuantityAvailable = quantityOnHand; // Initially all on-hand is available
        QuantityReserved = 0;
        QuantityAllocated = 0;
        LastMovementDate = DateTime.UtcNow;

        QueueDomainEvent(new StockLevelCreated { StockLevel = this });
    }

    public static StockLevel Create(
        DefaultIdType itemId,
        DefaultIdType warehouseId,
        DefaultIdType? warehouseLocationId = null,
        DefaultIdType? binId = null,
        DefaultIdType? lotNumberId = null,
        DefaultIdType? serialNumberId = null,
        int quantityOnHand = 0)
    {
        return new StockLevel(
            DefaultIdType.NewGuid(),
            itemId,
            warehouseId,
            warehouseLocationId,
            binId,
            lotNumberId,
            serialNumberId,
            quantityOnHand);
    }

    /// <summary>
    /// Increases the on-hand quantity (e.g., from receiving goods).
    /// </summary>
    public StockLevel IncreaseQuantity(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));

        QuantityOnHand += quantity;
        QuantityAvailable += quantity;
        LastMovementDate = DateTime.UtcNow;

        QueueDomainEvent(new StockLevelUpdated { StockLevel = this, QuantityChange = quantity, ChangeType = "INCREASE" });
        return this;
    }

    /// <summary>
    /// Decreases the on-hand quantity (e.g., from picking or shipment).
    /// </summary>
    public StockLevel DecreaseQuantity(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (QuantityAvailable < quantity) throw new Exceptions.StockLevel.InsufficientStockException(ItemId, WarehouseId, QuantityAvailable, quantity);

        QuantityOnHand -= quantity;
        QuantityAvailable -= quantity;
        LastMovementDate = DateTime.UtcNow;

        QueueDomainEvent(new StockLevelUpdated { StockLevel = this, QuantityChange = -quantity, ChangeType = "DECREASE" });
        return this;
    }

    /// <summary>
    /// Reserves quantity for an order or transfer (soft allocation).
    /// </summary>
    public StockLevel ReserveQuantity(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (QuantityAvailable < quantity) throw new Exceptions.StockLevel.InsufficientStockException(ItemId, WarehouseId, QuantityAvailable, quantity);

        QuantityAvailable -= quantity;
        QuantityReserved += quantity;
        LastMovementDate = DateTime.UtcNow;

        QueueDomainEvent(new StockLevelReserved { StockLevel = this, ReservedQuantity = quantity });
        return this;
    }

    /// <summary>
    /// Releases reserved quantity back to available (e.g., order cancelled).
    /// </summary>
    public StockLevel ReleaseReservation(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (QuantityReserved < quantity) throw new ArgumentException("Cannot release more than reserved quantity", nameof(quantity));

        QuantityReserved -= quantity;
        QuantityAvailable += quantity;
        LastMovementDate = DateTime.UtcNow;

        QueueDomainEvent(new StockLevelUpdated { StockLevel = this, QuantityChange = 0, ChangeType = "RELEASE_RESERVATION" });
        return this;
    }

    /// <summary>
    /// Allocates reserved quantity to a pick list (hard allocation).
    /// </summary>
    public StockLevel AllocateQuantity(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (QuantityReserved < quantity) throw new ArgumentException("Cannot allocate more than reserved quantity", nameof(quantity));

        QuantityReserved -= quantity;
        QuantityAllocated += quantity;
        LastMovementDate = DateTime.UtcNow;

        QueueDomainEvent(new StockLevelAllocated { StockLevel = this, AllocatedQuantity = quantity });
        return this;
    }

    /// <summary>
    /// Confirms picking (removes allocated quantity from on-hand).
    /// </summary>
    public StockLevel ConfirmPick(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(quantity));
        if (QuantityAllocated < quantity) throw new ArgumentException("Cannot confirm more than allocated quantity", nameof(quantity));

        QuantityAllocated -= quantity;
        QuantityOnHand -= quantity;
        LastMovementDate = DateTime.UtcNow;

        QueueDomainEvent(new StockLevelUpdated { StockLevel = this, QuantityChange = -quantity, ChangeType = "PICK_CONFIRMED" });
        return this;
    }

    /// <summary>
    /// Records a physical count and adjusts quantities.
    /// </summary>
    public StockLevel RecordCount(int countedQuantity)
    {
        if (countedQuantity < 0) throw new ArgumentException("Counted quantity cannot be negative", nameof(countedQuantity));

        var variance = countedQuantity - QuantityOnHand;
        QuantityOnHand = countedQuantity;
        QuantityAvailable = Math.Max(0, countedQuantity - QuantityReserved - QuantityAllocated);
        LastCountDate = DateTime.UtcNow;
        LastMovementDate = DateTime.UtcNow;

        QueueDomainEvent(new StockLevelCounted { StockLevel = this, CountedQuantity = countedQuantity, Variance = variance });
        return this;
    }

    /// <summary>
    /// Updates the location, bin, lot, and serial assignments for this stock level.
    /// Note: Quantity operations use specific methods (Reserve, Allocate, etc.).
    /// </summary>
    public StockLevel UpdateLocationAssignments(
        DefaultIdType? warehouseLocationId,
        DefaultIdType? binId,
        DefaultIdType? lotNumberId,
        DefaultIdType? serialNumberId)
    {
        bool isUpdated = false;

        if (WarehouseLocationId != warehouseLocationId)
        {
            WarehouseLocationId = warehouseLocationId;
            isUpdated = true;
        }

        if (BinId != binId)
        {
            BinId = binId;
            isUpdated = true;
        }

        if (LotNumberId != lotNumberId)
        {
            LotNumberId = lotNumberId;
            isUpdated = true;
        }

        if (SerialNumberId != serialNumberId)
        {
            SerialNumberId = serialNumberId;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new StockLevelUpdated { StockLevel = this, QuantityChange = 0, ChangeType = "LOCATION_UPDATE" });
        }

        return this;
    }
}
