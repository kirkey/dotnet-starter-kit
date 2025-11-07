namespace Accounting.Application.AccountsReceivableAccounts.RecordWriteOff.v1;

public sealed record RecordWriteOffCommand(DefaultIdType Id, decimal Amount) : IRequest<DefaultIdType>;

