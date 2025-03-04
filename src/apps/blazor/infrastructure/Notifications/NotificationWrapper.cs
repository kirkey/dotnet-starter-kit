using FSH.Starter.Blazor.Shared.Notifications;
using MediatR;

namespace FSH.Starter.Blazor.Infrastructure.Notifications;

public class NotificationWrapper<TNotificationMessage>(TNotificationMessage notification) : INotification
    where TNotificationMessage : INotificationMessage
{
    public TNotificationMessage Notification { get; } = notification;
}