namespace Store.Domain.Entities;

/// <summary>
/// Represents a single line item on a put-away task specifying what to put away and where.
/// </summary>
public sealed class PutAwayTaskItem : AuditableEntity
{
    /// <summary>
    /// Parent put-away task identifier.
    /// </summary>
    public DefaultIdType PutAwayTaskId { get; private set; }

    /// <summary>
    /// Item to be put away.
    /// </summary>
    public DefaultIdType ItemId { get; private set; }

    /// <summary>
    /// Destination bin for put-away.
    /// </summary>
    public DefaultIdType ToBinId { get; private set; }

    /// <summary>
    /// Lot number (for lot-tracked items).
    /// </summary>
    public DefaultIdType? LotNumberId { get; private set; }

    /// <summary>
    /// Serial number (for serial-tracked items).
    /// </summary>
    public DefaultIdType? SerialNumberId { get; private set; }

    /// <summary>
    /// Quantity to put away.
    /// </summary>
    public int QuantityToPutAway { get; private set; }

    /// <summary>
    /// Quantity actually put away.
    /// </summary>
    public int QuantityPutAway { get; private set; }

    /// <summary>
    /// Line status: Pending, PutAway, Exception.
    /// </summary>
    public string Status { get; private set; } = "Pending";

    /// <summary>
    /// Sequence number for optimal put-away path.
    /// </summary>
    public int SequenceNumber { get; private set; }

    /// <summary>
    /// Time when item was put away.
    /// </summary>
    public DateTime? PutAwayDate { get; private set; }

    /// <summary>
    /// Navigation property to item.
    /// </summary>
    public Item Item { get; private set; } = default!;

    /// <summary>
    /// Navigation property to destination bin.
    /// </summary>
    public Bin ToBin { get; private set; } = default!;

    /// <summary>
    /// Navigation property to lot number.
    /// </summary>
    public LotNumber? LotNumber { get; private set; }

    /// <summary>
    /// Navigation property to serial number.
    /// </summary>
    public SerialNumber? SerialNumber { get; private set; }

    private PutAwayTaskItem() { }

    private PutAwayTaskItem(
        DefaultIdType id,
        DefaultIdType putAwayTaskId,
        DefaultIdType itemId,
        DefaultIdType toBinId,
        DefaultIdType? lotNumberId,
        DefaultIdType? serialNumberId,
        int quantityToPutAway,
        string? notes)
    {
        if (putAwayTaskId == DefaultIdType.Empty) throw new ArgumentException("PutAwayTaskId is required", nameof(putAwayTaskId));
        if (itemId == DefaultIdType.Empty) throw new ArgumentException("ItemId is required", nameof(itemId));
        if (toBinId == DefaultIdType.Empty) throw new ArgumentException("ToBinId is required", nameof(toBinId));
        if (quantityToPutAway <= 0) throw new ArgumentException("QuantityToPutAway must be positive", nameof(quantityToPutAway));

        Id = id;
        PutAwayTaskId = putAwayTaskId;
        ItemId = itemId;
        ToBinId = toBinId;
        LotNumberId = lotNumberId;
        SerialNumberId = serialNumberId;
        QuantityToPutAway = quantityToPutAway;
        QuantityPutAway = 0;
        Status = "Pending";
        Notes = notes;
    }

    public static PutAwayTaskItem Create(
        DefaultIdType putAwayTaskId,
        DefaultIdType itemId,
        DefaultIdType toBinId,
        DefaultIdType? lotNumberId,
        DefaultIdType? serialNumberId,
        int quantityToPutAway,
        string? notes = null)
    {
        return new PutAwayTaskItem(
            DefaultIdType.NewGuid(),
            putAwayTaskId,
            itemId,
            toBinId,
            lotNumberId,
            serialNumberId,
            quantityToPutAway,
            notes);
    }

    public PutAwayTaskItem RecordPutAway(int quantityPutAway)
    {
        if (quantityPutAway <= 0) throw new ArgumentException("Quantity put away must be positive", nameof(quantityPutAway));
        if (quantityPutAway != QuantityToPutAway) throw new InvalidOperationException("Quantity put away must match quantity to put away");

        QuantityPutAway = quantityPutAway;
        PutAwayDate = DateTime.UtcNow;
        Status = "PutAway";

        return this;
    }

    public PutAwayTaskItem SetSequence(int sequenceNumber)
    {
        if (sequenceNumber < 0) throw new ArgumentException("Sequence number cannot be negative", nameof(sequenceNumber));
        SequenceNumber = sequenceNumber;
        return this;
    }
}
