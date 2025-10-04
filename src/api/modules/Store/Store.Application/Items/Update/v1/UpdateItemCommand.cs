namespace FSH.Starter.WebApi.Store.Application.Items.Update.v1;

/// <summary>
/// Command to update an existing item.
/// </summary>
public record UpdateItemCommand : IRequest<UpdateItemResponse>
{
    /// <summary>
    /// Gets or sets the item identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Gets or sets the item name.
    /// </summary>
    [DefaultValue("Sample Item")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the item description.
    /// </summary>
    [DefaultValue("Item description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the SKU (Stock Keeping Unit).
    /// </summary>
    [DefaultValue("SKU-001")]
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the barcode.
    /// </summary>
    [DefaultValue("0123456789012")]
    public string Barcode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the category identifier.
    /// </summary>
    public DefaultIdType? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the supplier identifier.
    /// </summary>
    public DefaultIdType? SupplierId { get; set; }

    /// <summary>
    /// Gets or sets the unit price.
    /// </summary>
    [DefaultValue(24.99)]
    public decimal? UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the cost.
    /// </summary>
    [DefaultValue(15.00)]
    public decimal? Cost { get; set; }

    /// <summary>
    /// Gets or sets the minimum stock level.
    /// </summary>
    [DefaultValue(10)]
    public int? MinimumStock { get; set; }

    /// <summary>
    /// Gets or sets the maximum stock level.
    /// </summary>
    [DefaultValue(500)]
    public int? MaximumStock { get; set; }

    /// <summary>
    /// Gets or sets the reorder point.
    /// </summary>
    [DefaultValue(25)]
    public int? ReorderPoint { get; set; }

    /// <summary>
    /// Gets or sets the reorder quantity.
    /// </summary>
    [DefaultValue(100)]
    public int? ReorderQuantity { get; set; }

    /// <summary>
    /// Gets or sets the lead time in days.
    /// </summary>
    [DefaultValue(7)]
    public int? LeadTimeDays { get; set; }

    /// <summary>
    /// Gets or sets whether the item is perishable.
    /// </summary>
    [DefaultValue(false)]
    public bool? IsPerishable { get; set; }

    /// <summary>
    /// Gets or sets whether the item is serial tracked.
    /// </summary>
    [DefaultValue(false)]
    public bool? IsSerialTracked { get; set; }

    /// <summary>
    /// Gets or sets whether the item is lot tracked.
    /// </summary>
    [DefaultValue(false)]
    public bool? IsLotTracked { get; set; }

    /// <summary>
    /// Gets or sets the shelf life in days.
    /// </summary>
    [DefaultValue(null)]
    public int? ShelfLifeDays { get; set; }

    /// <summary>
    /// Gets or sets the brand.
    /// </summary>
    [DefaultValue(null)]
    public string? Brand { get; set; }

    /// <summary>
    /// Gets or sets the manufacturer.
    /// </summary>
    [DefaultValue(null)]
    public string? Manufacturer { get; set; }

    /// <summary>
    /// Gets or sets the manufacturer part number.
    /// </summary>
    [DefaultValue(null)]
    public string? ManufacturerPartNumber { get; set; }

    /// <summary>
    /// Gets or sets the weight (use decimal, not double).
    /// </summary>
    [DefaultValue(null)]
    public decimal? Weight { get; set; }

    /// <summary>
    /// Gets or sets the weight unit.
    /// </summary>
    [DefaultValue(null)]
    public string? WeightUnit { get; set; }

    /// <summary>
    /// Gets or sets the length dimension.
    /// </summary>
    [DefaultValue(null)]
    public decimal? Length { get; set; }

    /// <summary>
    /// Gets or sets the width dimension.
    /// </summary>
    [DefaultValue(null)]
    public decimal? Width { get; set; }

    /// <summary>
    /// Gets or sets the height dimension.
    /// </summary>
    [DefaultValue(null)]
    public decimal? Height { get; set; }

    /// <summary>
    /// Gets or sets the dimension unit.
    /// </summary>
    [DefaultValue(null)]
    public string? DimensionUnit { get; set; }

    /// <summary>
    /// Gets or sets the unit of measure.
    /// </summary>
    [DefaultValue("EA")]
    public string? UnitOfMeasure { get; set; }

    /// <summary>
    /// Gets or sets whether the item is active.
    /// </summary>
    [DefaultValue(true)]
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets additional notes.
    /// </summary>
    [DefaultValue(null)]
    public string? Notes { get; set; }
}

