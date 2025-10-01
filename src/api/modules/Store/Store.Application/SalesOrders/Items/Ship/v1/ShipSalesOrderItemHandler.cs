using Store.Domain.Exceptions.SalesOrder;

namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Items.Ship.v1;

public sealed class ShipSalesOrderItemHandler(
    ILogger<ShipSalesOrderItemHandler> logger,
    [FromKeyedServices("store:sales-orders")] IRepository<SalesOrder> repository)
    : IRequestHandler<ShipSalesOrderItemCommand>
{
    public async Task Handle(ShipSalesOrderItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var so = await repository.FirstOrDefaultAsync(new Specs.SalesOrderByItemIdSpec(request.SalesOrderItemId), cancellationToken).ConfigureAwait(false);
        _ = so ?? throw new SalesOrderNotFoundException(request.SalesOrderItemId);

        var updated = so.ShipItemQuantity(request.SalesOrderItemId, request.ShippedQuantity);
        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Updated shipped quantity for sales order {SalesOrderId}", so.Id);
    }
}

