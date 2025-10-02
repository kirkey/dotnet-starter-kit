namespace Accounting.Application.FixedAssets.Create;

/// <summary>
/// Command to create a new Fixed Asset.
/// </summary>
public sealed record CreateFixedAssetCommand(
    string AssetName,
    DateTime PurchaseDate,
    decimal PurchasePrice,
    DefaultIdType DepreciationMethodId,
    int ServiceLife,
    decimal SalvageValue,
    DefaultIdType AccumulatedDepreciationAccountId,
    DefaultIdType DepreciationExpenseAccountId,
    string AssetType,
    string? SerialNumber = null,
    string? Location = null,
    string? Department = null,
    string? Description = null,
    string? Notes = null
) : IRequest<CreateFixedAssetResponse>;
