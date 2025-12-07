using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Pledge.v1;

/// <summary>
/// Handler for pledging a collateral.
/// </summary>
public sealed class PledgeCollateralHandler(
    [FromKeyedServices("microfinance:loancollaterals")] IRepository<LoanCollateral> repository,
    ILogger<PledgeCollateralHandler> logger)
    : IRequestHandler<PledgeCollateralCommand, PledgeCollateralResponse>
{
    public async Task<PledgeCollateralResponse> Handle(PledgeCollateralCommand request, CancellationToken cancellationToken)
    {
        var collateral = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Loan collateral with ID {request.Id} not found.");

        collateral.Pledge();

        await repository.UpdateAsync(collateral, cancellationToken);
        logger.LogInformation("Pledged collateral {CollateralId}", request.Id);

        return new PledgeCollateralResponse(collateral.Id, collateral.Status, "Collateral pledged successfully.");
    }
}
