
using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Domain.Events;
using FSH.Starter.WebApi.Todo.Features.Get.v1;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Todo.Domain.Events;
public record TodoItemCreated(DefaultIdType Id, string Name, string Description, string Notes) : DomainEvent;

public class TodoItemCreatedEventHandler(
    ILogger<TodoItemCreatedEventHandler> logger,
    ICacheService cache)
    : INotificationHandler<TodoItemCreated>
{
    public async Task Handle(TodoItemCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("handling todo item created domain event..");
        var cacheResponse = new GetTodoResponse(notification.Id, notification.Name, notification.Description, notification.Notes);
        await cache.SetAsync($"todo:{notification.Id}", cacheResponse, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
