using Accounting.Application.Customers.Queries;

namespace Accounting.Application.Customers.Update.v1;

/// <summary>
/// Handler for updating an existing customer.
/// </summary>
public sealed class CustomerUpdateHandler(
    ILogger<CustomerUpdateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Customer> repository)
    : IRequestHandler<CustomerUpdateCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var customer = await repository.FirstOrDefaultAsync(
            new CustomerByIdSpec(request.Id), cancellationToken);

        if (customer == null)
        {
            throw new CustomerByIdNotFoundException(request.Id);
        }

        customer.Update(
            customerName: request.CustomerName,
            billingAddress: request.BillingAddress,
            shippingAddress: request.ShippingAddress,
            email: request.Email,
            phone: request.Phone,
            contactName: request.ContactName,
            contactEmail: request.ContactEmail,
            contactPhone: request.ContactPhone,
            paymentTerms: request.PaymentTerms,
            taxExempt: request.TaxExempt,
            taxId: request.TaxId,
            discountPercentage: request.DiscountPercentage,
            salesRepresentative: request.SalesRepresentative,
            description: request.Description,
            notes: request.Notes);

        await repository.UpdateAsync(customer, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Customer updated {CustomerId}", customer.Id);
        return customer.Id;
    }
}

