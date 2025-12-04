using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.RecordPremium.v1;

/// <summary>
/// Handler for recording a premium payment.
/// </summary>
public sealed class RecordPremiumPaymentHandler(
    [FromKeyedServices("microfinance:insurancepolicies")] IRepository<InsurancePolicy> repository,
    ILogger<RecordPremiumPaymentHandler> logger) : IRequestHandler<RecordPremiumPaymentCommand, RecordPremiumPaymentResponse>
{
    public async Task<RecordPremiumPaymentResponse> Handle(RecordPremiumPaymentCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var policy = await repository.FirstOrDefaultAsync(new InsurancePolicyByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Insurance policy with ID {request.Id} not found.");

        policy.RecordPremiumPayment(request.Amount);

        await repository.UpdateAsync(policy, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Premium payment of {Amount} recorded for policy {PolicyNumber}", request.Amount, policy.PolicyNumber);

        return new RecordPremiumPaymentResponse(policy.Id, request.Amount, policy.TotalPremiumPaid);
    }
}
