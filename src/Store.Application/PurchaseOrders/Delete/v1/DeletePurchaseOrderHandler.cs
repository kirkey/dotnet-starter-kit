using Store.Domain.Exceptions.PurchaseOrder;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Delete.v1;

public sealed class DeletePurchaseOrderHandler(
    ILogger<DeletePurchaseOrderHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<DeletePurchaseOrderCommand>
{
    public async Task Handle(DeletePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var po = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderNotFoundException(request.Id);

        await repository.DeleteAsync(po, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("purchase order deleted {PurchaseOrderId}", po.Id);
    }
}

