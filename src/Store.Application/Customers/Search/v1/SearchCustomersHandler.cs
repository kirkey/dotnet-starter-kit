using FSH.Starter.WebApi.Store.Application.Customers.Get.v1;
using FSH.Starter.WebApi.Store.Application.Customers.Specs;


namespace FSH.Starter.WebApi.Store.Application.Customers.Search.v1;

public sealed class SearchCustomersHandler(
    [FromKeyedServices("store:customers")] IReadRepository<Customer> repository)
    : IRequestHandler<SearchCustomersCommand, PagedList<CustomerResponse>>
{
    public async Task<PagedList<CustomerResponse>> Handle(SearchCustomersCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCustomersSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CustomerResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
