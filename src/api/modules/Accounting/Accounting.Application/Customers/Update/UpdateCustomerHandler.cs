using Accounting.Domain;
using Accounting.Application.Customers.Exceptions;
using Accounting.Application.Customers.Queries;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Customers.Update;

public sealed class UpdateCustomerHandler(
    [FromKeyedServices("accounting")] IRepository<Customer> repository)
    : IRequestHandler<UpdateCustomerRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var customer = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (customer == null) throw new CustomerNotFoundException(request.Id);

        // Check for duplicate customer code (excluding current customer)
        if (!string.IsNullOrEmpty(request.CustomerCode) && request.CustomerCode != customer.CustomerCode)
        {
            var existingByCode = await repository.FirstOrDefaultAsync(
                new CustomerByCodeSpec(request.CustomerCode), cancellationToken);
            if (existingByCode != null)
            {
                throw new CustomerCodeAlreadyExistsException(request.CustomerCode);
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
