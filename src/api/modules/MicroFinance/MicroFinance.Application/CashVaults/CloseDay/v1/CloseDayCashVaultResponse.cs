namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.CloseDay.v1;

/// <summary>
/// Response after closing a cash vault for the day.
/// </summary>
public sealed record CloseDayCashVaultResponse(DefaultIdType CashVaultId, decimal VerifiedBalance, string Message);
