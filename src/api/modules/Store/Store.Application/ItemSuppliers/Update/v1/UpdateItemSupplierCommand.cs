namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Update.v1;

/// <summary>
/// Command to update an existing item-supplier relationship.
/// </summary>
public record UpdateItemSupplierCommand : IRequest<UpdateItemSupplierResponse>
{
    /// <summary>
    /// Gets or sets the item-supplier relationship identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    [DefaultValue("Item-supplier relationship")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the item identifier.
    /// </summary>
    public DefaultIdType ItemId { get; set; }

    /// <summary>
    /// Gets or sets the supplier identifier.
    /// </summary>
    public DefaultIdType SupplierId { get; set; }

    /// <summary>
    /// Gets or sets the supplier's part number.
    /// </summary>
    [DefaultValue("SUP-PART-12345")]
    public string? SupplierPartNumber { get; set; }

    /// <summary>
    /// Gets or sets the unit cost from this supplier.
    /// </summary>
    [DefaultValue(15.50)]
    public decimal? UnitCost { get; set; }

    /// <summary>
    /// Gets or sets the lead time in days.
    /// </summary>
    [DefaultValue(7)]
    public int? LeadTimeDays { get; set; }

    /// <summary>
    /// Gets or sets the minimum order quantity.
    /// </summary>
    [DefaultValue(10)]
    public int? MinimumOrderQuantity { get; set; }

    /// <summary>
    /// Gets or sets the packaging quantity.
    /// </summary>
    [DefaultValue(12)]
    public int? PackagingQuantity { get; set; }

    /// <summary>
    /// Gets or sets whether this is the preferred supplier.
    /// </summary>
    [DefaultValue(false)]
    public bool? IsPreferred { get; set; }

    /// <summary>
    /// Gets or sets whether this supplier is active.
    /// </summary>
    [DefaultValue(true)]
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets the reliability rating (0-100).
    /// </summary>
    [DefaultValue(null)]
    public decimal? ReliabilityRating { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [DefaultValue(null)]
    public string? Notes { get; set; }
}
