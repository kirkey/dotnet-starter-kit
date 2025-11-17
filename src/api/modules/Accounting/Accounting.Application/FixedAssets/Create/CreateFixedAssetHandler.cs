namespace Accounting.Application.FixedAssets.Create;

public sealed class CreateFixedAssetHandler(
    [FromKeyedServices("accounting:fixed-assets")] IRepository<FixedAsset> repository)
    : IRequestHandler<CreateFixedAssetCommand, CreateFixedAssetResponse>
{
    public async Task<CreateFixedAssetResponse> Handle(CreateFixedAssetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var fixedAsset = FixedAsset.Create(
            request.AssetName,
            request.PurchaseDate,
            request.PurchasePrice,
            request.DepreciationMethodId,
            request.ServiceLife,
            request.SalvageValue,
            request.AccumulatedDepreciationAccountId,
            request.DepreciationExpenseAccountId,
            request.AssetType,
            request.SerialNumber,
            request.Location,
            request.Department,
            request.GpsCoordinates,
            request.SubstationName,
            request.AssetUsoaId,
            request.RegulatoryClassification,
            request.VoltageRating,
            request.Capacity,
            request.Manufacturer,
            request.ModelNumber,
            request.RequiresUsoaReporting,
            request.Description,
            request.Notes,
            request.ImageUrl);

        await repository.AddAsync(fixedAsset, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateFixedAssetResponse(fixedAsset.Id);
    }
}
