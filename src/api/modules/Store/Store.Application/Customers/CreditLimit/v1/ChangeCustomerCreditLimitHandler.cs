using Store.Domain.Exceptions.Customer;

namespace FSH.Starter.WebApi.Store.Application.Customers.CreditLimit.v1;

/// <summary>
/// Handler to change a customer's credit limit.
/// </summary>
public sealed class ChangeCustomerCreditLimitHandler(
    [FromKeyedServices("store:customers")] IRepository<Customer> repository,
    ILogger<ChangeCustomerCreditLimitHandler> logger)
    : IRequestHandler<ChangeCustomerCreditLimitCommand, ChangeCustomerCreditLimitResponse>
{
    public async Task<ChangeCustomerCreditLimitResponse> Handle(ChangeCustomerCreditLimitCommand request, CancellationToken cancellationToken)
    {
        var customer = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
                        ?? throw new CustomerNotFoundException(request.Id);
        customer.ChangeCreditLimit(request.NewCreditLimit);
        await repository.UpdateAsync(customer, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Customer {CustomerId} credit limit changed to {Limit}", request.Id, request.NewCreditLimit);
        return new ChangeCustomerCreditLimitResponse(request.Id, customer.CreditLimit);
    }
}

