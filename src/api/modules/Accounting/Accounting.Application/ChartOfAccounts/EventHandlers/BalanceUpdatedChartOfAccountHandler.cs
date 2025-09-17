using Accounting.Domain.Events.ChartOfAccount;

namespace Accounting.Application.ChartOfAccounts.EventHandlers;

public class BalanceUpdatedChartOfAccountHandler(ILogger<BalanceUpdatedChartOfAccountHandler> logger) : INotificationHandler<ChartOfAccountBalanceUpdated>
{
    public async Task Handle(ChartOfAccountBalanceUpdated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("handling account balance updated domain event: {Balance}", notification.Balance);
        await Task.FromResult(notification).ConfigureAwait(false);
        logger.LogInformation("finished handling account balance updated domain event..");
    }
}
