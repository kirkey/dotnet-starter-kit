using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for investment/wealth products.
/// Creates mutual funds, bonds, and money market products.
/// </summary>
internal static class InvestmentProductSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 8;
        var existingCount = await context.InvestmentProducts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var products = new (string Code, string Name, string Type, string Risk, decimal MinInvest, decimal MgmtFee, decimal ReturnMin, decimal ReturnMax, int LockIn, string Desc)[]
        {
            // Low Risk Products
            ("INV-MM01", "Pera Mo Money Market Fund", InvestmentProduct.TypeMoneyMarket, InvestmentProduct.RiskLow, 
                1000, 0.5m, 3.0m, 5.0m, 0, "Low-risk money market fund for short-term investments. Ideal for emergency funds."),
            ("INV-TB01", "Gobyerno Treasury Bill Fund", InvestmentProduct.TypeTreasuryBill, InvestmentProduct.RiskLow, 
                5000, 0.25m, 4.0m, 6.0m, 30, "Government treasury bill fund with guaranteed returns."),
            ("INV-BD01", "Matatag Bond Fund", InvestmentProduct.TypeBond, InvestmentProduct.RiskLow, 
                10000, 0.75m, 5.0m, 7.5m, 90, "Corporate and government bond fund for stable income."),
            
            // Medium Risk Products
            ("INV-BL01", "Balanse Balanced Fund", InvestmentProduct.TypeBalanced, InvestmentProduct.RiskMedium, 
                5000, 1.25m, 6.0m, 12.0m, 180, "Balanced fund with mix of equity and fixed income."),
            ("INV-MF01", "Yaman Mo Mutual Fund", InvestmentProduct.TypeMutualFund, InvestmentProduct.RiskMedium, 
                2500, 1.5m, 8.0m, 15.0m, 90, "Diversified mutual fund for medium-term growth."),
            ("INV-UT01", "Negosyo Unit Trust", InvestmentProduct.TypeUnitTrust, InvestmentProduct.RiskMedium, 
                10000, 1.25m, 7.0m, 14.0m, 180, "Unit trust investing in Philippine blue-chip companies."),
            
            // High Risk Products
            ("INV-EQ01", "Aksyon Equity Fund", InvestmentProduct.TypeEquity, InvestmentProduct.RiskHigh, 
                5000, 1.75m, 10.0m, 25.0m, 365, "High-growth equity fund focused on PSE-listed stocks."),
            ("INV-EQ02", "Tech Tagumpay Fund", InvestmentProduct.TypeEquity, InvestmentProduct.RiskHigh, 
                10000, 2.0m, 12.0m, 30.0m, 365, "Technology-focused equity fund for aggressive investors."),
        };

        foreach (var p in products)
        {
            if (await context.InvestmentProducts.AnyAsync(x => x.Code == p.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var product = InvestmentProduct.Create(
                name: p.Name,
                code: p.Code,
                productType: p.Type,
                riskLevel: p.Risk,
                minimumInvestment: p.MinInvest,
                managementFeePercent: p.MgmtFee,
                expectedReturnMin: p.ReturnMin,
                expectedReturnMax: p.ReturnMax,
                lockInPeriodDays: p.LockIn,
                description: p.Desc);

            await context.InvestmentProducts.AddAsync(product, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} investment products (money market, bonds, equity funds)", tenant, targetCount);
    }
}
