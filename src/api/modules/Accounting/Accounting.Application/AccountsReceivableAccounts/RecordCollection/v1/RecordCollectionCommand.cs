namespace Accounting.Application.AccountsReceivableAccounts.RecordCollection.v1;

public sealed record RecordCollectionCommand(DefaultIdType Id, decimal Amount) : IRequest<DefaultIdType>;

