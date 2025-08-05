using Accounting.Domain;
using Accounting.Application.FixedAssets.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.FixedAssets.Update;

public sealed class UpdateFixedAssetHandler(
    [FromKeyedServices("accounting")] IRepository<FixedAsset> repository)
    : IRequestHandler<UpdateFixedAssetRequest, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateFixedAssetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var asset = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (asset == null) throw new FixedAssetNotFoundException(request.Id);

        asset.Update(request.AssetName, request.Location, request.Department,
                    request.SerialNumber, null, null, null, null, null,
                    null, null, null, request.Description, request.Notes);

        await repository.UpdateAsync(asset, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return asset.Id;
    }
}
