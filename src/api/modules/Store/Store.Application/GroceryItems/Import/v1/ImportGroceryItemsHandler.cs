using FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;
using FSH.Starter.WebApi.Store.Application.Categories.Specs;
using FSH.Starter.WebApi.Store.Application.Suppliers.Specs;

namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Import.v1;

/// <summary>
/// Imports grocery items from an Excel file and returns the total number of successfully imported items.
/// </summary>
public sealed class ImportGroceryItemsHandler(
    ILogger<ImportGroceryItemsHandler> logger,
    IGroceryItemImportParser parser,
    [FromKeyedServices("store:grocery-items")] IRepository<GroceryItem> repository,
    [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> readRepository,
    [FromKeyedServices("store:categories")] IReadRepository<Category> categoryReadRepository,
    [FromKeyedServices("store:suppliers")] IReadRepository<Supplier> supplierReadRepository)
    : IRequestHandler<ImportGroceryItemsCommand, int>
{
    /// <summary>
    /// Handles the import command and returns the count of imported items.
    /// </summary>
    /// <param name="request">The import command containing the file to process.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of items successfully imported.</returns>
    public async Task<int> Handle(ImportGroceryItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.File);

        var rows = await parser.ParseAsync(request.File, cancellationToken).ConfigureAwait(false);
        if (rows.Count == 0)
        {
            var noRowsMsg = "grocery import: file had no rows";
            logger.LogInformation(noRowsMsg);
            Console.WriteLine(noRowsMsg);
            return 0;
        }

        // Preload default Category and Supplier to satisfy domain requirements when missing
        var defaultCategory = await categoryReadRepository.FirstOrDefaultAsync(new CategoryByCodeSpec("UNCAT"), cancellationToken).ConfigureAwait(false)
            ?? await categoryReadRepository.FirstOrDefaultAsync(new CategoryByNameSpec("Uncategorized"), cancellationToken).ConfigureAwait(false);
        var defaultSupplier = await supplierReadRepository.FirstOrDefaultAsync(new SupplierByCodeSpec("SUP001"), cancellationToken).ConfigureAwait(false);

        if (defaultCategory is null)
        {
            logger.LogWarning("grocery import: default category not found (UNCAT). Items without CategoryId will fail domain rules");
        }
        if (defaultSupplier is null)
        {
            logger.LogWarning("grocery import: default supplier not found (SUP001). Items without SupplierId will fail domain rules");
        }

        int imported = 0;
        var errors = new List<string>();

        for (int i = 0; i < rows.Count; i++)
        {
            var rowIndex = i + 1; // human-friendly
            var row = rows[i];

            try
            {
                // Minimal strict validations: Name and Sku are required per request context
                if (string.IsNullOrWhiteSpace(row.Name))
                {
                    errors.Add($"Row {rowIndex}: Name is required");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(row.Sku))
                {
                    errors.Add($"Row {rowIndex}: SKU is required");
                    continue;
                }

                // Normalize and clamp to EF constraints
                var name = row.Name.Trim();
                if (name.Length > 200) name = name[..200];

                var sku = row.Sku.Trim();
                if (sku.Length > 50) sku = sku[..50]; // EF max 50

                string? brand = string.IsNullOrWhiteSpace(row.Brand) ? null : row.Brand!.Trim();
                if (brand is { Length: > 100 }) brand = brand[..100]; // EF max 100

                string? manufacturer = string.IsNullOrWhiteSpace(row.Manufacturer) ? null : row.Manufacturer!.Trim();
                if (manufacturer is { Length: > 100 }) manufacturer = manufacturer[..100]; // EF max 100

                string? weightUnit = string.IsNullOrWhiteSpace(row.WeightUnit) ? null : row.WeightUnit!.Trim();
                if (weightUnit is { Length: > 20 }) weightUnit = weightUnit[..20];

                // Ensure unique SKU (keep existing behavior to avoid conflicts)
                var existingSku = await readRepository.FirstOrDefaultAsync(new GroceryItemBySkuSpec(sku), cancellationToken).ConfigureAwait(false);
                if (existingSku is not null)
                {
                    errors.Add($"Row {rowIndex}: SKU '{sku}' already exists");
                    continue;
                }

                // Barcode: use provided or auto-generate unique
                string barcode;
                if (!string.IsNullOrWhiteSpace(row.Barcode))
                {
                    barcode = row.Barcode!.Trim();
                    if (barcode.Length > 100) barcode = barcode[..100];

                    // if barcode exists, generate a unique one instead of failing
                    var existingBarcode = await readRepository.FirstOrDefaultAsync(new GroceryItemByBarcodeSpec(barcode), cancellationToken).ConfigureAwait(false);
                    if (existingBarcode is not null)
                    {
                        barcode = await GenerateUniqueBarcodeAsync(sku, readRepository, cancellationToken).ConfigureAwait(false);
                    }
                }
                else
                {
                    barcode = await GenerateUniqueBarcodeAsync(sku, readRepository, cancellationToken).ConfigureAwait(false);
                }

                // Defaults for required numeric fields
                var price = row.Price is >= 0m ? row.Price.Value : 0m;
                var cost = row.Cost is >= 0m ? row.Cost.Value : 0m;
                if (price < cost)
                {
                    // adjust to satisfy domain (price >= cost)
                    price = cost;
                }

                // Stock defaults satisfying domain rules
                var min = row.MinimumStock is >= 0 ? row.MinimumStock!.Value : 0;
                var max = row.MaximumStock is > 0 ? row.MaximumStock!.Value : 100;
                if (min > max) min = 0; // fallback

                var curr = row.CurrentStock is >= 0 ? Math.Min(row.CurrentStock!.Value, max) : 0;
                var reorder = row.ReorderPoint is >= 0 ? Math.Min(row.ReorderPoint!.Value, max) : 0;

                // Perishability: if invalid, disable perishability to pass domain validation
                var isPerishable = row.IsPerishable ?? false;
                DateTime? expiry = row.ExpiryDate;
                if (isPerishable && (!expiry.HasValue || expiry.Value <= DateTime.UtcNow.Date))
                {
                    isPerishable = false;
                    expiry = null;
                }

                // Weight: default to 0 if missing/invalid
                var weight = row.Weight is >= 0m ? row.Weight.Value : 0m;
                // Domain requires WeightUnit only when Weight > 0. If Weight = 0, keep whatever provided.

                // Category and Supplier fallbacks if not provided
                var categoryId = row.CategoryId.HasValue && row.CategoryId.Value != DefaultIdType.Empty
                    ? row.CategoryId
                    : defaultCategory?.Id;

                var supplierId = row.SupplierId.HasValue && row.SupplierId.Value != DefaultIdType.Empty
                    ? row.SupplierId
                    : defaultSupplier?.Id;

                if (categoryId is null)
                {
                    errors.Add($"Row {rowIndex}: No CategoryId provided and default category not found");
                    continue;
                }
                if (supplierId is null)
                {
                    errors.Add($"Row {rowIndex}: No SupplierId provided and default supplier not found");
                    continue;
                }

                var entity = GroceryItem.Create(
                    name,
                    row.Description,
                    sku,
                    barcode,
                    price,
                    cost,
                    min,
                    max,
                    curr,
                    reorder,
                    isPerishable,
                    expiry,
                    brand,
                    manufacturer,
                    weight,
                    weightUnit,
                    categoryId,
                    supplierId,
                    row.WarehouseLocationId);

                await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
                imported++;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "grocery import: skipped row {RowIndex} due to error", rowIndex);
                errors.Add($"Row {rowIndex}: {ex.Message}");
            }
        }

        // after processing all rows, print all collected errors to the console
        if (errors.Count > 0)
        {
            foreach (var err in errors)
            {
                await Console.Error.WriteLineAsync(err);
            }
        }

        logger.LogInformation("grocery import: imported {Imported} items with {ErrorCount} errors", imported, errors.Count);
        var summaryMsg = $"grocery import: imported {imported} items with {errors.Count} errors";
        Console.WriteLine(summaryMsg);
        return imported;
    }

    /// <summary>
    /// Generates a unique barcode based on SKU and a short suffix. Ensures uniqueness in the repository.
    /// </summary>
    private static async Task<string> GenerateUniqueBarcodeAsync(
        string sku,
        IReadRepository<GroceryItem> readRepository,
        CancellationToken cancellationToken)
    {
        // Start with a sanitized candidate based on SKU
        string baseCandidate = ($"BAR-" + sku.Trim())
            .Replace(" ", "-", StringComparison.Ordinal)
            .ToUpperInvariant();
        if (baseCandidate.Length > 95) baseCandidate = baseCandidate[..95]; // leave room for suffix

        // Try candidate then add short suffixes until unique
        for (int attempt = 0; attempt < 5; attempt++)
        {
            string candidate = attempt == 0 ? baseCandidate : $"{baseCandidate}-{attempt}";
            if (candidate.Length > 100) candidate = candidate[..100];

            var exists = await readRepository.FirstOrDefaultAsync(new GroceryItemByBarcodeSpec(candidate), cancellationToken).ConfigureAwait(false);
            if (exists is null)
            {
                return candidate;
            }
        }

        // Fallback to GUID-based
        var guid = DefaultIdType.NewGuid().ToString("N").ToUpperInvariant();
        var finalCandidate = (baseCandidate + "-" + guid[..8]);
        if (finalCandidate.Length > 100) finalCandidate = finalCandidate[..100];
        return finalCandidate;
    }
}
