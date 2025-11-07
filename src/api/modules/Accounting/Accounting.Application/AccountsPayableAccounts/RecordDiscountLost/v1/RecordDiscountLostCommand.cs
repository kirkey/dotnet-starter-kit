namespace Accounting.Application.AccountsPayableAccounts.RecordDiscountLost.v1;

public sealed record RecordDiscountLostCommand(DefaultIdType Id, decimal DiscountAmount) : IRequest<DefaultIdType>;

