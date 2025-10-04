namespace FSH.Starter.WebApi.Store.Application.SerialNumbers.Update.v1;

/// <summary>
/// Command to update an existing serial number.
/// </summary>
public record UpdateSerialNumberCommand : IRequest<UpdateSerialNumberResponse>
{
    /// <summary>
    /// Gets or sets the serial number identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the serial number name.
    /// </summary>
    [DefaultValue("Serial Number SN-12345")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the serial number description.
    /// </summary>
    [DefaultValue("Serial number description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the unique serial number value.
    /// </summary>
    [DefaultValue("SN-12345")]
    public string SerialNumberValue { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the item identifier this serial number belongs to.
    /// </summary>
    public DefaultIdType ItemId { get; set; }

    /// <summary>
    /// Gets or sets the current warehouse identifier.
    /// </summary>
    public DefaultIdType? WarehouseId { get; set; }

    /// <summary>
    /// Gets or sets the warehouse location identifier.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; set; }

    /// <summary>
    /// Gets or sets the bin identifier.
    /// </summary>
    public DefaultIdType? BinId { get; set; }

    /// <summary>
    /// Gets or sets the lot number identifier.
    /// </summary>
    public DefaultIdType? LotNumberId { get; set; }

    /// <summary>
    /// Gets or sets the status (Available, Allocated, Shipped, Sold, Defective, Returned, InRepair, Scrapped).
    /// </summary>
    [DefaultValue("Available")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date the serial number was received into inventory.
    /// </summary>
    [DefaultValue("2025-10-04")]
    public DateTime ReceiptDate { get; set; }

    /// <summary>
    /// Gets or sets the manufacture date.
    /// </summary>
    [DefaultValue(null)]
    public DateTime? ManufactureDate { get; set; }

    /// <summary>
    /// Gets or sets the warranty expiration date.
    /// </summary>
    [DefaultValue(null)]
    public DateTime? WarrantyExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets the external reference.
    /// </summary>
    [DefaultValue(null)]
    public string? ExternalReference { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [DefaultValue(null)]
    public string? Notes { get; set; }
}

