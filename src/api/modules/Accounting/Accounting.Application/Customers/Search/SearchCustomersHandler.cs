using Accounting.Application.Customers.Dtos;

namespace Accounting.Application.Customers.Search;

public sealed class SearchCustomersHandler(
    [FromKeyedServices("accounting:customers")] IReadRepository<Customer> repository)
    : IRequestHandler<SearchCustomersQuery, PagedList<CustomerResponse>>
{
    public async Task<PagedList<CustomerResponse>> Handle(SearchCustomersQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCustomersSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CustomerResponse>(list, request.PageNumber, request.PageSize, totalCount);
    }
}
