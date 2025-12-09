using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for share accounts.
/// Creates 100 share accounts for realistic demo database.
/// </summary>
internal static class ShareAccountSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 100;
        var existingCount = await context.ShareAccounts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Where(m => m.IsActive).Take(100).ToListAsync(cancellationToken).ConfigureAwait(false);
        var products = await context.ShareProducts.ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 1 || products.Count < 1) return;

        var random = new Random(42);
        int accountNumber = 2001;
        
        for (int i = 0; i < Math.Min(targetCount, members.Count); i++)
        {
            var accNum = $"SHR-{accountNumber + i:D6}";
            var exists = await context.ShareAccounts.AnyAsync(sa => sa.AccountNumber == accNum, cancellationToken).ConfigureAwait(false);
            if (exists) continue;

            var product = products[i % products.Count];
            var openDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-random.Next(1, 18)));
            
            // Create account in Pending status
            var account = ShareAccount.Create(
                accountNumber: accNum,
                memberId: members[i].Id,
                shareProductId: product.Id,
                openedDate: openDate);

            // Add to context first
            await context.ShareAccounts.AddAsync(account, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            
            // Now approve and activate the account (workflow: Pending -> Approved -> Active)
            account.Approve("Auto-approved during seeding");
            account.Activate();
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Purchase varying amounts of shares (5-100)
            int shareCount = 5 + (random.Next(1, 20) * 5);
            account.PurchaseShares(shareCount, product.CurrentPrice);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        logger.LogInformation("[{Tenant}] seeded {Count} share accounts", tenant, targetCount);
    }
}
