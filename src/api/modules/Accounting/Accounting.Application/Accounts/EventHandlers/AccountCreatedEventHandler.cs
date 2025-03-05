using Accounting.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Accounts.EventHandlers;

public class AccountCreatedEventHandler(ILogger<AccountCreatedEventHandler> logger) : INotificationHandler<AccountCreated>
{
    public async Task Handle(AccountCreated notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("handling account created domain event..");
        await Task.FromResult(notification).ConfigureAwait(false);
        logger.LogInformation("finished handling account created domain event..");
    }
}
