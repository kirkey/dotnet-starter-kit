

using Store.Domain.Exceptions.StockAdjustment;

namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Update.v1;

public sealed class UpdateStockAdjustmentHandler(
    ILogger<UpdateStockAdjustmentHandler> logger,
    [FromKeyedServices("store:stock-adjustments")] IRepository<StockAdjustment> repository)
    : IRequestHandler<UpdateStockAdjustmentCommand, UpdateStockAdjustmentResponse>
{
    public async Task<UpdateStockAdjustmentResponse> Handle(UpdateStockAdjustmentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var stockAdjustment = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = stockAdjustment ?? throw new StockAdjustmentNotFoundException(request.Id);
        var updatedStockAdjustment = stockAdjustment.Update(
            request.GroceryItemId,
            request.WarehouseLocationId,
            request.AdjustmentType,
            request.QuantityAdjusted,
            request.Reason,
            request.Notes);
        await repository.UpdateAsync(updatedStockAdjustment, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("stock adjustment with id : {StockAdjustmentId} updated.", stockAdjustment.Id);
        return new UpdateStockAdjustmentResponse(stockAdjustment.Id);
    }
}
