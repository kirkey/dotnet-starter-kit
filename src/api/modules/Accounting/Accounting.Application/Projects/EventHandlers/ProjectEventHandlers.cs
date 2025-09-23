using Accounting.Domain.Events.Project;

namespace Accounting.Application.Projects.EventHandlers;

public sealed class ProjectCreatedEventHandler(ILogger<ProjectCreatedEventHandler> logger)
    : INotificationHandler<ProjectCreated>
{
    public Task Handle(ProjectCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Project created: {ProjectId} - {Name} - Budget: {BudgetedAmount}",
            notification.ProjectId,
            notification.Name,
            notification.BudgetedAmount);
        return Task.CompletedTask;
    }
}
