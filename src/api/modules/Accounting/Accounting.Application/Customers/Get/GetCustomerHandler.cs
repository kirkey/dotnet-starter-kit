using Accounting.Application.Customers.Dtos;
using Accounting.Application.Customers.Exceptions;

namespace Accounting.Application.Customers.Get;

public sealed class GetCustomerHandler(
    [FromKeyedServices("accounting:customers")] IReadRepository<Customer> repository,
    ICacheService cache)
    : IRequestHandler<GetCustomerQuery, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"customer:{request.Id}",
            async () =>
            {
                var customer = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (customer == null) throw new CustomerNotFoundException(request.Id);
                return customer.Adapt<CustomerResponse>();
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
