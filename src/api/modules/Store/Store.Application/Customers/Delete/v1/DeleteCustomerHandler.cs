using Store.Domain.Exceptions.Customer;


namespace FSH.Starter.WebApi.Store.Application.Customers.Delete.v1;

public sealed class DeleteCustomerHandler(
    ILogger<DeleteCustomerHandler> logger,
    [FromKeyedServices("store:customers")] IRepository<Customer> repository)
    : IRequestHandler<DeleteCustomerCommand>
{
    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var customer = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = customer ?? throw new CustomerNotFoundException(request.Id);
        await repository.DeleteAsync(customer, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("customer with id : {CustomerId} successfully deleted", customer.Id);
    }
}
