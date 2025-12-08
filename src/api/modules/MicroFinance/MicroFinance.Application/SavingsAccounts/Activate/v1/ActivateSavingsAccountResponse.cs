namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Activate.v1;

/// <summary>
/// Response after activating a savings account.
/// </summary>
public sealed record ActivateSavingsAccountResponse(DefaultIdType SavingsAccountId, string Status, string Message);
