using Accounting.Domain.Events.AccountingPeriod;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.AccountingPeriods.EventHandlers;

public sealed class AccountingPeriodCreatedEventHandler(ILogger<AccountingPeriodCreatedEventHandler> logger)
    : INotificationHandler<AccountingPeriodCreated>
{
    public Task Handle(AccountingPeriodCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Accounting Period created: {PeriodId} - {PeriodName} - Fiscal Year {FiscalYear}", 
            notification.Id, notification.PeriodName, notification.FiscalYear);
        return Task.CompletedTask;
    }
}
