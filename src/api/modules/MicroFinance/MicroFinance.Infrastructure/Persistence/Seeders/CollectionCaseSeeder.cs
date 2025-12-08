using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for collection cases.
/// Creates collection cases for overdue loans.
/// </summary>
internal static class CollectionCaseSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.CollectionCases.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        // Get disbursed loans (simulate some are overdue)
        var disbursedLoans = await context.Loans
            .Where(l => l.Status == Loan.StatusDisbursed)
            .Take(15)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var collectors = await context.Staff
            .Where(s => s.Role == Staff.RoleLoanOfficer && s.Status == Staff.StatusActive)
            .Take(5)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!disbursedLoans.Any() || !collectors.Any()) return;

        var random = new Random(42);
        int caseNumber = 7001;
        int caseCount = 0;

        foreach (var loan in disbursedLoans)
        {
            // Simulate 50% of loans are overdue
            if (random.NextDouble() > 0.5) continue;

            var daysPastDue = random.Next(1, 90);
            var overdueAmount = loan.PrincipalAmount / loan.TermMonths * (1 + (daysPastDue / 30));
            var collector = collectors[random.Next(collectors.Count)];

            var caseStatus = daysPastDue switch
            {
                < 15 => CollectionCase.StatusOpen,
                < 30 => CollectionCase.StatusInProgress,
                < 60 => CollectionCase.StatusLegal,
                _ => CollectionCase.StatusLegal
            };

            var priority = daysPastDue switch
            {
                < 7 => CollectionCase.PriorityLow,
                < 30 => CollectionCase.PriorityMedium,
                < 60 => CollectionCase.PriorityHigh,
                _ => CollectionCase.PriorityCritical
            };

            var collectionCase = CollectionCase.Create(
                caseNumber: $"COL-{caseNumber++:D6}",
                loanId: loan.Id,
                memberId: loan.MemberId,
                daysPastDue: daysPastDue,
                amountOverdue: Math.Round(overdueAmount, 2),
                totalOutstanding: loan.PrincipalAmount + (loan.PrincipalAmount * loan.InterestRate / 100));

            // Assign to collector
            collectionCase.Assign(collector.Id);
            
            if (caseStatus == CollectionCase.StatusLegal)
                collectionCase.EscalateToLegal("Exceeds 60 days past due - legal action required");

            await context.CollectionCases.AddAsync(collectionCase, cancellationToken).ConfigureAwait(false);
            caseCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} collection cases for overdue loans", tenant, caseCount);
    }
}
