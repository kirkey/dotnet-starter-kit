namespace Accounting.Application.AccountsReceivableAccounts.UpdateBalance.v1;

public sealed record UpdateArBalanceCommand(
    DefaultIdType Id,
    decimal Current0to30,
    decimal Days31to60,
    decimal Days61to90,
    decimal Over90Days
) : IRequest<DefaultIdType>;
