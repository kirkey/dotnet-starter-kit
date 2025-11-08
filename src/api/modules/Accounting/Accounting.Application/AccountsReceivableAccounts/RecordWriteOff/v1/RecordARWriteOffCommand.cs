namespace Accounting.Application.AccountsReceivableAccounts.RecordWriteOff.v1;

/// <summary>
/// Command to record a write-off for an accounts receivable account.
/// </summary>
public sealed record RecordARWriteOffCommand(DefaultIdType Id, decimal Amount) : IRequest<DefaultIdType>;

