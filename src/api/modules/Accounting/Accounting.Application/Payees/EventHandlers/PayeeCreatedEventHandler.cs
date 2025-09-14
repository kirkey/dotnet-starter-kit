using Accounting.Domain.Events.Payee;

namespace Accounting.Application.Payees.EventHandlers;

public class PayeeCreatedEventHandler(ILogger<PayeeCreatedEventHandler> logger) : INotificationHandler<PayeeCreated>
{
    public async Task Handle(PayeeCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling payee created domain event..");
        await Task.FromResult(notification).ConfigureAwait(false);
        logger.LogInformation("finished handling payee created domain event..");
    }
}

