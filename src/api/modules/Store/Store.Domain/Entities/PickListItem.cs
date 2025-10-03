namespace Store.Domain.Entities;

/// <summary>
/// Represents a single line item on a pick list specifying what to pick, where to pick from, and quantity.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define specific pick tasks with location and quantity details.
/// - Track picked vs. required quantities for accuracy.
/// - Support short picks with variance reporting.
/// - Enable serial number and lot number verification during picking.
/// - Track pick completion time for productivity analysis.
/// 
/// Business rules:
/// - Quantity to pick must be positive
/// - Picked quantity cannot exceed quantity to pick without approval
/// - Serial tracked items require serial number scanning
/// - Lot tracked items require lot number verification
/// </remarks>
public sealed class PickListItem : AuditableEntity
{
    /// <summary>
    /// Parent pick list identifier.
    /// </summary>
    public DefaultIdType PickListId { get; private set; }

    /// <summary>
    /// Item to be picked.
    /// </summary>
    public DefaultIdType ItemId { get; private set; }

    /// <summary>
    /// Bin location to pick from.
    /// </summary>
    public DefaultIdType? BinId { get; private set; }

    /// <summary>
    /// Lot number to pick (for lot-tracked items).
    /// </summary>
    public DefaultIdType? LotNumberId { get; private set; }

    /// <summary>
    /// Serial number to pick (for serial-tracked items).
    /// </summary>
    public DefaultIdType? SerialNumberId { get; private set; }

    /// <summary>
    /// Quantity required to pick.
    /// </summary>
    public int QuantityToPick { get; private set; }

    /// <summary>
    /// Quantity actually picked.
    /// </summary>
    public int QuantityPicked { get; private set; }

    /// <summary>
    /// Line status: Pending, Picked, Short, Substituted.
    /// </summary>
    public string Status { get; private set; } = "Pending";

    /// <summary>
    /// Sequence number for pick path optimization.
    /// </summary>
    public int SequenceNumber { get; private set; }

    /// <summary>
    /// Optional notes or pick instructions.
    /// Max length: 500.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Time when item was picked.
    /// </summary>
    public DateTime? PickedDate { get; private set; }

    /// <summary>
    /// Navigation property to item.
    /// </summary>
    public Item Item { get; private set; } = default!;

    /// <summary>
    /// Navigation property to bin.
    /// </summary>
    public Bin? Bin { get; private set; }

    /// <summary>
    /// Navigation property to lot number.
    /// </summary>
    public LotNumber? LotNumber { get; private set; }

    /// <summary>
    /// Navigation property to serial number.
    /// </summary>
    public SerialNumber? SerialNumber { get; private set; }

    private PickListItem() { }

    private PickListItem(
        DefaultIdType id,
        DefaultIdType pickListId,
        DefaultIdType itemId,
        DefaultIdType? binId,
        DefaultIdType? lotNumberId,
        DefaultIdType? serialNumberId,
        int quantityToPick,
        string? notes)
    {
        if (pickListId == Guid.Empty) throw new ArgumentException("PickListId is required", nameof(pickListId));
        if (itemId == Guid.Empty) throw new ArgumentException("ItemId is required", nameof(itemId));
        if (quantityToPick <= 0) throw new ArgumentException("QuantityToPick must be positive", nameof(quantityToPick));
        if (notes is { Length: > 500 }) throw new ArgumentException("Notes must not exceed 500 characters", nameof(notes));

        Id = id;
        PickListId = pickListId;
        ItemId = itemId;
        BinId = binId;
        LotNumberId = lotNumberId;
        SerialNumberId = serialNumberId;
        QuantityToPick = quantityToPick;
        QuantityPicked = 0;
        Status = "Pending";
        Notes = notes;
    }

    public static PickListItem Create(
        DefaultIdType pickListId,
        DefaultIdType itemId,
        DefaultIdType? binId,
        DefaultIdType? lotNumberId,
        DefaultIdType? serialNumberId,
        int quantityToPick,
        string? notes = null)
    {
        return new PickListItem(
            DefaultIdType.NewGuid(),
            pickListId,
            itemId,
            binId,
            lotNumberId,
            serialNumberId,
            quantityToPick,
            notes);
    }

    public PickListItem RecordPick(int quantityPicked)
    {
        if (quantityPicked <= 0) throw new ArgumentException("Quantity picked must be positive", nameof(quantityPicked));
        if (quantityPicked > QuantityToPick) throw new InvalidOperationException("Quantity picked cannot exceed quantity to pick");

        QuantityPicked = quantityPicked;
        PickedDate = DateTime.UtcNow;
        Status = quantityPicked == QuantityToPick ? "Picked" : "Short";

        return this;
    }

    public PickListItem SetSequence(int sequenceNumber)
    {
        if (sequenceNumber < 0) throw new ArgumentException("Sequence number cannot be negative", nameof(sequenceNumber));
        SequenceNumber = sequenceNumber;
        return this;
    }

    public bool IsShortPick() => Status == "Short" || (QuantityPicked > 0 && QuantityPicked < QuantityToPick);

    public int GetVariance() => QuantityPicked - QuantityToPick;
}
