using Accounting.Domain;
using Accounting.Application.Customers.Exceptions;
using Accounting.Application.Customers.Queries;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Customers.Create;

public sealed class CreateCustomerHandler(
    [FromKeyedServices("accounting:customers")] IRepository<Customer> repository)
    : IRequestHandler<CreateCustomerRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // check duplicates by code
        var existingByCode = await repository.FirstOrDefaultAsync(
            new CustomerByCodeSpec(request.CustomerCode), cancellationToken);
        if (existingByCode != null)
        {
            throw new CustomerCodeAlreadyExistsException(request.CustomerCode);
        }

        // optional duplicate by name
        var existingByName = await repository.FirstOrDefaultAsync(
            new CustomerByNameSpec(request.Name), cancellationToken);
        if (existingByName != null)
        {
            throw new CustomerNameAlreadyExistsException(request.Name);
        }

        var customer = Customer.Create(
            request.CustomerCode,
            request.Name,
            request.Address,
            request.BillingAddress,
            request.ContactPerson,
            request.Email,
            request.Terms,
            request.RevenueAccountCode,
            request.RevenueAccountName,
            request.Tin,
            request.PhoneNumber,
            request.CreditLimit,
            request.Description,
            request.Notes);

        await repository.AddAsync(customer, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}
