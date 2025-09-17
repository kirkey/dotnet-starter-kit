using Accounting.Domain.Events.ChartOfAccount;

namespace Accounting.Application.ChartOfAccounts.EventHandlers;

public class StatusChangedChartOfAccountHandler(ILogger<StatusChangedChartOfAccountHandler> logger) : INotificationHandler<ChartOfAccountStatusChanged>
{
    public async Task Handle(ChartOfAccountStatusChanged notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("handling account status changed domain event: {IsActive}", notification.IsActive);
        await Task.FromResult(notification).ConfigureAwait(false);
        logger.LogInformation("finished handling account status changed domain event..");
    }
}
