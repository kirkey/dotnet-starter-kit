namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Pledge.v1;

/// <summary>
/// Response after pledging a collateral.
/// </summary>
public sealed record PledgeCollateralResponse(Guid Id, string Status, string Message);
