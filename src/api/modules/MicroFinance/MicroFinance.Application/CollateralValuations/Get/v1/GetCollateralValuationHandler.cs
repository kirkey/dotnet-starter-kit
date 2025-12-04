using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Get.v1;

public sealed class GetCollateralValuationHandler(
    [FromKeyedServices("microfinance:collateralvaluations")] IReadRepository<CollateralValuation> repository)
    : IRequestHandler<GetCollateralValuationRequest, CollateralValuationResponse>
{
    public async Task<CollateralValuationResponse> Handle(
        GetCollateralValuationRequest request,
        CancellationToken cancellationToken)
    {
        var valuation = await repository.FirstOrDefaultAsync(
            new CollateralValuationByIdSpec(request.Id), cancellationToken)
            ?? throw new NotFoundException($"Collateral valuation {request.Id} not found");

        return new CollateralValuationResponse(
            valuation.Id,
            valuation.CollateralId,
            valuation.ValuationReference,
            valuation.Status,
            valuation.ValuationDate,
            valuation.ExpiryDate,
            valuation.ValuationMethod,
            valuation.AppraiserName,
            valuation.AppraiserCompany,
            valuation.AppraiserLicense,
            valuation.MarketValue,
            valuation.ForcedSaleValue,
            valuation.InsurableValue,
            valuation.PreviousValue,
            valuation.ValueChange,
            valuation.ValueChangePercent,
            valuation.Condition,
            valuation.Notes,
            valuation.DocumentPath,
            valuation.ApprovedById,
            valuation.ApprovedDate,
            valuation.RejectionReason);
    }
}
