namespace FSH.Starter.WebApi.Store.Application.Bins.Update.v1;

/// <summary>
/// Command to update an existing bin.
/// </summary>
public record UpdateBinCommand : IRequest<UpdateBinResponse>
{
    /// <summary>
    /// Gets or sets the bin identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the bin name.
    /// </summary>
    [DefaultValue("Bin A1-01")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bin description.
    /// </summary>
    [DefaultValue("Storage bin for small items")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the unique bin code.
    /// </summary>
    [DefaultValue("A1-01-01")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the warehouse location identifier.
    /// </summary>
    public DefaultIdType WarehouseLocationId { get; set; }

    /// <summary>
    /// Gets or sets the bin type (Shelf, Pallet, Floor, Rack, Drawer).
    /// </summary>
    [DefaultValue("Shelf")]
    public string BinType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the maximum capacity.
    /// </summary>
    [DefaultValue(100)]
    public decimal? Capacity { get; set; }

    /// <summary>
    /// Gets or sets the picking priority (lower = higher priority).
    /// </summary>
    [DefaultValue(0)]
    public int? Priority { get; set; }

    /// <summary>
    /// Gets or sets whether the bin is active.
    /// </summary>
    [DefaultValue(true)]
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets whether the bin can be picked from.
    /// </summary>
    [DefaultValue(true)]
    public bool? IsPickable { get; set; }

    /// <summary>
    /// Gets or sets whether the bin can receive put-away inventory.
    /// </summary>
    [DefaultValue(true)]
    public bool? IsPutable { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [DefaultValue(null)]
    public string? Notes { get; set; }
}

