namespace Accounting.Application.FixedAssets.Dtos;

public record FixedAssetDto(
    DefaultIdType Id,
    string AssetName,
    DateTime PurchaseDate,
    decimal PurchasePrice,
    int ServiceLife,
    DefaultIdType DepreciationMethodId,
    decimal SalvageValue,
    decimal CurrentBookValue,
    DefaultIdType AccumulatedDepreciationAccountId,
    DefaultIdType DepreciationExpenseAccountId,
    string? SerialNumber,
    string? Location,
    string? Department,
    bool IsDisposed,
    DateTime? DisposalDate,
    decimal? DisposalAmount,
    string? Description,
    string? Notes);

public record DepreciationEntryDto(
    DefaultIdType Id,
    DefaultIdType AssetId,
    DateTime DepreciationDate,
    decimal DepreciationAmount,
    string Method,
    string? Notes);
