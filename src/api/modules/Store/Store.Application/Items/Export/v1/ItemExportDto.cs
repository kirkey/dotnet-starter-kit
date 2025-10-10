namespace FSH.Starter.WebApi.Store.Application.Items.Export.v1;

/// <summary>
/// DTO representing an Item for Excel export.
/// Contains all relevant fields formatted for export and reporting.
/// </summary>
public sealed class ItemExportDto
{
    /// <summary>
    /// Item name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Item description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Stock keeping unit (SKU).
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Product barcode.
    /// </summary>
    public string Barcode { get; set; } = string.Empty;

    /// <summary>
    /// Unit selling price.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Supplier cost.
    /// </summary>
    public decimal Cost { get; set; }

    /// <summary>
    /// Profit margin percentage.
    /// </summary>
    public decimal ProfitMargin { get; set; }

    /// <summary>
    /// Minimum stock level.
    /// </summary>
    public int MinimumStock { get; set; }

    /// <summary>
    /// Maximum stock level.
    /// </summary>
    public int MaximumStock { get; set; }

    /// <summary>
    /// Reorder point.
    /// </summary>
    public int ReorderPoint { get; set; }

    /// <summary>
    /// Reorder quantity.
    /// </summary>
    public int ReorderQuantity { get; set; }

    /// <summary>
    /// Lead time in days.
    /// </summary>
    public int LeadTimeDays { get; set; }

    /// <summary>
    /// Whether the item is perishable.
    /// </summary>
    public bool IsPerishable { get; set; }

    /// <summary>
    /// Whether serial tracking is enabled.
    /// </summary>
    public bool IsSerialTracked { get; set; }

    /// <summary>
    /// Whether lot tracking is enabled.
    /// </summary>
    public bool IsLotTracked { get; set; }

    /// <summary>
    /// Shelf life in days.
    /// </summary>
    public int? ShelfLifeDays { get; set; }

    /// <summary>
    /// Brand name.
    /// </summary>
    public string Brand { get; set; } = string.Empty;

    /// <summary>
    /// Manufacturer name.
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Item weight.
    /// </summary>
    public decimal Weight { get; set; }

    /// <summary>
    /// Weight unit of measure.
    /// </summary>
    public string WeightUnit { get; set; } = string.Empty;

    /// <summary>
    /// Category name.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Supplier name.
    /// </summary>
    public string Supplier { get; set; } = string.Empty;

    /// <summary>
    /// Unit of measure.
    /// </summary>
    public string UnitOfMeasure { get; set; } = string.Empty;

    /// <summary>
    /// Whether the item is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Date the item was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// User who created the item.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;
}

