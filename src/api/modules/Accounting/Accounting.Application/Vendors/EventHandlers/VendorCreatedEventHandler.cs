using Accounting.Domain.Events.Vendor;

namespace Accounting.Application.Vendors.EventHandlers;

public class VendorCreatedEventHandler(ILogger<VendorCreatedEventHandler> logger) : INotificationHandler<VendorCreated>
{
    public async Task Handle(VendorCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("handling vendor created domain event..");
        await Task.FromResult(notification).ConfigureAwait(false);
        logger.LogInformation("finished handling vendor created domain event..");
    }
}
