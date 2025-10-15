using Accounting.Application.FixedAssets.Responses;
using Accounting.Application.FixedAssets.Specs;
using Accounting.Domain.Entities;

namespace Accounting.Application.FixedAssets.Get;

/// <summary>
/// Handler for getting a fixed asset by ID.
/// Uses database-level projection for optimal performance with caching.
/// </summary>
public sealed class GetFixedAssetHandler(
    [FromKeyedServices("accounting:fixedassets")] IReadRepository<FixedAsset> repository,
    ICacheService cache)
    : IRequestHandler<GetFixedAssetRequest, FixedAssetResponse>
{
    /// <summary>
    /// Handles the get fixed asset request.
    /// </summary>
    /// <param name="request">The request containing the fixed asset ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The fixed asset response.</returns>
    /// <exception cref="FixedAssetNotFoundException">Thrown when fixed asset is not found.</exception>
    public async Task<FixedAssetResponse> Handle(GetFixedAssetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"fixedasset:{request.Id}",
            async () =>
            {
                var spec = new GetFixedAssetSpec(request.Id);
                return await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false)
                    ?? throw new FixedAssetNotFoundException(request.Id);
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
