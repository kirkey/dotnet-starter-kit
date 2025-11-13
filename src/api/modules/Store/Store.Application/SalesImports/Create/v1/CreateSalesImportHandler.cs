using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Identity.Users.Abstractions;
using FSH.Starter.WebApi.Store.Application.SalesImports.Specs;

namespace FSH.Starter.WebApi.Store.Application.SalesImports.Create.v1;

/// <summary>
/// Handler for creating and processing sales imports from POS CSV files.
/// </summary>
public class CreateSalesImportHandler(
    IRepository<SalesImport> repository,
    IReadRepository<Item> itemRepository,
    IReadRepository<Warehouse> warehouseRepository,
    IReadRepository<StockLevel> stockLevelRepository,
    IRepository<InventoryTransaction> transactionRepository,
    ICurrentUser currentUser,
    ILogger<CreateSalesImportHandler> logger)
    : IRequestHandler<CreateSalesImportCommand, CreateSalesImportResponse>
{
    public async Task<CreateSalesImportResponse> Handle(CreateSalesImportCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating sales import {ImportNumber} for warehouse {WarehouseId}", 
            request.ImportNumber, request.WarehouseId);

        // Validate warehouse exists
        var warehouse = await warehouseRepository.GetByIdAsync(request.WarehouseId, cancellationToken);
        if (warehouse == null)
        {
            throw new NotFoundException($"Warehouse with ID {request.WarehouseId} not found");
        }

        // Check for duplicate import
        var existingImport = await repository.FirstOrDefaultAsync(
            new SalesImportByNumberSpec(request.ImportNumber), cancellationToken);
        if (existingImport != null)
        {
            throw new ConflictException($"Import with number {request.ImportNumber} already exists");
        }

        // Create sales import entity
        var salesImport = SalesImport.Create(
            request.ImportNumber,
            request.ImportDate,
            request.SalesPeriodFrom,
            request.SalesPeriodTo,
            request.WarehouseId,
            request.FileName,
            request.Notes,
            currentUser.GetUserId().ToString());

        // Parse CSV and create import items
        var csvRecords = ParseCsvData(request.CsvData);
        var importItems = new List<SalesImportItem>();
        int lineNumber = 1;

        foreach (var record in csvRecords)
        {
            try
            {
                var importItem = SalesImportItem.Create(
                    salesImport.Id,
                    lineNumber,
                    record.SaleDate,
                    record.Barcode,
                    record.ItemName,
                    record.QuantitySold,
                    record.UnitPrice);

                importItems.Add(importItem);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Error creating import item at line {LineNumber}: {Message}", 
                    lineNumber, ex.Message);
            }

            lineNumber++;
        }

        // Update import statistics
        salesImport.UpdateStatistics(
            totalRecords: importItems.Count,
            processedRecords: 0,
            errorRecords: 0,
            totalQuantity: importItems.Sum(x => x.QuantitySold),
            totalValue: importItems.Sum(x => x.TotalAmount));

        // Save the import
        await repository.AddAsync(salesImport, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Sales import {ImportNumber} created with {Count} records", 
            request.ImportNumber, importItems.Count);

        // Process import if auto-process is enabled
        if (request.AutoProcess)
        {
            await ProcessImportAsync(salesImport, importItems, cancellationToken);
        }

        return new CreateSalesImportResponse(
            salesImport.Id,
            salesImport.ImportNumber,
            salesImport.Status,
            salesImport.TotalRecords,
            salesImport.ProcessedRecords,
            salesImport.ErrorRecords);
    }

    /// <summary>
    /// Processes the import by matching items and creating inventory transactions.
    /// </summary>
    private async Task ProcessImportAsync(
        SalesImport salesImport, 
        List<SalesImportItem> importItems, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing sales import {ImportNumber}", salesImport.ImportNumber);

        salesImport.UpdateStatus("PROCESSING");
        await repository.UpdateAsync(salesImport, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        int processedCount = 0;
        int errorCount = 0;

        // Match items by barcode
        var barcodes = importItems.Select(x => x.Barcode.ToLowerInvariant()).Distinct().ToList();
        var items = await itemRepository.ListAsync(
            new ItemsByBarcodesSpec(barcodes), cancellationToken);
        var itemLookup = items.ToDictionary(x => x.Barcode.ToLowerInvariant(), x => x);

        foreach (var importItem in importItems)
        {
            try
            {
                // Try to match item by barcode
                if (!itemLookup.TryGetValue(importItem.Barcode.ToLowerInvariant(), out var item))
                {
                    importItem.MarkAsError($"Item with barcode {importItem.Barcode} not found in inventory");
                    errorCount++;
                    continue;
                }

                // Check stock availability (warning only, don't block)
                var stockLevel = await stockLevelRepository.FirstOrDefaultAsync(
                    new StockLevelByItemAndWarehouseSpec(item.Id, salesImport.WarehouseId), 
                    cancellationToken);

                if (stockLevel == null || stockLevel.QuantityOnHand < importItem.QuantitySold)
                {
                    logger.LogWarning(
                        "Insufficient stock for item {ItemId} ({Barcode}). Required: {Required}, Available: {Available}",
                        item.Id, item.Barcode, importItem.QuantitySold, stockLevel?.QuantityOnHand ?? 0);
                }

                // Create inventory OUT transaction
                var transactionNumber = $"SALE-{salesImport.ImportNumber}-{importItem.LineNumber}";
                var quantityBefore = stockLevel?.QuantityOnHand ?? 0;

                var transaction = InventoryTransaction.Create(
                    transactionNumber,
                    item.Id,
                    salesImport.WarehouseId,
                    null, // warehouseLocationId
                    null, // purchaseOrderId
                    "OUT",
                    "POS_SALE",
                    importItem.QuantitySold,
                    quantityBefore,
                    item.Cost,
                    importItem.SaleDate,
                    $"POS Sale - Import: {salesImport.ImportNumber}, Line: {importItem.LineNumber}",
                    $"Barcode: {importItem.Barcode}, Item: {importItem.ItemName}",
                    currentUser.GetUserId().ToString(),
                    true); // auto-approved

                await transactionRepository.AddAsync(transaction, cancellationToken);
                await transactionRepository.SaveChangesAsync(cancellationToken);

                // Mark import item as processed
                importItem.MarkAsProcessed(item.Id, transaction.Id);
                processedCount++;

                logger.LogDebug("Processed sale: Item {ItemId}, Quantity {Quantity}, Transaction {TransactionId}",
                    item.Id, importItem.QuantitySold, transaction.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing import item at line {LineNumber}", importItem.LineNumber);
                importItem.MarkAsError($"Processing error: {ex.Message}");
                errorCount++;
            }
        }

        // Update import statistics and status
        salesImport.UpdateStatistics(
            salesImport.TotalRecords,
            processedCount,
            errorCount,
            importItems.Where(x => x.IsProcessed).Sum(x => x.QuantitySold),
            importItems.Where(x => x.IsProcessed).Sum(x => x.TotalAmount));

        string finalStatus;
        if (processedCount == 0)
        {
            finalStatus = "FAILED";
        }
        else if (errorCount > 0)
        {
            finalStatus = "COMPLETED"; // Partial success
        }
        else
        {
            finalStatus = "COMPLETED";
        }

        salesImport.UpdateStatus(finalStatus);
        await repository.UpdateAsync(salesImport, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Sales import {ImportNumber} processed: {Processed} successful, {Errors} errors",
            salesImport.ImportNumber, processedCount, errorCount);
    }

    /// <summary>
    /// Parses CSV data into sale records.
    /// </summary>
    private List<SalesCsvRecord> ParseCsvData(string csvData)
    {
        var records = new List<SalesCsvRecord>();

        try
        {
            // Decode base64 if needed
            byte[] data;
            try
            {
                data = Convert.FromBase64String(csvData);
            }
            catch
            {
                // Not base64, treat as raw text
                data = Encoding.UTF8.GetBytes(csvData);
            }

            using var stream = new MemoryStream(data);
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                MissingFieldFound = null,
                BadDataFound = null
            });

            csv.Context.RegisterClassMap<SalesCsvRecordMap>();
            records = csv.GetRecords<SalesCsvRecord>().ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error parsing CSV data");
            throw new InvalidOperationException("Invalid CSV format: " + ex.Message, ex);
        }

        return records;
    }
}

/// <summary>
/// CSV record model for POS sales data.
/// </summary>
public class SalesCsvRecord
{
    public DateTime SaleDate { get; set; }
    public string Barcode { get; set; } = default!;
    public string? ItemName { get; set; }
    public int QuantitySold { get; set; }
    public decimal? UnitPrice { get; set; }
}

/// <summary>
/// CSV mapping configuration for flexible column names.
/// </summary>
public class SalesCsvRecordMap : ClassMap<SalesCsvRecord>
{
    public SalesCsvRecordMap()
    {
        Map(m => m.SaleDate).Name("SaleDate", "Date", "Transaction Date", "Sale Date");
        Map(m => m.Barcode).Name("Barcode", "ItemCode", "Item Code", "Product Code");
        Map(m => m.ItemName).Name("ItemName", "Item Name", "Product Name", "Description").Optional();
        Map(m => m.QuantitySold).Name("QuantitySold", "Quantity", "Qty", "Quantity Sold");
        Map(m => m.UnitPrice).Name("UnitPrice", "Price", "Unit Price", "Amount").Optional();
    }
}

