using FixedAssetNotFoundException = Accounting.Application.FixedAssets.Exceptions.FixedAssetNotFoundException;

namespace Accounting.Application.FixedAssets.Update;

public sealed class UpdateFixedAssetHandler(
    [FromKeyedServices("accounting:fixedassets")] IRepository<FixedAsset> repository)
    : IRequestHandler<UpdateFixedAssetCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(UpdateFixedAssetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var asset = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (asset == null) throw new FixedAssetNotFoundException(request.Id);

        asset.Update(
            request.AssetName,
            request.Location,
            request.Department,
            request.SerialNumber,
            request.GpsCoordinates,
            request.SubstationName,
            request.RegulatoryClassification,
            request.VoltageRating,
            request.Capacity,
            request.Manufacturer,
            request.ModelNumber,
            request.RequiresUsoaReporting,
            request.Description,
            request.Notes);

        await repository.UpdateAsync(asset, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return asset.Id;
    }
}
