namespace Accounting.Application.ChartOfAccounts.Deactivate.v1;

/// <summary>
/// Command to deactivate a chart of account.
/// </summary>
public sealed record DeactivateChartOfAccountCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

