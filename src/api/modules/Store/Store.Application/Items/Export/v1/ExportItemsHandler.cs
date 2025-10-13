using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Queries;
using FSH.Starter.WebApi.Store.Application.Items.Specs;

namespace FSH.Starter.WebApi.Store.Application.Items.Export.v1;

/// <summary>
/// Handler for exporting Items to Excel format.
/// Uses the generic export infrastructure with custom filtering and mapping.
/// </summary>
public sealed class ExportItemsHandler(
    IDataExportService exportService,
    [FromKeyedServices("store:items")] IReadRepository<Item> repository,
    ILogger<ExportItemsHandler> logger)
    : IRequestHandler<ExportItemsQuery, ExportResponse>
{
    public async Task<ExportResponse> Handle(ExportItemsQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Starting export for Items");

        try
        {
            // Build specification from filter
            var specification = request.Filter != null 
                ? new ExportItemsSpec(request.Filter) 
                : null;

            // Fetch data from repository
            var entities = specification != null
                ? await repository.ListAsync(specification, cancellationToken)
                : await repository.ListAsync(cancellationToken);

            logger.LogInformation("Found {Count} Items to export", entities.Count);

            // Map entities to export DTOs
            var exportData = entities.Select(MapToExportDto).ToList();

            // Handle empty data case
            if (exportData.Count == 0)
            {
                exportData.Add(new ItemExportDto());
            }

            // Export to Excel
            var excelBytes = exportService.ExportToBytes(exportData, request.SheetName);

            var fileName = $"Items_Export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";
            
            logger.LogInformation("Successfully exported {Count} Items", entities.Count);

            return ExportResponse.Create(excelBytes, entities.Count, fileName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Export failed for Items");
            throw new InvalidOperationException($"Export failed: {ex.Message}", ex);
        }
    }

    private ItemExportDto MapToExportDto(Item entity)
    {
        // Calculate profit margin
        var profitMargin = entity.UnitPrice > 0 
            ? ((entity.UnitPrice - entity.Cost) / entity.UnitPrice) * 100 
            : 0;

        return new ItemExportDto
        {
            Name = entity.Name,
            Description = entity.Description ?? string.Empty,
            Sku = entity.Sku,
            Barcode = entity.Barcode,
            UnitPrice = entity.UnitPrice,
            Cost = entity.Cost,
            ProfitMargin = Math.Round(profitMargin, 2),
            MinimumStock = entity.MinimumStock,
            MaximumStock = entity.MaximumStock,
            ReorderPoint = entity.ReorderPoint,
            ReorderQuantity = entity.ReorderQuantity,
            LeadTimeDays = entity.LeadTimeDays,
            IsPerishable = entity.IsPerishable,
            IsSerialTracked = entity.IsSerialTracked,
            IsLotTracked = entity.IsLotTracked,
            ShelfLifeDays = entity.ShelfLifeDays,
            Brand = entity.Brand ?? string.Empty,
            Manufacturer = entity.Manufacturer ?? string.Empty,
            Weight = entity.Weight,
            WeightUnit = entity.WeightUnit ?? string.Empty,
            Category = entity.Category.Name,
            Supplier = entity.Supplier.Name,
            UnitOfMeasure = entity.UnitOfMeasure,
            CreatedDate = entity.CreatedOn.DateTime,
            CreatedBy = entity.CreatedBy.ToString()
        };
    }
}

