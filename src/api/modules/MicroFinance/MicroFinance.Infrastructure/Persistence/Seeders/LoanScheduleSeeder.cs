using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for loan repayment schedules.
/// Creates installment schedules for disbursed loans to test repayment tracking.
/// </summary>
internal static class LoanScheduleSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        // Get disbursed loans that don't have schedules yet
        var disbursedLoans = await context.Loans
            .Where(l => l.Status == Loan.StatusDisbursed)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!disbursedLoans.Any()) return;

        var existingScheduleCount = await context.LoanSchedules.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingScheduleCount > 0) return; // Already seeded

        int totalInstallments = 0;

        foreach (var loan in disbursedLoans)
        {
            // Skip if this loan already has a schedule
            if (await context.LoanSchedules.AnyAsync(s => s.LoanId == loan.Id, cancellationToken).ConfigureAwait(false))
                continue;

            var disbursementDate = loan.DisbursementDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-60));
            var monthlyPayment = CalculateMonthlyPayment(loan.PrincipalAmount, loan.InterestRate, loan.TermMonths);
            var remainingPrincipal = loan.PrincipalAmount;

            for (int installment = 1; installment <= loan.TermMonths; installment++)
            {
                var dueDate = disbursementDate.AddMonths(installment);
                var interestAmount = Math.Round(remainingPrincipal * (loan.InterestRate / 100 / 12), 2);
                var principalAmount = Math.Round(monthlyPayment - interestAmount, 2);
                
                // Adjust last installment for rounding
                if (installment == loan.TermMonths)
                {
                    principalAmount = remainingPrincipal;
                }

                // Determine if this installment should be marked as paid (for testing)
                var isPaid = dueDate < DateOnly.FromDateTime(DateTime.UtcNow);
                var paidDate = isPaid ? dueDate.AddDays(Random.Shared.Next(-5, 10)) : (DateOnly?)null;
                var paidAmount = isPaid ? (principalAmount + interestAmount) : (decimal?)null;

                var schedule = LoanSchedule.Create(
                    loanId: loan.Id,
                    installmentNumber: installment,
                    dueDate: dueDate,
                    principalAmount: principalAmount,
                    interestAmount: interestAmount);

                if (isPaid)
                {
                    schedule.ApplyPayment(paidAmount!.Value, paidDate!.Value);
                }

                await context.LoanSchedules.AddAsync(schedule, cancellationToken).ConfigureAwait(false);
                remainingPrincipal -= principalAmount;
                totalInstallments++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} loan schedule installments for {LoanCount} disbursed loans", 
            tenant, totalInstallments, disbursedLoans.Count);
    }

    private static decimal CalculateMonthlyPayment(decimal principal, decimal annualRate, int months)
    {
        if (annualRate == 0) return principal / months;
        
        var monthlyRate = annualRate / 100 / 12;
        var payment = principal * (monthlyRate * (decimal)Math.Pow((double)(1 + monthlyRate), months)) 
                      / ((decimal)Math.Pow((double)(1 + monthlyRate), months) - 1);
        return Math.Round(payment, 2);
    }
}
