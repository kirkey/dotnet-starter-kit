using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for customer segments.
/// Creates marketing and risk-based customer segments.
/// </summary>
internal static class CustomerSegmentSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 12;
        var existingCount = await context.CustomerSegments.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var segments = new (string Code, string Name, string Type, string Desc, decimal? MinIncome, decimal? MaxIncome, string? Risk, int Priority)[]
        {
            // Value-based segments
            ("SEG-VIP", "VIP Members", CustomerSegment.TypeValue, "High-value members with large deposits and excellent payment history", 75000, null, "Low", 1),
            ("SEG-PREMIUM", "Premium Members", CustomerSegment.TypeValue, "Members with significant savings and loan history", 40000, 74999, "Low", 2),
            ("SEG-REGULAR", "Regular Members", CustomerSegment.TypeValue, "Active members with moderate activity", 15000, 39999, "Medium", 3),
            ("SEG-BASIC", "Basic Members", CustomerSegment.TypeValue, "New or low-activity members", 0, 14999, "Medium", 4),
            
            // Demographic segments
            ("SEG-FARMER", "Magsasaka", CustomerSegment.TypeDemographic, "Agricultural sector members - farmers and fisherfolk", null, null, null, 5),
            ("SEG-WOMEN", "Kababaihan", CustomerSegment.TypeDemographic, "Women entrepreneurs and household heads", null, null, null, 6),
            ("SEG-YOUTH", "Kabataan", CustomerSegment.TypeDemographic, "Young professionals and entrepreneurs under 30", null, null, null, 7),
            ("SEG-SENIOR", "Senior Citizens", CustomerSegment.TypeDemographic, "Members aged 60 and above", null, null, null, 8),
            
            // Risk segments
            ("SEG-LOW-RISK", "Mababang Panganib", CustomerSegment.TypeRisk, "Members with excellent credit scores and payment history", null, null, "Low", 9),
            ("SEG-MED-RISK", "Katamtamang Panganib", CustomerSegment.TypeRisk, "Members with moderate credit risk", null, null, "Medium", 10),
            ("SEG-HIGH-RISK", "Mataas na Panganib", CustomerSegment.TypeRisk, "Members requiring closer monitoring", null, null, "High", 11),
            
            // Lifecycle segments
            ("SEG-NEW", "Bagong Miyembro", CustomerSegment.TypeLifecycle, "Members who joined in the last 6 months", null, null, null, 12),
        };

        foreach (var seg in segments)
        {
            if (await context.CustomerSegments.AnyAsync(s => s.Code == seg.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var segment = CustomerSegment.Create(
                name: seg.Name,
                code: seg.Code,
                segmentType: seg.Type,
                description: seg.Desc,
                priority: seg.Priority);

            if (seg.MinIncome.HasValue || seg.MaxIncome.HasValue || !string.IsNullOrEmpty(seg.Risk))
            {
                segment.Update(
                    minIncomeLevel: seg.MinIncome,
                    maxIncomeLevel: seg.MaxIncome,
                    riskLevel: seg.Risk);
            }

            await context.CustomerSegments.AddAsync(segment, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} customer segments", tenant, targetCount);
    }
}
