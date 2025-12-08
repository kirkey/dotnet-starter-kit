using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for loan write-offs.
/// Creates loan write-off records for defaulted loans.
/// </summary>
internal static class LoanWriteOffSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.LoanWriteOffs.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        // Get written-off loans (already processed write-offs to create history records)
        var defaultedLoans = await context.Loans
            .Where(l => l.Status == Loan.StatusWrittenOff)
            .Take(5)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (defaultedLoans.Count == 0) return;

        // Get staff IDs for approvals
        var staffIds = await context.Staff.Select(s => s.Id).ToListAsync(cancellationToken).ConfigureAwait(false);
        if (!staffIds.Any()) return;

        var random = new Random(42);
        int writeOffNumber = 18001;
        int writeOffCount = 0;

        var reasons = new[]
        {
            "Borrower deceased, no estate available for recovery",
            "Borrower relocated overseas, untraceable for over 12 months",
            "Multiple failed collection attempts over 18 months",
            "Legal action not cost-effective relative to outstanding amount",
            "Borrower declared bankruptcy, no assets for recovery",
        };

        foreach (var loan in defaultedLoans)
        {
            var outstandingPrincipal = loan.OutstandingPrincipal;
            var outstandingInterest = loan.OutstandingInterest;
            var outstandingPenalties = outstandingPrincipal * 0.05m; // Estimate 5% penalties

            var writeOff = LoanWriteOff.Create(
                loanId: loan.Id,
                writeOffNumber: $"WO-{writeOffNumber++:D6}",
                writeOffType: LoanWriteOff.TypeFull,
                reason: reasons[random.Next(reasons.Length)],
                principalWriteOff: outstandingPrincipal,
                interestWriteOff: outstandingInterest,
                penaltiesWriteOff: outstandingPenalties,
                feesWriteOff: 0m,
                daysPastDue: 180,
                collectionAttempts: random.Next(5, 15));

            // Submit for approval
            writeOff.SubmitForApproval();

            // Approve and process older write-offs
            if (random.NextDouble() < 0.7)
            {
                var writeOffDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(30, 90)));
                writeOff.Approve(staffIds[random.Next(staffIds.Count)], "Compliance Manager", writeOffDate);
                writeOff.Process();

                // Some have partial recovery
                if (random.NextDouble() < 0.2)
                {
                    var recoveryAmount = outstandingPrincipal * (0.1m + (decimal)random.NextDouble() * 0.2m);
                    writeOff.RecordRecovery(Math.Round(recoveryAmount, 2));
                }
            }

            await context.LoanWriteOffs.AddAsync(writeOff, cancellationToken).ConfigureAwait(false);
            writeOffCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} loan write-offs", tenant, writeOffCount);
    }
}
