namespace FSH.Starter.WebApi.Store.Application.Customers.Create.v1;

public sealed class CreateCustomerHandler(
    ILogger<CreateCustomerHandler> logger,
    [FromKeyedServices("store:customers")] IRepository<Customer> repository)
    : IRequestHandler<CreateCustomerCommand, CreateCustomerResponse>
{
    public async Task<CreateCustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var customer = Customer.Create(
            request.Name!,
            request.Description,
            request.Code!,
            request.CustomerType!,
            request.ContactPerson!,
            request.Email!,
            request.Phone!,
            request.Address!,
            request.City!,
            request.State,
            request.Country!,
            request.PostalCode,
            request.CreditLimit,
            request.PaymentTermsDays,
            request.DiscountPercentage,
            request.TaxNumber,
            request.BusinessLicense,
            request.Notes);

        await repository.AddAsync(customer, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("customer created {CustomerId}", customer.Id);
        return new CreateCustomerResponse(customer.Id);
    }
}
