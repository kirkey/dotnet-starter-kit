using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for loan guarantors.
/// Assigns guarantors to loans to test guarantee workflows.
/// </summary>
internal static class LoanGuarantorSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.LoanGuarantors.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return; // Already seeded

        // Get loans that need guarantors (approved or disbursed loans)
        var loans = await context.Loans
            .Where(l => l.Status == Loan.StatusApproved || l.Status == Loan.StatusDisbursed)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!loans.Any()) return;

        // Get members to use as guarantors (not the loan holder)
        var members = await context.Members
            .Where(m => m.IsActive)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (members.Count < 5) return;

        int guarantorCount = 0;
        var random = new Random(42); // Fixed seed for consistent results

        foreach (var loan in loans)
        {
            // Skip loans that already have guarantors
            if (await context.LoanGuarantors.AnyAsync(g => g.LoanId == loan.Id, cancellationToken).ConfigureAwait(false))
                continue;

            // Assign 1-2 guarantors per loan
            var numGuarantors = random.Next(1, 3);
            var availableGuarantors = members.Where(m => m.Id != loan.MemberId).ToList();
            
            if (availableGuarantors.Count < numGuarantors) continue;

            for (int i = 0; i < numGuarantors; i++)
            {
                var guarantorIndex = (guarantorCount + i) % availableGuarantors.Count;
                var guarantorMember = availableGuarantors[guarantorIndex];
                
                // Each guarantor covers a portion of the loan
                var guaranteeAmount = Math.Round(loan.PrincipalAmount / numGuarantors, 2);

                var guarantor = LoanGuarantor.Create(
                    loanId: loan.Id,
                    guarantorMemberId: guarantorMember.Id,
                    guaranteedAmount: guaranteeAmount,
                    relationship: GetRandomRelationship(random));

                // Set status based on loan status
                if (loan.Status == Loan.StatusDisbursed)
                {
                    guarantor.Approve();
                }
                else if (random.NextDouble() > 0.7) // 30% chance of being approved already
                {
                    guarantor.Approve();
                }

                await context.LoanGuarantors.AddAsync(guarantor, cancellationToken).ConfigureAwait(false);
                guarantorCount++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} loan guarantors for approved/disbursed loans", tenant, guarantorCount);
    }

    private static string GetRandomRelationship(Random random)
    {
        var relationships = new[] { "Family", "Friend", "Business Partner", "Colleague", "Neighbor", "Group Member" };
        return relationships[random.Next(relationships.Length)];
    }
}
