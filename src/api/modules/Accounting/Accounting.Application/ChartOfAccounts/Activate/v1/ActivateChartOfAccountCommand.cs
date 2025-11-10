namespace Accounting.Application.ChartOfAccounts.Activate.v1;

/// <summary>
/// Command to activate a chart of account.
/// </summary>
public sealed record ActivateChartOfAccountCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

