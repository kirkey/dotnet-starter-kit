using Accounting.Domain;
using Accounting.Application.FixedAssets.Dtos;
using Accounting.Application.FixedAssets.Exceptions;
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return item!;
    }
}
