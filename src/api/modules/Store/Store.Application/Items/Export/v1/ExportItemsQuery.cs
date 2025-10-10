using FSH.Framework.Core.Storage.Queries;

namespace FSH.Starter.WebApi.Store.Application.Items.Export.v1;

/// <summary>
/// Query for exporting Items to Excel format.
/// </summary>
public sealed record ExportItemsQuery : IRequest<ExportResponse>
{
    /// <summary>
    /// Filter criteria for the items to export.
    /// </summary>
    public ItemExportFilter? Filter { get; init; }

    /// <summary>
    /// The worksheet name. Defaults to "Items".
    /// </summary>
    public string SheetName { get; init; } = "Items";
}

/// <summary>
/// Filter DTO for exporting items with various criteria.
/// </summary>
public sealed class ItemExportFilter
{
    /// <summary>
    /// Search term for name, SKU, or barcode.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Filter by category ID.
    /// </summary>
    public DefaultIdType? CategoryId { get; set; }

    /// <summary>
    /// Filter by supplier ID.
    /// </summary>
    public DefaultIdType? SupplierId { get; set; }

    /// <summary>
    /// Filter by perishable status.
    /// </summary>
    public bool? IsPerishable { get; set; }

    /// <summary>
    /// Filter by minimum price.
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Filter by maximum price.
    /// </summary>
    public decimal? MaxPrice { get; set; }
}

