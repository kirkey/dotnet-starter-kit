using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Create.v1;

public sealed class CreateCollateralReleaseHandler(
    [FromKeyedServices("microfinance:collateralreleases")] IRepository<CollateralRelease> repository,
    ILogger<CreateCollateralReleaseHandler> logger)
    : IRequestHandler<CreateCollateralReleaseCommand, CreateCollateralReleaseResponse>
{
    public async Task<CreateCollateralReleaseResponse> Handle(
        CreateCollateralReleaseCommand request,
        CancellationToken cancellationToken)
    {
        var release = CollateralRelease.Create(
            request.CollateralId,
            request.LoanId,
            request.ReleaseReference,
            request.RequestedById,
            request.ReleaseMethod);

        await repository.AddAsync(release, cancellationToken);

        logger.LogInformation("Collateral release created: {ReleaseId} for collateral {CollateralId}",
            release.Id, request.CollateralId);

        return new CreateCollateralReleaseResponse(release.Id);
    }
}
