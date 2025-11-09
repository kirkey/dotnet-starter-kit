using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Commands;
using Npgsql;

namespace FSH.Starter.WebApi.Store.Application.Items.Import.v1;

/// <summary>
/// Handler for importing Items from Excel files.
/// Implements strict validation and mapping rules for Store Items.
/// </summary>
public sealed class ImportItemsHandler(
    IDataImportService importService,
    [FromKeyedServices("store:items")] IRepository<Item> repository,
    [FromKeyedServices("store:items")] IReadRepository<Item> readRepository,
    [FromKeyedServices("store:categories")] IReadRepository<Category> categoryRepository,
    [FromKeyedServices("store:suppliers")] IReadRepository<Supplier> supplierRepository,
    ILogger<ImportItemsHandler> logger)
    : IRequestHandler<ImportItemsCommand, ImportResponse>
{
    public async Task<ImportResponse> Handle(ImportItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.File);

        logger.LogInformation("Starting import for Items from file: {FileName}", request.File.Name);

        try
        {
            // Validate file structure if required
            if (request.ValidateStructure)
            {
                var expectedColumns = GetExpectedColumns();
                var validationResult = await importService.ValidateFileStructureAsync(
                    request.File, expectedColumns, request.SheetName, cancellationToken);

                if (!validationResult.IsValid)
                {
                    logger.LogWarning("File structure validation failed: {Errors}", 
                        string.Join(", ", validationResult.Errors));
                    return ImportResponse.Failure(validationResult.Errors);
                }
            }

            // Parse the file
            var rows = await importService.ParseAsync<ItemImportRow>(
                request.File, request.SheetName, cancellationToken);

            if (rows.Count == 0)
            {
                logger.LogInformation("No rows found in the import file");
                return ImportResponse.Success(0);
            }

            // Process rows
            var result = await ProcessRowsAsync(rows, cancellationToken);

            logger.LogInformation("Import completed: {Imported} successful, {Failed} failed out of {Total} total",
                result.ImportedCount, result.FailedCount, result.TotalCount);

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Import failed for Items");
            return ImportResponse.Failure(new List<string> { $"Import failed: {ex.Message}" });
        }
    }

    private async Task<ImportResponse> ProcessRowsAsync(IReadOnlyList<ItemImportRow> rows, CancellationToken cancellationToken)
    {
        var errors = new List<string>();
        var validEntities = new List<Item>();

        logger.LogInformation("Starting validation and caching for {Count} rows", rows.Count);

        // Load all existing items, categories, and suppliers into memory for fast validation
        var existingItems = await readRepository.ListAsync(cancellationToken);
        var existingSkus = new HashSet<string>(existingItems.Select(i => i.Sku.ToUpperInvariant()), StringComparer.OrdinalIgnoreCase);
        var existingBarcodes = new HashSet<string>(existingItems.Select(i => i.Barcode.ToUpperInvariant()), StringComparer.OrdinalIgnoreCase);

        var categories = await categoryRepository.ListAsync(cancellationToken);
        var categoryIds = new HashSet<DefaultIdType>(categories.Select(c => c.Id));
        var defaultCategoryId = categories.FirstOrDefault()?.Id 
            ?? throw new InvalidOperationException("No categories found. Please create at least one category before importing items.");

        var suppliers = await supplierRepository.ListAsync(cancellationToken);
        var supplierIds = new HashSet<DefaultIdType>(suppliers.Select(s => s.Id));
        var defaultSupplierId = suppliers.FirstOrDefault()?.Id 
            ?? throw new InvalidOperationException("No suppliers found. Please create at least one supplier before importing items.");

        logger.LogInformation("Loaded {ItemCount} existing items, {CategoryCount} categories, {SupplierCount} suppliers", 
            existingItems.Count, categories.Count, suppliers.Count);

        // Track SKUs and barcodes from the import file to detect duplicates within the file
        var importSkus = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var importBarcodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Validate all rows
        for (int i = 0; i < rows.Count; i++)
        {
            var rowIndex = i + 1;
            var row = rows[i];

            try
            {
                // Validate the row with cached data
                var validationErrors = ValidateRow(
                    row, 
                    rowIndex, 
                    existingSkus, 
                    existingBarcodes,
                    importSkus,
                    importBarcodes,
                    categoryIds,
                    supplierIds);

                if (validationErrors.Any())
                {
                    errors.AddRange(validationErrors);
                    continue;
                }

                // Map to entity
                var entity = MapToEntity(row, defaultCategoryId, defaultSupplierId);
                validEntities.Add(entity);

                // Track SKUs and barcodes from this import batch
                importSkus.Add(row.Sku!.Trim().ToUpperInvariant());
                importBarcodes.Add(row.Barcode!.Trim().ToUpperInvariant());
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to process row {RowIndex}", rowIndex);
                errors.Add($"Row {rowIndex}: {ex.Message}");
            }

            // Log progress for large imports
            if ((i + 1) % 1000 == 0)
            {
                logger.LogInformation("Validated {Count}/{Total} rows", i + 1, rows.Count);
            }
        }

        // Bulk insert valid entities
        var successfullyInserted = 0;
        if (validEntities.Count > 0)
        {
            logger.LogInformation("Inserting {Count} valid items in batches", validEntities.Count);

            const int batchSize = 1000;
            var batches = validEntities.Chunk(batchSize).ToList();

            for (int batchIndex = 0; batchIndex < batches.Count; batchIndex++)
            {
                // Check if operation has been canceled before processing the next batch
                if (cancellationToken.IsCancellationRequested)
                {
                    logger.LogWarning("Import operation canceled after inserting {Count} items. Remaining {Remaining} items not processed.", 
                        successfullyInserted, validEntities.Count - successfullyInserted);
                    break;
                }
                
                var batch = batches[batchIndex].ToArray();
                
                try
                {
                    await repository.AddRangeAsync(batch, cancellationToken);
                    await repository.SaveChangesAsync(cancellationToken);

                    successfullyInserted += batch.Length;
                    logger.LogInformation("Inserted batch {BatchIndex}/{TotalBatches} ({Count} items)", 
                        batchIndex + 1, batches.Count, batch.Length);
                }
                catch (OperationCanceledException)
                {
                    // If operation was canceled due to timeout, log all items in the batch as failed
                    logger.LogWarning("Batch {BatchIndex} canceled due to timeout. Marking {Count} items as failed.", 
                        batchIndex + 1, batch.Length);
                    
                    foreach (var item in batch)
                    {
                        errors.Add($"Failed to insert item with SKU '{item.Sku}': Operation timed out");
                    }
                    
                    // Stop processing further batches if we've hit a timeout
                    logger.LogWarning("Stopping import due to operation timeout after inserting {Count} items", successfullyInserted);
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Batch {BatchIndex} failed with error: {Message}. Attempting individual inserts to identify problem items", 
                        batchIndex + 1, ex.Message);
                    
                    // Try inserting items individually to identify the problematic ones
                    // Use a new cancellation token source with a reasonable timeout for individual items
                    using var individualCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                    using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, individualCts.Token);
                    
                    foreach (var item in batch)
                    {
                        try
                        {
                            // Check if the main cancellation token is already canceled
                            if (cancellationToken.IsCancellationRequested)
                            {
                                errors.Add($"Failed to insert item with SKU '{item.Sku}': Operation canceled");
                                continue;
                            }
                            
                            await repository.AddAsync(item, linkedCts.Token);
                            await repository.SaveChangesAsync(linkedCts.Token);
                            successfullyInserted++;
                        }
                        catch (OperationCanceledException)
                        {
                            errors.Add($"Failed to insert item with SKU '{item.Sku}': Operation timed out");
                            logger.LogWarning("Individual insert timed out for SKU: {Sku}", item.Sku);
                        }
                        catch (Exception itemEx)
                        {
                            var errorMessage = $"Failed to insert item with SKU '{item.Sku}': {GetDetailedErrorMessage(itemEx)}";
                            logger.LogError(itemEx, "Failed to insert item with SKU: {Sku}", item.Sku);
                            errors.Add(errorMessage);
                        }
                    }
                }
            }
        }

        var failedCount = rows.Count - successfullyInserted;

        logger.LogInformation("Import summary: {Successful} successful, {Failed} failed, {Total} total", 
            successfullyInserted, failedCount, rows.Count);

        return failedCount > 0
            ? ImportResponse.PartialSuccess(successfullyInserted, failedCount, errors)
            : ImportResponse.Success(successfullyInserted);
    }

    /// <summary>
    /// Extracts detailed error message from exception, especially for database constraint violations.
    /// </summary>
    private static string GetDetailedErrorMessage(Exception ex)
    {
        if (ex.InnerException is PostgresException pgEx)
        {
            return $"{pgEx.MessageText} (Field: {pgEx.ColumnName ?? "unknown"}, SqlState: {pgEx.SqlState})";
        }

        return ex.InnerException?.Message ?? ex.Message;
    }

    private static IEnumerable<string> GetExpectedColumns()
    {
        return new[]
        {
            nameof(ItemImportRow.Name),
            nameof(ItemImportRow.Sku),
            nameof(ItemImportRow.Barcode),
            nameof(ItemImportRow.Brand),
        };
    }

    /// <summary>
    /// Validates a single import row with strict business rules using cached data.
    /// </summary>
    private static IReadOnlyList<string> ValidateRow(
        ItemImportRow row,
        int rowIndex,
        HashSet<string> existingSkus,
        HashSet<string> existingBarcodes,
        HashSet<string> importSkus,
        HashSet<string> importBarcodes,
        HashSet<DefaultIdType> categoryIds,
        HashSet<DefaultIdType> supplierIds)
    {
        var errors = new List<string>();

        // Validate required fields
        if (string.IsNullOrWhiteSpace(row.Name))
        {
            errors.Add($"Row {rowIndex}: Name is required");
        }
        else if (row.Name.Length > 200)
        {
            errors.Add($"Row {rowIndex}: Name cannot exceed 200 characters");
        }

        if (string.IsNullOrWhiteSpace(row.Sku))
        {
            errors.Add($"Row {rowIndex}: SKU is required");
        }
        else if (row.Sku.Trim().Length > 50)
        {
            errors.Add($"Row {rowIndex}: SKU cannot exceed 50 characters (actual: {row.Sku.Trim().Length})");
        }
        else
        {
            var skuUpper = row.Sku.Trim().ToUpperInvariant();
            
            // Check for duplicate SKU in existing items
            if (existingSkus.Contains(skuUpper))
            {
                errors.Add($"Row {rowIndex}: Item with SKU '{row.Sku}' already exists in database");
            }
            // Check for duplicate SKU within the import file
            else if (importSkus.Contains(skuUpper))
            {
                errors.Add($"Row {rowIndex}: Duplicate SKU '{row.Sku}' found within import file");
            }
        }

        if (string.IsNullOrWhiteSpace(row.Barcode))
        {
            errors.Add($"Row {rowIndex}: Barcode is required");
        }
        else if (row.Barcode.Trim().Length > 50)
        {
            errors.Add($"Row {rowIndex}: Barcode cannot exceed 50 characters (actual: {row.Barcode.Trim().Length})");
        }
        else
        {
            var barcodeUpper = row.Barcode.Trim().ToUpperInvariant();
            
            // Check for duplicate Barcode in existing items
            if (existingBarcodes.Contains(barcodeUpper))
            {
                errors.Add($"Row {rowIndex}: Item with Barcode '{row.Barcode}' already exists in database");
            }
            // Check for duplicate Barcode within the import file
            else if (importBarcodes.Contains(barcodeUpper))
            {
                errors.Add($"Row {rowIndex}: Duplicate Barcode '{row.Barcode}' found within import file");
            }
        }

        // Validate pricing
        if (row.Price is < 0)
        {
            errors.Add($"Row {rowIndex}: Price must be >= 0");
        }

        if (row.Cost is < 0)
        {
            errors.Add($"Row {rowIndex}: Cost must be >= 0");
        }

        if (row is { Price: not null, Cost: not null } && row.Price.Value < row.Cost.Value)
        {
            errors.Add($"Row {rowIndex}: Price must be greater than or equal to Cost");
        }

        // Validate stock levels
        if (row.MinimumStock is < 0)
        {
            errors.Add($"Row {rowIndex}: Minimum Stock must be >= 0");
        }

        if (row.MaximumStock is < 0)
        {
            errors.Add($"Row {rowIndex}: Maximum Stock must be >= 0");
        }

        if (row is { MinimumStock: not null, MaximumStock: not null } &&
            row.MinimumStock.Value > row.MaximumStock.Value)
        {
            errors.Add($"Row {rowIndex}: Minimum Stock must be less than or equal to Maximum Stock");
        }

        if (row.ReorderPoint is < 0)
        {
            errors.Add($"Row {rowIndex}: Reorder Point must be >= 0");
        }

        if (row is { ReorderPoint: not null, MaximumStock: not null } &&
            row.ReorderPoint.Value > row.MaximumStock.Value)
        {
            errors.Add($"Row {rowIndex}: Reorder Point must be less than or equal to Maximum Stock");
        }

        // Validate optional fields
        if (!string.IsNullOrWhiteSpace(row.Brand) && row.Brand.Length > 200)
        {
            errors.Add($"Row {rowIndex}: Brand cannot exceed 200 characters");
        }
        
        if (!string.IsNullOrWhiteSpace(row.Description) && row.Description.Length > 2000)
        {
            errors.Add($"Row {rowIndex}: Description cannot exceed 2000 characters");
        }

        if (!string.IsNullOrWhiteSpace(row.Manufacturer) && row.Manufacturer.Length > 200)
        {
            errors.Add($"Row {rowIndex}: Manufacturer cannot exceed 200 characters");
        }

        if (!string.IsNullOrWhiteSpace(row.WeightUnit) && row.WeightUnit.Trim().Length > 10)
        {
            errors.Add($"Row {rowIndex}: Weight Unit cannot exceed 10 characters (actual: {row.WeightUnit.Trim().Length})");
        }

        if (row.Weight is < 0)
        {
            errors.Add($"Row {rowIndex}: Weight must be >= 0");
        }

        // Validate Category exists (only if provided)
        if (row.CategoryId.HasValue && !categoryIds.Contains(row.CategoryId.Value))
        {
            errors.Add($"Row {rowIndex}: Category with ID '{row.CategoryId}' does not exist");
        }

        // Validate Supplier exists (only if provided)
        if (row.SupplierId.HasValue && !supplierIds.Contains(row.SupplierId.Value))
        {
            errors.Add($"Row {rowIndex}: Supplier with ID '{row.SupplierId}' does not exist");
        }

        return errors;
    }

    /// <summary>
    /// Maps an import row to an Item domain entity.
    /// Uses default Category and Supplier if not provided in the import file.
    /// </summary>
    private static Item MapToEntity(ItemImportRow row, DefaultIdType defaultCategoryId, DefaultIdType defaultSupplierId)
    {
        // Use provided IDs or defaults
        var categoryId = row.CategoryId ?? defaultCategoryId;
        var supplierId = row.SupplierId ?? defaultSupplierId;

        var item = Item.Create(
            name: row.Name!.Trim(),
            description: row.Description?.Trim(),
            sku: row.Sku!.Trim(),
            barcode: row.Barcode!.Trim(),
            unitPrice: row.Price ?? 0,
            cost: row.Cost ?? 0,
            minimumStock: row.MinimumStock ?? 0,
            maximumStock: row.MaximumStock ?? 10,
            reorderPoint: row.ReorderPoint ?? 0,
            reorderQuantity: row.ReorderPoint ?? 0, // Use ReorderPoint as default for ReorderQuantity
            leadTimeDays: 7, // Default lead time
            categoryId: categoryId,
            supplierId: supplierId,
            unitOfMeasure: "EA", // Default unit of measure
            isPerishable: row.IsPerishable ?? false,
            isSerialTracked: false, // Not in import row
            isLotTracked: false, // Not in import row
            shelfLifeDays: null, // Not in import row
            brand: row.Brand?.Trim(),
            manufacturer: row.Manufacturer?.Trim(),
            manufacturerPartNumber: null, // Not in import row
            weight: row.Weight ?? 0,
            weightUnit: row.WeightUnit?.Trim(),
            length: null, // Not in import row
            width: null, // Not in import row
            height: null, // Not in import row
            dimensionUnit: null); // Not in import row

        return item;
    }
}

