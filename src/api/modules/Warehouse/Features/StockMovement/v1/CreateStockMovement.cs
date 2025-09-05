using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;
using FSH.Starter.WebApi.Warehouse.Exceptions;

namespace FSH.Starter.WebApi.Warehouse.Features.StockMovement.v1;

public sealed record CreateStockMovementRequest(
    DefaultIdType WarehouseId,
    string ProductSku,
    string ProductName,
    StockMovementType MovementType,
    decimal Quantity,
    string UnitOfMeasureName,
    string UnitOfMeasureSymbol,
    string? UnitOfMeasureDescription = null,
    string? ReferenceNumber = null,
    string? Notes = null,
    DefaultIdType? SourceWarehouseId = null,
    DefaultIdType? DestinationWarehouseId = null);

public sealed record CreateStockMovementResponse(DefaultIdType Id);

public sealed class CreateStockMovementHandler(
    IRepository<StockMovement> stockMovementRepository,
    IRepository<Domain.Warehouse> warehouseRepository,
    IRepository<InventoryItem> inventoryRepository)
{
    public async Task<CreateStockMovementResponse> Handle(CreateStockMovementRequest request, CancellationToken cancellationToken)
    {
        // Validate warehouse exists and is active
        var warehouse = await warehouseRepository.GetByIdAsync(request.WarehouseId, cancellationToken);
        if (warehouse is null)
        {
            throw new WarehouseNotFoundException(request.WarehouseId);
        }

        if (!warehouse.IsActive)
        {
            throw new InactiveWarehouseException(request.WarehouseId);
        }

        // Validate source and destination warehouses for transfers
        if (request.MovementType == StockMovementType.Transfer)
        {
            if (request.SourceWarehouseId == null || request.DestinationWarehouseId == null)
            {
                throw new InvalidStockMovementException("Transfer movements require both source and destination warehouses.");
            }

            var sourceWarehouse = await warehouseRepository.GetByIdAsync(request.SourceWarehouseId.Value, cancellationToken);
            var destinationWarehouse = await warehouseRepository.GetByIdAsync(request.DestinationWarehouseId.Value, cancellationToken);

            if (sourceWarehouse is null)
            {
                throw new WarehouseNotFoundException(request.SourceWarehouseId.Value);
            }

            if (destinationWarehouse is null)
            {
                throw new WarehouseNotFoundException(request.DestinationWarehouseId.Value);
            }

            if (!sourceWarehouse.IsActive || !destinationWarehouse.IsActive)
            {
                throw new InvalidStockMovementException("Both source and destination warehouses must be active for transfers.");
            }
        }

        // For outbound movements, check if sufficient stock is available
        if (request.MovementType == StockMovementType.Outbound || request.MovementType == StockMovementType.Transfer)
        {
            var inventoryItem = await inventoryRepository.FirstOrDefaultAsync(
                i => i.WarehouseId == request.WarehouseId && i.ProductSku == request.ProductSku,
                cancellationToken);

            if (inventoryItem is null)
            {
                throw new InventoryItemNotFoundException(request.WarehouseId, request.ProductSku);
            }

            if (inventoryItem.AvailableStock < request.Quantity)
            {
                throw new InsufficientStockException(request.ProductSku, inventoryItem.AvailableStock, request.Quantity);
            }
        }

        var unitOfMeasure = new UnitOfMeasure(
            request.UnitOfMeasureName,
            request.UnitOfMeasureSymbol,
            request.UnitOfMeasureDescription);

        StockMovement stockMovement;

        switch (request.MovementType)
        {
            case StockMovementType.Inbound:
                stockMovement = StockMovement.CreateInbound(
                    request.WarehouseId,
                    request.ProductSku,
                    request.ProductName,
                    request.Quantity,
                    unitOfMeasure,
                    request.ReferenceNumber,
                    request.Notes);
                break;

            case StockMovementType.Outbound:
                stockMovement = StockMovement.CreateOutbound(
                    request.WarehouseId,
                    request.ProductSku,
                    request.ProductName,
                    request.Quantity,
                    unitOfMeasure,
                    request.ReferenceNumber,
                    request.Notes);
                break;

            case StockMovementType.Transfer:
                stockMovement = StockMovement.CreateTransfer(
                    request.SourceWarehouseId!.Value,
                    request.DestinationWarehouseId!.Value,
                    request.ProductSku,
                    request.ProductName,
                    request.Quantity,
                    unitOfMeasure,
                    request.ReferenceNumber,
                    request.Notes);
                break;

            default:
                throw new InvalidStockMovementException($"Movement type {request.MovementType} is not supported for creation.");
        }

        await stockMovementRepository.AddAsync(stockMovement, cancellationToken);
        await stockMovementRepository.SaveChangesAsync(cancellationToken);

        return new CreateStockMovementResponse(stockMovement.Id);
    }
}
