using Store.Domain.Exceptions.SalesOrder;

namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Items.Remove.v1;

public sealed class RemoveSalesOrderItemHandler(
    ILogger<RemoveSalesOrderItemHandler> logger,
    [FromKeyedServices("store:sales-orders")] IRepository<SalesOrder> repository)
    : IRequestHandler<RemoveSalesOrderItemCommand>
{
    public async Task Handle(RemoveSalesOrderItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var so = await repository.GetByIdAsync(request.SalesOrderId, cancellationToken).ConfigureAwait(false);
        _ = so ?? throw new SalesOrderNotFoundException(request.SalesOrderId);

        var updated = so.RemoveItem(request.GroceryItemId);

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Removed item from sales order {SalesOrderId}", so.Id);
    }
}

