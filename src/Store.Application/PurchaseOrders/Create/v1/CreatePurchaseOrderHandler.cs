using Store.Domain.Exceptions.Supplier;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Create.v1;

public sealed class CreatePurchaseOrderHandler(
    ILogger<CreatePurchaseOrderHandler> logger,
    [FromKeyedServices("store:purchase-orders")] IRepository<PurchaseOrder> repository,
    [FromKeyedServices("store:suppliers")] IReadRepository<Supplier> supplierRepository)
    : IRequestHandler<CreatePurchaseOrderCommand, CreatePurchaseOrderResponse>
{
    public async Task<CreatePurchaseOrderResponse> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify supplier exists
        var supplier = await supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken).ConfigureAwait(false);
        _ = supplier ?? throw new SupplierNotFoundException(request.SupplierId);

        var po = PurchaseOrder.Create(
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

        await repository.AddAsync(po, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Purchase order created {PurchaseOrderId}", po.Id);
        return new CreatePurchaseOrderResponse(po.Id);
    }
}

