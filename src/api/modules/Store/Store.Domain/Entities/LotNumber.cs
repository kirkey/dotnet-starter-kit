namespace Store.Domain.Entities;

/// <summary>
/// Represents a lot or batch number for tracking groups of inventory items with traceability and expiration management.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track inventory lots/batches for quality control and product recalls.
/// - Manage expiration dates for perishable items with lot-level precision.
/// - Enable FIFO/FEFO picking strategies based on lot dates.
/// - Support regulatory compliance with batch traceability requirements.
/// - Track lot-specific quality test results and certifications.
/// - Enable lot-to-lot inventory movements and transfers.
/// - Generate lot expiration reports for proactive management.
/// 
/// Default values:
/// - LotNumber: required unique lot identifier (example: "LOT-2025-001", "BATCH-A1234")
/// - ItemId: required item reference
/// - ManufactureDate: optional production date
/// - ExpirationDate: optional expiry date (required for perishable items)
/// - ReceiptDate: date lot was received
/// - QuantityReceived: original quantity received
/// - QuantityRemaining: current available quantity
/// - Status: "Active" (Active, Expired, Quarantine, Recalled)
/// 
/// Business rules:
/// - LotNumber must be unique per item
/// - ExpirationDate must be after ManufactureDate
/// - Cannot use expired lots for picking
/// - Recalled lots must be quarantined
/// - FEFO picking uses lots expiring soonest first
/// </remarks>
/// <seealso cref="Store.Domain.Events.LotNumberCreated"/>
/// <seealso cref="Store.Domain.Events.LotNumberUpdated"/>
/// <seealso cref="Store.Domain.Exceptions.LotNumber.LotNumberNotFoundException"/>
public sealed class LotNumber : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique lot/batch number.
    /// Example: "LOT-2025-001", "BATCH-A1234".
    /// Max length: 100.
    /// </summary>
    public string LotCode { get; private set; } = null!;

    /// <summary>
    /// Item this lot belongs to.
    /// </summary>
    public DefaultIdType ItemId { get; private set; }

    /// <summary>
    /// Supplier this lot was received from.
    /// </summary>
    public DefaultIdType? SupplierId { get; private set; }

    /// <summary>
    /// Date the lot was manufactured/produced.
    /// </summary>
    public DateTime? ManufactureDate { get; private set; }

    /// <summary>
    /// Date the lot expires.
    /// </summary>
    public DateTime? ExpirationDate { get; private set; }

    /// <summary>
    /// Date the lot was received into inventory.
    /// </summary>
    public DateTime ReceiptDate { get; private set; }

    /// <summary>
    /// Original quantity received for this lot.
    /// </summary>
    public int QuantityReceived { get; private set; }

    /// <summary>
    /// Current remaining quantity for this lot.
    /// </summary>
    public int QuantityRemaining { get; private set; }

    /// <summary>
    /// Lot status: Active, Expired, Quarantine, Recalled.
    /// </summary>
    public string Status { get; private set; } = "Active";

    /// <summary>
    /// Optional quality test results or notes.
    /// Max length: 1000.
    /// </summary>
    public string? QualityNotes { get; private set; }

    /// <summary>
    /// Navigation property to item.
    /// </summary>
    public Item Item { get; private set; } = null!;

    /// <summary>
    /// Navigation property to supplier.
    /// </summary>
    public Supplier? Supplier { get; private set; }

    private LotNumber() { }

    private LotNumber(
        DefaultIdType id,
        string lotCode,
        DefaultIdType itemId,
        DefaultIdType? supplierId,
        DateTime? manufactureDate,
        DateTime? expirationDate,
        DateTime receiptDate,
        int quantityReceived,
        string? qualityNotes)
    {
        if (string.IsNullOrWhiteSpace(lotCode)) throw new ArgumentException("LotCode is required", nameof(lotCode));
        if (lotCode.Length > 100) throw new ArgumentException("LotCode must not exceed 100 characters", nameof(lotCode));

        if (itemId == DefaultIdType.Empty) throw new ArgumentException("ItemId is required", nameof(itemId));

        if (manufactureDate.HasValue && expirationDate.HasValue && expirationDate.Value <= manufactureDate.Value)
            throw new ArgumentException("ExpirationDate must be after ManufactureDate", nameof(expirationDate));

        if (quantityReceived < 0) throw new ArgumentException("QuantityReceived cannot be negative", nameof(quantityReceived));

        if (qualityNotes is { Length: > 1000 }) throw new ArgumentException("QualityNotes must not exceed 1000 characters", nameof(qualityNotes));

        Id = id;
        LotCode = lotCode;
        ItemId = itemId;
        SupplierId = supplierId;
        ManufactureDate = manufactureDate;
        ExpirationDate = expirationDate;
        ReceiptDate = receiptDate == default ? DateTime.UtcNow : receiptDate;
        QuantityReceived = quantityReceived;
        QuantityRemaining = quantityReceived;
        QualityNotes = qualityNotes;
        Status = "Active";

        QueueDomainEvent(new LotNumberCreated { LotNumber = this });
    }

    public static LotNumber Create(
        string lotCode,
        DefaultIdType itemId,
        int quantityReceived,
        DefaultIdType? supplierId = null,
        DateTime? manufactureDate = null,
        DateTime? expirationDate = null,
        DateTime? receiptDate = null,
        string? qualityNotes = null)
    {
        return new LotNumber(
            DefaultIdType.NewGuid(),
            lotCode,
            itemId,
            supplierId,
            manufactureDate,
            expirationDate,
            receiptDate ?? DateTime.Now,
            quantityReceived,
            qualityNotes);
    }

    public LotNumber UpdateQuantity(int quantityChange)
    {
        QuantityRemaining += quantityChange;
        if (QuantityRemaining < 0) QuantityRemaining = 0;

        QueueDomainEvent(new LotNumberUpdated { LotNumber = this });
        return this;
    }

    public LotNumber UpdateStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Status is required", nameof(status));
        
        var validStatuses = new[] { "Active", "Expired", "Quarantine", "Recalled" };
        if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException($"Status must be one of: {string.Join(", ", validStatuses)}", nameof(status));

        Status = status;
        QueueDomainEvent(new LotNumberUpdated { LotNumber = this });
        return this;
    }

    public bool IsExpired() => ExpirationDate.HasValue && ExpirationDate.Value <= DateTime.UtcNow;

    public bool IsExpiringSoon(int daysThreshold = 7) =>
        ExpirationDate.HasValue && ExpirationDate.Value <= DateTime.UtcNow.AddDays(daysThreshold);

    /// <summary>
    /// Updates the name, description, and notes fields.
    /// </summary>
    /// <param name="name">New name.</param>
    /// <param name="description">New description.</param>
    /// <param name="notes">New notes.</param>
    /// <returns>Updated LotNumber instance.</returns>
    public LotNumber UpdateDetails(string? name, string? description, string? notes)
    {
        if (name?.Length > 1024) throw new ArgumentException("Name must not exceed 1024 characters", nameof(name));
        if (description?.Length > 2048) throw new ArgumentException("Description must not exceed 2048 characters", nameof(description));
        if (notes?.Length > 2048) throw new ArgumentException("Notes must not exceed 2048 characters", nameof(notes));

        if (!string.IsNullOrWhiteSpace(name)) Name = name;
        Description = description;
        Notes = notes;
        QueueDomainEvent(new LotNumberUpdated { LotNumber = this });
        return this;
    }
}
