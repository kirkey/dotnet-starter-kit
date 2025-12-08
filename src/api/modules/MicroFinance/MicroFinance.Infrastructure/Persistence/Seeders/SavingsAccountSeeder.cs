using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for savings accounts with comprehensive test data.
/// Creates 200 accounts across various products and statuses for realistic demo database.
/// </summary>
internal static class SavingsAccountSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 200;
        var existingCount = await context.SavingsAccounts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Where(m => m.IsActive).Take(200).ToListAsync(cancellationToken).ConfigureAwait(false);
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
            
            // Additional 120 accounts for comprehensive demo (80-199)
            // Regular Savings - Various balances
            (80, regularSavings, 450, "Active", 5),
            (81, regularSavings, 780, "Active", 8),
            (82, regularSavings, 1520, "Active", 12),
            (83, regularSavings, 2100, "Active", 15),
            (84, regularSavings, 3250, "Active", 18),
            (85, regularSavings, 560, "Active", 4),
            (86, regularSavings, 1890, "Active", 10),
            (87, regularSavings, 2450, "Active", 13),
            (88, regularSavings, 380, "Active", 3),
            (89, regularSavings, 4200, "Active", 20),
            (90, regularSavings, 1050, "Active", 7),
            (91, regularSavings, 620, "Active", 5),
            (92, regularSavings, 1750, "Active", 11),
            (93, regularSavings, 890, "Active", 6),
            (94, regularSavings, 2980, "Active", 14),
            (95, regularSavings, 1280, "Active", 9),
            (96, regularSavings, 3650, "Active", 17),
            (97, regularSavings, 520, "Active", 4),
            (98, regularSavings, 2150, "Active", 12),
            (99, regularSavings, 1420, "Active", 8),
            
            // Business Savings - More accounts
            (100, businessSavings, 4500, "Active", 6),
            (101, businessSavings, 7800, "Active", 9),
            (102, businessSavings, 9200, "Active", 12),
            (103, businessSavings, 5600, "Active", 7),
            (104, businessSavings, 13500, "Active", 15),
            (105, businessSavings, 8100, "Active", 10),
            (106, businessSavings, 6200, "Active", 8),
            (107, businessSavings, 11800, "Active", 13),
            (108, businessSavings, 7300, "Active", 9),
            (109, businessSavings, 16500, "Active", 18),
            (110, businessSavings, 4800, "Active", 5),
            (111, businessSavings, 10200, "Active", 11),
            (112, businessSavings, 8900, "Active", 10),
            (113, businessSavings, 5100, "Active", 6),
            (114, businessSavings, 14200, "Active", 14),
            
            // Emergency Fund - More accounts
            (115, emergencyFund, 850, "Active", 4),
            (116, emergencyFund, 1650, "Active", 7),
            (117, emergencyFund, 2350, "Active", 10),
            (118, emergencyFund, 980, "Active", 5),
            (119, emergencyFund, 3200, "Active", 13),
            (120, emergencyFund, 1450, "Active", 6),
            (121, emergencyFund, 2750, "Active", 11),
            (122, emergencyFund, 580, "Active", 3),
            (123, emergencyFund, 1980, "Active", 8),
            (124, emergencyFund, 4100, "Active", 16),
            (125, emergencyFund, 720, "Active", 4),
            (126, emergencyFund, 2580, "Active", 12),
            (127, emergencyFund, 1120, "Active", 5),
            (128, emergencyFund, 3450, "Active", 14),
            (129, emergencyFund, 1850, "Active", 9),
            
            // Fixed Savings - More accounts
            (130, fixedSavings, 5500, "Active", 10),
            (131, fixedSavings, 8200, "Active", 12),
            (132, fixedSavings, 11500, "Active", 15),
            (133, fixedSavings, 6800, "Active", 8),
            (134, fixedSavings, 16000, "Active", 20),
            (135, fixedSavings, 9200, "Active", 11),
            (136, fixedSavings, 7200, "Active", 9),
            (137, fixedSavings, 13500, "Active", 16),
            (138, fixedSavings, 10500, "Active", 13),
            (139, fixedSavings, 19000, "Active", 22),
            (140, fixedSavings, 6200, "Active", 7),
            (141, fixedSavings, 14800, "Active", 18),
            (142, fixedSavings, 8800, "Active", 10),
            (143, fixedSavings, 17500, "Active", 21),
            (144, fixedSavings, 7800, "Active", 9),
            
            // Premium Savings - High-value accounts
            (145, premiumSavings, 28000, "Active", 11),
            (146, premiumSavings, 42000, "Active", 15),
            (147, premiumSavings, 68000, "Active", 22),
            (148, premiumSavings, 38000, "Active", 13),
            (149, premiumSavings, 95000, "Active", 30),
            (150, premiumSavings, 52000, "Active", 18),
            (151, premiumSavings, 78000, "Active", 25),
            (152, premiumSavings, 32000, "Active", 10),
            (153, premiumSavings, 88000, "Active", 28),
            (154, premiumSavings, 62000, "Active", 20),
            
            // Additional dormant accounts
            (155, regularSavings, 35, "Dormant", 40),
            (156, regularSavings, 65, "Dormant", 45),
            (157, emergencyFund, 45, "Dormant", 38),
            (158, businessSavings, 120, "Dormant", 42),
            (159, regularSavings, 90, "Dormant", 48),
            
            // Additional frozen accounts
            (160, businessSavings, 8500, "Frozen", 10),
            (161, regularSavings, 2200, "Frozen", 8),
            (162, emergencyFund, 1500, "Frozen", 6),
            (163, premiumSavings, 35000, "Frozen", 12),
            
            // Final batch of regular active accounts
            (164, regularSavings, 1680, "Active", 7),
            (165, regularSavings, 2320, "Active", 10),
            (166, regularSavings, 850, "Active", 5),
            (167, regularSavings, 3180, "Active", 13),
            (168, regularSavings, 1290, "Active", 6),
            (169, regularSavings, 4520, "Active", 18),
            (170, regularSavings, 720, "Active", 4),
            (171, regularSavings, 1950, "Active", 9),
            (172, regularSavings, 2680, "Active", 11),
            (173, regularSavings, 1050, "Active", 5),
            (174, regularSavings, 3820, "Active", 15),
            (175, regularSavings, 580, "Active", 3),
            (176, regularSavings, 2150, "Active", 10),
            (177, regularSavings, 1480, "Active", 7),
            (178, regularSavings, 3250, "Active", 14),
            (179, regularSavings, 920, "Active", 4),
            (180, regularSavings, 1720, "Active", 8),
            (181, regularSavings, 2890, "Active", 12),
            (182, regularSavings, 650, "Active", 3),
            (183, regularSavings, 2480, "Active", 11),
            (184, regularSavings, 1180, "Active", 6),
            (185, businessSavings, 5850, "Active", 7),
            (186, businessSavings, 9500, "Active", 11),
            (187, emergencyFund, 1380, "Active", 5),
            (188, emergencyFund, 2620, "Active", 10),
            (189, fixedSavings, 7500, "Active", 9),
            (190, fixedSavings, 11200, "Active", 14),
            (191, premiumSavings, 48000, "Active", 16),
            (192, premiumSavings, 72000, "Active", 23),
            (193, regularSavings, 1850, "Active", 8),
            (194, regularSavings, 3120, "Active", 13),
            (195, businessSavings, 7200, "Active", 9),
            (196, emergencyFund, 1950, "Active", 7),
            (197, fixedSavings, 9800, "Active", 11),
            (198, regularSavings, 2250, "Active", 9),
            (199, regularSavings, 1580, "Active", 6),
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
