namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Unfreeze.v1;

/// <summary>
/// Response after unfreezing account.
/// </summary>
public sealed record UnfreezeAccountResponse(Guid AccountId, string Status, string Message);
