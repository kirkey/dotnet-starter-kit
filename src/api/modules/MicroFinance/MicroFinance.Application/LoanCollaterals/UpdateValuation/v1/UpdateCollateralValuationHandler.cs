using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.UpdateValuation.v1;

/// <summary>
/// Handler for updating collateral valuation.
/// </summary>
public sealed class UpdateCollateralValuationHandler(
    [FromKeyedServices("microfinance:loancollaterals")] IRepository<LoanCollateral> repository,
    ILogger<UpdateCollateralValuationHandler> logger)
    : IRequestHandler<UpdateCollateralValuationCommand, UpdateCollateralValuationResponse>
{
    public async Task<UpdateCollateralValuationResponse> Handle(UpdateCollateralValuationCommand request, CancellationToken cancellationToken)
    {
        var collateral = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Loan collateral with ID {request.Id} not found.");

        collateral.UpdateValuation(
            request.EstimatedValue,
            request.ForcedSaleValue,
            request.ValuationDate);

        await repository.UpdateAsync(collateral, cancellationToken);
        logger.LogInformation("Updated valuation for collateral {CollateralId}", request.Id);

        return new UpdateCollateralValuationResponse(
            collateral.Id,
            collateral.EstimatedValue,
            collateral.ForcedSaleValue,
            collateral.ValuationDate);
    }
}
