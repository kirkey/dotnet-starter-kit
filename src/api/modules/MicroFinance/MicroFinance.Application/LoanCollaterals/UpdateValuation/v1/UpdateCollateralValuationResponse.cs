namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.UpdateValuation.v1;

/// <summary>
/// Response after updating collateral valuation.
/// </summary>
public sealed record UpdateCollateralValuationResponse(
    DefaultIdType Id,
    decimal EstimatedValue,
    decimal? ForcedSaleValue,
    DateOnly ValuationDate);
