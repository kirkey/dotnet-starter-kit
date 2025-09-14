using Store.Domain.Exceptions.SalesOrder;

namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Items.UpdateQuantity.v1;

public sealed class UpdateSalesOrderItemQuantityHandler(
    ILogger<UpdateSalesOrderItemQuantityHandler> logger,
    [FromKeyedServices("store:sales-orders")] IRepository<SalesOrder> repository)
    : IRequestHandler<UpdateSalesOrderItemQuantityCommand>
{
    public async Task Handle(UpdateSalesOrderItemQuantityCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var so = await repository.FirstOrDefaultAsync(new Specs.SalesOrderByItemIdSpec(request.SalesOrderItemId), cancellationToken).ConfigureAwait(false);
        _ = so ?? throw new SalesOrderNotFoundException(request.SalesOrderItemId);

        var updated = so.UpdateItemQuantity(request.SalesOrderItemId, request.Quantity);
        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Updated sales order item quantity for order {SalesOrderId}", so.Id);
    }
}

