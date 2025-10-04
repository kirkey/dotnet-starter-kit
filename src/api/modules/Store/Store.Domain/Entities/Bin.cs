namespace Store.Domain.Entities;

/// <summary>
/// Represents a specific storage bin or container within a warehouse location for precise inventory placement.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define specific storage bins, shelves, pallets, or containers within warehouse locations.
/// - Enable bin-level inventory tracking for precise stock location and picking accuracy.
/// - Support directed put-away strategies based on bin characteristics and capacity.
/// - Track bin capacity utilization and optimize storage space usage.
/// - Enable bin-to-bin movements and inventory reorganization.
/// - Support FIFO/LIFO/FEFO picking strategies at the bin level.
/// - Generate bin reports for space utilization and inventory density analysis.
/// 
/// Default values:
/// - Code: required unique identifier within location (example: "A1-01-01", "RACK-5-SHELF-3")
/// - WarehouseLocationId: required parent location reference
/// - BinType: required classification (example: "Shelf", "Pallet", "Floor", "Rack")
/// - Capacity: optional maximum capacity in units or cubic measure
/// - CurrentUtilization: 0.00 (percentage of capacity used)
/// - IsActive: true (bins are active by default)
/// - IsPickable: true (bin can be picked from)
/// - IsPutable: true (bin can receive inventory)
/// - Priority: 0 (picking priority, lower = higher priority)
/// 
/// Business rules:
/// - Code must be unique within the warehouse location
/// - Bin must belong to an active warehouse location
/// - Cannot deactivate bins with current inventory
/// - Capacity constraints must be enforced during put-away
/// - Bin type determines storage rules and equipment requirements
/// </remarks>
/// <seealso cref="Store.Domain.Events.BinCreated"/>
/// <seealso cref="Store.Domain.Events.BinUpdated"/>
/// <seealso cref="Store.Domain.Exceptions.Bin.BinNotFoundException"/>
public sealed class Bin : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique bin code within the warehouse location.
    /// Example: "A1-01-01", "RACK-5-SHELF-3", "PALLET-001".
    /// Max length: 50.
    /// </summary>
    public string Code { get; private set; } = default!;

    /// <summary>
    /// Warehouse location this bin belongs to.
    /// </summary>
    public DefaultIdType WarehouseLocationId { get; private set; }

    /// <summary>
    /// Type of bin. Example: "Shelf", "Pallet", "Floor", "Rack", "Drawer".
    /// Max length: 50.
    /// </summary>
    public string BinType { get; private set; } = default!;

    /// <summary>
    /// Maximum capacity in units or volume.
    /// Example: 100 for 100 units, 1000 for 1000 cubic feet.
    /// </summary>
    public decimal? Capacity { get; private set; }

    /// <summary>
    /// Current utilization percentage (0-100).
    /// Example: 75.5 for 75.5% full.
    /// </summary>
    public decimal? CurrentUtilization { get; private set; }

    /// <summary>
    /// Whether bin is active and available for use.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Whether bin can be picked from.
    /// </summary>
    public bool IsPickable { get; private set; } = true;

    /// <summary>
    /// Whether bin can receive put-away inventory.
    /// </summary>
    public bool IsPutable { get; private set; } = true;

    /// <summary>
    /// Picking priority (lower number = higher priority).
    /// Example: 1 for primary pick face, 10 for reserve storage.
    /// </summary>
    public int Priority { get; private set; }

    /// <summary>
    /// Navigation property to warehouse location.
    /// </summary>
    public WarehouseLocation WarehouseLocation { get; private set; } = default!;

    private Bin() { }

    private Bin(
        DefaultIdType id,
        string name,
        string? description,
        string code,
        DefaultIdType warehouseLocationId,
        string binType,
        decimal? capacity,
        int priority)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));

        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code is required", nameof(code));
        if (code.Length > 50) throw new ArgumentException("Code must not exceed 50 characters", nameof(code));

        if (warehouseLocationId == DefaultIdType.Empty) throw new ArgumentException("WarehouseLocationId is required", nameof(warehouseLocationId));

        if (string.IsNullOrWhiteSpace(binType)) throw new ArgumentException("BinType is required", nameof(binType));
        if (binType.Length > 50) throw new ArgumentException("BinType must not exceed 50 characters", nameof(binType));

        if (capacity is < 0) throw new ArgumentException("Capacity cannot be negative", nameof(capacity));
        if (priority < 0) throw new ArgumentException("Priority cannot be negative", nameof(priority));

        Id = id;
        Name = name;
        Description = description;
        Code = code;
        WarehouseLocationId = warehouseLocationId;
        BinType = binType;
        Capacity = capacity;
        Priority = priority;
        CurrentUtilization = 0;
        IsActive = true;
        IsPickable = true;
        IsPutable = true;

        QueueDomainEvent(new BinCreated { Bin = this });
    }

    public static Bin Create(
        string name,
        string? description,
        string code,
        DefaultIdType warehouseLocationId,
        string binType,
        decimal? capacity = null,
        int priority = 0)
    {
        return new Bin(
            DefaultIdType.NewGuid(),
            name,
            description,
            code,
            warehouseLocationId,
            binType,
            capacity,
            priority);
    }

    public Bin Update(
        string? name,
        string? description,
        string? binType,
        decimal? capacity,
        int? priority,
        bool? isPickable,
        bool? isPutable)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));
            Name = name;
            isUpdated = true;
        }

        if (!string.Equals(Description, description, StringComparison.OrdinalIgnoreCase))
        {
            Description = description;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(binType) && !string.Equals(BinType, binType, StringComparison.OrdinalIgnoreCase))
        {
            if (binType.Length > 50) throw new ArgumentException("BinType must not exceed 50 characters", nameof(binType));
            BinType = binType;
            isUpdated = true;
        }

        if (capacity.HasValue && Capacity != capacity.Value)
        {
            if (capacity.Value < 0) throw new ArgumentException("Capacity cannot be negative", nameof(capacity));
            Capacity = capacity.Value;
            isUpdated = true;
        }

        if (priority.HasValue && Priority != priority.Value)
        {
            if (priority.Value < 0) throw new ArgumentException("Priority cannot be negative", nameof(priority));
            Priority = priority.Value;
            isUpdated = true;
        }

        if (isPickable.HasValue && IsPickable != isPickable.Value)
        {
            IsPickable = isPickable.Value;
            isUpdated = true;
        }

        if (isPutable.HasValue && IsPutable != isPutable.Value)
        {
            IsPutable = isPutable.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new BinUpdated { Bin = this });
        }

        return this;
    }

    public Bin UpdateUtilization(decimal utilizationPercentage)
    {
        if (utilizationPercentage < 0 || utilizationPercentage > 100)
            throw new ArgumentException("Utilization must be between 0 and 100", nameof(utilizationPercentage));

        CurrentUtilization = utilizationPercentage;
        QueueDomainEvent(new BinUpdated { Bin = this });
        return this;
    }
}
