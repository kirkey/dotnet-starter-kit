using FSH.Framework.Core.Caching;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounting.Domain.Events;
public class AccountUpdatedEventHandler(
    ILogger<AccountUpdatedEventHandler> logger,
    ICacheService cache)
    : INotificationHandler<AccountUpdated>
{
    public async Task Handle(AccountUpdated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling account updated domain event..");
        await cache.SetAsync($"account:{notification.Account.Id}", notification.Account, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
