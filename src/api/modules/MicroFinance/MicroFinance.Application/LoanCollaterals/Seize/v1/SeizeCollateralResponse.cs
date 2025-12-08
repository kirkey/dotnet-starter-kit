namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Seize.v1;

/// <summary>
/// Response after seizing a collateral.
/// </summary>
public sealed record SeizeCollateralResponse(DefaultIdType Id, string Status, string Message);
