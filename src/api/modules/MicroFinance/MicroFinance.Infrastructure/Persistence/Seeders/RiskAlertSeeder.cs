using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for risk alerts.
/// Creates risk monitoring alerts for various risk scenarios.
/// </summary>
internal static class RiskAlertSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.RiskAlerts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var riskCategories = await context.RiskCategories.Take(5).ToListAsync(cancellationToken).ConfigureAwait(false);

        var random = new Random(42);
        int alertNumber = 22001;

        var alerts = new (string Title, string Desc, string Severity, string Source, int DaysAgo)[]
        {
            // Credit Risk
            ("PAR Threshold Exceeded", "Portfolio at Risk (>30 days) has exceeded 5% threshold, currently at 6.2%", RiskAlert.SeverityHigh, RiskAlert.SourceThreshold, -30),
            ("Large Loan Concentration", "Top 10 borrowers represent 18% of portfolio, exceeding 15% limit", RiskAlert.SeverityMedium, RiskAlert.SourceAutomatic, -25),
            ("New Loan Default Spike", "Default rate for loans <6 months old increased by 40% MoM", RiskAlert.SeverityCritical, RiskAlert.SourceAutomatic, -20),
            
            // Operational Risk
            ("System Downtime Detected", "Core banking system experienced 2 hour outage affecting transactions", RiskAlert.SeverityHigh, RiskAlert.SourceAutomatic, -15),
            ("Failed Transaction Rate High", "Transaction failure rate at 8%, exceeds 5% threshold", RiskAlert.SeverityMedium, RiskAlert.SourceThreshold, -12),
            ("Branch Cash Shortage", "Makati branch reported cash shortage during reconciliation", RiskAlert.SeverityHigh, RiskAlert.SourceManual, -10),
            
            // Compliance Risk
            ("KYC Document Expiry", "157 member KYC documents expiring within 30 days", RiskAlert.SeverityMedium, RiskAlert.SourceAutomatic, -8),
            ("AML Alert Backlog", "35 AML alerts pending investigation beyond SLA", RiskAlert.SeverityHigh, RiskAlert.SourceThreshold, -5),
            ("Regulatory Report Due", "BSP quarterly report submission due in 5 days", RiskAlert.SeverityMedium, RiskAlert.SourceAutomatic, -3),
            
            // Liquidity Risk
            ("Liquidity Ratio Below Target", "Liquidity coverage ratio at 95%, below 100% target", RiskAlert.SeverityMedium, RiskAlert.SourceThreshold, -18),
            ("Large Withdrawal Request", "Fixed deposit maturity of ₱5M scheduled, ensure liquidity", RiskAlert.SeverityLow, RiskAlert.SourceAutomatic, -2),
            
            // Market Risk
            ("Interest Rate Environment Change", "BSP policy rate increased by 25bps, review loan pricing", RiskAlert.SeverityLow, RiskAlert.SourceExternal, -22),
            
            // Fraud Risk
            ("Suspicious Transaction Pattern", "Multiple small deposits from same source detected", RiskAlert.SeverityHigh, RiskAlert.SourceAutomatic, -7),
            ("Duplicate Application Detected", "Same ID documents used in multiple loan applications", RiskAlert.SeverityCritical, RiskAlert.SourceAutomatic, -4),
        };

        foreach (var a in alerts)
        {
            var alertDate = DateTimeOffset.UtcNow.AddDays(a.DaysAgo);
            var categoryId = riskCategories.Any() ? riskCategories[random.Next(riskCategories.Count)].Id : (Guid?)null;

            var alert = RiskAlert.Create(
                alertNumber: $"RISK-{alertNumber++:D6}",
                title: a.Title,
                description: a.Desc,
                severity: a.Severity,
                source: a.Source,
                alertDate: alertDate,
                riskCategoryId: categoryId);

            // Process based on age
            if (a.DaysAgo < -20)
            {
                alert.Acknowledge(Guid.NewGuid());
                alert.StartInvestigation("Assigned to risk team for investigation");
                alert.Resolve("Root cause identified and corrective action taken", "Implemented controls to prevent recurrence");
            }
            else if (a.DaysAgo < -10)
            {
                alert.Acknowledge(Guid.NewGuid());
                if (a.Severity == RiskAlert.SeverityCritical || a.Severity == RiskAlert.SeverityHigh)
                {
                    alert.StartInvestigation("Under active investigation");
                }
            }
            else if (a.DaysAgo < -5)
            {
                alert.Acknowledge(Guid.NewGuid());
            }

            await context.RiskAlerts.AddAsync(alert, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} risk alerts", tenant, alerts.Length);
    }
}
