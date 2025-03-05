using FSH.Framework.Core.Caching;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounting.Domain.Events;
public class AccountCreatedEventHandler(
    ILogger<AccountCreatedEventHandler> logger,
    ICacheService cache)
    : INotificationHandler<AccountCreated>
{
    public async Task Handle(AccountCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling account created domain event..");
        await cache.SetAsync($"account:{notification.Id}", notification, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
