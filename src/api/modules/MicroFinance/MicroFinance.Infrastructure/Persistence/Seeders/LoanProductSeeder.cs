using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for loan products.
/// </summary>
internal static class LoanProductSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 10;
        var existingCount = await context.LoanProducts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var products = new (string Code, string Name, string Desc, decimal MinAmt, decimal MaxAmt, decimal Rate, string Method, int MinTerm, int MaxTerm, string Freq)[]
        {
            ("PERSONAL-LOAN", "Personal Loan", "Standard personal loan for general purposes", 100, 10000, 12, "Declining", 3, 24, "Monthly"),
            ("AGRI-LOAN", "Agricultural Loan", "Seasonal loans for farming activities", 500, 50000, 10, "Flat", 6, 12, "Monthly"),
            ("MICRO-BUSINESS", "Micro Business Loan", "Working capital for small businesses", 200, 25000, 15, "Declining", 3, 36, "Monthly"),
            ("EMERGENCY-LOAN", "Emergency Loan", "Quick disbursement for emergencies", 50, 2000, 18, "Flat", 1, 6, "Weekly"),
            ("EDU-LOAN", "Education Loan", "School fees and education expenses", 100, 15000, 8, "Declining", 6, 48, "Monthly"),
            ("HOUSING-LOAN", "Housing Improvement Loan", "Home repairs and improvements", 1000, 100000, 9, "Declining", 12, 60, "Monthly"),
            ("ASSET-LOAN", "Asset Finance Loan", "Equipment and vehicle purchase", 500, 75000, 11, "Declining", 6, 48, "Monthly"),
            ("GROUP-LOAN", "Group Solidarity Loan", "Joint liability group loans", 50, 5000, 14, "Flat", 3, 12, "Weekly"),
            ("SOLAR-LOAN", "Solar Energy Loan", "Solar panels and renewable energy", 200, 20000, 7, "Declining", 12, 36, "Monthly"),
            ("WATER-SANIT", "Water & Sanitation Loan", "Water tanks and sanitation facilities", 100, 10000, 8, "Flat", 6, 24, "Monthly"),
        };

        for (int i = existingCount; i < products.Length; i++)
        {
            var p = products[i];
            if (await context.LoanProducts.AnyAsync(x => x.Code == p.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var product = LoanProduct.Create(
                code: p.Code,
                name: p.Name,
                description: p.Desc,
                currencyCode: "USD",
                minLoanAmount: p.MinAmt,
                maxLoanAmount: p.MaxAmt,
                interestRate: p.Rate,
                interestMethod: p.Method,
                minTermMonths: p.MinTerm,
                maxTermMonths: p.MaxTerm,
                repaymentFrequency: p.Freq);

            await context.LoanProducts.AddAsync(product, cancellationToken).ConfigureAwait(false);
        }

        logger.LogInformation("[{Tenant}] seeded loan products", tenant);
    }
}
