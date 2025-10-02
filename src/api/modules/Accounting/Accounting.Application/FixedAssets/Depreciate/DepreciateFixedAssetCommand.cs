namespace Accounting.Application.FixedAssets.Depreciate;

/// <summary>
/// Command to record depreciation for a fixed asset.
/// </summary>
public sealed record DepreciateFixedAssetCommand(
    DefaultIdType FixedAssetId,
    decimal DepreciationAmount,
    DateTime DepreciationDate,
    string? DepreciationMethod = null
) : IRequest<DefaultIdType>;
