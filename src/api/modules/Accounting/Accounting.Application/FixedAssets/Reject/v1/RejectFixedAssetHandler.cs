namespace Accounting.Application.FixedAssets.Reject.v1;

/// <summary>
/// Handler to reject a fixed asset.
/// </summary>
public sealed class RejectFixedAssetHandler(IRepository<FixedAsset> repository)
    : IRequestHandler<RejectFixedAssetCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(RejectFixedAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await repository.GetByIdAsync(request.FixedAssetId, cancellationToken)
            ?? throw new NotFoundException($"Fixed asset {request.FixedAssetId} not found");

        asset.Reject(request.RejectedBy, request.Reason);
        await repository.UpdateAsync(asset, cancellationToken);

        return asset.Id;
    }
}

