namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Import;

/// <summary>
/// Strongly-typed row parsed from the import Excel file for GroceryItems.
/// </summary>
public sealed class GroceryItemImportRow
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public string? Sku { get; init; }
    public string? Barcode { get; init; }
    public decimal? Price { get; init; }
    public decimal? Cost { get; init; }
    public int? MinimumStock { get; init; }
    public int? MaximumStock { get; init; }
    public int? CurrentStock { get; init; }
    public int? ReorderPoint { get; init; }
    public bool? IsPerishable { get; init; }
    public DateTime? ExpiryDate { get; init; }
    public string? Brand { get; init; }
    public string? Manufacturer { get; init; }
    public decimal? Weight { get; init; }
    public string? WeightUnit { get; init; }
    public DefaultIdType? CategoryId { get; init; }
    public DefaultIdType? SupplierId { get; init; }
    public DefaultIdType? WarehouseLocationId { get; init; }
}
