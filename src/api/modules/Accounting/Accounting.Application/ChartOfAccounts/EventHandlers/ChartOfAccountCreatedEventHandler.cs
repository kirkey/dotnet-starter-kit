using Accounting.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.ChartOfAccounts.EventHandlers;

public class ChartOfAccountCreatedEventHandler(ILogger<ChartOfAccountCreatedEventHandler> logger) : INotificationHandler<ChartOfAccountCreated>
{
    public async Task Handle(ChartOfAccountCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling account created domain event..");
        await Task.FromResult(notification).ConfigureAwait(false);
        logger.LogInformation("finished handling account created domain event..");
    }
}
