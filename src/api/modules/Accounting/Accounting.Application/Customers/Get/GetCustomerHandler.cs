using Accounting.Domain;
using Accounting.Application.Customers.Dtos;
using Accounting.Application.Customers.Exceptions;
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Persistence;
using Mapster;
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
                return customer.Adapt<CustomerDto>();
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
