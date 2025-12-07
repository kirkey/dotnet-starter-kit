using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for loan restructures.
/// Creates loan restructuring records for troubled loans.
/// </summary>
internal static class LoanRestructureSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.LoanRestructures.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var troubledLoans = await context.Loans
            .Where(l => l.Status == Loan.StatusInArrears || l.Status == Loan.StatusRescheduled)
            .Take(10)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!troubledLoans.Any()) return;

        var random = new Random(42);
        int restructureNumber = 19001;
        int restructureCount = 0;

        var restructureData = new (string Type, string Reason)[]
        {
            (LoanRestructure.TypeTermExtension, "Member requested extended term due to income reduction from job loss"),
            (LoanRestructure.TypeRateReduction, "Interest rate reduced as retention measure for long-standing member"),
            (LoanRestructure.TypePaymentHoliday, "3-month payment holiday granted due to medical emergency"),
            (LoanRestructure.TypeTermExtension, "Term extended by 6 months to reduce monthly amortization"),
            (LoanRestructure.TypeRefinance, "Refinanced with new terms after partial settlement"),
            (LoanRestructure.TypeConsolidation, "Multiple loans consolidated into single facility"),
            (LoanRestructure.TypePrincipalReduction, "Principal reduced by 10% as settlement negotiation"),
            (LoanRestructure.TypePaymentHoliday, "Payment holiday due to natural disaster affecting livelihood"),
        };

        int dataIndex = 0;
        foreach (var loan in troubledLoans)
        {
            var data = restructureData[dataIndex++ % restructureData.Length];
            var requestDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(30, 120)));

            var restructure = LoanRestructure.Create(
                loanId: loan.Id,
                restructureNumber: $"RS-{restructureNumber++:D6}",
                restructureType: data.Type,
                reason: data.Reason,
                requestDate: requestDate,
                originalPrincipal: loan.PrincipalAmount,
                originalInterestRate: loan.InterestRate,
                originalTerm: loan.LoanTermMonths);

            // Set new terms based on type
            var newRate = data.Type == LoanRestructure.TypeRateReduction
                ? loan.InterestRate * 0.8m
                : loan.InterestRate;
            var newTerm = data.Type == LoanRestructure.TypeTermExtension
                ? loan.LoanTermMonths + 6
                : loan.LoanTermMonths;
            var newPrincipal = data.Type == LoanRestructure.TypePrincipalReduction
                ? loan.OutstandingPrincipal * 0.9m
                : loan.OutstandingPrincipal;

            restructure.SetNewTerms(
                newPrincipal: newPrincipal,
                newInterestRate: newRate,
                newTerm: newTerm);

            // Submit for approval
            restructure.SubmitForApproval();

            // Approve most restructures
            if (random.NextDouble() < 0.85)
            {
                restructure.Approve(Guid.NewGuid(), "Credit Committee");
                restructure.Activate(requestDate.AddDays(random.Next(7, 21)));

                // Complete some older ones
                if (random.NextDouble() < 0.3)
                {
                    restructure.Complete("Restructured loan fully repaid");
                }
            }
            else if (random.NextDouble() < 0.5)
            {
                restructure.Reject("Does not meet restructuring criteria");
            }

            await context.LoanRestructures.AddAsync(restructure, cancellationToken).ConfigureAwait(false);
            restructureCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} loan restructures", tenant, restructureCount);
    }
}
