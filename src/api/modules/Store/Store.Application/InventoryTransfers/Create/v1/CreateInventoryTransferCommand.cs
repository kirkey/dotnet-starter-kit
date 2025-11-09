namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Create.v1;

/// <summary>
/// Command to create a new inventory transfer.
/// </summary>
public record CreateInventoryTransferCommand : IRequest<CreateInventoryTransferResponse>
{
    /// <summary>
    /// Gets or sets the transfer name.
    /// </summary>
    [DefaultValue("Inventory Transfer")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets or sets the transfer description.
    /// </summary>
    [DefaultValue(null)]
    public string? Description { get; init; }

    /// <summary>
    /// Gets or sets the transfer number.
    /// </summary>
    [DefaultValue("TRF001")]
    public string TransferNumber { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the source warehouse identifier.
    /// </summary>
    public DefaultIdType FromWarehouseId { get; init; }

    /// <summary>
    /// Gets or sets the destination warehouse identifier.
    /// </summary>
    public DefaultIdType ToWarehouseId { get; init; }

    /// <summary>
    /// Gets or sets the source location identifier.
    /// </summary>
    [DefaultValue(null)]
    public DefaultIdType? FromLocationId { get; init; }

    /// <summary>
    /// Gets or sets the destination location identifier.
    /// </summary>
    [DefaultValue(null)]
    public DefaultIdType? ToLocationId { get; init; }

    /// <summary>
    /// Gets or sets the transfer date.
    /// </summary>
    [DefaultValue("2024-01-01")]
    public DateTime TransferDate { get; init; }

    /// <summary>
    /// Gets or sets the expected arrival date.
    /// </summary>
    [DefaultValue(null)]
    public DateTime? ExpectedArrivalDate { get; init; }

    /// <summary>
    /// Gets or sets the transfer type.
    /// </summary>
    [DefaultValue("Standard")]
    public string TransferType { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the priority.
    /// </summary>
    [DefaultValue("Normal")]
    public string Priority { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the transport method.
    /// </summary>
    [DefaultValue(null)]
    public string? TransportMethod { get; init; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [DefaultValue(null)]
    public string? Notes { get; init; }

    /// <summary>
    /// Gets or sets who requested the transfer.
    /// </summary>
    [DefaultValue(null)]
    public string? RequestedBy { get; init; }
}
