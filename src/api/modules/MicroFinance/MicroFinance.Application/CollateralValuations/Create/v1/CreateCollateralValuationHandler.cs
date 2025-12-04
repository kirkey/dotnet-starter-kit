using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Create.v1;

public sealed class CreateCollateralValuationHandler(
    [FromKeyedServices("microfinance:collateralvaluations")] IRepository<CollateralValuation> repository,
    ILogger<CreateCollateralValuationHandler> logger)
    : IRequestHandler<CreateCollateralValuationCommand, CreateCollateralValuationResponse>
{
    public async Task<CreateCollateralValuationResponse> Handle(
        CreateCollateralValuationCommand request,
        CancellationToken cancellationToken)
    {
        var valuation = CollateralValuation.Create(
            request.CollateralId,
            request.ValuationReference,
            request.ValuationDate,
            request.ValuationMethod,
            request.MarketValue,
            request.ForcedSaleValue,
            request.InsurableValue,
            request.AppraiserName,
            request.AppraiserCompany,
            request.PreviousValue);

        await repository.AddAsync(valuation, cancellationToken);

        logger.LogInformation("Collateral valuation created: {ValuationId} for collateral {CollateralId}",
            valuation.Id, request.CollateralId);

        return new CreateCollateralValuationResponse(valuation.Id);
    }
}
