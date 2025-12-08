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

        // Get disbursed loans that might need restructuring
        var troubledLoans = await context.Loans
            .Where(l => l.Status == Loan.StatusDisbursed)
            .Take(10)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (troubledLoans.Count == 0) return;

        // Get staff IDs for approvals
        var staffIds = await context.Staff.Select(s => s.Id).ToListAsync(cancellationToken).ConfigureAwait(false);
        if (!staffIds.Any()) return;

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

            // Calculate new terms based on restructure type
            var newRate = data.Type == LoanRestructure.TypeRateReduction
                ? loan.InterestRate * 0.8m
                : loan.InterestRate;
            var newTerm = data.Type == LoanRestructure.TypeTermExtension
                ? loan.TermMonths + 6
                : loan.TermMonths;
            var newPrincipal = data.Type == LoanRestructure.TypePrincipalReduction
                ? loan.OutstandingPrincipal * 0.9m
                : loan.OutstandingPrincipal;
            var newInstallment = newPrincipal / newTerm;

            var restructure = LoanRestructure.Create(
                loanId: loan.Id,
                restructureNumber: $"RS-{restructureNumber++:D6}",
                restructureType: data.Type,
                originalPrincipal: loan.PrincipalAmount,
                originalInterestRate: loan.InterestRate,
                originalRemainingTerm: loan.TermMonths,
                originalInstallmentAmount: loan.PrincipalAmount / loan.TermMonths,
                newPrincipal: newPrincipal,
                newInterestRate: newRate,
                newTerm: newTerm,
                newInstallmentAmount: newInstallment,
                reason: data.Reason);

            // Submit for approval
            restructure.SubmitForApproval();

            // Approve most restructures
            if (random.NextDouble() < 0.85)
            {
                var effectiveDate = requestDate.AddDays(random.Next(7, 21));
                restructure.Approve(staffIds[random.Next(staffIds.Count)], "Credit Committee", effectiveDate);
                restructure.Activate();

                // Complete some older ones
                if (random.NextDouble() < 0.3)
                {
                    restructure.Complete();
                }
            }
            else if (random.NextDouble() < 0.5)
            {
                restructure.Reject(staffIds[random.Next(staffIds.Count)], "Does not meet restructuring criteria");
            }

            await context.LoanRestructures.AddAsync(restructure, cancellationToken).ConfigureAwait(false);
            restructureCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} loan restructures", tenant, restructureCount);
    }
}
