﻿namespace FSH.Framework.Infrastructure.Identity.Audit;
public class AuditPublishedEventHandler(ILogger<AuditPublishedEventHandler> logger, IdentityDbContext context) : INotificationHandler<AuditPublishedEvent>
{
    public async Task Handle(AuditPublishedEvent notification, CancellationToken cancellationToken)
    {
        if (context == null) return;
        logger.LogInformation("received audit trails");
        try
        {
            await context.Set<AuditTrail>().AddRangeAsync(notification.Trails!, default).ConfigureAwait(false);
            await context.SaveChangesAsync(default).ConfigureAwait(false);
        }
        catch
        {
            logger.LogError("error while saving audit trail");
        }
        return;
    }
}
