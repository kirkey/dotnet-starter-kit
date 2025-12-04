using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Cancel.v1;

/// <summary>
/// Handler for cancelling an insurance policy.
/// </summary>
public sealed class CancelInsurancePolicyHandler(
    [FromKeyedServices("microfinance:insurancepolicies")] IRepository<InsurancePolicy> repository,
    ILogger<CancelInsurancePolicyHandler> logger) : IRequestHandler<CancelInsurancePolicyCommand, CancelInsurancePolicyResponse>
{
    public async Task<CancelInsurancePolicyResponse> Handle(CancelInsurancePolicyCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var policy = await repository.FirstOrDefaultAsync(new InsurancePolicyByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Insurance policy with ID {request.Id} not found.");

        policy.Cancel(request.Reason);

        await repository.UpdateAsync(policy, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Insurance policy {PolicyNumber} cancelled: {Reason}", policy.PolicyNumber, request.Reason);

        return new CancelInsurancePolicyResponse(policy.Id, policy.Status);
    }
}
