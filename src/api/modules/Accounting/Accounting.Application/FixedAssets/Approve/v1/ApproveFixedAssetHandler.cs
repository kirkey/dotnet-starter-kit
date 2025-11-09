using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.FixedAssets.Approve.v1;

/// <summary>
/// Handler to approve a fixed asset.
/// </summary>
public sealed class ApproveFixedAssetHandler(
    ICurrentUser currentUser,
    IRepository<FixedAsset> repository)
    : IRequestHandler<ApproveFixedAssetCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ApproveFixedAssetCommand request, CancellationToken cancellationToken)
    {
        var asset = await repository.GetByIdAsync(request.FixedAssetId, cancellationToken)
            ?? throw new NotFoundException($"Fixed asset {request.FixedAssetId} not found");

        var approverId = currentUser.GetUserId();
        var approverName = currentUser.GetUserName();
        
        asset.Approve(approverId, approverName);
        await repository.UpdateAsync(asset, cancellationToken);

        return asset.Id;
    }
}

