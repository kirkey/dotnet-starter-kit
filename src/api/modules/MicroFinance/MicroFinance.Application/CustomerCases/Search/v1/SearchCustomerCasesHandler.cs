using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CustomerCases.Search.v1;

public sealed class SearchCustomerCasesHandler(
    [FromKeyedServices("microfinance:customercases")] IReadRepository<CustomerCase> repository)
    : IRequestHandler<SearchCustomerCasesCommand, PagedList<CustomerCaseSummaryResponse>>
{
    public async Task<PagedList<CustomerCaseSummaryResponse>> Handle(
        SearchCustomerCasesCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCustomerCasesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CustomerCaseSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
