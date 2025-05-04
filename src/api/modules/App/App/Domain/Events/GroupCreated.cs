using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Domain.Events;
using FSH.Starter.WebApi.App.Features.Get.v1;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.App.Domain.Events;

public record GroupCreated(
    DefaultIdType Id,
    string Application,
    string Parent,
    string? Tag,
    int Number,
    string Code,
    string Name,
    decimal Amount,
    DefaultIdType? EmployeeId,
    string? EmployeeName,
    string? Description,
    string? Notes) : DomainEvent;

public class GroupCreatedEventHandler(
    ILogger<GroupCreatedEventHandler> logger,
    ICacheService cache)
    : INotificationHandler<GroupCreated>
{
    public async Task Handle(GroupCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("handling App group created domain event..");
        var cacheResponse = new GroupGetResponse(notification.Id, notification.Application, notification.Parent,
            notification.Tag, notification.Number, notification.Code, notification.Name, notification.Amount,
            notification.EmployeeId, notification.EmployeeName, notification.Description, notification.Notes);
        await cache.SetAsync($"Group:{notification.Id}", cacheResponse, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
