using Accounting.Application.Customers.Dtos;
using Accounting.Domain;
using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.Customers.Search;

public sealed class SearchCustomersHandler(
    [FromKeyedServices("accounting:customers")] IReadRepository<Customer> repository)
    : IRequestHandler<SearchCustomersRequest, PagedList<CustomerDto>>
{
    public async Task<PagedList<CustomerDto>> Handle(SearchCustomersRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCustomersSpec(request);
        var list = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CustomerDto>(list, request.PageNumber, request.PageSize, totalCount);
    }
}


