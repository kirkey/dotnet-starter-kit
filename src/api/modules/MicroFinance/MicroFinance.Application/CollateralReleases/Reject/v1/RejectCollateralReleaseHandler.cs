using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Reject.v1;

/// <summary>
/// Handler for rejecting a collateral release.
/// </summary>
public sealed class RejectCollateralReleaseHandler(
    ILogger<RejectCollateralReleaseHandler> logger,
    [FromKeyedServices("microfinance:collateralreleases")] IRepository<CollateralRelease> repository)
    : IRequestHandler<RejectCollateralReleaseCommand, RejectCollateralReleaseResponse>
{
    /// <summary>
    /// Handles the reject collateral release command.
    /// </summary>
    public async Task<RejectCollateralReleaseResponse> Handle(
        RejectCollateralReleaseCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var release = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new KeyNotFoundException($"Collateral release with ID {request.Id} not found.");

        release.Reject(request.Reason, request.RejectedById);

        await repository.UpdateAsync(release, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Collateral release {Id} rejected: {Reason}", release.Id, request.Reason);

        return new RejectCollateralReleaseResponse(
            release.Id,
            release.Status,
            request.Reason);
    }
}
