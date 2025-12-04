using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.UpdateValuation.v1;

/// <summary>
/// Command to update the valuation of a loan collateral.
/// </summary>
public sealed record UpdateCollateralValuationCommand(
    Guid Id,
    decimal EstimatedValue,
    decimal? ForcedSaleValue = null,
    DateOnly? ValuationDate = null) : IRequest<UpdateCollateralValuationResponse>;
