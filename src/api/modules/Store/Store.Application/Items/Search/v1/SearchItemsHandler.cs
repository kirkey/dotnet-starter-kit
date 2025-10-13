using FSH.Starter.WebApi.Store.Application.Items.Specs;

namespace FSH.Starter.WebApi.Store.Application.Items.Search.v1;

public sealed class SearchItemsHandler(
    [FromKeyedServices("store:items")] IReadRepository<Item> repository)
    : IRequestHandler<SearchItemsCommand, PagedList<ItemResponse>>
{
    public async Task<PagedList<ItemResponse>> Handle(SearchItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchItemsSpec(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<ItemResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
