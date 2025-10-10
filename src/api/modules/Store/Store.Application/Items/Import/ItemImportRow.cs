namespace FSH.Starter.WebApi.Store.Application.Items.Import;

/// <summary>
/// Represents a single row of data from an Item import file.
/// Contains all fields that can be imported for an Item entity.
/// </summary>
public sealed class ItemImportRow
{
    /// <summary>
    /// Gets or sets the name of the item.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the item.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the SKU (Stock Keeping Unit) of the item.
    /// </summary>
    public string? Sku { get; set; }

    /// <summary>
    /// Gets or sets the barcode of the item.
    /// </summary>
    public string? Barcode { get; set; }

    /// <summary>
    /// Gets or sets the selling price of the item.
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Gets or sets the cost of the item.
    /// </summary>
    public decimal? Cost { get; set; }

    /// <summary>
    /// Gets or sets the minimum stock level.
    /// </summary>
    public int? MinimumStock { get; set; }

    /// <summary>
    /// Gets or sets the maximum stock level.
    /// </summary>
    public int? MaximumStock { get; set; }

    /// <summary>
    /// Gets or sets the current stock level.
    /// </summary>
    public int? CurrentStock { get; set; }

    /// <summary>
    /// Gets or sets the reorder point.
    /// </summary>
    public int? ReorderPoint { get; set; }

    /// <summary>
    /// Gets or sets whether the item is perishable.
    /// </summary>
    public bool? IsPerishable { get; set; }

    /// <summary>
    /// Gets or sets the expiry date for perishable items.
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the brand of the item.
    /// </summary>
    public string? Brand { get; set; }

    /// <summary>
    /// Gets or sets the manufacturer of the item.
    /// </summary>
    public string? Manufacturer { get; set; }

    /// <summary>
    /// Gets or sets the weight of the item.
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// Gets or sets the weight unit (kg, g, lb, oz, etc.).
    /// </summary>
    public string? WeightUnit { get; set; }

    /// <summary>
    /// Gets or sets the category ID of the item.
    /// </summary>
    public DefaultIdType? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the supplier ID of the item.
    /// </summary>
    public DefaultIdType? SupplierId { get; set; }

    /// <summary>
    /// Gets or sets the warehouse location ID of the item.
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; set; }
}

