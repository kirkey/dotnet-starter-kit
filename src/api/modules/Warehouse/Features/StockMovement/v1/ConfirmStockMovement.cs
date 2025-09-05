using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;
using FSH.Starter.WebApi.Warehouse.Exceptions;

namespace FSH.Starter.WebApi.Warehouse.Features.StockMovement.v1;

public sealed record ConfirmStockMovementRequest(DefaultIdType Id);

public sealed record ConfirmStockMovementResponse(DefaultIdType Id, StockMovementStatus Status);

public sealed class ConfirmStockMovementHandler(
    IRepository<Domain.StockMovement> stockMovementRepository,
    IRepository<InventoryItem> inventoryRepository)
{
    public async Task<ConfirmStockMovementResponse> Handle(ConfirmStockMovementRequest request, CancellationToken cancellationToken)
    {
        var stockMovement = await stockMovementRepository.GetByIdAsync(request.Id, cancellationToken);

        if (stockMovement is null)
        {
            throw new StockMovementNotFoundException(request.Id);
        }

        if (stockMovement.Status != StockMovementStatus.Pending)
        {
            throw new InvalidStockMovementException($"Stock movement {request.Id} cannot be confirmed. Current status: {stockMovement.Status}");
        }

        // Confirm the stock movement
        stockMovement.Confirm();

        // Update inventory based on movement type
        await UpdateInventoryForMovement(stockMovement, cancellationToken);

        await stockMovementRepository.UpdateAsync(stockMovement, cancellationToken);
        await stockMovementRepository.SaveChangesAsync(cancellationToken);

        return new ConfirmStockMovementResponse(stockMovement.Id, stockMovement.Status);
    }

    private async Task UpdateInventoryForMovement(Domain.StockMovement stockMovement, CancellationToken cancellationToken)
    {
        switch (stockMovement.MovementType)
        {
            case StockMovementType.Inbound:
                await HandleInboundMovement(stockMovement, cancellationToken);
                break;

            case StockMovementType.Outbound:
                await HandleOutboundMovement(stockMovement, cancellationToken);
                break;

            case StockMovementType.Transfer:
                await HandleTransferMovement(stockMovement, cancellationToken);
                break;

            default:
                throw new InvalidStockMovementException($"Movement type {stockMovement.MovementType} is not supported for confirmation.");
        }
    }

    private async Task HandleInboundMovement(Domain.StockMovement stockMovement, CancellationToken cancellationToken)
    {
        var inventoryItem = await inventoryRepository.FirstOrDefaultAsync(
            i => i.WarehouseId == stockMovement.WarehouseId && i.ProductSku == stockMovement.ProductSku,
            cancellationToken);

        if (inventoryItem is null)
        {
            // Create new inventory item
            inventoryItem = InventoryItem.Create(
                stockMovement.WarehouseId,
                stockMovement.ProductSku,
                stockMovement.ProductName,
                stockMovement.Quantity,
                0, // Default minimum stock
                0, // Default maximum stock  
                stockMovement.UnitOfMeasure);

            await inventoryRepository.AddAsync(inventoryItem, cancellationToken);
        }
        else
        {
            inventoryItem.AddStock(stockMovement.Quantity, $"Inbound movement: {stockMovement.ReferenceNumber}");
            await inventoryRepository.UpdateAsync(inventoryItem, cancellationToken);
        }
    }

    private async Task HandleOutboundMovement(Domain.StockMovement stockMovement, CancellationToken cancellationToken)
    {
        var inventoryItem = await inventoryRepository.FirstOrDefaultAsync(
            i => i.WarehouseId == stockMovement.WarehouseId && i.ProductSku == stockMovement.ProductSku,
            cancellationToken);

        if (inventoryItem is null)
        {
            throw new InventoryItemNotFoundException(stockMovement.WarehouseId, stockMovement.ProductSku);
        }

        inventoryItem.RemoveStock(stockMovement.Quantity, $"Outbound movement: {stockMovement.ReferenceNumber}");
        await inventoryRepository.UpdateAsync(inventoryItem, cancellationToken);
    }

    private async Task HandleTransferMovement(Domain.StockMovement stockMovement, CancellationToken cancellationToken)
    {
        if (stockMovement.SourceWarehouseId == null || stockMovement.DestinationWarehouseId == null)
        {
            throw new InvalidStockMovementException("Transfer movements require both source and destination warehouses.");
        }

        // Remove from source warehouse
        var sourceInventory = await inventoryRepository.FirstOrDefaultAsync(
            i => i.WarehouseId == stockMovement.SourceWarehouseId && i.ProductSku == stockMovement.ProductSku,
            cancellationToken);

        if (sourceInventory is null)
        {
            throw new InventoryItemNotFoundException(stockMovement.SourceWarehouseId.Value, stockMovement.ProductSku);
        }

        sourceInventory.RemoveStock(stockMovement.Quantity, $"Transfer to warehouse {stockMovement.DestinationWarehouseId}: {stockMovement.ReferenceNumber}");
        await inventoryRepository.UpdateAsync(sourceInventory, cancellationToken);

        // Add to destination warehouse
        var destinationInventory = await inventoryRepository.FirstOrDefaultAsync(
            i => i.WarehouseId == stockMovement.DestinationWarehouseId && i.ProductSku == stockMovement.ProductSku,
            cancellationToken);

        if (destinationInventory is null)
        {
            // Create new inventory item at destination
            destinationInventory = InventoryItem.Create(
                stockMovement.DestinationWarehouseId.Value,
                stockMovement.ProductSku,
                stockMovement.ProductName,
                stockMovement.Quantity,
                0, // Default minimum stock
                0, // Default maximum stock
                stockMovement.UnitOfMeasure);

            await inventoryRepository.AddAsync(destinationInventory, cancellationToken);
        }
        else
        {
            destinationInventory.AddStock(stockMovement.Quantity, $"Transfer from warehouse {stockMovement.SourceWarehouseId}: {stockMovement.ReferenceNumber}");
            await inventoryRepository.UpdateAsync(destinationInventory, cancellationToken);
        }
    }
}
