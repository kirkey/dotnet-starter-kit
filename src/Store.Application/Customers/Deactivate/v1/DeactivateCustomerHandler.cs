using Store.Domain.Exceptions.Customer;

namespace FSH.Starter.WebApi.Store.Application.Customers.Deactivate.v1;

/// <summary>
/// Handler to deactivate a customer account.
/// </summary>
public sealed class DeactivateCustomerHandler(
    [FromKeyedServices("store:customers")] IRepository<Customer> repository,
    ILogger<DeactivateCustomerHandler> logger)
    : IRequestHandler<DeactivateCustomerCommand, DeactivateCustomerResponse>
{
    public async Task<DeactivateCustomerResponse> Handle(DeactivateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
                        ?? throw new CustomerNotFoundException(request.Id);
        customer.Deactivate();
        await repository.UpdateAsync(customer, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Customer {CustomerId} deactivated", request.Id);
        return new DeactivateCustomerResponse(request.Id);
    }
}

