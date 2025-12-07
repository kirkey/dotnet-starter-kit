using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for marketing campaigns.
/// Creates marketing campaign records for member outreach.
/// </summary>
internal static class MarketingCampaignSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.MarketingCampaigns.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var random = new Random(42);

        var campaigns = new (string Name, string Code, string Type, string Channels, decimal Budget, int Target, string Desc, int DaysAgo, int Duration)[]
        {
            // Active promotions
            ("Pasko Loan Promo 2024", "PASKO-2024", MarketingCampaign.TypePromotion, "SMS,Email,Facebook", 100000m, 5000, "Christmas loan special - reduced interest rates for the holiday season", -30, 60),
            ("Back to School Loan", "BTS-2024", MarketingCampaign.TypePromotion, "SMS,Viber", 50000m, 2000, "Education loan campaign for school enrollment period", -15, 45),
            ("Agri-Loan Season", "AGRI-2024", MarketingCampaign.TypePromotion, "SMS,Community", 75000m, 3000, "Agricultural loan promo for planting season", -45, 90),
            
            // Education campaigns
            ("Financial Literacy Week", "FINLIT-2024", MarketingCampaign.TypeEducation, "SMS,Facebook,Workshop", 30000m, 10000, "Financial education awareness campaign", -60, 7),
            ("Savings Challenge", "SAVE-2024", MarketingCampaign.TypeEducation, "SMS,App", 20000m, 4000, "Encourage regular savings through gamification", -20, 30),
            
            // Retention
            ("Loyal Member Rewards", "LOYAL-2024", MarketingCampaign.TypeRetention, "SMS,Email", 40000m, 2500, "Appreciation program for long-term members", -90, 180),
            ("Inactive Member Win-Back", "WINBACK-2024", MarketingCampaign.TypeReactivation, "SMS,Call", 25000m, 1000, "Re-engage dormant members with special offers", -10, 60),
            
            // Cross-sell
            ("Insurance Bundling", "INS-CROSS", MarketingCampaign.TypeCrossSell, "SMS,Email", 15000m, 1500, "Promote insurance products to loan borrowers", -5, 30),
            ("Mobile Wallet Adoption", "WALLET-2024", MarketingCampaign.TypeCrossSell, "SMS,App", 35000m, 8000, "Drive mobile wallet registration and usage", -25, 45),
            
            // Acquisition
            ("New Member Referral", "REFER-2024", MarketingCampaign.TypeAcquisition, "SMS,Facebook,Community", 60000m, 5000, "Member-get-member referral program", -40, 120),
            ("OFW Family Campaign", "OFW-2024", MarketingCampaign.TypeAcquisition, "Facebook,Radio", 80000m, 3000, "Target OFW families for remittance and savings", -50, 90),
            ("Market Vendor Outreach", "VENDOR-2024", MarketingCampaign.TypeAcquisition, "Community,SMS", 45000m, 2000, "Microenterprise acquisition campaign", -35, 60),
        };

        foreach (var c in campaigns)
        {
            var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(c.DaysAgo));
            var endDate = startDate.AddDays(c.Duration);

            var campaign = MarketingCampaign.Create(
                name: c.Name,
                code: c.Code,
                campaignType: c.Type,
                startDate: startDate,
                channels: c.Channels,
                budget: c.Budget,
                targetAudience: c.Target,
                description: c.Desc,
                endDate: endDate);

            // Approve and potentially activate based on dates
            campaign.Approve(Guid.NewGuid());

            if (startDate <= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                campaign.Launch();
                
                // Add some metrics for active campaigns
                var reachedPct = 0.3 + random.NextDouble() * 0.5;
                var responsePct = 0.05 + random.NextDouble() * 0.15;
                var conversionPct = 0.02 + random.NextDouble() * 0.08;
                var spent = c.Budget * (0.2m + (decimal)random.NextDouble() * 0.6m);

                // Record metrics using individual methods
                campaign.RecordReach((int)(c.Target * reachedPct));
                for (int i = 0; i < (int)(c.Target * responsePct); i++) campaign.RecordResponse();
                for (int i = 0; i < (int)(c.Target * conversionPct); i++) campaign.RecordConversion();
                campaign.RecordSpending(spent);

                // Complete if end date has passed
                if (endDate < DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    campaign.Complete();
                }
            }

            await context.MarketingCampaigns.AddAsync(campaign, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} marketing campaigns", tenant, campaigns.Length);
    }
}
