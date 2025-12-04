using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Approve.v1;

/// <summary>
/// Handler for approving an insurance claim.
/// </summary>
public sealed class ApproveInsuranceClaimHandler(
    [FromKeyedServices("microfinance:insuranceclaims")] IRepository<InsuranceClaim> repository,
    ILogger<ApproveInsuranceClaimHandler> logger) : IRequestHandler<ApproveInsuranceClaimCommand, ApproveInsuranceClaimResponse>
{
    public async Task<ApproveInsuranceClaimResponse> Handle(ApproveInsuranceClaimCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var claim = await repository.FirstOrDefaultAsync(new InsuranceClaimByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Insurance claim with ID {request.Id} not found.");

        claim.Approve(request.ApproverId, request.ApprovedAmount);

        await repository.UpdateAsync(claim, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Insurance claim {ClaimNumber} approved for {ApprovedAmount}", claim.ClaimNumber, request.ApprovedAmount);

        return new ApproveInsuranceClaimResponse(claim.Id, request.ApprovedAmount, claim.Status);
    }
}
