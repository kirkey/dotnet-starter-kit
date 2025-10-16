using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.Commands;
using FSH.Starter.WebApi.Store.Application.Items.Specs;
using FSH.Starter.WebApi.Store.Application.Categories.Specs;
using FSH.Starter.WebApi.Store.Application.Suppliers.Specs;

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
        var importedCount = 0;

        for (int i = 0; i < rows.Count; i++)
        {
            var rowIndex = i + 1;
            var row = rows[i];

            try
            {
                // Validate the row
                var validationErrors = await ValidateRowAsync(row, rowIndex, cancellationToken);
                if (validationErrors.Any())
                {
                    errors.AddRange(validationErrors);
                    continue;
                }

                // Map to entity
                var entity = await MapToEntityAsync(row, cancellationToken);

                // Save entity
                await repository.AddAsync(entity, cancellationToken);
                await repository.SaveChangesAsync(cancellationToken);

                importedCount++;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to import row {RowIndex}", rowIndex);
                errors.Add($"Row {rowIndex}: {ex.Message}");
            }
        }

        var failedCount = rows.Count - importedCount;
        return failedCount > 0
            ? ImportResponse.PartialSuccess(importedCount, failedCount, errors)
            : ImportResponse.Success(importedCount);
    }

    private IEnumerable<string> GetExpectedColumns()
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
    /// Validates a single import row with strict business rules.
    /// </summary>
    private async Task<IReadOnlyList<string>> ValidateRowAsync(
        ItemImportRow row,
        int rowIndex,
        CancellationToken cancellationToken)
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
        else if (row.Sku.Length > 100)
        {
            errors.Add($"Row {rowIndex}: SKU cannot exceed 100 characters");
        }
        else
        {
            // Check for duplicate SKU
            var existingBySku = await readRepository.FirstOrDefaultAsync(
                new ItemBySkuSpec(row.Sku.Trim()), cancellationToken);
            if (existingBySku != null)
            {
                errors.Add($"Row {rowIndex}: Item with SKU '{row.Sku}' already exists");
            }
        }

        if (string.IsNullOrWhiteSpace(row.Barcode))
        {
            errors.Add($"Row {rowIndex}: Barcode is required");
        }
        else if (row.Barcode.Length > 100)
        {
            errors.Add($"Row {rowIndex}: Barcode cannot exceed 100 characters");
        }
        else
        {
            // Check for duplicate Barcode
            var existingByBarcode = await readRepository.FirstOrDefaultAsync(
                new ItemByBarcodeSpec(row.Barcode.Trim()), cancellationToken);
            if (existingByBarcode != null)
            {
                errors.Add($"Row {rowIndex}: Item with Barcode '{row.Barcode}' already exists");
            }
        }

        // Validate pricing
        // if (row.Price is null or < 0)
        // {
        //     errors.Add($"Row {rowIndex}: Price is required and must be >= 0");
        // }
        //
        // if (row.Cost is null or < 0)
        // {
        //     errors.Add($"Row {rowIndex}: Cost is required and must be >= 0");
        // }
        //
        // if (row is { Price: not null, Cost: not null } && row.Price.Value < row.Cost.Value)
        // {
        //     errors.Add($"Row {rowIndex}: Price must be greater than or equal to Cost");
        // }

        // Validate stock levels
        // if (row.MinimumStock is null or < 0)
        // {
        //     errors.Add($"Row {rowIndex}: Minimum Stock is required and must be >= 0");
        // }
        //
        // if (!row.MaximumStock.HasValue || row.MaximumStock.Value <= 0)
        // {
        //     errors.Add($"Row {rowIndex}: Maximum Stock is required and must be > 0");
        // }
        //
        // if (row is { MinimumStock: not null, MaximumStock: not null } &&
        //     row.MinimumStock.Value > row.MaximumStock.Value)
        // {
        //     errors.Add($"Row {rowIndex}: Minimum Stock must be less than or equal to Maximum Stock");
        // }
        //
        // if (row.ReorderPoint is null or < 0)
        // {
        //     errors.Add($"Row {rowIndex}: Reorder Point is required and must be >= 0");
        // }
        //
        // if (row is { ReorderPoint: not null, MaximumStock: not null } &&
        //     row.ReorderPoint.Value > row.MaximumStock.Value)
        // {
        //     errors.Add($"Row {rowIndex}: Reorder Point must be less than or equal to Maximum Stock");
        // }

        // Validate optional fields
        if (!string.IsNullOrWhiteSpace(row.Brand) && row.Brand.Length > 200)
        {
            errors.Add($"Row {rowIndex}: Brand cannot exceed 200 characters");
        }
        
        // if (!string.IsNullOrWhiteSpace(row.Description) && row.Description.Length > 2000)
        // {
        //     errors.Add($"Row {rowIndex}: Description cannot exceed 2000 characters");
        // }
        //
        // if (!string.IsNullOrWhiteSpace(row.Manufacturer) && row.Manufacturer.Length > 200)
        // {
        //     errors.Add($"Row {rowIndex}: Manufacturer cannot exceed 200 characters");
        // }
        //
        // if (!string.IsNullOrWhiteSpace(row.WeightUnit) && row.WeightUnit.Length > 20)
        // {
        //     errors.Add($"Row {rowIndex}: Weight Unit cannot exceed 20 characters");
        // }
        //
        // if (row.Weight is < 0)
        // {
        //     errors.Add($"Row {rowIndex}: Weight must be >= 0");
        // }

        // Validate Category exists (only if provided)
        if (row.CategoryId.HasValue)
        {
            var category = await categoryRepository.FirstOrDefaultAsync(
                new GetCategorySpecs(row.CategoryId.Value), cancellationToken);
            if (category == null)
            {
                errors.Add($"Row {rowIndex}: Category with ID '{row.CategoryId}' does not exist");
            }
        }
        // Note: CategoryId is optional - will use default if not provided

        // Validate Supplier exists (only if provided)
        if (row.SupplierId.HasValue)
        {
            var supplier = await supplierRepository.FirstOrDefaultAsync(
                new GetSupplierSpecs(row.SupplierId.Value), cancellationToken);
            if (supplier == null)
            {
                errors.Add($"Row {rowIndex}: Supplier with ID '{row.SupplierId}' does not exist");
            }
        }
        // Note: SupplierId is optional - will use default if not provided

        // Validate expiry date for perishable items
        // if (row is { IsPerishable: true, ExpiryDate: not null } && row.ExpiryDate.Value < DateTime.UtcNow)
        // {
        //     errors.Add($"Row {rowIndex}: Expiry Date for perishable items cannot be in the past");
        // }

        return errors;
    }

    /// <summary>
    /// Maps an import row to an Item domain entity.
    /// Uses default Category and Supplier if not provided in the import file.
    /// </summary>
    private async Task<Item> MapToEntityAsync(ItemImportRow row, CancellationToken cancellationToken)
    {
        // Get or use default CategoryId
        var categoryId = row.CategoryId ?? await GetOrCreateDefaultCategoryAsync(cancellationToken);
        
        // Get or use default SupplierId
        var supplierId = row.SupplierId ?? await GetOrCreateDefaultSupplierAsync(cancellationToken);

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

    /// <summary>
    /// Gets the first available category or throws an exception if none exists.
    /// In production, you should create a default "Uncategorized" category during setup.
    /// </summary>
    private async Task<DefaultIdType> GetOrCreateDefaultCategoryAsync(CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.ListAsync(cancellationToken);
        var defaultCategory = categories.FirstOrDefault();
        
        if (defaultCategory == null)
        {
            throw new InvalidOperationException(
                "No categories found in the system. Please create at least one category before importing items without CategoryId.");
        }

        return defaultCategory.Id;
    }

    /// <summary>
    /// Gets the first available supplier or throws an exception if none exists.
    /// In production, you should create a default "Unknown Supplier" during setup.
    /// </summary>
    private async Task<DefaultIdType> GetOrCreateDefaultSupplierAsync(CancellationToken cancellationToken)
    {
        var suppliers = await supplierRepository.ListAsync(cancellationToken);
        var defaultSupplier = suppliers.FirstOrDefault();
        
        if (defaultSupplier == null)
        {
            throw new InvalidOperationException(
                "No suppliers found in the system. Please create at least one supplier before importing items without SupplierId.");
        }

        return defaultSupplier.Id;
    }
}

