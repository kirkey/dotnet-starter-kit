using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for communication logs.
/// Creates communication history for members.
/// </summary>
internal static class CommunicationLogSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.CommunicationLogs.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var members = await context.Members.Take(30).ToListAsync(cancellationToken).ConfigureAwait(false);
        var templates = await context.CommunicationTemplates.ToListAsync(cancellationToken).ConfigureAwait(false);

        if (!members.Any()) return;

        var random = new Random(42);
        int logCount = 0;

        var channels = new[] { CommunicationLog.ChannelSms, CommunicationLog.ChannelEmail, CommunicationLog.ChannelPush };
        var purposes = new[] { "Payment Reminder", "Welcome Message", "Loan Approval", "Birthday Greeting", "Promo Announcement" };

        foreach (var member in members)
        {
            // Generate 3-8 communication logs per member
            int numLogs = random.Next(3, 9);

            for (int i = 0; i < numLogs; i++)
            {
                var channel = channels[random.Next(channels.Length)];
                var purpose = purposes[random.Next(purposes.Length)];
                var sentDate = DateTimeOffset.UtcNow.AddDays(-random.Next(1, 180));

                var recipient = channel switch
                {
                    CommunicationLog.ChannelSms => member.PhoneNumber ?? $"+639{random.Next(100000000, 999999999)}",
                    CommunicationLog.ChannelEmail => member.Email ?? "member@email.com",
                    _ => member.Id.ToString()
                };

                var log = CommunicationLog.Create(
                    channel: channel,
                    recipient: recipient,
                    body: $"{purpose} - Sent via {channel} to {member.FirstName} {member.LastName}",
                    memberId: member.Id,
                    subject: purpose);

                // Mark as sent first
                log.MarkSent();
                
                // Most messages are delivered successfully
                if (random.NextDouble() > 0.1)
                {
                    log.MarkDelivered();
                    
                    // Some are read/opened (especially email)
                    if (channel == CommunicationLog.ChannelEmail && random.NextDouble() > 0.3)
                    {
                        log.MarkOpened();
                    }
                }
                else
                {
                    log.MarkFailed("Delivery failed - recipient unreachable");
                }

                await context.CommunicationLogs.AddAsync(log, cancellationToken).ConfigureAwait(false);
                logCount++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} communication logs", tenant, logCount);
    }
}
