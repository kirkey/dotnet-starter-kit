using FSH.Starter.WebApi.Store.Application.Categories.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.Categories.Search.v1;

public sealed class SearchCategoriesHandler(
    [FromKeyedServices("store:categories")] IReadRepository<Category> repository)
    : IRequestHandler<SearchCategoriesCommand, PagedList<CategoryResponse>>
{
    public async Task<PagedList<CategoryResponse>> Handle(SearchCategoriesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchCategoriesSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<CategoryResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}

