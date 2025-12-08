using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionActions.Search.v1;

public sealed class SearchCollectionActionsHandler(
    [FromKeyedServices("microfinance:collectionactions")] IReadRepository<CollectionAction> repository)
    : IRequestHandler<SearchCollectionActionsCommand, PagedList<CollectionActionSummaryResponse>>
{
    public async Task<PagedList<CollectionActionSummaryResponse>> Handle(
        SearchCollectionActionsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCollectionActionsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CollectionActionSummaryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
