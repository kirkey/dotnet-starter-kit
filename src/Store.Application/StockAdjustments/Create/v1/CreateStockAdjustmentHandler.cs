using Store.Domain.Exceptions.GroceryItem;

namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Create.v1;

public sealed class CreateStockAdjustmentHandler(
    ILogger<CreateStockAdjustmentHandler> logger,
    [FromKeyedServices("store:stock-adjustments")] IRepository<StockAdjustment> repository,
    [FromKeyedServices("store:warehouses")] IRepository<Warehouse> warehouseRepository,
    [FromKeyedServices("store:grocery-items")] IRepository<GroceryItem> groceryItemRepository,
    [FromKeyedServices("store:warehouse-locations")] IRepository<WarehouseLocation> warehouseLocationRepository)
    : IRequestHandler<CreateStockAdjustmentCommand, CreateStockAdjustmentResponse>
{
    public async Task<CreateStockAdjustmentResponse> Handle(CreateStockAdjustmentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        // Verify warehouse exists
        var warehouse = await warehouseRepository.GetByIdAsync(request.WarehouseId, cancellationToken).ConfigureAwait(false);
        _ = warehouse ?? throw new WarehouseNotFoundException(request.WarehouseId);

        // Verify grocery item exists
        var groceryItem = await groceryItemRepository.GetByIdAsync(request.GroceryItemId, cancellationToken).ConfigureAwait(false);
        _ = groceryItem ?? throw new GroceryItemNotFoundException(request.GroceryItemId);
        
        // Verify warehouse location exists (if provided)
        if (request.WarehouseLocationId.HasValue)
        {
            var loc = await warehouseLocationRepository.GetByIdAsync(request.WarehouseLocationId.Value, cancellationToken).ConfigureAwait(false);
            _ = loc ?? throw new WarehouseLocationNotFoundException(request.WarehouseLocationId.Value);
        }
        
        var stockAdjustment = StockAdjustment.Create(
            request.AdjustmentNumber,
            request.GroceryItemId,
            request.WarehouseId,
            request.WarehouseLocationId,
            request.AdjustmentDate,
            request.AdjustmentType,
            request.Reason,
            request.QuantityBefore,
            request.AdjustmentQuantity,
            request.UnitCost,
            request.Reference,
            request.Notes,
            request.AdjustedBy,
            request.BatchNumber,
            request.ExpiryDate);
            
        await repository.AddAsync(stockAdjustment, cancellationToken).ConfigureAwait(false);
        
        logger.LogInformation("stock adjustment created {StockAdjustmentId}", stockAdjustment.Id);
        return new CreateStockAdjustmentResponse(stockAdjustment.Id);
    }
}
