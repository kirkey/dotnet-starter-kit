using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for insurance policies.
/// Creates policies for members with various insurance products.
/// </summary>
internal static class InsurancePolicySeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.InsurancePolicies.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var members = await context.Members.Where(m => m.IsActive).Take(30).ToListAsync(cancellationToken).ConfigureAwait(false);
        var products = await context.InsuranceProducts.Where(p => p.Status == InsuranceProduct.StatusActive).ToListAsync(cancellationToken).ConfigureAwait(false);
        var disbursedLoans = await context.Loans.Where(l => l.Status == Loan.StatusDisbursed).Take(20).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (!members.Any() || !products.Any()) return;

        var random = new Random(42);
        int policyNumber = 5001;
        int policyCount = 0;

        // Filipino beneficiary names
        var beneficiaries = new (string Name, string Relation)[]
        {
            ("Maria Santos", "Asawa"),
            ("Jose Dela Cruz", "Asawa"),
            ("Ana Reyes", "Anak"),
            ("Pedro Garcia", "Magulang"),
            ("Rosa Mendoza", "Kapatid"),
            ("Juan Bautista", "Asawa"),
            ("Lorna Aquino", "Anak"),
            ("Ricardo Torres", "Magulang"),
        };

        // Loan protection policies for disbursed loans
        var loanProtection = products.FirstOrDefault(p => p.InsuranceType == InsuranceProduct.TypeLoanProtection);
        if (loanProtection != null)
        {
            foreach (var loan in disbursedLoans)
            {
                var member = members.FirstOrDefault(m => m.Id == loan.MemberId);
                if (member == null) continue;

                var beneficiary = beneficiaries[random.Next(beneficiaries.Length)];
                var startDate = loan.DisbursementDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-3));
                var endDate = loan.ExpectedEndDate ?? startDate.AddMonths(loan.TermMonths);
                var premium = loan.PrincipalAmount * (loanProtection.PremiumRate / 100);

                var policy = InsurancePolicy.Create(
                    insuranceProductId: loanProtection.Id,
                    memberId: member.Id,
                    policyNumber: $"POL-{policyNumber++:D6}",
                    startDate: startDate,
                    endDate: endDate,
                    coverageAmount: loan.PrincipalAmount,
                    premiumAmount: premium,
                    waitingPeriodDays: 0,
                    loanId: loan.Id,
                    beneficiaryName: beneficiary.Name,
                    beneficiaryRelation: beneficiary.Relation,
                    beneficiaryContact: $"+639{170000000 + random.Next(10000000):D7}");

                policy.RecordPremiumPayment(premium);
                await context.InsurancePolicies.AddAsync(policy, cancellationToken).ConfigureAwait(false);
                policyCount++;
            }
        }

        // Life insurance policies for some members
        var lifeInsurance = products.FirstOrDefault(p => p.InsuranceType == InsuranceProduct.TypeLifeInsurance);
        if (lifeInsurance != null)
        {
            for (int i = 0; i < Math.Min(15, members.Count); i++)
            {
                var member = members[i];
                var beneficiary = beneficiaries[i % beneficiaries.Length];
                var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-random.Next(1, 12)));
                var coverageAmount = 50000 + (random.Next(1, 10) * 10000);
                var premium = coverageAmount * (lifeInsurance.PremiumRate / 100);

                var policy = InsurancePolicy.Create(
                    insuranceProductId: lifeInsurance.Id,
                    memberId: member.Id,
                    policyNumber: $"POL-{policyNumber++:D6}",
                    startDate: startDate,
                    endDate: startDate.AddYears(1),
                    coverageAmount: coverageAmount,
                    premiumAmount: premium,
                    waitingPeriodDays: 30,
                    beneficiaryName: beneficiary.Name,
                    beneficiaryRelation: beneficiary.Relation,
                    beneficiaryContact: $"+639{180000000 + random.Next(10000000):D7}");

                policy.RecordPremiumPayment(premium);
                await context.InsurancePolicies.AddAsync(policy, cancellationToken).ConfigureAwait(false);
                policyCount++;
            }
        }

        // Crop/Livestock insurance for farmers
        var cropInsurance = products.FirstOrDefault(p => p.InsuranceType == InsuranceProduct.TypeCropInsurance);
        var livestockInsurance = products.FirstOrDefault(p => p.InsuranceType == InsuranceProduct.TypeLivestockInsurance);
        
        var farmers = members.Where(m => m.Occupation?.Contains("Farmer") == true || m.Occupation?.Contains("Magsasaka") == true).Take(10).ToList();
        foreach (var farmer in farmers)
        {
            var product = random.NextDouble() > 0.5 ? cropInsurance : livestockInsurance;
            if (product == null) continue;

            var beneficiary = beneficiaries[random.Next(beneficiaries.Length)];
            var startDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-random.Next(1, 6)));
            var coverageAmount = 10000 + (random.Next(1, 5) * 5000);
            var premium = coverageAmount * (product.PremiumRate / 100);

            var policy = InsurancePolicy.Create(
                insuranceProductId: product.Id,
                memberId: farmer.Id,
                policyNumber: $"POL-{policyNumber++:D6}",
                startDate: startDate,
                endDate: startDate.AddMonths(6),
                coverageAmount: coverageAmount,
                premiumAmount: premium,
                waitingPeriodDays: 14,
                beneficiaryName: beneficiary.Name,
                beneficiaryRelation: beneficiary.Relation);

            policy.RecordPremiumPayment(premium);
            await context.InsurancePolicies.AddAsync(policy, cancellationToken).ConfigureAwait(false);
            policyCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} insurance policies (loan protection, life, crop/livestock)", tenant, policyCount);
    }
}
