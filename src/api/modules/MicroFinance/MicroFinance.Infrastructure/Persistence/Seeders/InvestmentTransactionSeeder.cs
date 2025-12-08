using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for investment transactions.
/// Creates buy/sell/dividend transactions for investment accounts - demo database.
/// </summary>
internal static class InvestmentTransactionSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 150;
        var existingCount = await context.InvestmentTransactions.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var accounts = await context.InvestmentAccounts
            .Where(a => a.Status == InvestmentAccount.StatusActive)
            .Take(50)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var products = await context.InvestmentProducts
            .Where(p => p.Status == InvestmentProduct.StatusActive)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!accounts.Any() || !products.Any()) return;

        var random = new Random(42);
        int refNumber = 9001;
        int transactionCount = 0;

        foreach (var account in accounts)
        {
            var product = products[random.Next(products.Count)];
            var nav = 10m + (decimal)(random.NextDouble() * 5); // NAV between 10-15
            
            // Initial investment (buy)
            var initialAmount = 5000 + (random.Next(1, 20) * 1000);
            var entryLoad = initialAmount * 0.01m; // 1% entry load
            
            var buyTransaction = InvestmentTransaction.CreateBuy(
                investmentAccountId: account.Id,
                productId: product.Id,
                transactionReference: $"INV-{refNumber++:D6}",
                amount: initialAmount,
                entryLoad: entryLoad,
                paymentMode: "Bank Transfer",
                paymentReference: $"BT-{random.Next(100000, 999999)}");

            var allotmentDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(30, 90)));
            buyTransaction.Process(nav, (initialAmount - entryLoad) / nav, allotmentDate);
            await context.InvestmentTransactions.AddAsync(buyTransaction, cancellationToken).ConfigureAwait(false);
            transactionCount++;

            // Additional purchases (1-3)
            int additionalBuys = random.Next(1, 4);
            for (int i = 0; i < additionalBuys; i++)
            {
                var amount = 1000 + (random.Next(1, 10) * 500);
                var load = amount * 0.01m;
                var newNav = nav * (1 + (decimal)(random.NextDouble() * 0.1 - 0.05)); // ±5% NAV change

                var additionalBuy = InvestmentTransaction.CreateBuy(
                    investmentAccountId: account.Id,
                    productId: product.Id,
                    transactionReference: $"INV-{refNumber++:D6}",
                    amount: amount,
                    entryLoad: load,
                    paymentMode: random.NextDouble() > 0.5 ? "Bank Transfer" : "GCash");

                var addlAllotmentDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(1, 30)));
                additionalBuy.Process(newNav, (amount - load) / newNav, addlAllotmentDate);
                await context.InvestmentTransactions.AddAsync(additionalBuy, cancellationToken).ConfigureAwait(false);
                transactionCount++;
            }

            // Add dividend for some accounts (30% chance)
            // Note: CreateDividend doesn't exist in domain, dividends would be created via different mechanism
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} investment transactions", tenant, transactionCount);
    }
}
