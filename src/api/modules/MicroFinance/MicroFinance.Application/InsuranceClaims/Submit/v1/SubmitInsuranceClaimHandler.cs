using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Submit.v1;

/// <summary>
/// Handler for submitting a new insurance claim.
/// </summary>
public sealed class SubmitInsuranceClaimHandler(
    [FromKeyedServices("microfinance:insuranceclaims")] IRepository<InsuranceClaim> repository,
    ILogger<SubmitInsuranceClaimHandler> logger) : IRequestHandler<SubmitInsuranceClaimCommand, SubmitInsuranceClaimResponse>
{
    public async Task<SubmitInsuranceClaimResponse> Handle(SubmitInsuranceClaimCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var claimNumber = $"CLM-{DateTime.UtcNow:yyyyMMdd}-{DefaultIdType.NewGuid().ToString()[..8].ToUpperInvariant()}";

        var claim = InsuranceClaim.Create(
            request.PolicyId,
            claimNumber,
            request.ClaimType,
            request.IncidentDate,
            request.ClaimAmount,
            request.Description);

        claim.File();

        await repository.AddAsync(claim, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Insurance claim {ClaimNumber} submitted for policy {PolicyId}", claimNumber, request.PolicyId);

        return new SubmitInsuranceClaimResponse(claim.Id, claimNumber);
    }
}
