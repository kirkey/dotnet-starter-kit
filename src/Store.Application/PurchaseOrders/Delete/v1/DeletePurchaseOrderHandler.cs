using Store.Domain.Exceptions.PurchaseOrder;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Delete.v1;

public sealed class DeletePurchaseOrderHandler(
    ILogger<DeletePurchaseOrderHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository,
    [FromKeyedServices("store:inventory-transactions")] IReadRepository<InventoryTransaction> txReadRepository)
    : IRequestHandler<DeletePurchaseOrderCommand>
{
    public async Task Handle(DeletePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var po = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderNotFoundException(request.Id);

        // Guard: prevent deleting a PO that has related inventory transactions (receipts, adjustments)
        var hasTx = await txReadRepository.AnyAsync(new Specs.InventoryTransactionsByPurchaseOrderIdSpec(request.Id), cancellationToken).ConfigureAwait(false);
        if (hasTx)
            throw new ConflictException($"Purchase order '{request.Id}' cannot be deleted because related inventory transactions exist.");

        await repository.DeleteAsync(po, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("purchase order deleted {PurchaseOrderId}", po.Id);
    }
}
