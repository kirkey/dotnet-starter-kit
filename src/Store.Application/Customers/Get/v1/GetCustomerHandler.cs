using Accounting.Application.Customers.Exceptions;



namespace FSH.Starter.WebApi.Store.Application.Customers.Get.v1;

public sealed class GetCustomerHandler(
    [FromKeyedServices("store:customers")] IReadRepository<Customer> repository,
    ICacheService cache)
    : IRequestHandler<GetCustomerRequest, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"customer:{request.Id}",
            async () =>
            {
                var spec = new GetCustomerSpecs(request.Id);
                var response = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false) ?? 
                               throw new CustomerNotFoundException(request.Id);
                return response;
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return item!;
    }
}
