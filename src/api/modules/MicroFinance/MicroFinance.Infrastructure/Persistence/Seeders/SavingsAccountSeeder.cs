using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for savings accounts with comprehensive test data.
/// Creates 40 accounts across various products and statuses for testing deposits, withdrawals, and account management.
/// </summary>
internal static class SavingsAccountSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 40;
        var existingCount = await context.SavingsAccounts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Where(m => m.IsActive).Take(40).ToListAsync(cancellationToken).ConfigureAwait(false);
        var products = await context.SavingsProducts.Where(p => p.IsActive).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 10 || products.Count < 3) return;

        // Get product references
        var regularSavings = products.FirstOrDefault(p => p.Code == "REGULAR-SAVINGS") ?? products[0];
        var juniorSavings = products.FirstOrDefault(p => p.Code == "JUNIOR-SAVINGS") ?? products[0];
        var fixedSavings = products.FirstOrDefault(p => p.Code == "FIXED-SAVINGS") ?? products[0];
        var emergencyFund = products.FirstOrDefault(p => p.Code == "EMERGENCY-FUND") ?? products[0];
        var businessSavings = products.FirstOrDefault(p => p.Code == "BUSINESS-SAVINGS") ?? products[0];
        var premiumSavings = products.FirstOrDefault(p => p.Code == "PREMIUM-SAVINGS") ?? products[0];

        var accountData = new (int MemberIdx, SavingsProduct Product, decimal OpeningBalance, string Status, int MonthsAgo)[]
        {
            // Active accounts with various balances - Regular Savings
            (0, regularSavings, 500, "Active", 12),
            (1, regularSavings, 1200, "Active", 10),
            (2, regularSavings, 800, "Active", 8),
            (3, regularSavings, 2500, "Active", 15),
            (4, regularSavings, 350, "Active", 6),
            (5, regularSavings, 1800, "Active", 9),
            (6, regularSavings, 950, "Active", 11),
            (7, regularSavings, 3200, "Active", 18),
            (8, regularSavings, 150, "Active", 3),
            (9, regularSavings, 4500, "Active", 24),
            
            // Business Savings accounts
            (10, businessSavings, 5000, "Active", 12),
            (11, businessSavings, 8500, "Active", 9),
            (12, businessSavings, 12000, "Active", 15),
            (13, businessSavings, 3500, "Active", 6),
            (14, businessSavings, 7200, "Active", 8),
            
            // Emergency Fund accounts
            (15, emergencyFund, 1000, "Active", 6),
            (16, emergencyFund, 2000, "Active", 8),
            (17, emergencyFund, 500, "Active", 4),
            (18, emergencyFund, 3000, "Active", 12),
            (19, emergencyFund, 1500, "Active", 5),
            
            // Fixed Savings (higher minimums)
            (20, fixedSavings, 5000, "Active", 12),
            (21, fixedSavings, 10000, "Active", 18),
            (22, fixedSavings, 7500, "Active", 9),
            (23, fixedSavings, 15000, "Active", 24),
            (24, fixedSavings, 8000, "Active", 6),
            
            // Premium Savings (high-value accounts)
            (25, premiumSavings, 25000, "Active", 12),
            (26, premiumSavings, 50000, "Active", 24),
            (27, premiumSavings, 35000, "Active", 18),
            
            // Dormant accounts (for testing status filters)
            (28, regularSavings, 50, "Dormant", 30),
            (29, regularSavings, 100, "Dormant", 36),
            (30, emergencyFund, 75, "Dormant", 24),
            
            // Frozen accounts (for testing freeze/unfreeze)
            (31, regularSavings, 5000, "Frozen", 12),
            (32, businessSavings, 15000, "Frozen", 9),
            (33, premiumSavings, 40000, "Frozen", 6),
            
            // More active accounts for variety
            (34, regularSavings, 2800, "Active", 7),
            (35, businessSavings, 6500, "Active", 10),
            (36, emergencyFund, 2200, "Active", 8),
            (37, fixedSavings, 12000, "Active", 15),
            (38, regularSavings, 1100, "Active", 4),
            (39, premiumSavings, 30000, "Active", 12),
        };

        int accountNumber = 1001;
        for (int i = 0; i < accountData.Length && i + existingCount < targetCount; i++)
        {
            var data = accountData[i];
            var accNum = $"SAV-{accountNumber + i:D6}";
            
            if (await context.SavingsAccounts.AnyAsync(sa => sa.AccountNumber == accNum, cancellationToken).ConfigureAwait(false))
                continue;

            var member = members[data.MemberIdx % members.Count];
            var product = data.Product;

            var account = SavingsAccount.Create(
                accountNumber: accNum,
                memberId: member.Id,
                savingsProductId: product.Id,
                openingBalance: data.OpeningBalance,
                openedDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-data.MonthsAgo)));

            // Apply status based on data - Note: Dormant accounts are just old Active accounts
            // Freeze is the only status change method available
            if (data.Status == "Frozen")
            {
                account.Freeze("Suspicious activity - under review");
            }
            // For dormant, we keep them as Active but with old dates (dormancy would be determined by last activity)

            await context.SavingsAccounts.AddAsync(account, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} savings accounts across various products and statuses", tenant, targetCount);
    }
}
