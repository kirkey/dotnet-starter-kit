namespace FSH.Starter.WebApi.Store.Application.LotNumbers.Update.v1;

/// <summary>
/// Command to update an existing lot number.
/// </summary>
public record UpdateLotNumberCommand : IRequest<UpdateLotNumberResponse>
{
    /// <summary>
    /// Gets or sets the lot number identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the lot number name.
    /// </summary>
    [DefaultValue("Lot Batch 2025-001")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the lot number description.
    /// </summary>
    [DefaultValue("Lot description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the unique lot/batch code.
    /// </summary>
    [DefaultValue("LOT-2025-001")]
    public string LotCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the item identifier this lot belongs to.
    /// </summary>
    public DefaultIdType ItemId { get; set; }

    /// <summary>
    /// Gets or sets the supplier identifier.
    /// </summary>
    public DefaultIdType? SupplierId { get; set; }

    /// <summary>
    /// Gets or sets the manufacture date.
    /// </summary>
    [DefaultValue(null)]
    public DateTime? ManufactureDate { get; set; }

    /// <summary>
    /// Gets or sets the expiration date.
    /// </summary>
    [DefaultValue(null)]
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets the receipt date.
    /// </summary>
    [DefaultValue("2025-10-04")]
    public DateTime? ReceiptDate { get; set; }

    /// <summary>
    /// Gets or sets the quantity received.
    /// </summary>
    [DefaultValue(100)]
    public int QuantityReceived { get; set; }

    /// <summary>
    /// Gets or sets the quantity remaining.
    /// </summary>
    [DefaultValue(100)]
    public int QuantityRemaining { get; set; }

    /// <summary>
    /// Gets or sets the status (Active, Expired, Quarantine, Recalled).
    /// </summary>
    [DefaultValue("Active")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets quality notes.
    /// </summary>
    [DefaultValue(null)]
    public string? QualityNotes { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [DefaultValue(null)]
    public string? Notes { get; set; }
}

