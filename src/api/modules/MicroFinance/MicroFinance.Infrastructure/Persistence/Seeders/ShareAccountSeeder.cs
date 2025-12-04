using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for share accounts.
/// </summary>
internal static class ShareAccountSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 10;
        var existingCount = await context.ShareAccounts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
        var products = await context.ShareProducts.Take(2).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 1 || products.Count < 1) return;

        int accountNumber = 2001;
        for (int i = 0; i < Math.Min(10, members.Count); i++)
        {
            var accNum = $"SHR-{accountNumber + i:D6}";
            var exists = await context.ShareAccounts.AnyAsync(sa => sa.AccountNumber == accNum, cancellationToken).ConfigureAwait(false);
            if (exists) continue;

            var account = ShareAccount.Create(
                accountNumber: accNum,
                memberId: members[i].Id,
                shareProductId: products[i % products.Count].Id,
                openedDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-3)));

            // Purchase some initial shares
            account.PurchaseShares((i + 1) * 5, products[i % products.Count].CurrentPrice);

            await context.ShareAccounts.AddAsync(account, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded share accounts", tenant);
    }
}
