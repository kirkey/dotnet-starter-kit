namespace Accounting.Application.FixedAssets.Create;

public sealed class CreateFixedAssetHandler(
    [FromKeyedServices("accounting:fixedassets")] IRepository<FixedAsset> repository)
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
            request.SerialNumber,
            request.Location,
            request.Department,
            request.Description,
            request.Notes);

        await repository.AddAsync(fixedAsset, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateFixedAssetResponse(fixedAsset.Id);
    }
}
