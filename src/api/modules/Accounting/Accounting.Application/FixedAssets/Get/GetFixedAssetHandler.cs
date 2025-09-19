using Accounting.Application.FixedAssets.Responses;

namespace Accounting.Application.FixedAssets.Get;

public sealed class GetFixedAssetHandler(
    [FromKeyedServices("accounting:fixedassets")] IReadRepository<FixedAsset> repository,
    ICacheService cache)
    : IRequestHandler<GetFixedAssetRequest, FixedAssetResponse>
{
    public async Task<FixedAssetResponse> Handle(GetFixedAssetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"fixedasset:{request.Id}",
            async () =>
            {
                var asset = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (asset == null) throw new FixedAssetNotFoundException(request.Id);
                return asset;
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item.Adapt<FixedAssetResponse>();
    }
}
