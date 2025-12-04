using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Create.v1;

/// <summary>
/// Handler for creating a new insurance policy.
/// </summary>
public sealed class CreateInsurancePolicyHandler(
    [FromKeyedServices("microfinance:insurancepolicies")] IRepository<InsurancePolicy> repository,
    ILogger<CreateInsurancePolicyHandler> logger) : IRequestHandler<CreateInsurancePolicyCommand, CreateInsurancePolicyResponse>
{
    public async Task<CreateInsurancePolicyResponse> Handle(CreateInsurancePolicyCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var policyNumber = $"INS-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpperInvariant()}";

        var policy = InsurancePolicy.Create(
            request.InsuranceProductId,
            request.MemberId,
            policyNumber,
            request.StartDate,
            request.EndDate,
            request.CoverageAmount,
            request.PremiumAmount,
            request.WaitingPeriodDays,
            request.LoanId,
            request.BeneficiaryName,
            request.BeneficiaryRelation);

        await repository.AddAsync(policy, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Insurance policy {PolicyNumber} created for member {MemberId}", policyNumber, request.MemberId);

        return new CreateInsurancePolicyResponse(policy.Id, policyNumber);
    }
}
