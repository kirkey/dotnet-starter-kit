using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Get.v1;

/// <summary>
/// Handler for retrieving an insurance claim by ID.
/// </summary>
public sealed class GetInsuranceClaimHandler(
    [FromKeyedServices("microfinance:insuranceclaims")] IReadRepository<InsuranceClaim> repository)
    : IRequestHandler<GetInsuranceClaimRequest, InsuranceClaimResponse>
{
    public async Task<InsuranceClaimResponse> Handle(GetInsuranceClaimRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var claim = await repository.FirstOrDefaultAsync(new InsuranceClaimByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Insurance claim with ID {request.Id} not found.");

        return new InsuranceClaimResponse(
            claim.Id,
            claim.InsurancePolicyId,
            claim.ClaimNumber,
            claim.ClaimType,
            claim.ClaimAmount,
            claim.ApprovedAmount,
            claim.IncidentDate,
            claim.FiledDate,
            claim.Status,
            claim.Description,
            claim.RejectionReason,
            claim.PaymentDate);
    }
}
