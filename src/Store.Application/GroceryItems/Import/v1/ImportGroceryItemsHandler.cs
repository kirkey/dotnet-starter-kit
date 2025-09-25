using FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Import.v1;

/// <summary>
/// Imports grocery items from an Excel file and returns detailed results including success/failure counts.
/// </summary>
public sealed class ImportGroceryItemsHandler(
    ILogger<ImportGroceryItemsHandler> logger,
    IGroceryItemImportParser parser,
    [FromKeyedServices("store:grocery-items")] IRepository<GroceryItem> repository,
    [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> readRepository)
    : IRequestHandler<ImportGroceryItemsCommand, ImportGroceryItemsResponse>
{
    public async Task<ImportGroceryItemsResponse> Handle(ImportGroceryItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.File);

        var rows = await parser.ParseAsync(request.File, cancellationToken).ConfigureAwait(false);
        if (rows.Count == 0)
        {
            logger.LogInformation("grocery import: file had no rows");
            return ImportGroceryItemsResponse.Create(0, 0);
        }

        int imported = 0;
        var errors = new List<string>();
        int rowIndex = 1;

        foreach (var row in rows)
        {
            try
            {
                // Strict validations mirroring domain rules preemptively
                if (string.IsNullOrWhiteSpace(row.Name))
                {
                    errors.Add($"Row {rowIndex}: Name is required");
                    rowIndex++;
                    continue;
                }
                if (string.IsNullOrWhiteSpace(row.Sku))
                {
                    errors.Add($"Row {rowIndex}: SKU is required");
                    rowIndex++;
                    continue;
                }
                if (string.IsNullOrWhiteSpace(row.Barcode))
                {
                    errors.Add($"Row {rowIndex}: Barcode is required");
                    rowIndex++;
                    continue;
                }

                var price = row.Price ?? 0m;
                var cost = row.Cost ?? 0m;
                if (price < 0m || cost < 0m || price < cost)
                {
                    errors.Add($"Row {rowIndex}: Invalid pricing - Price must be >= Cost and both must be >= 0");
                    rowIndex++;
                    continue;
                }

                var min = row.MinimumStock ?? 0;
                var max = row.MaximumStock ?? 0;
                var curr = row.CurrentStock ?? 0;
                var reorder = row.ReorderPoint ?? 0;
                if (min < 0 || max <= 0 || min > max)
                {
                    errors.Add($"Row {rowIndex}: Invalid stock levels - MinimumStock must be >= 0, MaximumStock must be > 0, and MinimumStock <= MaximumStock");
                    rowIndex++;
                    continue;
                }
                if (curr < 0 || curr > max)
                {
                    errors.Add($"Row {rowIndex}: CurrentStock must be between 0 and MaximumStock");
                    rowIndex++;
                    continue;
                }
                if (reorder < 0 || reorder > max)
                {
                    errors.Add($"Row {rowIndex}: ReorderPoint must be between 0 and MaximumStock");
                    rowIndex++;
                    continue;
                }

                var weight = row.Weight ?? 0m;
                if (weight < 0m)
                {
                    errors.Add($"Row {rowIndex}: Invalid weight - Weight must be >= 0");
                    rowIndex++;
                    continue;
                }
                if (weight > 0 && string.IsNullOrWhiteSpace(row.WeightUnit))
                {
                    errors.Add($"Row {rowIndex}: WeightUnit is required when Weight is specified");
                    rowIndex++;
                    continue;
                }

                var isPerishable = row.IsPerishable ?? false;
                if (isPerishable)
                {
                    if (!row.ExpiryDate.HasValue)
                    {
                        errors.Add($"Row {rowIndex}: ExpiryDate is required for perishable items");
                        rowIndex++;
                        continue;
                    }
                    if (row.ExpiryDate.Value <= DateTime.UtcNow.Date)
                    {
                        errors.Add($"Row {rowIndex}: ExpiryDate must be in the future for perishable items");
                        rowIndex++;
                        continue;
                    }
                }

                // Require Category and Supplier per domain defaults
                if (!row.CategoryId.HasValue || row.CategoryId.Value == Guid.Empty)
                {
                    errors.Add($"Row {rowIndex}: CategoryId is required");
                    rowIndex++;
                    continue;
                }
                if (!row.SupplierId.HasValue || row.SupplierId.Value == Guid.Empty)
                {
                    errors.Add($"Row {rowIndex}: SupplierId is required");
                    rowIndex++;
                    continue;
                }

                // Duplicate checks
                var existingSku = await readRepository.FirstOrDefaultAsync(new GroceryItemBySkuSpec(row.Sku!), cancellationToken).ConfigureAwait(false);
                if (existingSku is not null)
                {
                    errors.Add($"Row {rowIndex}: SKU already exists");
                    rowIndex++;
                    continue;
                }
                var existingBarcode = await readRepository.FirstOrDefaultAsync(new GroceryItemByBarcodeSpec(row.Barcode!), cancellationToken).ConfigureAwait(false);
                if (existingBarcode is not null)
                {
                    errors.Add($"Row {rowIndex}: Barcode already exists");
                    rowIndex++;
                    continue;
                }

                var entity = GroceryItem.Create(
                    row.Name!,
                    row.Description,
                    row.Sku!,
                    row.Barcode!,
                    price,
                    cost,
                    min,
                    max,
                    curr,
                    reorder,
                    isPerishable,
                    row.ExpiryDate,
                    row.Brand,
                    row.Manufacturer,
                    weight,
                    row.WeightUnit,
                    row.CategoryId,
                    row.SupplierId,
                    row.WarehouseLocationId);

                await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
                imported++;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "grocery import: skipped row due to error");
                rowIndex++;
            }
        }

        logger.LogInformation("grocery import: imported {Count} items with {ErrorCount} errors", imported, errors.Count);
        return ImportGroceryItemsResponse.Create(rows.Count, imported, errors);
    }
}
