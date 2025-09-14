using Store.Domain.Exceptions.Customer;

namespace FSH.Starter.WebApi.Store.Application.SalesOrders.Create.v1;

public sealed class CreateSalesOrderHandler(
    ILogger<CreateSalesOrderHandler> logger,
    [FromKeyedServices("store:sales-orders")] IRepository<SalesOrder> repository,
    [FromKeyedServices("store:customers")] IReadRepository<Customer> customerRepository)
    : IRequestHandler<CreateSalesOrderCommand, CreateSalesOrderResponse>
{
    public async Task<CreateSalesOrderResponse> Handle(CreateSalesOrderCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Basic validation of customer existence is performed in handler
        var customer = await customerRepository.GetByIdAsync(request.CustomerId ?? default, cancellationToken).ConfigureAwait(false);
        _ = customer ?? throw new CustomerNotFoundException(request.CustomerId ?? default);

        // Create minimal sales order; more fields can be set by extending the command
        var orderNumber = request.CustomerId.HasValue ? $"SO-{request.CustomerId.Value}" : $"SO-{DefaultIdType.NewGuid()}";

        var so = SalesOrder.Create(orderNumber, request.CustomerId ?? DefaultIdType.NewGuid(), DateTime.UtcNow);

        await repository.AddAsync(so, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Sales order created {SalesOrderId}", so.Id);
        return new CreateSalesOrderResponse(so.Id);
    }
}
