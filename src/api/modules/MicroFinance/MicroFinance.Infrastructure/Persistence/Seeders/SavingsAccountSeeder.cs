using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for savings accounts with comprehensive test data.
/// Creates 80 accounts across various products and statuses for testing deposits, withdrawals, and account management.
/// </summary>
internal static class SavingsAccountSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 80;
        var existingCount = await context.SavingsAccounts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Where(m => m.IsActive).Take(80).ToListAsync(cancellationToken).ConfigureAwait(false);
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
            (10, regularSavings, 680, "Active", 7),
            (11, regularSavings, 2100, "Active", 13),
            (12, regularSavings, 1450, "Active", 9),
            (13, regularSavings, 890, "Active", 5),
            (14, regularSavings, 3700, "Active", 20),
            
            // Business Savings accounts
            (15, businessSavings, 5000, "Active", 12),
            (16, businessSavings, 8500, "Active", 9),
            (17, businessSavings, 12000, "Active", 15),
            (18, businessSavings, 3500, "Active", 6),
            (19, businessSavings, 7200, "Active", 8),
            (20, businessSavings, 15000, "Active", 18),
            (21, businessSavings, 9800, "Active", 11),
            (22, businessSavings, 6300, "Active", 7),
            (23, businessSavings, 18500, "Active", 24),
            (24, businessSavings, 4200, "Active", 4),
            
            // Emergency Fund accounts
            (25, emergencyFund, 1000, "Active", 6),
            (26, emergencyFund, 2000, "Active", 8),
            (27, emergencyFund, 500, "Active", 4),
            (28, emergencyFund, 3000, "Active", 12),
            (29, emergencyFund, 1500, "Active", 5),
            (30, emergencyFund, 2500, "Active", 10),
            (31, emergencyFund, 800, "Active", 3),
            (32, emergencyFund, 4000, "Active", 15),
            (33, emergencyFund, 1200, "Active", 6),
            (34, emergencyFund, 3500, "Active", 9),
            
            // Fixed Savings (higher minimums)
            (35, fixedSavings, 5000, "Active", 12),
            (36, fixedSavings, 10000, "Active", 18),
            (37, fixedSavings, 7500, "Active", 9),
            (38, fixedSavings, 15000, "Active", 24),
            (39, fixedSavings, 8000, "Active", 6),
            (40, fixedSavings, 12000, "Active", 15),
            (41, fixedSavings, 20000, "Active", 24),
            (42, fixedSavings, 6000, "Active", 8),
            (43, fixedSavings, 9500, "Active", 12),
            (44, fixedSavings, 18000, "Active", 20),
            
            // Premium Savings (high-value accounts)
            (45, premiumSavings, 25000, "Active", 12),
            (46, premiumSavings, 50000, "Active", 24),
            (47, premiumSavings, 35000, "Active", 18),
            (48, premiumSavings, 75000, "Active", 30),
            (49, premiumSavings, 45000, "Active", 15),
            (50, premiumSavings, 60000, "Active", 20),
            (51, premiumSavings, 100000, "Active", 36),
            (52, premiumSavings, 85000, "Active", 28),
            
            // Dormant accounts (for testing status filters)
            (53, regularSavings, 50, "Dormant", 30),
            (54, regularSavings, 100, "Dormant", 36),
            (55, emergencyFund, 75, "Dormant", 24),
            (56, regularSavings, 25, "Dormant", 42),
            (57, businessSavings, 200, "Dormant", 28),
            (58, regularSavings, 80, "Dormant", 32),
            
            // Frozen accounts (for testing freeze/unfreeze)
            (59, regularSavings, 5000, "Frozen", 12),
            (60, businessSavings, 15000, "Frozen", 9),
            (61, premiumSavings, 40000, "Frozen", 6),
            (62, fixedSavings, 10000, "Frozen", 8),
            
            // More active accounts for variety
            (63, regularSavings, 2800, "Active", 7),
            (64, businessSavings, 6500, "Active", 10),
            (65, emergencyFund, 2200, "Active", 8),
            (66, fixedSavings, 12000, "Active", 15),
            (67, regularSavings, 1100, "Active", 4),
            (68, premiumSavings, 30000, "Active", 12),
            (69, regularSavings, 1650, "Active", 6),
            (70, businessSavings, 5800, "Active", 9),
            (71, emergencyFund, 1800, "Active", 7),
            (72, fixedSavings, 8500, "Active", 10),
            (73, regularSavings, 920, "Active", 5),
            (74, premiumSavings, 55000, "Active", 18),
            (75, regularSavings, 3100, "Active", 11),
            (76, businessSavings, 11000, "Active", 14),
            (77, emergencyFund, 2800, "Active", 9),
            (78, fixedSavings, 14000, "Active", 16),
            (79, regularSavings, 1350, "Active", 6),
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
