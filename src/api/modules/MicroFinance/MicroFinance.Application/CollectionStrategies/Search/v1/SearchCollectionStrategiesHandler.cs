using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Search.v1;

public sealed class SearchCollectionStrategiesHandler(
    [FromKeyedServices("microfinance:collectionstrategies")] IReadRepository<CollectionStrategy> repository)
    : IRequestHandler<SearchCollectionStrategiesCommand, PagedList<CollectionStrategySummaryResponse>>
{
    public async Task<PagedList<CollectionStrategySummaryResponse>> Handle(
        SearchCollectionStrategiesCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCollectionStrategiesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CollectionStrategySummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
