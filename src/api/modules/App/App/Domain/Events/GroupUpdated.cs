using FSH.Framework.Core.Caching;
using FSH.Framework.Core.Domain.Events;
using FSH.Starter.WebApi.App.Features.Get.v1;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.App.Domain.Events;

public record GroupUpdated(Group Item) : DomainEvent;

public class GroupUpdatedEventHandler(
    ILogger<GroupUpdatedEventHandler> logger,
    ICacheService cache)
    : INotificationHandler<GroupUpdated>
{
    public async Task Handle(GroupUpdated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("handling App group update domain event..");
        var cacheResponse = new GroupGetResponse(
            notification.Item.Id, notification.Item.Application, notification.Item.Parent, notification.Item.Tag,
            notification.Item.Number, notification.Item.Code, notification.Item.Name, notification.Item.Amount,
            notification.Item.EmployeeId, notification.Item.EmployeeName,
            notification.Item.Description, notification.Item.Notes);
        await cache.SetAsync($"Group:{notification.Item.Id}", cacheResponse, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
