namespace Store.Domain.Entities;

/// <summary>
/// Represents the relationship between an item and its suppliers with pricing, lead time, and procurement details.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track multiple suppliers per item for sourcing flexibility.
/// - Store supplier-specific pricing, lead times, and order quantities.
/// - Support automatic supplier selection based on lead time, cost, or performance.
/// - Enable supplier comparison for procurement decisions.
/// - Track supplier part numbers and packaging information.
/// - Support preferred supplier designation and ranking.
/// 
/// Default values:
/// - ItemId: required item reference
/// - SupplierId: required supplier reference
/// - SupplierPartNumber: optional supplier's part number
/// - UnitCost: required cost per unit from this supplier
/// - LeadTimeDays: required delivery lead time in days
/// - MinimumOrderQuantity: 1 (minimum quantity for orders)
/// - IsPreferred: false (preferred supplier flag)
/// - IsActive: true (supplier is active for this item)
/// 
/// Business rules:
/// - Combination of ItemId and SupplierId must be unique
/// - UnitCost must be positive
/// - LeadTimeDays must be non-negative
/// - MinimumOrderQuantity must be positive
/// - Only one preferred supplier per item recommended
/// </remarks>
/// <seealso cref="Store.Domain.Events.ItemSupplierCreated"/>
/// <seealso cref="Store.Domain.Events.ItemSupplierUpdated"/>
/// <seealso cref="Store.Domain.Exceptions.ItemSupplier.ItemSupplierNotFoundException"/>
public sealed class ItemSupplier : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Item identifier.
    /// </summary>
    public DefaultIdType ItemId { get; private set; }

    /// <summary>
    /// Supplier identifier.
    /// </summary>
    public DefaultIdType SupplierId { get; private set; }

    /// <summary>
    /// Supplier's part number for this item.
    /// Example: "SUP-PART-12345".
    /// Max length: 100.
    /// </summary>
    public string? SupplierPartNumber { get; private set; }

    /// <summary>
    /// Unit cost from this supplier.
    /// Example: 15.50 for $15.50 per unit.
    /// </summary>
    public decimal UnitCost { get; private set; }

    /// <summary>
    /// Lead time in days for delivery from this supplier.
    /// Example: 7 for one week lead time.
    /// </summary>
    public int LeadTimeDays { get; private set; }

    /// <summary>
    /// Minimum order quantity required by supplier.
    /// Example: 10 for minimum order of 10 units.
    /// </summary>
    public int MinimumOrderQuantity { get; private set; }

    /// <summary>
    /// Standard packaging quantity (units per package).
    /// Example: 12 for dozen packs, 24 for case packs.
    /// </summary>
    public int? PackagingQuantity { get; private set; }
    
    /// <summary>
    /// Whether this is the preferred supplier for this item.
    /// </summary>
    public bool IsPreferred { get; private set; }

    /// <summary>
    /// Whether this supplier is currently active for this item.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// Supplier's delivery reliability rating (0-100).
    /// Example: 95 for 95% on-time delivery.
    /// </summary>
    public decimal? ReliabilityRating { get; private set; }

    /// <summary>
    /// Last date price was updated.
    /// </summary>
    public DateTime? LastPriceUpdate { get; private set; }

    /// <summary>
    /// Navigation property to item.
    /// </summary>
    public Item Item { get; private set; } = default!;

    /// <summary>
    /// Navigation property to supplier.
    /// </summary>
    public Supplier Supplier { get; private set; } = default!;

    private ItemSupplier() { }

    private ItemSupplier(
        DefaultIdType id,
        DefaultIdType itemId,
        DefaultIdType supplierId,
        string? supplierPartNumber,
        decimal unitCost,
        int leadTimeDays,
        int minimumOrderQuantity,
        int? packagingQuantity,
        bool isPreferred)
    {
        if (itemId == DefaultIdType.Empty) throw new ArgumentException("ItemId is required", nameof(itemId));
        if (supplierId == DefaultIdType.Empty) throw new ArgumentException("SupplierId is required", nameof(supplierId));

        if (supplierPartNumber is { Length: > 100 }) throw new ArgumentException("SupplierPartNumber must not exceed 100 characters", nameof(supplierPartNumber));

        if (unitCost < 0) throw new ArgumentException("UnitCost cannot be negative", nameof(unitCost));
        if (leadTimeDays < 0) throw new ArgumentException("LeadTimeDays cannot be negative", nameof(leadTimeDays));
        if (minimumOrderQuantity <= 0) throw new ArgumentException("MinimumOrderQuantity must be positive", nameof(minimumOrderQuantity));
        if (packagingQuantity is <= 0) throw new ArgumentException("PackagingQuantity must be positive", nameof(packagingQuantity));

        Id = id;
        ItemId = itemId;
        SupplierId = supplierId;
        SupplierPartNumber = supplierPartNumber;
        UnitCost = unitCost;
        LeadTimeDays = leadTimeDays;
        MinimumOrderQuantity = minimumOrderQuantity;
        PackagingQuantity = packagingQuantity;
        IsPreferred = isPreferred;
        IsActive = true;
        LastPriceUpdate = DateTime.UtcNow;

        QueueDomainEvent(new ItemSupplierCreated { ItemSupplier = this });
    }

    public static ItemSupplier Create(
        DefaultIdType itemId,
        DefaultIdType supplierId,
        decimal unitCost,
        int leadTimeDays,
        int minimumOrderQuantity = 1,
        string? supplierPartNumber = null,
        int? packagingQuantity = null,
        bool isPreferred = false)
    {
        return new ItemSupplier(
            DefaultIdType.NewGuid(),
            itemId,
            supplierId,
            supplierPartNumber,
            unitCost,
            leadTimeDays,
            minimumOrderQuantity,
            packagingQuantity,
            isPreferred);
    }

    public ItemSupplier UpdatePricing(decimal unitCost)
    {
        if (unitCost < 0) throw new ArgumentException("UnitCost cannot be negative", nameof(unitCost));

        UnitCost = unitCost;
        LastPriceUpdate = DateTime.UtcNow;
        LastPriceUpdate = DateTime.UtcNow;

        QueueDomainEvent(new ItemSupplierUpdated { ItemSupplier = this });
        return this;
    }

    public ItemSupplier UpdateLeadTime(int leadTimeDays)
    {
        if (leadTimeDays < 0) throw new ArgumentException("LeadTimeDays cannot be negative", nameof(leadTimeDays));

        LeadTimeDays = leadTimeDays;
        QueueDomainEvent(new ItemSupplierUpdated { ItemSupplier = this });
        return this;
    }

    public ItemSupplier SetPreferred(bool isPreferred)
    {
        IsPreferred = isPreferred;
        QueueDomainEvent(new ItemSupplierUpdated { ItemSupplier = this });
        return this;
    }

    public ItemSupplier UpdateReliabilityRating(decimal rating)
    {
        if (rating < 0 || rating > 100) throw new ArgumentException("ReliabilityRating must be between 0 and 100", nameof(rating));

        ReliabilityRating = rating;
        QueueDomainEvent(new ItemSupplierUpdated { ItemSupplier = this });
        return this;
    }

    public ItemSupplier Activate()
    {
        if (IsActive) return this;

        IsActive = true;
        QueueDomainEvent(new ItemSupplierUpdated { ItemSupplier = this });
        return this;
    }

    public ItemSupplier Deactivate()
    {
        if (!IsActive) return this;

        IsActive = false;
        QueueDomainEvent(new ItemSupplierUpdated { ItemSupplier = this });
        return this;
    }
}
