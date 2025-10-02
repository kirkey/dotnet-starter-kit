using Accounting.Domain.Entities;

namespace Accounting.Application.FixedAssets.Depreciate;

/// <summary>
/// Handler for recording fixed asset depreciation.
/// </summary>
public sealed class DepreciateFixedAssetHandler(
    ILogger<DepreciateFixedAssetHandler> logger,
    [FromKeyedServices("accounting:fixedassets")] IRepository<FixedAsset> repository)
    : IRequestHandler<DepreciateFixedAssetCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(DepreciateFixedAssetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var fixedAsset = await repository.GetByIdAsync(request.FixedAssetId, cancellationToken);
        
        if (fixedAsset == null)
        {
            throw new NotFoundException($"Fixed asset with id {request.FixedAssetId} not found");
        }

        if (request.DepreciationAmount <= 0)
        {
            throw new ArgumentException("Depreciation amount must be positive.");
        }

        fixedAsset.AddDepreciation(request.DepreciationAmount, request.DepreciationDate, request.DepreciationMethod);

        await repository.UpdateAsync(fixedAsset, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Fixed asset {AssetId} depreciated by {Amount} using method {Method}", 
            request.FixedAssetId, request.DepreciationAmount, request.DepreciationMethod ?? "Not specified");

        return fixedAsset.Id;
    }
}
