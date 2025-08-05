using MediatR;

namespace Accounting.Application.FixedAssets.Update;

public record UpdateFixedAssetRequest(
    DefaultIdType Id,
    string? AssetName = null,
    DefaultIdType? DepreciationMethodId = null,
    int? ServiceLife = null,
    decimal? SalvageValue = null,
    string? SerialNumber = null,
    string? Location = null,
    string? Department = null,
    string? Description = null,
    string? Notes = null) : IRequest<DefaultIdType>;
