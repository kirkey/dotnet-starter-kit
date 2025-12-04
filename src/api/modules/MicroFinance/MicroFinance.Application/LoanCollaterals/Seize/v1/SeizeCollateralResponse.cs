namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Seize.v1;

/// <summary>
/// Response after seizing a collateral.
/// </summary>
public sealed record SeizeCollateralResponse(Guid Id, string Status, string Message);
