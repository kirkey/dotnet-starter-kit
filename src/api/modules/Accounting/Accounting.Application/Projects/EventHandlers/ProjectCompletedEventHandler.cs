using Accounting.Domain.Events.Project;

namespace Accounting.Application.Projects.EventHandlers;

/// <summary>
/// Notification handler that logs when a project has been completed.
/// </summary>
public sealed class ProjectCompletedEventHandler(ILogger<ProjectCompletedEventHandler> logger)
    : INotificationHandler<ProjectCompleted>
{
    /// <summary>
    /// Handles the <see cref="ProjectCompleted"/> domain event and writes a structured log entry.
    /// </summary>
    public Task Handle(ProjectCompleted notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Project completed: {ProjectId} on {CompletionDate} - ActualCost: {ActualCost} - ActualRevenue: {ActualRevenue}",
            notification.ProjectId,
            notification.CompletionDate,
            notification.FinalActualCost,
            notification.FinalActualRevenue);
        return Task.CompletedTask;
    }
}

