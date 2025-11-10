namespace Accounting.Application.ChartOfAccounts.UpdateBalance.v1;

/// <summary>
/// Command to update a chart of account balance.
/// </summary>
public sealed record UpdateChartOfAccountBalanceCommand(
    DefaultIdType Id,
    decimal NewBalance
) : IRequest<DefaultIdType>;

