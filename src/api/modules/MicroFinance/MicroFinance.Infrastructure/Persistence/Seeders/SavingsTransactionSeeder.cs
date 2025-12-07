using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for savings transactions with comprehensive test data.
/// Creates transaction history for savings accounts to test deposits, withdrawals, and transaction history.
/// </summary>
internal static class SavingsTransactionSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 100;
        var existingCount = await context.SavingsTransactions.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var accounts = await context.SavingsAccounts
            .Where(sa => sa.Status == SavingsAccount.StatusActive)
            .Take(20)
            .ToListAsync(cancellationToken).ConfigureAwait(false);

        if (accounts.Count < 5) return;

        var random = new Random(42); // Fixed seed for reproducibility
        int transactionNumber = 10001;

        // Generate transactions for each active account
        foreach (var account in accounts)
        {
            // Generate 3-8 transactions per account
            int transactionCount = random.Next(3, 9);
            var currentBalance = account.Balance;

            for (int i = 0; i < transactionCount && existingCount + transactionNumber - 10001 < targetCount; i++)
            {
                var txnRef = $"TXN-{transactionNumber++:D8}";
                
                if (await context.SavingsTransactions.AnyAsync(t => t.Reference == txnRef, cancellationToken).ConfigureAwait(false))
                    continue;

                // Alternate between deposits and withdrawals, with more deposits
                bool isDeposit = random.NextDouble() > 0.3;
                string txnType = isDeposit ? SavingsTransaction.TypeDeposit : SavingsTransaction.TypeWithdrawal;
                
                decimal amount;
                if (isDeposit)
                {
                    // Deposits: 50-2000
                    amount = Math.Round((decimal)(random.NextDouble() * 1950 + 50), 2);
                    currentBalance += amount;
                }
                else
                {
                    // Withdrawals: 20-500 (never more than balance)
                    decimal maxWithdraw = Math.Min(500, currentBalance * 0.5m);
                    if (maxWithdraw < 20) continue; // Skip if balance too low
                    amount = Math.Round((decimal)(random.NextDouble() * (double)(maxWithdraw - 20) + 20), 2);
                    currentBalance -= amount;
                }

                // Transaction date spread over last 6 months
                int daysAgo = random.Next(1, 180);
                var txnDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-daysAgo));

                var paymentMethods = new[] { "Cash", "Bank Transfer", "Mobile Money", "Cheque" };
                var paymentMethod = paymentMethods[random.Next(paymentMethods.Length)];

                var transaction = SavingsTransaction.Create(
                    savingsAccountId: account.Id,
                    reference: txnRef,
                    transactionType: txnType,
                    amount: amount,
                    balanceAfter: currentBalance,
                    transactionDate: txnDate,
                    description: isDeposit 
                        ? $"Deposit via {paymentMethod}" 
                        : $"Withdrawal via {paymentMethod}",
                    paymentMethod: paymentMethod);

                await context.SavingsTransactions.AddAsync(transaction, cancellationToken).ConfigureAwait(false);
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded savings transactions for testing deposit/withdrawal history", tenant);
    }
}
