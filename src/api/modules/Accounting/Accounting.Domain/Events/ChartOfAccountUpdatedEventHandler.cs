using FSH.Framework.Core.Caching;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounting.Domain.Events;
public class ChartOfAccountUpdatedEventHandler(
    ILogger<ChartOfAccountUpdatedEventHandler> logger,
    ICacheService cache)
    : INotificationHandler<ChartOfAccountUpdated>
{
    public async Task Handle(ChartOfAccountUpdated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling account updated domain event..");
        await cache.SetAsync($"account:{notification.ChartOfAccount.Id}", notification.ChartOfAccount, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
