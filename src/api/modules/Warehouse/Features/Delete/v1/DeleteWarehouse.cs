using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Exceptions;

namespace FSH.Starter.WebApi.Warehouse.Features.Delete.v1;

public sealed record DeleteWarehouseRequest(DefaultIdType Id);

public sealed record DeleteWarehouseResponse(DefaultIdType Id);

public sealed class DeleteWarehouseHandler(
    IRepository<Domain.Warehouse> warehouseRepository,
    IRepository<Domain.InventoryItem> inventoryRepository,
    IRepository<Domain.StockMovement> stockMovementRepository)
{
    public async Task<DeleteWarehouseResponse> Handle(DeleteWarehouseRequest request, CancellationToken cancellationToken)
    {
        var warehouse = await warehouseRepository.GetByIdAsync(request.Id, cancellationToken);

        if (warehouse is null)
        {
            throw new WarehouseNotFoundException(request.Id);
        }

        // Check if warehouse has active inventory or stock movements
        var hasInventory = await inventoryRepository.AnyAsync(
            i => i.WarehouseId == request.Id && i.CurrentStock > 0, 
            cancellationToken);

        if (hasInventory)
        {
            throw new InvalidOperationException("Cannot delete warehouse with existing inventory. Please transfer all stock first.");
        }

        var hasPendingMovements = await stockMovementRepository.AnyAsync(
            sm => (sm.WarehouseId == request.Id || sm.SourceWarehouseId == request.Id || sm.DestinationWarehouseId == request.Id) 
                  && sm.Status == Domain.ValueObjects.StockMovementStatus.Pending,
            cancellationToken);

        if (hasPendingMovements)
        {
            throw new InvalidOperationException("Cannot delete warehouse with pending stock movements. Please complete or cancel all pending movements first.");
        }

        // Instead of hard delete, deactivate the warehouse
        warehouse.Deactivate();
        await warehouseRepository.UpdateAsync(warehouse, cancellationToken);
        await warehouseRepository.SaveChangesAsync(cancellationToken);

        return new DeleteWarehouseResponse(warehouse.Id);
    }
}
