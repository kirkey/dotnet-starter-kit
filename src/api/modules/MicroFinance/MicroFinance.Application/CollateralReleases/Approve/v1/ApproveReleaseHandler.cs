using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Get.v1;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Approve.v1;

public sealed class ApproveReleaseHandler(
    [FromKeyedServices("microfinance:collateralreleases")] IRepository<CollateralRelease> repository,
    ILogger<ApproveReleaseHandler> logger)
    : IRequestHandler<ApproveReleaseCommand, ApproveReleaseResponse>
{
    public async Task<ApproveReleaseResponse> Handle(
        ApproveReleaseCommand request,
        CancellationToken cancellationToken)
    {
        var release = await repository.FirstOrDefaultAsync(
            new CollateralReleaseByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Collateral release {request.Id} not found");

        release.Approve(request.ApprovedById);
        await repository.UpdateAsync(release, cancellationToken);

        logger.LogInformation("Collateral release approved: {ReleaseId}", release.Id);

        return new ApproveReleaseResponse(release.Id, release.Status, release.ApprovedDate!.Value);
    }
}
