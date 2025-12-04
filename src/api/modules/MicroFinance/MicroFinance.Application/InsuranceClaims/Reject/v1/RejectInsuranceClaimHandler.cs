using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Reject.v1;

/// <summary>
/// Handler for rejecting an insurance claim.
/// </summary>
public sealed class RejectInsuranceClaimHandler(
    [FromKeyedServices("microfinance:insuranceclaims")] IRepository<InsuranceClaim> repository,
    ILogger<RejectInsuranceClaimHandler> logger) : IRequestHandler<RejectInsuranceClaimCommand, RejectInsuranceClaimResponse>
{
    public async Task<RejectInsuranceClaimResponse> Handle(RejectInsuranceClaimCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var claim = await repository.FirstOrDefaultAsync(new InsuranceClaimByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Insurance claim with ID {request.Id} not found.");

        claim.Reject(request.RejectedById, request.Reason);

        await repository.UpdateAsync(claim, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Insurance claim {ClaimNumber} rejected: {Reason}", claim.ClaimNumber, request.Reason);

        return new RejectInsuranceClaimResponse(claim.Id, claim.Status);
    }
}
