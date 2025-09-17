using Accounting.Domain.Events.ChartOfAccount;

namespace Accounting.Application.ChartOfAccounts.EventHandlers;

public class UpdatedChartOfAccountHandler(ILogger<UpdatedChartOfAccountHandler> logger) : INotificationHandler<ChartOfAccountUpdated>
{
    public async Task Handle(ChartOfAccountUpdated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("handling account updated domain event..");
        await Task.FromResult(notification).ConfigureAwait(false);
        logger.LogInformation("finished handling account updated domain event..");
    }
}
