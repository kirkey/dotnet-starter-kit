


using Store.Domain.Exceptions.GroceryItem;

namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Get.v1;

public sealed class GetGroceryItemHandler(
    [FromKeyedServices("store:grocery-items")] IReadRepository<GroceryItem> repository,
    ICacheService cache)
    : IRequestHandler<GetGroceryItemRequest, GroceryItemResponse>
{
    public async Task<GroceryItemResponse> Handle(GetGroceryItemRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"grocery-item:{request.Id}",
            async () =>
            {
                var spec = new GetGroceryItemSpecs(request.Id);
                var response = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false) ?? 
                               throw new GroceryItemNotFoundException(request.Id);
                return response;
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return item!;
    }
}
