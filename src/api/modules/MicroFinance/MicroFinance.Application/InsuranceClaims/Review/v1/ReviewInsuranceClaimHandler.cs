using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Review.v1;

/// <summary>
/// Handler for reviewing an insurance claim.
/// </summary>
public sealed class ReviewInsuranceClaimHandler(
    [FromKeyedServices("microfinance:insuranceclaims")] IRepository<InsuranceClaim> repository,
    ILogger<ReviewInsuranceClaimHandler> logger) : IRequestHandler<ReviewInsuranceClaimCommand, ReviewInsuranceClaimResponse>
{
    public async Task<ReviewInsuranceClaimResponse> Handle(ReviewInsuranceClaimCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var claim = await repository.FirstOrDefaultAsync(new InsuranceClaimByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Insurance claim with ID {request.Id} not found.");

        claim.StartReview(request.ReviewerId);

        await repository.UpdateAsync(claim, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Insurance claim {ClaimNumber} under review by {ReviewerId}", claim.ClaimNumber, request.ReviewerId);

        return new ReviewInsuranceClaimResponse(claim.Id, claim.Status);
    }
}
