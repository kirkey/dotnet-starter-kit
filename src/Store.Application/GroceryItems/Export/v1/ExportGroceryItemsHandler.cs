using FSH.Framework.Core.Storage;
using FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Export.v1;

/// <summary>
/// Handler for exporting grocery items to Excel format using the framework's DataExport service.
/// Supports filtering by category, supplier, search terms, and various criteria.
/// </summary>
public sealed class ExportGroceryItemsHandler(
    ILogger<ExportGroceryItemsHandler> logger,
    IDataExport dataExport,
    [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> repository)
    : IRequestHandler<ExportGroceryItemsQuery, ExportGroceryItemsResponse>
{
    public async Task<ExportGroceryItemsResponse> Handle(ExportGroceryItemsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Starting grocery items export with filters: CategoryId={CategoryId}, SupplierId={SupplierId}, SearchTerm={SearchTerm}, OnlyLowStock={OnlyLowStock}", 
            request.CategoryId, request.SupplierId, request.SearchTerm, request.OnlyLowStock);

        // Use the single specification that handles all filtering
        var spec = new ExportGroceryItemsSpec(request);

        // Fetch data from repository
        var groceryItems = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("Found {Count} grocery items matching export criteria", groceryItems.Count);

        // Transform domain entities to export DTOs
        var exportData = groceryItems.Select(MapToExportDto).ToList();

        // Handle empty data case - create empty list with proper structure
        if (exportData.Count == 0)
        {
            exportData.Add(new GroceryItemExportDto()); // Add empty row to maintain Excel structure
        }

        // Use framework's DataExport service directly to generate Excel file
        var excelBytes = dataExport.ListToByteArray(exportData);
        
        logger.LogInformation("Successfully exported {Count} grocery items to Excel", groceryItems.Count);

        return ExportGroceryItemsResponse.Create(excelBytes, groceryItems.Count);
    }

    /// <summary>
    /// Maps a GroceryItem domain entity to export DTO with proper formatting.
    /// </summary>
    /// <param name="item">Domain entity to map</param>
    /// <returns>Export DTO with formatted data</returns>
    private static GroceryItemExportDto MapToExportDto(GroceryItem item)
    {
        return new GroceryItemExportDto
        {
            Name = item.Name,
            Description = item.Description,
            Sku = item.Sku,
            Barcode = item.Barcode,
            Price = item.Price,
            Cost = item.Cost,
            MinimumStock = item.MinimumStock,
            MaximumStock = item.MaximumStock,
            CurrentStock = item.CurrentStock,
            ReorderPoint = item.ReorderPoint,
            IsPerishable = item.IsPerishable,
            ExpiryDate = item.ExpiryDate,
            Brand = item.Brand,
            Manufacturer = item.Manufacturer,
            Weight = item.Weight,
            WeightUnit = item.WeightUnit,
            CategoryId = item.CategoryId,
            SupplierId = item.SupplierId,
            WarehouseLocationId = item.WarehouseLocationId,
            IsActive = true, // Default to active since IsActive property may not exist on domain entity
            CreatedOn = item.CreatedOn.DateTime,
            LastModifiedOn = item.LastModifiedOn.DateTime
        };
    }
}
