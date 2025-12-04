using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for savings accounts.
/// </summary>
internal static class SavingsAccountSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 10;
        var existingCount = await context.SavingsAccounts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);
        var products = await context.SavingsProducts.Take(3).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 1 || products.Count < 1) return;

        int accountNumber = 1001;
        for (int i = 0; i < Math.Min(10, members.Count); i++)
        {
            var accNum = $"SAV-{accountNumber + i:D6}";
            var exists = await context.SavingsAccounts.AnyAsync(sa => sa.AccountNumber == accNum, cancellationToken).ConfigureAwait(false);
            if (exists) continue;

            var account = SavingsAccount.Create(
                accountNumber: accNum,
                memberId: members[i].Id,
                savingsProductId: products[i % products.Count].Id,
                openingBalance: (i + 1) * 100,
                openedDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-3)));

            await context.SavingsAccounts.AddAsync(account, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded savings accounts", tenant);
    }
}
