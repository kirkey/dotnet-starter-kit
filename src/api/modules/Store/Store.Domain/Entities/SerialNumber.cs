namespace Store.Domain.Entities;

/// <summary>
/// Represents a unique serial number for tracking individual inventory items with complete unit-level traceability.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track high-value items with individual serial numbers for asset management and warranties.
/// - Enable unit-level traceability for recalls, repairs, and returns.
/// - Support serialized inventory movements and transfers.
/// - Track serial number history including sales, repairs, and warranty claims.
/// - Prevent duplicate serial number entries and unauthorized usage.
/// - Enable serial number scanning for receiving, picking, and shipping verification.
/// - Generate serial number reports for audit and compliance requirements.
/// 
/// Default values:
/// - SerialNumberValue: required unique serial identifier (example: "SN-12345", "ABC123XYZ")
/// - ItemId: required item reference
/// - Status: "Available" (Available, Allocated, Shipped, Sold, Defective, Returned)
/// - ReceiptDate: date serial was received
/// - WarehouseId: optional current warehouse location
/// 
/// Business rules:
/// - SerialNumberValue must be globally unique
/// - Each serial represents exactly one unit
/// - Status transitions must follow business rules
/// - Cannot ship or sell serial numbers in Defective status
/// - Serial number history must be maintained for audit trail
/// </remarks>
/// <seealso cref="Store.Domain.Events.SerialNumberCreated"/>
/// <seealso cref="Store.Domain.Events.SerialNumberUpdated"/>
/// <seealso cref="Store.Domain.Exceptions.SerialNumber.SerialNumberNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.SerialNumber.DuplicateSerialNumberException"/>
public sealed class SerialNumber : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique serial number value.
    /// Example: "SN-12345", "ABC123XYZ789".
    /// Max length: 100.
    /// </summary>
    public string SerialNumberValue { get; private set; } = default!;

    /// <summary>
    /// Item this serial number belongs to.
    /// </summary>
    public DefaultIdType ItemId { get; private set; }

    /// <summary>
    /// Current warehouse where serial number is located.
    /// </summary>
    public DefaultIdType? WarehouseId { get; private set; }

    /// <summary>
    /// Current location within warehouse.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; private set; }

    /// <summary>
    /// Current bin where serial number is stored.
    /// </summary>
    public DefaultIdType? BinId { get; private set; }

    /// <summary>
    /// Associated lot number if applicable.
    /// </summary>
    public DefaultIdType? LotNumberId { get; private set; }

    /// <summary>
    /// Serial number status: Available, Allocated, Shipped, Sold, Defective, Returned, InRepair.
    /// </summary>
    public string Status { get; private set; } = "Available";

    /// <summary>
    /// Date serial number was received into inventory.
    /// </summary>
    public DateTime ReceiptDate { get; private set; }

    /// <summary>
    /// Optional manufacture date from manufacturer.
    /// </summary>
    public DateTime? ManufactureDate { get; private set; }

    /// <summary>
    /// Optional warranty expiration date.
    /// </summary>
    public DateTime? WarrantyExpirationDate { get; private set; }

    /// <summary>
    /// Optional reference to external system (e.g., asset management system).
    /// Max length: 100.
    /// </summary>
    public string? ExternalReference { get; private set; }

    /// <summary>
    /// Optional notes about condition, repairs, etc.
    /// Max length: 1000.
    /// </summary>
    

    /// <summary>
    /// Navigation property to item.
    /// </summary>
    public Item Item { get; private set; } = default!;

    /// <summary>
    /// Navigation property to warehouse.
    /// </summary>
    public Warehouse? Warehouse { get; private set; }

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

    private SerialNumber() { }

    private SerialNumber(
        DefaultIdType id,
        string serialNumberValue,
        DefaultIdType itemId,
        DefaultIdType? warehouseId,
        DefaultIdType? warehouseLocationId,
        DefaultIdType? binId,
        DefaultIdType? lotNumberId,
        DateTime receiptDate,
        DateTime? manufactureDate,
        DateTime? warrantyExpirationDate,
        string? externalReference,
        string? notes)
    {
        if (string.IsNullOrWhiteSpace(serialNumberValue)) throw new ArgumentException("SerialNumberValue is required", nameof(serialNumberValue));
        if (serialNumberValue.Length > 100) throw new ArgumentException("SerialNumberValue must not exceed 100 characters", nameof(serialNumberValue));

        if (itemId == DefaultIdType.Empty) throw new ArgumentException("ItemId is required", nameof(itemId));

        if (externalReference is { Length: > 100 }) throw new ArgumentException("ExternalReference must not exceed 100 characters", nameof(externalReference));
        if (notes is { Length: > 1000 }) throw new ArgumentException("Notes must not exceed 1000 characters", nameof(notes));

        Id = id;
        SerialNumberValue = serialNumberValue;
        ItemId = itemId;
        WarehouseId = warehouseId;
        WarehouseLocationId = warehouseLocationId;
        BinId = binId;
        LotNumberId = lotNumberId;
        ReceiptDate = receiptDate == default ? DateTime.UtcNow : receiptDate;
        ManufactureDate = manufactureDate;
        WarrantyExpirationDate = warrantyExpirationDate;
        ExternalReference = externalReference;
        Notes = notes;
        Status = "Available";

        QueueDomainEvent(new SerialNumberCreated { SerialNumber = this });
    }

    public static SerialNumber Create(
        string serialNumberValue,
        DefaultIdType itemId,
        DefaultIdType? warehouseId = null,
        DefaultIdType? warehouseLocationId = null,
        DefaultIdType? binId = null,
        DefaultIdType? lotNumberId = null,
        DateTime? receiptDate = null,
        DateTime? manufactureDate = null,
        DateTime? warrantyExpirationDate = null,
        string? externalReference = null,
        string? notes = null)
    {
        return new SerialNumber(
            DefaultIdType.NewGuid(),
            serialNumberValue,
            itemId,
            warehouseId,
            warehouseLocationId,
            binId,
            lotNumberId,
            receiptDate ?? DateTime.UtcNow,
            manufactureDate,
            warrantyExpirationDate,
            externalReference,
            notes);
    }

    public SerialNumber UpdateStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status)) throw new ArgumentException("Status is required", nameof(status));
        
        var validStatuses = new[] { "Available", "Allocated", "Shipped", "Sold", "Defective", "Returned", "InRepair", "Scrapped" };
        if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException($"Status must be one of: {string.Join(", ", validStatuses)}", nameof(status));

        Status = status;
        QueueDomainEvent(new SerialNumberUpdated { SerialNumber = this });
        return this;
    }

    public SerialNumber UpdateLocation(
        DefaultIdType? warehouseId,
        DefaultIdType? warehouseLocationId,
        DefaultIdType? binId)
    {
        bool isUpdated = false;

        if (WarehouseId != warehouseId)
        {
            WarehouseId = warehouseId;
            isUpdated = true;
        }

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

        if (isUpdated)
        {
            QueueDomainEvent(new SerialNumberUpdated { SerialNumber = this });
        }

        return this;
    }

    public SerialNumber AddNotes(string additionalNotes)
    {
        if (string.IsNullOrWhiteSpace(additionalNotes)) return this;

        Notes = string.IsNullOrWhiteSpace(Notes)
            ? additionalNotes
            : $"{Notes}\n{DateTime.UtcNow:yyyy-MM-dd HH:mm}: {additionalNotes}";

        if (Notes.Length > 1000) Notes = Notes.Substring(Notes.Length - 1000);

        QueueDomainEvent(new SerialNumberUpdated { SerialNumber = this });
        return this;
    }

    /// <summary>
    /// Updates the name and description fields.
    /// </summary>
    public SerialNumber UpdateDetails(string? name, string? description)
    {
        if (name?.Length > 1024) throw new ArgumentException("Name must not exceed 1024 characters", nameof(name));
        if (description?.Length > 2048) throw new ArgumentException("Description must not exceed 2048 characters", nameof(description));

        if (!string.IsNullOrWhiteSpace(name)) Name = name;
        Description = description;
        QueueDomainEvent(new SerialNumberUpdated { SerialNumber = this });
        return this;
    }

    public bool IsWarrantyValid() =>
        WarrantyExpirationDate.HasValue && WarrantyExpirationDate.Value > DateTime.UtcNow;
}
