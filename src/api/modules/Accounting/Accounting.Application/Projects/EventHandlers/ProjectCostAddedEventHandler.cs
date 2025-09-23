using Accounting.Domain.Events.Project;

namespace Accounting.Application.Projects.EventHandlers;

/// <summary>
/// Notification handler that logs when a project cost is added.
/// </summary>
public sealed class ProjectCostAddedEventHandler(ILogger<ProjectCostAddedEventHandler> logger)
    : INotificationHandler<ProjectCostAdded>
{
    /// <summary>
    /// Handles the <see cref="ProjectCostAdded"/> domain event and writes a structured log entry.
    /// </summary>
    public Task Handle(ProjectCostAdded notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Project cost added: {ProjectId} +{Amount} => TotalActualCost: {TotalActualCost}",
            notification.ProjectId,
            notification.Amount,
            notification.TotalActualCost);
        return Task.CompletedTask;
    }
}

