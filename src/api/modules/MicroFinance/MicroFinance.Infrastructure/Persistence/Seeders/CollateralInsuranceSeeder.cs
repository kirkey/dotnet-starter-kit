using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for collateral insurance.
/// Creates insurance records for loan collaterals.
/// </summary>
internal static class CollateralInsuranceSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.CollateralInsurances.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var collaterals = await context.LoanCollaterals
            .Take(25)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!collaterals.Any()) return;

        var random = new Random(42);
        int policyNumber = 24001;
        int insuranceCount = 0;

        var insurers = new (string Name, string Contact, string Phone, string Email)[]
        {
            ("Pioneer Insurance & Surety Corporation", "Juan Santos", "+632-8849-0101", "claims@pioneer.ph"),
            ("Malayan Insurance Co., Inc.", "Maria Cruz", "+632-8242-5151", "service@malayan.ph"),
            ("Charter Ping An Insurance", "Pedro Reyes", "+632-8638-6888", "info@charterpingan.ph"),
            ("Prudential Guarantee and Assurance", "Ana Garcia", "+632-8816-6041", "support@prudentialguarantee.ph"),
            ("Standard Insurance Co., Inc.", "Luis Tan", "+632-8845-1111", "claims@standard.com.ph"),
        };

        var insuranceTypes = new[] { CollateralInsurance.TypeComprehensive, CollateralInsurance.TypeFire, CollateralInsurance.TypeAllRisk, CollateralInsurance.TypeProperty };

        foreach (var collateral in collaterals)
        {
            // Not all collaterals need insurance (about 70%)
            if (random.NextDouble() > 0.7) continue;

            var insurer = insurers[random.Next(insurers.Length)];
            var insuranceType = insuranceTypes[random.Next(insuranceTypes.Length)];
            var effectiveDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(30, 300)));
            var expiryDate = effectiveDate.AddYears(1);
            var coverageAmount = collateral.EstimatedValue * 0.9m;
            var premium = coverageAmount * 0.02m; // 2% of coverage
            var deductible = coverageAmount * 0.05m; // 5% deductible

            var insurance = CollateralInsurance.Create(
                collateralId: collateral.Id,
                policyNumber: $"INS-{policyNumber++:D6}",
                insurerName: insurer.Name,
                insuranceType: insuranceType,
                coverageAmount: coverageAmount,
                premiumAmount: premium,
                deductible: deductible,
                effectiveDate: effectiveDate,
                expiryDate: expiryDate,
                isMfiAsBeneficiary: true);

            insurance.Update(
                insurerContact: insurer.Contact,
                insurerPhone: insurer.Phone,
                insurerEmail: insurer.Email,
                autoRenewal: true,
                renewalReminderDays: 30);

            // Mark as premium paid
            insurance.RecordPremiumPayment(effectiveDate.AddDays(random.Next(0, 7)), effectiveDate.AddYears(1));

            // Check for expired policies
            if (expiryDate < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                insurance.Expire();
            }
            else if ((expiryDate.ToDateTime(TimeOnly.MinValue) - DateTime.UtcNow).TotalDays < 30)
            {
                insurance.MarkPendingRenewal();
            }

            await context.CollateralInsurances.AddAsync(insurance, cancellationToken).ConfigureAwait(false);
            insuranceCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} collateral insurances", tenant, insuranceCount);
    }
}
