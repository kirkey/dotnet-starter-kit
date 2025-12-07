using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Seize.v1;

/// <summary>
/// Handler for seizing a collateral.
/// </summary>
public sealed class SeizeCollateralHandler(
    [FromKeyedServices("microfinance:loancollaterals")] IRepository<LoanCollateral> repository,
    ILogger<SeizeCollateralHandler> logger)
    : IRequestHandler<SeizeCollateralCommand, SeizeCollateralResponse>
{
    public async Task<SeizeCollateralResponse> Handle(SeizeCollateralCommand request, CancellationToken cancellationToken)
    {
        var collateral = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Loan collateral with ID {request.Id} not found.");

        collateral.Seize(request.Reason);

        await repository.UpdateAsync(collateral, cancellationToken);
        logger.LogInformation("Seized collateral {CollateralId}", request.Id);

        return new SeizeCollateralResponse(collateral.Id, collateral.Status, "Collateral seized successfully.");
    }
}
