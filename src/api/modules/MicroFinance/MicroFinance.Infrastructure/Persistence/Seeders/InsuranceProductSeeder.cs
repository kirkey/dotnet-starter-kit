using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for insurance products.
/// Creates loan protection, life, and other insurance products.
/// </summary>
internal static class InsuranceProductSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 6;
        var existingCount = await context.InsuranceProducts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var insuranceData = new (string Code, string Name, string Type, string Provider, decimal MinCoverage, decimal MaxCoverage, string PremiumCalc, decimal PremiumRate, int? MinAge, int? MaxAge, bool MandatoryWithLoan)[]
        {
            // Loan Protection Insurance
            ("INS-LP01", "Loan Protection Basic", InsuranceProduct.TypeLoanProtection, "MFI Insurance Partner", 500, 50000, InsuranceProduct.PremiumPercentOfLoan, 0.5m, null, null, true),
            ("INS-LP02", "Loan Protection Premium", InsuranceProduct.TypeLoanProtection, "National Insurance Co", 1000, 100000, InsuranceProduct.PremiumPercentOfLoan, 0.8m, null, null, false),
            
            // Life Insurance
            ("INS-LF01", "Member Life Cover", InsuranceProduct.TypeLifeInsurance, "Life Assurance Corp", 5000, 100000, InsuranceProduct.PremiumAgeBasedTable, 1.2m, 18, 65, false),
            ("INS-GR01", "Group Life Insurance", InsuranceProduct.TypeGroupInsurance, "Life Assurance Corp", 2000, 50000, InsuranceProduct.PremiumFlat, 150m, 18, 70, false),
            
            // Agricultural Insurance
            ("INS-CR01", "Seasonal Crop Insurance", InsuranceProduct.TypeCropInsurance, "Agricultural Insurance Ltd", 1000, 25000, InsuranceProduct.PremiumPercentOfCoverage, 3.5m, null, null, false),
            ("INS-LV01", "Livestock Protection", InsuranceProduct.TypeLivestockInsurance, "Agricultural Insurance Ltd", 500, 20000, InsuranceProduct.PremiumPercentOfCoverage, 4.0m, null, null, false),
        };

        foreach (var data in insuranceData)
        {
            if (await context.InsuranceProducts.AnyAsync(i => i.Code == data.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var product = InsuranceProduct.Create(
                code: data.Code,
                name: data.Name,
                insuranceType: data.Type,
                minCoverage: data.MinCoverage,
                maxCoverage: data.MaxCoverage,
                premiumCalculation: data.PremiumCalc,
                premiumRate: data.PremiumRate,
                description: $"{data.Name} - provided by {data.Provider}",
                provider: data.Provider,
                waitingPeriodDays: 30,
                premiumUpfront: true,
                mandatoryWithLoan: data.MandatoryWithLoan,
                minAge: data.MinAge,
                maxAge: data.MaxAge);

            await context.InsuranceProducts.AddAsync(product, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} insurance products (loan protection, life, crop, livestock)", tenant, targetCount);
    }
}
