using Accounting.Domain;
using Accounting.Application.FixedAssets.Dtos;
using Accounting.Application.FixedAssets.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.FixedAssets.Get;

public sealed class GetFixedAssetHandler(
    [FromKeyedServices("accounting")] IReadRepository<FixedAsset> repository)
    : IRequestHandler<GetFixedAssetRequest, FixedAssetDto>
{
    public async Task<FixedAssetDto> Handle(GetFixedAssetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var asset = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (asset == null) throw new FixedAssetNotFoundException(request.Id);

        return new FixedAssetDto(
            asset.Id,
            asset.AssetName,
            asset.PurchaseDate,
            asset.PurchasePrice,
            asset.ServiceLife,
            asset.DepreciationMethodId,
            asset.SalvageValue,
            asset.CurrentBookValue,
            asset.AccumulatedDepreciationAccountId,
            asset.DepreciationExpenseAccountId,
            asset.SerialNumber,
            asset.Location,
            asset.Department,
            asset.IsDisposed,
            asset.DisposalDate,
            asset.DisposalAmount,
            asset.Description,
            asset.Notes);
    }
}
