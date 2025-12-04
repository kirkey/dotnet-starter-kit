using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Activate.v1;

/// <summary>
/// Handler for reinstating a lapsed insurance policy.
/// </summary>
public sealed class ActivateInsurancePolicyHandler(
    [FromKeyedServices("microfinance:insurancepolicies")] IRepository<InsurancePolicy> repository,
    ILogger<ActivateInsurancePolicyHandler> logger) : IRequestHandler<ActivateInsurancePolicyCommand, ActivateInsurancePolicyResponse>
{
    public async Task<ActivateInsurancePolicyResponse> Handle(ActivateInsurancePolicyCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var policy = await repository.FirstOrDefaultAsync(new InsurancePolicyByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Insurance policy with ID {request.Id} not found.");

        policy.Reinstate();

        await repository.UpdateAsync(policy, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Insurance policy {PolicyNumber} reinstated", policy.PolicyNumber);

        return new ActivateInsurancePolicyResponse(policy.Id, policy.Status);
    }
}
