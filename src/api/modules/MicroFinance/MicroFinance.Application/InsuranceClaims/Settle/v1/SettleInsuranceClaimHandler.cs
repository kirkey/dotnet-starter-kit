using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Settle.v1;

/// <summary>
/// Handler for settling an approved insurance claim.
/// </summary>
public sealed class SettleInsuranceClaimHandler(
    [FromKeyedServices("microfinance:insuranceclaims")] IRepository<InsuranceClaim> repository,
    ILogger<SettleInsuranceClaimHandler> logger) : IRequestHandler<SettleInsuranceClaimCommand, SettleInsuranceClaimResponse>
{
    public async Task<SettleInsuranceClaimResponse> Handle(SettleInsuranceClaimCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var claim = await repository.FirstOrDefaultAsync(new InsuranceClaimByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Insurance claim with ID {request.Id} not found.");

        var paidAmount = claim.ApprovedAmount ?? 0;
        claim.Pay(paidAmount, request.PaymentReference ?? $"PAY-{DateTime.UtcNow:yyyyMMdd}", request.SettlementDate);

        await repository.UpdateAsync(claim, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Insurance claim {ClaimNumber} settled on {SettlementDate}", claim.ClaimNumber, request.SettlementDate);

        return new SettleInsuranceClaimResponse(claim.Id, paidAmount, request.SettlementDate, claim.Status);
    }
}
