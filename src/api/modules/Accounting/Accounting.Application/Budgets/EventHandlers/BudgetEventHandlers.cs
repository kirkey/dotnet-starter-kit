using Accounting.Domain.Events.Budget;

namespace Accounting.Application.Budgets.EventHandlers;

public sealed class BudgetCreatedEventHandler(ILogger<BudgetCreatedEventHandler> logger)
    : INotificationHandler<BudgetCreated>
{
    public Task Handle(BudgetCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Budget created: {BudgetId} - {BudgetName}", notification.Id, notification.BudgetName);
        return Task.CompletedTask;
    }
}

public sealed class BudgetUpdatedEventHandler(ILogger<BudgetUpdatedEventHandler> logger)
    : INotificationHandler<BudgetUpdated>
{
    public Task Handle(BudgetUpdated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Budget updated: {BudgetId}", notification.Budget.Id);
        return Task.CompletedTask;
    }
}
