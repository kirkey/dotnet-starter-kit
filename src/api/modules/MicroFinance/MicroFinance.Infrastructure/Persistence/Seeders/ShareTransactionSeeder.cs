using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for share transactions.
/// Creates purchase and dividend history for share accounts - demo database.
/// </summary>
internal static class ShareTransactionSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 250;
        var existingCount = await context.ShareTransactions.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var shareAccounts = await context.ShareAccounts
            .Include(s => s.ShareProduct)
            .Where(s => s.Status == ShareAccount.StatusActive)
            .Take(100)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!shareAccounts.Any()) return;

        var random = new Random(42);
        int transactionCount = existingCount;
        int refNumber = 8001 + existingCount;

        foreach (var account in shareAccounts)
        {
            var pricePerShare = account.ShareProduct?.NominalValue ?? 100m;
            var currentBalance = 0;
            var transactionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-12));

            // Initial purchase
            var initialShares = random.Next(10, 50);
            currentBalance += initialShares;
            var initialPurchase = ShareTransaction.Create(
                shareAccountId: account.Id,
                reference: $"SHR-{refNumber++:D6}",
                transactionType: ShareTransaction.TypePurchase,
                numberOfShares: initialShares,
                pricePerShare: pricePerShare,
                sharesBalanceAfter: currentBalance,
                transactionDate: transactionDate);
            
            await context.ShareTransactions.AddAsync(initialPurchase, cancellationToken).ConfigureAwait(false);
            transactionCount++;

            // Additional purchases over time (2-4 transactions)
            var additionalPurchases = random.Next(2, 5);
            for (int i = 0; i < additionalPurchases; i++)
            {
                transactionDate = transactionDate.AddDays(random.Next(30, 90));
                if (transactionDate > DateOnly.FromDateTime(DateTime.UtcNow)) break;

                var shares = random.Next(5, 20);
                currentBalance += shares;
                var purchase = ShareTransaction.Create(
                    shareAccountId: account.Id,
                    reference: $"SHR-{refNumber++:D6}",
                    transactionType: ShareTransaction.TypePurchase,
                    numberOfShares: shares,
                    pricePerShare: pricePerShare,
                    sharesBalanceAfter: currentBalance,
                    transactionDate: transactionDate);

                await context.ShareTransactions.AddAsync(purchase, cancellationToken).ConfigureAwait(false);
                transactionCount++;
            }

            // Add a dividend for accounts older than 6 months (annual dividend)
            if (transactionDate < DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-6)))
            {
                var dividendDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-3));
                var dividendShares = Math.Max(1, currentBalance / 20); // 5% stock dividend
                currentBalance += dividendShares;

                var dividend = ShareTransaction.Create(
                    shareAccountId: account.Id,
                    reference: $"DIV-{refNumber++:D6}",
                    transactionType: ShareTransaction.TypeDividend,
                    numberOfShares: dividendShares,
                    pricePerShare: pricePerShare,
                    sharesBalanceAfter: currentBalance,
                    transactionDate: dividendDate);

                await context.ShareTransactions.AddAsync(dividend, cancellationToken).ConfigureAwait(false);
                transactionCount++;
            }

            // Some accounts have partial redemptions (sell back)
            if (random.NextDouble() > 0.7 && currentBalance > 20)
            {
                var redemptionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(10, 60)));
                var redeemShares = random.Next(5, Math.Min(15, currentBalance / 3));
                currentBalance -= redeemShares;

                var redemption = ShareTransaction.Create(
                    shareAccountId: account.Id,
                    reference: $"SHR-{refNumber++:D6}",
                    transactionType: ShareTransaction.TypeRedemption,
                    numberOfShares: redeemShares,
                    pricePerShare: pricePerShare,
                    sharesBalanceAfter: currentBalance,
                    transactionDate: redemptionDate);

                await context.ShareTransactions.AddAsync(redemption, cancellationToken).ConfigureAwait(false);
                transactionCount++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} share transactions (purchases, dividends, redemptions)", tenant, transactionCount);
    }
}
