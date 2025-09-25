namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Import.v1;

using FSH.Starter.WebApi.Store.Application.GroceryItems.Import;
using FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

/// <summary>
/// Imports grocery items from an Excel file and returns the count of successfully created items.
/// </summary>
public sealed class ImportGroceryItemsHandler(
    ILogger<ImportGroceryItemsHandler> logger,
    IGroceryItemImportParser parser,
    [FromKeyedServices("store:grocery-items")] IRepository<GroceryItem> repository,
    [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> readRepository)
    : IRequestHandler<ImportGroceryItemsCommand, int>
{
    public async Task<int> Handle(ImportGroceryItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.File);

        var rows = await parser.ParseAsync(request.File, cancellationToken).ConfigureAwait(false);
        if (rows.Count == 0)
        {
            logger.LogInformation("grocery import: file had no rows");
            return 0;
        }

        int imported = 0;

        foreach (var row in rows)
        {
            try
            {
                // Strict validations mirroring domain rules preemptively
                if (string.IsNullOrWhiteSpace(row.Name)) continue;
                if (string.IsNullOrWhiteSpace(row.Sku)) continue;
                if (string.IsNullOrWhiteSpace(row.Barcode)) continue;

                var price = row.Price ?? 0m;
                var cost = row.Cost ?? 0m;
                if (price < 0m || cost < 0m || price < cost) continue;

                var min = row.MinimumStock ?? 0;
                var max = row.MaximumStock ?? 0;
                var curr = row.CurrentStock ?? 0;
                var reorder = row.ReorderPoint ?? 0;
                if (min < 0 || max <= 0 || min > max) continue;
                if (curr < 0 || curr > max) continue;
                if (reorder < 0 || reorder > max) continue;

                var weight = row.Weight ?? 0m;
                if (weight < 0m) continue;
                if (weight > 0 && string.IsNullOrWhiteSpace(row.WeightUnit)) continue;

                var isPerishable = row.IsPerishable ?? false;
                if (isPerishable)
                {
                    if (!row.ExpiryDate.HasValue) continue;
                    if (row.ExpiryDate.Value <= DateTime.UtcNow.Date) continue;
                }

                // Require Category and Supplier per domain defaults
                if (!row.CategoryId.HasValue || row.CategoryId.Value == Guid.Empty) continue;
                if (!row.SupplierId.HasValue || row.SupplierId.Value == Guid.Empty) continue;

                // Duplicate checks
                var existingSku = await readRepository.FirstOrDefaultAsync(new GroceryItemBySkuSpec(row.Sku!), cancellationToken).ConfigureAwait(false);
                if (existingSku is not null) continue;
                var existingBarcode = await readRepository.FirstOrDefaultAsync(new GroceryItemByBarcodeSpec(row.Barcode!), cancellationToken).ConfigureAwait(false);
                if (existingBarcode is not null) continue;

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
                // Skip invalid rows; log and continue
                logger.LogWarning(ex, "grocery import: skipped row due to error");
            }
        }

        logger.LogInformation("grocery import: imported {Count} items", imported);
        return imported;
    }
}

