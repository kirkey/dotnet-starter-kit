using Accounting.Application.FixedAssets.Dtos;
using FixedAssetNotFoundException = Accounting.Application.FixedAssets.Exceptions.FixedAssetNotFoundException;

namespace Accounting.Application.FixedAssets.Get;

public sealed class GetFixedAssetHandler(
    [FromKeyedServices("accounting:fixedassets")] IReadRepository<FixedAsset> repository,
    ICacheService cache)
    : IRequestHandler<GetFixedAssetRequest, FixedAssetDto>
{
    public async Task<FixedAssetDto> Handle(GetFixedAssetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"fixedasset:{request.Id}",
            async () =>
            {
                var asset = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (asset == null) throw new FixedAssetNotFoundException(request.Id);
                return asset.Adapt<FixedAssetDto>();
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
