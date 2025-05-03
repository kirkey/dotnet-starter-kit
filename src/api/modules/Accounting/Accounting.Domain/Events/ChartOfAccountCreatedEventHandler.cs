using FSH.Framework.Core.Caching;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounting.Domain.Events;
public class ChartOfAccountCreatedEventHandler(
    ILogger<ChartOfAccountCreatedEventHandler> logger,
    ICacheService cache)
    : INotificationHandler<ChartOfAccountCreated>
{
    public async Task Handle(ChartOfAccountCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling account created domain event..");
        await cache.SetAsync($"account:{notification.Id}", notification, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
