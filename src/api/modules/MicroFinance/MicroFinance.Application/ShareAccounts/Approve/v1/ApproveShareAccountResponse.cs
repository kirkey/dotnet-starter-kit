namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Approve.v1;

/// <summary>
/// Response after approving a share account.
/// </summary>
public sealed record ApproveShareAccountResponse(DefaultIdType ShareAccountId, string Status, string Message);
