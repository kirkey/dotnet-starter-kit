using Store.Domain.Exceptions.Customer;



namespace FSH.Starter.WebApi.Store.Application.Customers.Update.v1;

public sealed class UpdateCustomerHandler(
    ILogger<UpdateCustomerHandler> logger,
    [FromKeyedServices("store:customers")] IRepository<Customer> repository)
    : IRequestHandler<UpdateCustomerCommand, UpdateCustomerResponse>
{
    public async Task<UpdateCustomerResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var customer = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        _ = customer ?? throw new CustomerNotFoundException(request.Id);
        var updatedCustomer = customer.Update(
            request.Name,
            request.Description,
            request.Code,
            request.CustomerType,
            request.ContactPerson,
            request.Email,
            request.Phone,
            request.Address,
            request.City,
            request.State,
            request.Country,
            request.PostalCode,
            request.CreditLimit,
            request.PaymentTermsDays,
            request.DiscountPercentage,
            request.TaxNumber,
            request.BusinessLicense,
            request.Notes);
        await repository.UpdateAsync(updatedCustomer, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("customer with id : {CustomerId} updated.", customer.Id);
        return new UpdateCustomerResponse(customer.Id);
    }
}
