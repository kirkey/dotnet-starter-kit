using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.Search.v1;

public sealed class SearchPromiseToPaysHandler(
    [FromKeyedServices("microfinance:promisetopays")] IReadRepository<PromiseToPay> repository)
    : IRequestHandler<SearchPromiseToPaysCommand, PagedList<PromiseToPaySummaryResponse>>
{
    public async Task<PagedList<PromiseToPaySummaryResponse>> Handle(
        SearchPromiseToPaysCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPromiseToPaysSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<PromiseToPaySummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
