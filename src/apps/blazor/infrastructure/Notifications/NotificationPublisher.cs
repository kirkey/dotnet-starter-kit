using FSH.Starter.Blazor.Shared.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.Blazor.Infrastructure.Notifications;

public class NotificationPublisher(ILogger<NotificationPublisher> logger, IPublisher mediator) : INotificationPublisher
{
    public Task PublishAsync(INotificationMessage notification)
    {
        logger.LogInformation("Publishing Notification : {notification}", notification.GetType().Name);
        return mediator.Publish(CreateNotificationWrapper(notification));
    }

    private static INotification CreateNotificationWrapper(INotificationMessage notification) =>
        (INotification)Activator.CreateInstance(
            typeof(NotificationWrapper<>).MakeGenericType(notification.GetType()), notification)!;
}
