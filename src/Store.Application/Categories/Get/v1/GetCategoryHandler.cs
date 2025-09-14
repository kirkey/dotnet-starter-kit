using Store.Domain.Exceptions.Category;

namespace FSH.Starter.WebApi.Store.Application.Categories.Get.v1;

public sealed class GetCategoryHandler(
    [FromKeyedServices("store:categories")] IReadRepository<Category> repository,
    ICacheService cache)
    : IRequestHandler<GetCategoryRequest, CategoryResponse>
{
    public async Task<CategoryResponse> Handle(GetCategoryRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"category:{request.Id}",
            async () =>
            {
                var spec = new GetCategorySpecs(request.Id);
                var response = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false) ??
                               throw new CategoryNotFoundException(request.Id);
                return response;
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return item!;
    }
}

