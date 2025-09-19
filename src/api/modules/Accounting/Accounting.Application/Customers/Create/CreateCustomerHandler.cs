using Accounting.Application.Customers.Exceptions;
using Accounting.Application.Customers.Queries;

namespace Accounting.Application.Customers.Create;

public sealed class CreateCustomerHandler(
    [FromKeyedServices("accounting:customers")] IRepository<Customer> repository)
    : IRequestHandler<CreateCustomerCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var customerCode = request.CustomerCode?.Trim() ?? string.Empty;
        var name = request.Name?.Trim() ?? string.Empty;
        var address = request.Address?.Trim();
        var billingAddress = request.BillingAddress?.Trim();
        var contactPerson = request.ContactPerson?.Trim();
        var email = request.Email?.Trim();
        var terms = request.Terms?.Trim();
        var revenueAccountCode = request.RevenueAccountCode?.Trim();
        var revenueAccountName = request.RevenueAccountName?.Trim();
        var tin = request.Tin?.Trim();
        var phoneNumber = request.PhoneNumber?.Trim();

        // check duplicates by code
        var existingByCode = await repository.FirstOrDefaultAsync(
            new CustomerByCodeSpec(customerCode), cancellationToken);
        if (existingByCode != null)
        {
            throw new CustomerCodeAlreadyExistsException(customerCode);
        }

        // optional duplicate by name
        var existingByName = await repository.FirstOrDefaultAsync(
            new CustomerByNameSpec(name), cancellationToken);
        if (existingByName != null)
        {
            throw new CustomerNameAlreadyExistsException(name);
        }

        var customer = Customer.Create(
            customerCode,
            name,
            address,
            billingAddress,
            contactPerson,
            email,
            terms,
            revenueAccountCode,
            revenueAccountName,
            tin,
            phoneNumber,
            request.CreditLimit,
            request.Description?.Trim(),
            request.Notes?.Trim());

        await repository.AddAsync(customer, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
