using Accounting.Domain.Events.AccountingPeriod;

namespace Accounting.Application.AccountingPeriods.EventHandlers;

/// <summary>
/// Handles domain events related to AccountingPeriod entities.
/// </summary>
/// <remarks>
/// Current handlers log informational messages for created accounting periods.
/// Additional handlers (audit, integration, notifications) can be added here.
/// </remarks>
public sealed class AccountingPeriodCreatedEventHandler(ILogger<AccountingPeriodCreatedEventHandler> logger)
    : INotificationHandler<AccountingPeriodCreated>
{
    /// <summary>
    /// Handles the <see cref="AccountingPeriodCreated"/> domain event by logging a concise informational message.
    /// </summary>
    /// <param name="notification">Domain event instance containing the created period details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task Handle(AccountingPeriodCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Accounting Period created: {PeriodId} - {PeriodName} - Fiscal Year {FiscalYear}", 
            notification.Id, notification.PeriodName, notification.FiscalYear);
        return Task.CompletedTask;
    }
}
