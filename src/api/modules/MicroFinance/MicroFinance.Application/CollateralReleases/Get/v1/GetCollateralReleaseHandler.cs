using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Get.v1;

public sealed class GetCollateralReleaseHandler(
    [FromKeyedServices("microfinance:collateralreleases")] IReadRepository<CollateralRelease> repository)
    : IRequestHandler<GetCollateralReleaseRequest, CollateralReleaseResponse>
{
    public async Task<CollateralReleaseResponse> Handle(
        GetCollateralReleaseRequest request,
        CancellationToken cancellationToken)
    {
        var release = await repository.FirstOrDefaultAsync(
            new CollateralReleaseByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Collateral release {request.Id} not found");

        return new CollateralReleaseResponse(
            release.Id,
            release.CollateralId,
            release.LoanId,
            release.ReleaseReference,
            release.Status,
            release.RequestDate,
            release.RequestedById,
            release.ReleaseMethod,
            release.RecipientName,
            release.RecipientIdNumber,
            release.RecipientContact,
            release.ApprovedDate,
            release.ApprovedById,
            release.ReleasedDate,
            release.ReleasedById,
            release.RejectionReason,
            release.Notes,
            release.ReleaseDocumentPath,
            release.DocumentsReturned,
            release.RegistrationCleared);
    }
}
