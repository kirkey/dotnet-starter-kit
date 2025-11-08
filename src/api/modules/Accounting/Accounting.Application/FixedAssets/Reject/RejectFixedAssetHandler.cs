using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Application.FixedAssets.Reject;

/// <summary>
/// Handler for rejecting a fixed asset acquisition.
/// Validates the asset exists and is in a state that can be rejected,
/// then marks it as rejected with the rejector's information and reason.
/// </summary>
public sealed class RejectFixedAssetHandler(
    ILogger<RejectFixedAssetHandler> logger,
    [FromKeyedServices("accounting:fixedassets")] IRepository<FixedAsset> repository)
    : IRequestHandler<RejectFixedAssetCommand, DefaultIdType>
{
    /// <summary>
    /// Handles the rejection of a fixed asset.
    /// </summary>
    /// <param name="request">The reject fixed asset command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The ID of the rejected fixed asset.</returns>
    /// <exception cref="FixedAssetNotFoundException">Thrown when the fixed asset is not found.</exception>
    /// <exception cref="FixedAssetAlreadyDisposedException">Thrown when the asset is already disposed.</exception>
    public async Task<DefaultIdType> Handle(RejectFixedAssetCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var fixedAsset = await repository.GetByIdAsync(request.FixedAssetId, cancellationToken)
            ?? throw new FixedAssetNotFoundException(request.FixedAssetId);

        // Reject the fixed asset using domain method
        fixedAsset.Reject(request.RejectedBy, request.Reason);

        await repository.UpdateAsync(fixedAsset, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Fixed Asset {AssetName} (ID: {AssetId}) rejected by {RejectedBy}. Reason: {Reason}",
            fixedAsset.AssetName,
            fixedAsset.Id,
            request.RejectedBy,
            request.Reason ?? "Not specified");

        return fixedAsset.Id;
    }
}
