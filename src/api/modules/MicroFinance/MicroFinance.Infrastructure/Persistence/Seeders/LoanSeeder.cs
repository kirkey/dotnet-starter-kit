using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for loans.
/// </summary>
internal static class LoanSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 10;
        var existingCount = await context.Loans.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
        var products = await context.LoanProducts.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 1 || products.Count < 1) return;

        int loanNumber = 3001;
        for (int i = 0; i < Math.Min(10, members.Count); i++)
        {
            var loanNum = $"LN-{loanNumber + i:D6}";
            var exists = await context.Loans.AnyAsync(l => l.LoanNumber == loanNum, cancellationToken).ConfigureAwait(false);
            if (exists) continue;

            var product = products[i % products.Count];
            var loan = Loan.Create(
                memberId: members[i].Id,
                loanProductId: product.Id,
                loanNumber: loanNum,
                principalAmount: product.MinLoanAmount + ((product.MaxLoanAmount - product.MinLoanAmount) / 10 * (i + 1)),
                interestRate: product.InterestRate,
                termMonths: product.MinTermMonths + ((product.MaxTermMonths - product.MinTermMonths) / 5 * (i % 5)),
                repaymentFrequency: product.RepaymentFrequency,
                purpose: $"Loan purpose for member {i + 1}");

            // Approve and disburse some loans
            if (i < 5)
            {
                loan.Approve(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30)));
                if (i < 3)
                {
                    loan.Disburse(
                        DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-25)),
                        DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(loan.TermMonths)));
                }
            }

            await context.Loans.AddAsync(loan, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded loans", tenant);
    }
}
