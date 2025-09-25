namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Export.v1;

/// <summary>
/// Query to export grocery items to an Excel file.
/// Supports optional filtering by category, supplier, or search criteria.
/// </summary>
/// <param name="CategoryId">Optional category ID to filter grocery items</param>
/// <param name="SupplierId">Optional supplier ID to filter grocery items</param>
/// <param name="SearchTerm">Optional search term to filter by name, SKU, or barcode</param>
/// <param name="IncludeInactive">Whether to include inactive items in export (default: false)</param>
/// <param name="OnlyLowStock">Whether to export only items below reorder point (default: false)</param>
/// <param name="OnlyPerishable">Whether to export only perishable items (default: false)</param>
public sealed record ExportGroceryItemsQuery(
    DefaultIdType? CategoryId = null,
    DefaultIdType? SupplierId = null,
    string? SearchTerm = null,
    bool IncludeInactive = false,
    bool OnlyLowStock = false,
    bool OnlyPerishable = false) : IRequest<ExportGroceryItemsResponse>;
