using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for collateral valuations.
/// Creates appraisal records for loan collaterals.
/// </summary>
internal static class CollateralValuationSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.CollateralValuations.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var collaterals = await context.LoanCollaterals
            .Take(20)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!collaterals.Any()) return;

        var random = new Random(42);
        int valuationNumber = 21001;
        int valuationCount = 0;

        var appraisers = new (string Name, string Company, string License)[]
        {
            ("Engr. Ricardo Santos", "Santos Appraisal Services", "PRC-APR-2019-12345"),
            ("Engr. Maria Cruz", "Metro Manila Valuers Inc.", "PRC-APR-2020-23456"),
            ("Engr. Jose Reyes", "Philippine Property Assessors", "PRC-APR-2018-34567"),
            ("Engr. Ana Garcia", "Visayas Appraisal Group", "PRC-APR-2021-45678"),
            ("Engr. Pedro Lim", "Mindanao Real Estate Valuers", "PRC-APR-2017-56789"),
        };

        foreach (var collateral in collaterals)
        {
            var appraiser = appraisers[random.Next(appraisers.Length)];
            var valuationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(30, 180)));
            var marketValue = collateral.AppraisedValue;
            var forcedSaleValue = marketValue * 0.7m;
            var insurableValue = marketValue * 0.8m;

            var valuation = CollateralValuation.Create(
                collateralId: collateral.Id,
                valuationReference: $"VAL-{valuationNumber++:D6}",
                valuationDate: valuationDate,
                valuationMethod: CollateralValuation.MethodMarket,
                marketValue: marketValue,
                forcedSaleValue: forcedSaleValue,
                insurableValue: insurableValue,
                appraiserName: appraiser.Name,
                appraiserCompany: appraiser.Company);

            valuation.SetAppraiserLicense(appraiser.License);
            valuation.SetCondition("Good condition, well-maintained");
            valuation.SetExpiryDate(valuationDate.AddYears(1));

            // Submit and approve most valuations
            valuation.Submit();
            if (random.NextDouble() < 0.9)
            {
                valuation.Approve(Guid.NewGuid());
            }

            // Check for expiry
            if (valuation.ExpiryDate < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                valuation.MarkExpired();
            }

            await context.CollateralValuations.AddAsync(valuation, cancellationToken).ConfigureAwait(false);
            valuationCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} collateral valuations", tenant, valuationCount);
    }
}
