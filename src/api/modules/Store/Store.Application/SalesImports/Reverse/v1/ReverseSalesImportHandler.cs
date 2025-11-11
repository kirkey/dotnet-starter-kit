using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Identity.Users.Abstractions;
using FSH.Starter.WebApi.Store.Application.SalesImports.Specs;
using Store.Domain.Entities;

namespace FSH.Starter.WebApi.Store.Application.SalesImports.Reverse.v1;

/// <summary>
/// Handler for reversing a sales import and creating offsetting inventory transactions.
/// </summary>
public class ReverseSalesImportHandler(
    IRepository<SalesImport> repository,
    IReadRepository<SalesImportItem> itemRepository,
    IRepository<InventoryTransaction> transactionRepository,
    ICurrentUser currentUser,
    ILogger<ReverseSalesImportHandler> logger)
    : IRequestHandler<ReverseSalesImportCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ReverseSalesImportCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Reversing sales import {ImportId}", request.Id);

        // Get the sales import with items
        var salesImport = await repository.FirstOrDefaultAsync(
            new SalesImportByIdWithItemsSpec(request.Id), cancellationToken);

        if (salesImport == null)
        {
            throw new NotFoundException($"Sales import with ID {request.Id} not found");
        }

        // Validate that import can be reversed
        if (salesImport.IsReversed)
        {
            throw new ConflictException($"Sales import {salesImport.ImportNumber} has already been reversed");
        }

        if (salesImport.Status != "COMPLETED")
        {
            throw new ConflictException($"Only completed imports can be reversed. Current status: {salesImport.Status}");
        }

        var userId = currentUser.GetUserId().ToString();

        // Create reversing transactions for all processed items
        var processedItems = salesImport.Items.Where(x => x.IsProcessed && x.ItemId.HasValue).ToList();
        
        logger.LogInformation("Creating {Count} reversing transactions for import {ImportNumber}", 
            processedItems.Count, salesImport.ImportNumber);

        foreach (var item in processedItems)
        {
            try
            {
                // Create IN transaction to reverse the OUT transaction
                var transactionNumber = $"SREV-{salesImport.ImportNumber}-{item.LineNumber}";
                
                var reversalTransaction = InventoryTransaction.Create(
                    transactionNumber,
                    item.ItemId!.Value,
                    salesImport.WarehouseId,
                    null, // warehouseLocationId
                    null, // purchaseOrderId
                    "IN",
                    "SALE_REVERSAL",
                    item.QuantitySold,
                    0, // quantityBefore will be updated by the system
                    item.UnitPrice ?? 0,
                    DateTime.UtcNow,
                    $"Reversal of import {salesImport.ImportNumber} - Line {item.LineNumber}",
                    $"Reason: {request.Reason}. Original Transaction: {item.InventoryTransaction?.TransactionNumber}",
                    userId,
                    true); // auto-approved

                await transactionRepository.AddAsync(reversalTransaction, cancellationToken);
                
                logger.LogDebug("Created reversal transaction {TransactionNumber} for item {ItemId}",
                    transactionNumber, item.ItemId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating reversal transaction for item line {LineNumber}", item.LineNumber);
                throw new InvalidOperationException(
                    $"Failed to create reversal transaction for line {item.LineNumber}: {ex.Message}", ex);
            }
        }

        await transactionRepository.SaveChangesAsync(cancellationToken);

        // Mark the import as reversed
        salesImport.Reverse(request.Reason, userId);
        await repository.UpdateAsync(salesImport, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Sales import {ImportNumber} successfully reversed with {Count} transactions",
            salesImport.ImportNumber, processedItems.Count);

        return salesImport.Id;
    }
}

