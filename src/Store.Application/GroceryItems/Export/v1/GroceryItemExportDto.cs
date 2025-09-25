namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Export.v1;

/// <summary>
/// Data transfer object for exporting grocery items to Excel.
/// Contains all relevant fields formatted for export display.
/// </summary>
public sealed class GroceryItemExportDto
{
    /// <summary>
    /// Product name for display
    /// </summary>
    public string Name { get; init; } = default!;

    /// <summary>
    /// Product description
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Stock keeping unit identifier
    /// </summary>
    public string Sku { get; init; } = default!;

    /// <summary>
    /// Product barcode for scanning
    /// </summary>
    public string Barcode { get; init; } = default!;

    /// <summary>
    /// Selling price per unit
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// Supplier cost per unit
    /// </summary>
    public decimal Cost { get; init; }

    /// <summary>
    /// Minimum stock level (safety stock)
    /// </summary>
    public int MinimumStock { get; init; }

    /// <summary>
    /// Maximum allowed stock level
    /// </summary>
    public int MaximumStock { get; init; }

    /// <summary>
    /// Current available stock quantity
    /// </summary>
    public int CurrentStock { get; init; }

    /// <summary>
    /// Reorder point trigger level
    /// </summary>
    public int ReorderPoint { get; init; }

    /// <summary>
    /// Whether item is perishable
    /// </summary>
    public bool IsPerishable { get; init; }

    /// <summary>
    /// Expiry date for perishable items
    /// </summary>
    public DateTime? ExpiryDate { get; init; }

    /// <summary>
    /// Product brand name
    /// </summary>
    public string? Brand { get; init; }

    /// <summary>
    /// Manufacturer name
    /// </summary>
    public string? Manufacturer { get; init; }

    /// <summary>
    /// Item weight for shipping/handling
    /// </summary>
    public decimal Weight { get; init; }

    /// <summary>
    /// Weight unit measurement (kg, lbs, oz, etc.)
    /// </summary>
    public string? WeightUnit { get; init; }

    /// <summary>
    /// Category ID reference
    /// </summary>
    public DefaultIdType? CategoryId { get; init; }

    /// <summary>
    /// Supplier ID reference
    /// </summary>
    public DefaultIdType? SupplierId { get; init; }

    /// <summary>
    /// Warehouse location ID reference
    /// </summary>
    public DefaultIdType? WarehouseLocationId { get; init; }

    /// <summary>
    /// Whether the item is currently active
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// Date when item was created
    /// </summary>
    public DateTime CreatedOn { get; init; }

    /// <summary>
    /// Date when item was last modified
    /// </summary>
    public DateTime? LastModifiedOn { get; init; }
}
