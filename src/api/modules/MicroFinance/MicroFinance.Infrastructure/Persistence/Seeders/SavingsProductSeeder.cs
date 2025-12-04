using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for savings products.
/// </summary>
internal static class SavingsProductSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 10;
        var existingCount = await context.SavingsProducts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var products = new (string Code, string Name, string Desc, decimal Rate, string Calc, string Freq, decimal MinOpen, decimal MinBal)[]
        {
            ("REGULAR-SAVINGS", "Regular Savings", "Standard savings account for members", 3.5m, "Daily", "Monthly", 10, 100),
            ("JUNIOR-SAVINGS", "Junior Savings", "Savings account for children under 18", 4.0m, "Daily", "Monthly", 5, 0),
            ("FIXED-SAVINGS", "Fixed Savings", "Higher interest with withdrawal restrictions", 5.5m, "Daily", "Quarterly", 100, 500),
            ("EMERGENCY-FUND", "Emergency Fund", "Quick access emergency savings", 2.5m, "Daily", "Monthly", 20, 0),
            ("GOAL-SAVINGS", "Goal Savings", "Target-based savings for specific goals", 4.5m, "Daily", "Monthly", 25, 0),
            ("BUSINESS-SAVINGS", "Business Savings", "Savings for business members", 3.0m, "Daily", "Monthly", 50, 200),
            ("PREMIUM-SAVINGS", "Premium Savings", "High-value savings with premium rates", 6.0m, "Daily", "Monthly", 1000, 5000),
            ("PENSION-SAVINGS", "Pension Savings", "Retirement savings product", 5.0m, "Daily", "Annually", 100, 0),
            ("FESTIVE-SAVINGS", "Festive Savings", "Seasonal savings for holidays", 4.0m, "Daily", "Monthly", 10, 0),
            ("COOPERATIVE-SAVINGS", "Cooperative Savings", "Mandatory member savings", 3.5m, "Daily", "Monthly", 50, 100),
        };

        for (int i = existingCount; i < products.Length; i++)
        {
            var p = products[i];
            if (await context.SavingsProducts.AnyAsync(x => x.Code == p.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var product = SavingsProduct.Create(
                code: p.Code,
                name: p.Name,
                description: p.Desc,
                currencyCode: "USD",
                interestRate: p.Rate,
                interestCalculation: p.Calc,
                interestPostingFrequency: p.Freq,
                minOpeningBalance: p.MinOpen,
                minBalanceForInterest: p.MinBal);

            await context.SavingsProducts.AddAsync(product, cancellationToken).ConfigureAwait(false);
        }

        logger.LogInformation("[{Tenant}] seeded savings products", tenant);
    }
}
