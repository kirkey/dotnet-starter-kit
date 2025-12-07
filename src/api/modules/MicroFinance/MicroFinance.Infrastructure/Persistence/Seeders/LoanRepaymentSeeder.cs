using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for loan repayments with comprehensive test data.
/// Creates repayment history for disbursed loans to test repayment recording and tracking.
/// </summary>
internal static class LoanRepaymentSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 50;
        var existingCount = await context.LoanRepayments.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        // Get disbursed loans that should have repayments
        var disbursedLoans = await context.Loans
            .Where(l => l.Status == Loan.StatusDisbursed)
            .Take(15)
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        if (disbursedLoans.Count < 3) return;

        var random = new Random(42); // Fixed seed for reproducibility
        int receiptNumber = 50001;

        // Generate repayments for each disbursed loan
        foreach (var loan in disbursedLoans)
        {
            // Calculate expected monthly payment (simplified)
            decimal monthlyInterest = loan.PrincipalAmount * (loan.InterestRate / 100) / 12;
            decimal monthlyPrincipal = loan.PrincipalAmount / loan.TermMonths;
            decimal expectedPayment = monthlyPrincipal + monthlyInterest;

            // Generate 1-4 repayments per loan
            int repaymentCount = random.Next(1, 5);
            
            for (int i = 0; i < repaymentCount && existingCount + receiptNumber - 50001 < targetCount; i++)
            {
                var receiptNum = $"RCP-{receiptNumber++:D8}";
                
                if (await context.LoanRepayments.AnyAsync(r => r.ReceiptNumber == receiptNum, cancellationToken).ConfigureAwait(false))
                    continue;

                // Payment date spread based on disbursement
                int daysAfterDisbursement = 30 * (i + 1) + random.Next(-5, 10);
                var paymentDate = loan.DisbursementDate?.AddDays(daysAfterDisbursement) 
                    ?? DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(30, 90)));

                // Slight variation in payment amounts
                decimal variationFactor = 0.9m + (decimal)random.NextDouble() * 0.2m; // 0.9 to 1.1
                decimal principalPaid = Math.Round(monthlyPrincipal * variationFactor, 2);
                decimal interestPaid = Math.Round(monthlyInterest * variationFactor, 2);
                
                // Occasionally include penalty (10% chance)
                decimal penaltyPaid = random.NextDouble() < 0.1 ? Math.Round((decimal)(random.NextDouble() * 50 + 10), 2) : 0;

                var paymentMethods = new[] { "Cash", "Bank Transfer", "Mobile Money", "Payroll Deduction" };
                var paymentMethod = paymentMethods[random.Next(paymentMethods.Length)];

                var repayment = LoanRepayment.Create(
                    loanId: loan.Id,
                    receiptNumber: receiptNum,
                    principalAmount: principalPaid,
                    interestAmount: interestPaid,
                    paymentMethod: paymentMethod,
                    penaltyAmount: penaltyPaid,
                    repaymentDate: paymentDate,
                    notes: penaltyPaid > 0 ? "Late payment - includes penalty" : null);

                await context.LoanRepayments.AddAsync(repayment, cancellationToken).ConfigureAwait(false);
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded loan repayments for testing payment history and tracking", tenant);
    }
}
