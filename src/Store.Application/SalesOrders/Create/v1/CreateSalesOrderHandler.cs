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

        // Validate customer exists
        var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken).ConfigureAwait(false);
        _ = customer ?? throw new CustomerNotFoundException(request.CustomerId);

        // Build an order number using the customer id (or fallback to a new guid string)
        var orderNumber = $"SO-{request.CustomerId}";

        // Create the sales order (domain will set derived totals based on items)
        var so = SalesOrder.Create(orderNumber, request.CustomerId, DateTime.UtcNow);

        await repository.AddAsync(so, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Sales order created {SalesOrderId} for customer {CustomerId}", so.Id, request.CustomerId);
        return new CreateSalesOrderResponse(so.Id);
    }
}
