using Store.Domain.Exceptions.Customer;
using Store.Domain.Exceptions.SalesOrder;

namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Update.v1;

public sealed class UpdateSalesOrderHandler(
    ILogger<UpdateSalesOrderHandler> logger,
    [FromKeyedServices("store:sales-orders")] IRepository<SalesOrder> repository,
    [FromKeyedServices("store:customers")] IReadRepository<Customer> customerRepository)
    : IRequestHandler<UpdateSalesOrderCommand, UpdateSalesOrderResponse>
{
    public async Task<UpdateSalesOrderResponse> Handle(UpdateSalesOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var so = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = so ?? throw new SalesOrderNotFoundException(request.Id);

        // Ensure we have a non-null CustomerId to pass to repository/aggregate
        var cid = request.CustomerId;
        var customer = await customerRepository.GetByIdAsync(cid, cancellationToken).ConfigureAwait(false);
        _ = customer ?? throw new CustomerNotFoundException(cid);

        var updated = so.Update(so.OrderNumber, cid, so.OrderDate, so.DeliveryDate, so.Status, so.OrderType, so.PaymentStatus, so.PaymentMethod, so.DeliveryAddress, so.IsUrgent, so.SalesPersonId, so.WarehouseId);

        await repository.UpdateAsync(updated, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("sales order with id : {SalesOrderId} updated.", so.Id);
        return new UpdateSalesOrderResponse(so.Id);
    }
}
