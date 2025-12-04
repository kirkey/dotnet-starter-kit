using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Get.v1;

/// <summary>
/// Handler for retrieving an insurance policy by ID.
/// </summary>
public sealed class GetInsurancePolicyHandler(
    [FromKeyedServices("microfinance:insurancepolicies")] IReadRepository<InsurancePolicy> repository)
    : IRequestHandler<GetInsurancePolicyRequest, InsurancePolicyResponse>
{
    public async Task<InsurancePolicyResponse> Handle(GetInsurancePolicyRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var policy = await repository.FirstOrDefaultAsync(new InsurancePolicyByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Insurance policy with ID {request.Id} not found.");

        return new InsurancePolicyResponse(
            policy.Id,
            policy.MemberId,
            policy.InsuranceProductId,
            policy.PolicyNumber,
            policy.PremiumAmount,
            policy.CoverageAmount,
            policy.StartDate,
            policy.EndDate,
            policy.Status,
            policy.LoanId,
            policy.BeneficiaryName,
            policy.TotalPremiumPaid,
            policy.Claims.Count);
    }
}
