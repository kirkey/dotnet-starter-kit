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

        var defaultedLoans = await context.Loans
            .Where(l => l.Status == Loan.StatusDefaulted)
            .Take(5)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!defaultedLoans.Any()) return;

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
            var requestDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(30, 180)));
            var outstandingPrincipal = loan.OutstandingPrincipal;
            var outstandingInterest = loan.OutstandingInterest;
            var outstandingPenalties = loan.TotalPenalties * 0.3m; // Partial penalty

            var writeOff = LoanWriteOff.Create(
                loanId: loan.Id,
                writeOffNumber: $"WO-{writeOffNumber++:D6}",
                writeOffType: LoanWriteOff.TypeFull,
                reason: reasons[random.Next(reasons.Length)],
                requestDate: requestDate,
                principalWriteOff: outstandingPrincipal,
                interestWriteOff: outstandingInterest,
                penaltyWriteOff: outstandingPenalties);

            // Submit for approval
            writeOff.SubmitForApproval();

            // Approve and process older write-offs
            if (random.NextDouble() < 0.7)
            {
                writeOff.Approve(Guid.NewGuid(), "Compliance Manager");
                writeOff.Process(requestDate.AddDays(random.Next(7, 30)));

                // Some have partial recovery
                if (random.NextDouble() < 0.2)
                {
                    var recoveryAmount = outstandingPrincipal * (0.1m + (decimal)random.NextDouble() * 0.2m);
                    writeOff.RecordRecovery(
                        recoveryAmount: Math.Round(recoveryAmount, 2),
                        recoveryDate: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(1, 30))),
                        notes: "Partial recovery from asset liquidation");
                }
            }

            await context.LoanWriteOffs.AddAsync(writeOff, cancellationToken).ConfigureAwait(false);
            writeOffCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} loan write-offs", tenant, writeOffCount);
    }
}
