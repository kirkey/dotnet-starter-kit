using Accounting.Domain;
using Accounting.Application.FixedAssets.Exceptions;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.FixedAssets.Delete;

public sealed class DeleteFixedAssetHandler(
    [FromKeyedServices("accounting:fixedassets")] IRepository<FixedAsset> repository)
    : IRequestHandler<DeleteFixedAssetRequest>
{
    public async Task Handle(DeleteFixedAssetRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var asset = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (asset == null) throw new FixedAssetNotFoundException(request.Id);

        // Check if asset is already disposed - disposed assets should not be deleted
        if (asset.IsDisposed) throw new FixedAssetAlreadyDisposedException(request.Id);

        await repository.DeleteAsync(asset, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
