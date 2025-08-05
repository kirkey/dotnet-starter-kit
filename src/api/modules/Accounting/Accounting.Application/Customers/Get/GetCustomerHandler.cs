using Accounting.Domain;
using Accounting.Application.Customers.Dtos;
using Accounting.Application.Customers.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Customers.Get;

public sealed class GetCustomerHandler(
    [FromKeyedServices("accounting")] IReadRepository<Customer> repository)
    : IRequestHandler<GetCustomerRequest, CustomerDto>
{
    public async Task<CustomerDto> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var customer = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (customer == null) throw new CustomerNotFoundException(request.Id);

        return new CustomerDto(
            customer.Id,
            customer.CustomerCode,
            customer.Name!,
            customer.Address,
            customer.BillingAddress,
            customer.ContactPerson,
            customer.Email,
            customer.Terms,
            customer.RevenueAccountCode,
            customer.RevenueAccountName,
            customer.Tin,
            customer.PhoneNumber,
            customer.IsActive,
            customer.CreditLimit,
            customer.CurrentBalance,
            customer.Description,
            customer.Notes);
    }
}
