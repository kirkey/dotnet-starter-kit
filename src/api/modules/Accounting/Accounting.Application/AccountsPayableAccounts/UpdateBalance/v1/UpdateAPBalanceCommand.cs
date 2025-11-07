namespace Accounting.Application.AccountsPayableAccounts.UpdateBalance.v1;

public sealed record UpdateAPBalanceCommand(
    DefaultIdType Id,
    decimal Current0to30,
    decimal Days31to60,
    decimal Days61to90,
    decimal Over90Days
) : IRequest<DefaultIdType>;
