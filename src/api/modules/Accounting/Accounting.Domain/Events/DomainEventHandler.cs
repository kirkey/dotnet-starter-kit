namespace Accounting.Domain.Events;

public abstract class DomainEventHandler<TEvent>(
    ILogger logger,
    ICacheService cache,
    string cachePrefix)
    : INotificationHandler<TEvent>
    where TEvent : class, INotification
{
    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {EventType} domain event..", typeof(TEvent).Name);
        await cache.SetAsync($"{cachePrefix}:{typeof(TEvent).Name}", notification, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
