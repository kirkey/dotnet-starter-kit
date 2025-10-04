namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Update.v1;

/// <summary>
/// Command to update an existing inventory transfer.
/// </summary>
public record UpdateInventoryTransferCommand : IRequest<UpdateInventoryTransferResponse>
{
    /// <summary>
    /// Gets or sets the inventory transfer identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the inventory transfer.
    /// </summary>
    [DefaultValue("Transfer to Main Warehouse")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the inventory transfer.
    /// </summary>
    [DefaultValue("Transfer items between warehouses")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the transfer number.
    /// </summary>
    [DefaultValue("TRF001")]
    public string TransferNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the transfer type (Standard, Emergency, Replenishment, Return).
    /// </summary>
    [DefaultValue("Standard")]
    public string TransferType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source warehouse identifier.
    /// </summary>
    public DefaultIdType FromWarehouseId { get; set; }

    /// <summary>
    /// Gets or sets the destination warehouse identifier.
    /// </summary>
    public DefaultIdType ToWarehouseId { get; set; }

    /// <summary>
    /// Gets or sets the transfer date.
    /// </summary>
    [DefaultValue("2024-01-01")]
    public DateTime TransferDate { get; set; }

    /// <summary>
    /// Gets or sets the expected arrival date.
    /// </summary>
    [DefaultValue(null)]
    public DateTime? ExpectedArrivalDate { get; set; }

    /// <summary>
    /// Gets or sets the status of the transfer (Pending, Draft, Approved, InTransit, Completed, Cancelled).
    /// </summary>
    [DefaultValue("Pending")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the priority level (Low, Normal, High, Urgent).
    /// </summary>
    [DefaultValue("Normal")]
    public string Priority { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the transport method.
    /// </summary>
    [DefaultValue(null)]
    public string? TransportMethod { get; set; }

    /// <summary>
    /// Gets or sets the tracking number.
    /// </summary>
    [DefaultValue(null)]
    public string? TrackingNumber { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [DefaultValue(null)]
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the reason for the transfer.
    /// </summary>
    [DefaultValue("Inventory Rebalancing")]
    public string Reason { get; set; } = string.Empty;
}
