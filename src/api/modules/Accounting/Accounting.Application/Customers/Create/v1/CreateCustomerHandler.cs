using Accounting.Application.Customers.Queries;

namespace Accounting.Application.Customers.Create.v1;

/// <summary>
/// Handler for creating a new customer.
/// </summary>
public sealed class CreateCustomerHandler(
    ILogger<CreateCustomerHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Customer> repository)
    : IRequestHandler<CreateCustomerCommand, CustomerCreateResponse>
{
    public async Task<CustomerCreateResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate customer number
        var existingByNumber = await repository.FirstOrDefaultAsync(
            new CustomerByNumberSpec(request.CustomerNumber), cancellationToken);
        if (existingByNumber != null)
        {
            throw new DuplicateCustomerNumberException(request.CustomerNumber);
        }

        var customer = Customer.Create(
            customerNumber: request.CustomerNumber,
            customerName: request.CustomerName,
            customerType: request.CustomerType,
            billingAddress: request.BillingAddress,
            shippingAddress: request.ShippingAddress,
            email: request.Email,
            phone: request.Phone,
            contactName: request.ContactName,
            creditLimit: request.CreditLimit,
            paymentTerms: request.PaymentTerms,
            taxExempt: request.TaxExempt,
            taxId: request.TaxId,
            discountPercentage: request.DiscountPercentage,
            defaultRateScheduleId: request.DefaultRateScheduleId,
            receivableAccountId: request.ReceivableAccountId,
            salesRepresentative: request.SalesRepresentative,
            description: request.Description,
            notes: request.Notes);

        await repository.AddAsync(customer, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Customer created {CustomerId} - {CustomerNumber}", customer.Id, customer.CustomerNumber);
        return new CustomerCreateResponse(customer.Id);
    }
}

