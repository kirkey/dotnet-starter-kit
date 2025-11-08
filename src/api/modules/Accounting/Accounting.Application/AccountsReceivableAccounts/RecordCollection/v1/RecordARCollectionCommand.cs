namespace Accounting.Application.AccountsReceivableAccounts.RecordCollection.v1;

/// <summary>
/// Command to record a collection (payment received) for an accounts receivable account.
/// </summary>
public sealed record RecordARCollectionCommand(DefaultIdType Id, decimal Amount) : IRequest<DefaultIdType>;

