
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Domain.Events;
using FSH.Starter.WebApi.Todo.Features.Get.v1;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Todo.Domain.Events;
public record TodoItemUpdated(TodoItem Item) : DomainEvent;

public class TodoItemUpdatedEventHandler(
    ILogger<TodoItemUpdatedEventHandler> logger,
    ICacheService cache)
    : INotificationHandler<TodoItemUpdated>
{
    public async Task Handle(TodoItemUpdated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("handling todo item update domain event..");
        var cacheResponse = new GetTodoResponse(notification.Item.Id, notification.Item.Title, notification.Item.Note);
        await cache.SetAsync($"todo:{notification.Item.Id}", cacheResponse, cancellationToken: cancellationToken);
    }
}
