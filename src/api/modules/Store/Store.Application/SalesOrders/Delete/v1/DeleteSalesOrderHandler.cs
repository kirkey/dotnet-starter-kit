using Store.Domain.Exceptions.SalesOrder;

namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Delete.v1;

public sealed class DeleteSalesOrderHandler(
    ILogger<DeleteSalesOrderHandler> logger,
    [FromKeyedServices("store:sales-orders")] IRepository<SalesOrder> repository)
    : IRequestHandler<DeleteSalesOrderCommand>
{
    public async Task Handle(DeleteSalesOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var so = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = so ?? throw new SalesOrderNotFoundException(request.Id);

        await repository.DeleteAsync(so, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("sales order deleted {SalesOrderId}", so.Id);
    }
}

