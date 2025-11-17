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
    string? GpsCoordinates = null,
    string? SubstationName = null,
    DefaultIdType? AssetUsoaId = null,
    string? RegulatoryClassification = null,
    decimal? VoltageRating = null,
    decimal? Capacity = null,
    string? Manufacturer = null,
    string? ModelNumber = null,
    bool RequiresUsoaReporting = true,
    string? Description = null,
    string? Notes = null,
    string? ImageUrl = null
) : IRequest<CreateFixedAssetResponse>;
