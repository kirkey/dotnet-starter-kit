using Accounting.Domain.Events.Project;

namespace Accounting.Application.Projects.EventHandlers;

public sealed class ProjectCreatedEventHandler(ILogger<ProjectCreatedEventHandler> logger)
    : INotificationHandler<ProjectCreated>
{
    public Task Handle(ProjectCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Project created: {Id} - {ProjectName} - Budget: {BudgetedAmount}", 
            notification.Id, notification.ProjectName, notification.BudgetedAmount);
        return Task.CompletedTask;
    }
}
