using Accounting.Application.Customers.Exceptions;
using Accounting.Application.Customers.Queries;

namespace Accounting.Application.Customers.Update;

public sealed class UpdateCustomerHandler(
    [FromKeyedServices("accounting:customers")] IRepository<Customer> repository)
    : IRequestHandler<UpdateCustomerCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var customer = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (customer == null) throw new CustomerNotFoundException(request.Id);

        // Check for duplicate customer code (excluding current customer)
        if (!string.IsNullOrEmpty(request.CustomerCode) && request.CustomerCode != customer.CustomerCode)
        {
            var existingByCode = await repository.FirstOrDefaultAsync(
                new CustomerByCodeSpec(request.CustomerCode, request.Id), cancellationToken);
            if (existingByCode != null)
            {
                throw new CustomerCodeAlreadyExistsException(request.CustomerCode);
            }
        }

        // Check for duplicate name (excluding current customer)
        if (!string.IsNullOrEmpty(request.Name) && !string.Equals(request.Name, customer.Name, StringComparison.OrdinalIgnoreCase))
        {
            var existingByName = await repository.FirstOrDefaultAsync(
                new CustomerByNameSpec(request.Name, request.Id), cancellationToken);
            if (existingByName != null)
            {
                throw new CustomerNameAlreadyExistsException(request.Name);
            }
        }

        customer.Update(request.CustomerCode, request.Name, request.Address, 
                       request.BillingAddress, request.ContactPerson, request.Email, 
                       request.Terms, request.RevenueAccountCode, request.RevenueAccountName,
                       request.Tin, request.PhoneNumber, request.CreditLimit, 
                       request.Description, request.Notes);

        await repository.UpdateAsync(customer, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
