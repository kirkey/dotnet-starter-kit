using Accounting.Domain.Entities;
using FixedAssetAlreadyDisposedException = Accounting.Application.FixedAssets.Exceptions.FixedAssetAlreadyDisposedException;
using FixedAssetNotFoundException = Accounting.Application.FixedAssets.Exceptions.FixedAssetNotFoundException;

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
