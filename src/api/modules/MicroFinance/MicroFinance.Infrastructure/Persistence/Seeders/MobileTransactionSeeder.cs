using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for mobile transactions.
/// Creates transaction history for mobile wallets.
/// </summary>
internal static class MobileTransactionSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.MobileTransactions.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var wallets = await context.MobileWallets
            .Where(w => w.Status == MobileWallet.StatusActive)
            .Take(30)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!wallets.Any()) return;

        var random = new Random(42);
        int refNumber = 11001;
        int transactionCount = 0;

        var merchants = new[] 
        { 
            "7-Eleven", "Ministop", "Jollibee", "McDonald's", "SM Store", 
            "Robinsons", "Mercury Drug", "Meralco", "Manila Water", "PLDT Home"
        };

        foreach (var wallet in wallets)
        {
            // Generate 5-15 transactions per wallet
            int txnCount = random.Next(5, 16);
            var currentBalance = wallet.Balance;

            for (int i = 0; i < txnCount; i++)
            {
                var txnRef = $"MTX-{refNumber++:D8}";
                var txnDate = DateTimeOffset.UtcNow.AddDays(-random.Next(1, 90));

                // Transaction types: TopUp, Payment, Transfer, Withdrawal
                var txnTypes = new[] { "TopUp", "Payment", "Transfer", "Withdrawal" };
                var txnType = txnTypes[random.Next(txnTypes.Length)];

                decimal amount;
                string? recipient = null;
                string? merchant = null;

                switch (txnType)
                {
                    case "TopUp":
                        amount = (random.Next(1, 20) * 100); // 100-2000
                        currentBalance += amount;
                        break;
                    case "Payment":
                        amount = Math.Min((random.Next(1, 10) * 50), currentBalance * 0.3m); // 50-500
                        merchant = merchants[random.Next(merchants.Length)];
                        currentBalance -= amount;
                        break;
                    case "Transfer":
                        amount = Math.Min((random.Next(1, 5) * 100), currentBalance * 0.4m); // 100-500
                        recipient = $"+639{random.Next(100000000, 999999999)}";
                        currentBalance -= amount;
                        break;
                    case "Withdrawal":
                        amount = Math.Min((random.Next(1, 10) * 100), currentBalance * 0.5m); // 100-1000
                        currentBalance -= amount;
                        break;
                    default:
                        continue;
                }

                if (amount <= 0) continue;

                var transaction = MobileTransaction.Create(
                    mobileWalletId: wallet.Id,
                    transactionReference: txnRef,
                    transactionType: txnType,
                    amount: Math.Round(amount, 2),
                    balanceAfter: Math.Max(0, Math.Round(currentBalance, 2)),
                    recipientNumber: recipient,
                    merchantName: merchant);

                transaction.Complete();
                await context.MobileTransactions.AddAsync(transaction, cancellationToken).ConfigureAwait(false);
                transactionCount++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} mobile transactions", tenant, transactionCount);
    }
}
