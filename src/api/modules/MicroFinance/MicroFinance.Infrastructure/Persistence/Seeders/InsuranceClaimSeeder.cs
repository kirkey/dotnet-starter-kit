using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for insurance claims.
/// Creates insurance claim records.
/// </summary>
internal static class InsuranceClaimSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.InsuranceClaims.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var policies = await context.InsurancePolicies
            .Where(p => p.Status == InsurancePolicy.StatusActive)
            .Take(15)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!policies.Any()) return;

        var random = new Random(42);
        int claimNumber = 12001;
        int claimCount = 0;

        var claimReasons = new[]
        {
            ("Hospitalization", "Member hospitalized due to illness"),
            ("Accident", "Work-related accident requiring medical attention"),
            ("Death", "Death benefit claim for beneficiary"),
            ("Crop Damage", "Typhoon damage to agricultural crops"),
            ("Livestock Death", "Disease outbreak affecting livestock"),
            ("Property Damage", "Fire damage to insured property"),
        };

        // Create claims for 30% of policies
        foreach (var policy in policies.Where(_ => random.NextDouble() < 0.3))
        {
            var reason = claimReasons[random.Next(claimReasons.Length)];
            var claimDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(1, 90)));
            var claimAmount = policy.CoverageAmount * (0.1m + (decimal)random.NextDouble() * 0.4m); // 10-50% of coverage

            var claim = InsuranceClaim.Create(
                insurancePolicyId: policy.Id,
                claimNumber: $"CLM-{claimNumber++:D6}",
                claimType: reason.Item1,
                incidentDate: claimDate,
                claimAmount: Math.Round(claimAmount, 2),
                description: reason.Item2);

            claim.File();
            var userId = Guid.NewGuid();
            claim.StartReview(userId);

            // Process claims based on random outcome
            var outcome = random.NextDouble();
            if (outcome < 0.6) // 60% approved
            {
                claim.Approve(userId, claimAmount * 0.9m);
                if (random.NextDouble() < 0.8)
                {
                    claim.Pay(claimAmount * 0.9m, $"PAY-{claimNumber:D6}", claimDate.AddDays(random.Next(7, 30)));
                }
            }
            else if (outcome < 0.8) // 20% pending
            {
                claim.RequestDocuments("Please submit additional medical certificates");
            }
            else // 20% rejected
            {
                claim.Reject(userId, "Claim does not meet policy criteria");
            }

            await context.InsuranceClaims.AddAsync(claim, cancellationToken).ConfigureAwait(false);
            claimCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} insurance claims", tenant, claimCount);
    }
}
