using Store.Domain.Exceptions.StockAdjustment;

namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Delete.v1;

public sealed class DeleteStockAdjustmentHandler(
    ILogger<DeleteStockAdjustmentHandler> logger,
    [FromKeyedServices("store:stock-adjustments")] IRepository<StockAdjustment> repository)
    : IRequestHandler<DeleteStockAdjustmentCommand>
{
    public async Task Handle(DeleteStockAdjustmentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var stockAdjustment = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = stockAdjustment ?? throw new StockAdjustmentNotFoundException(request.Id);
        await repository.DeleteAsync(stockAdjustment, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("stock adjustment with id : {StockAdjustmentId} successfully deleted", stockAdjustment.Id);
    }
}
