using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using FSH.Starter.WebApi.Warehouse.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Features.PurchaseOrders.Update.v1;

public sealed class UpdatePurchaseOrderHandler(
    ILogger<UpdatePurchaseOrderHandler> logger,
    [FromKeyedServices("warehouse")] IRepository<PurchaseOrder> repository)
    : IRequestHandler<UpdatePurchaseOrderCommand, UpdatePurchaseOrderResponse>
{
    public async Task<UpdatePurchaseOrderResponse> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = entity ?? throw new PurchaseOrderNotFoundException(request.Id);
        entity.Update(request.OrderNumber, request.OrderDate, request.ExpectedDeliveryDate, request.ActualDeliveryDate, request.Status, request.SubTotal, request.TaxAmount, request.TotalAmount, request.Notes);
        await repository.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("purchase order updated {PurchaseOrderId}", entity.Id);
        return new UpdatePurchaseOrderResponse(entity.Id);
    }
}

