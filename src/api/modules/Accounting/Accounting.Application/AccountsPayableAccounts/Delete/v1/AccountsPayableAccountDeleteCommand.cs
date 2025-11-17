namespace Accounting.Application.AccountsPayableAccounts.Delete.v1;

/// <summary>
/// Command to delete an existing accounts payable account.
/// </summary>
public sealed record AccountsPayableAccountDeleteCommand(DefaultIdType Id) : IRequest;

