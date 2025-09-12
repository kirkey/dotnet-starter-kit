

namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Search.v1;

public sealed class SearchGroceryItemsHandler(
    [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> repository)
    : IRequestHandler<SearchGroceryItemsCommand, PagedList<GroceryItemResponse>>
{
    public async Task<PagedList<GroceryItemResponse>> Handle(SearchGroceryItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchGroceryItemsSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<GroceryItemResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
