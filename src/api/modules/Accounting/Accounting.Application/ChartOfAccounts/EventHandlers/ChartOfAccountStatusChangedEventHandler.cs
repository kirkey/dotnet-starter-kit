using Accounting.Domain.Events.ChartOfAccount;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.ChartOfAccounts.EventHandlers;

public class ChartOfAccountStatusChangedEventHandler(ILogger<ChartOfAccountStatusChangedEventHandler> logger) : INotificationHandler<ChartOfAccountStatusChanged>
{
    public async Task Handle(ChartOfAccountStatusChanged notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("handling account status changed domain event: {IsActive}", notification.IsActive);
        await Task.FromResult(notification).ConfigureAwait(false);
        logger.LogInformation("finished handling account status changed domain event..");
    }
}


