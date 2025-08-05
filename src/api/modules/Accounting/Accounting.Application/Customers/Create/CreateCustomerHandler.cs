using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Customers.Create;

public sealed class CreateCustomerHandler(
    [FromKeyedServices("accounting")] IRepository<Customer> repository)
    : IRequestHandler<CreateCustomerRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

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
