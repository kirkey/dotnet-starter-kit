using FSH.Framework.Core.Exceptions;
using Store.Domain.Exceptions.PurchaseOrder;
using Store.Domain.Exceptions.Supplier;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Update.v1;

public sealed class UpdatePurchaseOrderHandler(
    ILogger<UpdatePurchaseOrderHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository,
    [FromKeyedServices("store:suppliers")] IReadRepository<Supplier> supplierRepository)
    : IRequestHandler<UpdatePurchaseOrderCommand, UpdatePurchaseOrderResponse>
{
    public async Task<UpdatePurchaseOrderResponse> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var po = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = po ?? throw new PurchaseOrderNotFoundException(request.Id);

        // verify supplier exists and active
        var supplier = await supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken).ConfigureAwait(false);
        _ = supplier ?? throw new SupplierNotFoundException(request.SupplierId);
        if (!supplier.IsActive)
            throw new ConflictException($"Supplier '{supplier.Id}' is inactive and cannot accept purchase orders.");

        var updated = po.Update(
            request.OrderNumber,
            request.SupplierId,
            request.OrderDate,
            request.ExpectedDeliveryDate,
            request.Status,
            request.Notes,
            request.DeliveryAddress,
            request.ContactPerson,
            request.ContactPhone,
            request.IsUrgent);

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("purchase order with id : {PurchaseOrderId} updated.", po.Id);
        return new UpdatePurchaseOrderResponse(po.Id);
    }
}
