namespace Accounting.Application.AccountsReceivableAccounts.UpdateAllowance.v1;

public sealed record UpdateARAllowanceCommand(DefaultIdType Id, decimal AllowanceAmount) : IRequest<DefaultIdType>;

