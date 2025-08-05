using Accounting.Domain.Events.Currency;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Currencies.EventHandlers;

public sealed class CurrencyCreatedEventHandler(ILogger<CurrencyCreatedEventHandler> logger)
    : INotificationHandler<CurrencyCreated>
{
    public Task Handle(CurrencyCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Currency created: {CurrencyId} - {CurrencyCode} - {CurrencyName}", 
            notification.Id, notification.CurrencyCode, notification.CurrencyName);
        return Task.CompletedTask;
    }
}
