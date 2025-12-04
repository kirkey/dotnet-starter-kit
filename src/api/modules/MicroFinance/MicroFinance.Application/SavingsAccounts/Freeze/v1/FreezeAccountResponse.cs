namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Freeze.v1;

/// <summary>
/// Response after freezing account.
/// </summary>
public sealed record FreezeAccountResponse(Guid AccountId, string Status, string Message);
