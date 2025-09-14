using Store.Domain.Exceptions.SalesOrder;

namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Items.Add.v1;

public sealed class AddSalesOrderItemHandler(
    ILogger<AddSalesOrderItemHandler> logger,
    [FromKeyedServices("store:sales-orders")] IRepository<SalesOrder> repository)
    : IRequestHandler<AddSalesOrderItemCommand, AddSalesOrderItemResponse>
{
    public async Task<AddSalesOrderItemResponse> Handle(AddSalesOrderItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var so = await repository.GetByIdAsync(request.SalesOrderId, cancellationToken).ConfigureAwait(false);
        _ = so ?? throw new SalesOrderNotFoundException(request.SalesOrderId);

        var updated = so.AddItem(request.GroceryItemId, request.Quantity, request.UnitPrice, request.Discount);
        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Added item to sales order {SalesOrderId}", so.Id);
        return new AddSalesOrderItemResponse(so.Id);
    }
}
