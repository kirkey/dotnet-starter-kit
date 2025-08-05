using MediatR;

namespace Accounting.Application.FixedAssets.Create;

public record CreateFixedAssetRequest(
    string AssetName,
    DateTime PurchaseDate,
    decimal PurchasePrice,
    DefaultIdType DepreciationMethodId,
    int ServiceLife,
    decimal SalvageValue,
    DefaultIdType AccumulatedDepreciationAccountId,
    DefaultIdType DepreciationExpenseAccountId,
    string? SerialNumber = null,
    string? Location = null,
    string? Department = null,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
