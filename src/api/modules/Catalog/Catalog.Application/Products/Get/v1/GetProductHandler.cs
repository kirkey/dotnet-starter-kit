using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Catalog.Domain;
using FSH.Starter.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.Catalog.Application.Products.Get.v1;
public sealed class GetProductHandler(
    [FromKeyedServices("catalog:products")] IReadRepository<Product> repository,
    ICacheService cache)
    : IRequestHandler<GetProductRequest, ProductResponse>
{
    public async Task<ProductResponse> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"product:{request.Id}",
            async () =>
            {
                var spec = new GetProductSpecs(request.Id);
                var response = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false) ?? 
                               throw new ProductNotFoundException(request.Id);
                return response;
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return item!;
    }
}
