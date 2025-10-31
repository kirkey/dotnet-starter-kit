using Accounting.Application.FixedAssets.Responses;

namespace Accounting.Application.FixedAssets.Specs;

/// <summary>
/// Specification to retrieve a fixed asset by ID projected to <see cref="FixedAssetResponse"/>.
/// Performs database-level projection for optimal performance.
/// </summary>
public sealed class GetFixedAssetSpec : Specification<FixedAsset, FixedAssetResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetFixedAssetSpec"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the fixed asset to retrieve.</param>
    public GetFixedAssetSpec(DefaultIdType id)
    {
        Query.Where(f => f.Id == id);
    }
}

