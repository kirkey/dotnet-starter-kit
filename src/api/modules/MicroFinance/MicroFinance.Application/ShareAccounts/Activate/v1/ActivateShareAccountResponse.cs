namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Activate.v1;

/// <summary>
/// Response after activating a share account.
/// </summary>
public sealed record ActivateShareAccountResponse(DefaultIdType ShareAccountId, string Status, string Message);
