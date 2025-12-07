using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for AML alerts.
/// Creates anti-money laundering alert records.
/// </summary>
internal static class AmlAlertSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.AmlAlerts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var members = await context.Members.Take(20).ToListAsync(cancellationToken).ConfigureAwait(false);
        if (!members.Any()) return;

        var random = new Random(42);
        int alertNumber = 14001;
        int alertCount = 0;

        var alertTypes = new (string Type, string Desc, string Severity)[]
        {
            ("LargeTransaction", "Single transaction exceeds threshold of ₱500,000", AmlAlert.SeverityHigh),
            ("StructuredTransactions", "Multiple transactions appear structured to avoid reporting", AmlAlert.SeverityCritical),
            ("UnusualActivity", "Transaction pattern inconsistent with customer profile", AmlAlert.SeverityMedium),
            ("PEPMatch", "Customer matched against PEP watchlist", AmlAlert.SeverityHigh),
            ("HighRiskJurisdiction", "Transaction involving high-risk country", AmlAlert.SeverityMedium),
            ("RapidMovement", "Funds deposited and withdrawn within 24 hours", AmlAlert.SeverityMedium),
            ("SanctionsMatch", "Potential match with sanctions list", AmlAlert.SeverityCritical),
        };

        // Create alerts for 15% of members
        foreach (var member in members.Where(_ => random.NextDouble() < 0.15))
        {
            var alertType = alertTypes[random.Next(alertTypes.Length)];
            var alertDate = DateTimeOffset.UtcNow.AddDays(-random.Next(1, 60));
            var transactionAmount = 100000 + random.Next(0, 900000);

            var alert = AmlAlert.Create(
                alertNumber: $"AML-{alertNumber++:D6}",
                memberId: member.Id,
                alertType: alertType.Type,
                description: alertType.Desc,
                severity: alertType.Severity,
                alertDate: alertDate,
                transactionAmount: transactionAmount);

            // Process alerts based on severity and random outcome
            var outcome = random.NextDouble();
            if (alertType.Severity == AmlAlert.SeverityCritical)
            {
                alert.Escalate("Escalated for senior compliance review");
            }
            else if (outcome < 0.4)
            {
                alert.Investigate("Under investigation by compliance officer");
                if (random.NextDouble() < 0.3)
                {
                    alert.Close("False positive - legitimate business transaction", "Reviewed KYC documentation");
                }
            }
            else if (outcome < 0.7)
            {
                alert.Close("False positive - within normal transaction range", "Compared with historical pattern");
            }

            await context.AmlAlerts.AddAsync(alert, cancellationToken).ConfigureAwait(false);
            alertCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} AML alerts", tenant, alertCount);
    }
}
