using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Verify.v1;

/// <summary>
/// Handler for verifying a collateral.
/// </summary>
public sealed class VerifyCollateralHandler(
    IRepository<LoanCollateral> repository,
    ILogger<VerifyCollateralHandler> logger)
    : IRequestHandler<VerifyCollateralCommand, VerifyCollateralResponse>
{
    public async Task<VerifyCollateralResponse> Handle(VerifyCollateralCommand request, CancellationToken cancellationToken)
    {
        var collateral = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new Exception($"Loan collateral with ID {request.Id} not found.");

        collateral.Verify();

        await repository.UpdateAsync(collateral, cancellationToken);
        logger.LogInformation("Verified collateral {CollateralId}", request.Id);

        return new VerifyCollateralResponse(collateral.Id, collateral.Status, "Collateral verified successfully.");
    }
}
