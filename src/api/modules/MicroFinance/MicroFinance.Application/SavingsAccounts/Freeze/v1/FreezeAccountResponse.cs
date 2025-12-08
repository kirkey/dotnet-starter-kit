namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Freeze.v1;

/// <summary>
/// Response after freezing account.
/// </summary>
public sealed record FreezeAccountResponse(DefaultIdType AccountId, string Status, string Message);
