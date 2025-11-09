using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.FixedAssets.Approve;

/// <summary>
/// Handler for approving a fixed asset acquisition.
/// Validates the asset exists and is in a state that can be approved,
/// then marks it as approved with the approver's information from the current user session.
/// </summary>
public sealed class ApproveFixedAssetHandler(
    ILogger<ApproveFixedAssetHandler> logger,
    ICurrentUser currentUser,
    [FromKeyedServices("accounting:fixedassets")] IRepository<FixedAsset> repository)
    : IRequestHandler<ApproveFixedAssetCommand, DefaultIdType>
{
    /// <summary>
    /// Handles the approval of a fixed asset.
    /// </summary>
    /// <param name="request">The approve fixed asset command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the approved fixed asset.</returns>
    /// <exception cref="FixedAssetNotFoundException">Thrown when the fixed asset is not found.</exception>
    /// <exception cref="FixedAssetAlreadyApprovedException">Thrown when the asset is already approved.</exception>
    /// <exception cref="FixedAssetAlreadyDisposedException">Thrown when the asset is already disposed.</exception>
    public async Task<DefaultIdType> Handle(ApproveFixedAssetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var fixedAsset = await repository.GetByIdAsync(request.FixedAssetId, cancellationToken)
            ?? throw new FixedAssetNotFoundException(request.FixedAssetId);

        var approverId = currentUser.GetUserId();
        var approverName = currentUser.GetUserName();
        
        // Approve the fixed asset using domain method
        fixedAsset.Approve(approverId, approverName);

        await repository.UpdateAsync(fixedAsset, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Fixed Asset {AssetName} (ID: {AssetId}) approved by {ApprovedBy}",
            fixedAsset.AssetName,
            fixedAsset.Id,
            approverName);

        return fixedAsset.Id;
    }
}
