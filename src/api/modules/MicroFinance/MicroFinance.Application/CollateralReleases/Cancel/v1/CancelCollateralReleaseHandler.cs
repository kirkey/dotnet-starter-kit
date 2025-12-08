using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Cancel.v1;

/// <summary>
/// Handler for cancelling a collateral release.
/// </summary>
public sealed class CancelCollateralReleaseHandler(
    ILogger<CancelCollateralReleaseHandler> logger,
    [FromKeyedServices("microfinance:collateralreleases")] IRepository<CollateralRelease> repository)
    : IRequestHandler<CancelCollateralReleaseCommand, CancelCollateralReleaseResponse>
{
    /// <summary>
    /// Handles the cancel collateral release command.
    /// </summary>
    public async Task<CancelCollateralReleaseResponse> Handle(
        CancelCollateralReleaseCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var release = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new KeyNotFoundException($"Collateral release with ID {request.Id} not found.");

        release.Cancel(request.Reason);

        await repository.UpdateAsync(release, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Collateral release {Id} cancelled: {Reason}", release.Id, request.Reason);

        return new CancelCollateralReleaseResponse(
            release.Id,
            release.Status);
    }
}
