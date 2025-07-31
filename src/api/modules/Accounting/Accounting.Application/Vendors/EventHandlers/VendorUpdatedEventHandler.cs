using MediatR;
using Accounting.Domain.Events.Vendor;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.Vendors.EventHandlers;

public class VendorUpdatedEventHandler(ILogger<VendorUpdatedEventHandler> logger) : INotificationHandler<VendorUpdated>
{
    public async Task Handle(VendorUpdated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("handling vendor updated domain event..");
        await Task.FromResult(notification).ConfigureAwait(false);
        logger.LogInformation("finished handling vendor updated domain event..");
    }
}
