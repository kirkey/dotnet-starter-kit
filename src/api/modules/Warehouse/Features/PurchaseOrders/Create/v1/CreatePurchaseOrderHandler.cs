using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.PurchaseOrders.Create.v1;

public sealed class CreatePurchaseOrderHandler(
    ILogger<CreatePurchaseOrderHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<CreatePurchaseOrderCommand, CreatePurchaseOrderResponse>
{
    public async Task<CreatePurchaseOrderResponse> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = PurchaseOrder.Create(request.OrderNumber, request.OrderDate, request.ExpectedDeliveryDate, request.SubTotal, request.TaxAmount, request.TotalAmount, request.Notes, request.CreatedByName, request.SupplierId, request.WarehouseId);
        await repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("purchase order created {PurchaseOrderId}", entity.Id);
        return new CreatePurchaseOrderResponse(entity.Id);
    }
}

