using Accounting.Domain;
using Accounting.Application.Customers.Dtos;
using Accounting.Application.Customers.Exceptions;
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Customers.Get;

public sealed class GetCustomerHandler(
    [FromKeyedServices("accounting:customers")] IReadRepository<Customer> repository,
    ICacheService cache)
    : IRequestHandler<GetCustomerRequest, CustomerDto>
{
    public async Task<CustomerDto> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"customer:{request.Id}",
            async () =>
            {
                var customer = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
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
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
