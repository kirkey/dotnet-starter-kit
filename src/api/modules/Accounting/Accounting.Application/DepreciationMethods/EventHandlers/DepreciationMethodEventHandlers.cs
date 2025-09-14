using Accounting.Domain.Events.DepreciationMethod;

namespace Accounting.Application.DepreciationMethods.EventHandlers;

public sealed class DepreciationMethodCreatedEventHandler(ILogger<DepreciationMethodCreatedEventHandler> logger)
    : INotificationHandler<DepreciationMethodCreated>
{
    public Task Handle(DepreciationMethodCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Depreciation Method created: {MethodId} - {MethodName}", 
            notification.Id, notification.MethodName);
        return Task.CompletedTask;
    }
}
