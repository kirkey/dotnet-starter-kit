using Store.Domain.Exceptions.Customer;

namespace FSH.Starter.WebApi.Store.Application.Customers.Activate.v1;

/// <summary>
/// Handler to activate a customer account.
/// </summary>
public sealed class ActivateCustomerHandler(
    [FromKeyedServices("store:customers")] IRepository<Customer> repository,
    ILogger<ActivateCustomerHandler> logger)
    : IRequestHandler<ActivateCustomerCommand, ActivateCustomerResponse>
{
    public async Task<ActivateCustomerResponse> Handle(ActivateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
                        ?? throw new CustomerNotFoundException(request.Id);
        customer.Activate();
        await repository.UpdateAsync(customer, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Customer {CustomerId} activated", request.Id);
        return new ActivateCustomerResponse(request.Id);
    }
}

