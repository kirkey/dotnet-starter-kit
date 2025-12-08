using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for collection strategies.
/// Creates automated collection workflow rules.
/// </summary>
internal static class CollectionStrategySeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 10;
        var existingCount = await context.CollectionStrategies.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var strategies = new (string Code, string Name, int TriggerDays, int? MaxDays, string Action, string? Template, int Priority)[]
        {
            // Early reminder
            ("COL-001", "Due Date Reminder", 0, 0, "SMS", "Magandang araw! Paalaala po na ang inyong bayarin ay due na ngayong araw. Salamat po!", 1),
            
            // 1-7 days past due
            ("COL-002", "First SMS Reminder", 1, 3, "SMS", "Paalaala: Ang inyong loan payment ay overdue na po. Mangyaring magbayad kaagad. Salamat.", 2),
            ("COL-003", "Second SMS Reminder", 4, 7, "SMS", "Pangalawang paalaala: May outstanding balance pa po kayo. Mangyaring makipag-ugnayan sa aming opisina.", 3),
            
            // 8-14 days
            ("COL-004", "Phone Call", 8, 14, "Call", null, 4),
            ("COL-005", "Email Notice", 8, 14, "Email", null, 5),
            
            // 15-30 days
            ("COL-006", "Field Visit Warning SMS", 15, 20, "SMS", "Babala: Kung hindi po kayo makakabayad sa loob ng 10 araw, magkakaroon po ng field visit.", 6),
            ("COL-007", "First Field Visit", 21, 30, "FieldVisit", null, 7),
            
            // 31-60 days
            ("COL-008", "Demand Letter", 31, 45, "DemandLetter", null, 8),
            ("COL-009", "Second Field Visit", 46, 60, "FieldVisit", null, 9),
            
            // 60+ days
            ("COL-010", "Legal Notice", 61, null, "LegalNotice", null, 10),
        };

        foreach (var strat in strategies)
        {
            if (await context.CollectionStrategies.AnyAsync(s => s.Code == strat.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var strategy = CollectionStrategy.Create(
                code: strat.Code,
                name: strat.Name,
                triggerDaysPastDue: strat.TriggerDays,
                actionType: strat.Action,
                priority: strat.Priority);

            if (strat.MaxDays.HasValue || strat.Template != null)
            {
                strategy.Update(
                    maxDaysPastDue: strat.MaxDays,
                    messageTemplate: strat.Template);
            }

            await context.CollectionStrategies.AddAsync(strategy, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} collection strategies", tenant, targetCount);
    }
}
