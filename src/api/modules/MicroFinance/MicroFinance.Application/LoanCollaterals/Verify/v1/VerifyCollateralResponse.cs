namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.Verify.v1;

/// <summary>
/// Response after verifying a collateral.
/// </summary>
public sealed record VerifyCollateralResponse(DefaultIdType Id, string Status, string Message);
