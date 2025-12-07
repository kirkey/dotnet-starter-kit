using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for mobile wallets.
/// Creates mobile wallet accounts for members.
/// </summary>
internal static class MobileWalletSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 50;
        var existingCount = await context.MobileWallets.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Where(m => m.IsActive).Take(50).ToListAsync(cancellationToken).ConfigureAwait(false);
        if (!members.Any()) return;

        var random = new Random(42);
        var providers = new[] { "GCash", "Maya", "Coins.ph", "GrabPay", "ShopeePay" };
        var tiers = new[] { MobileWallet.TierBasic, MobileWallet.TierStandard, MobileWallet.TierPremium };

        int walletCount = 0;

        foreach (var member in members.Take(targetCount))
        {
            var provider = providers[random.Next(providers.Length)];
            var tier = tiers[random.Next(tiers.Length)];
            
            var dailyLimit = tier switch
            {
                MobileWallet.TierBasic => 5000m,
                MobileWallet.TierStandard => 25000m,
                MobileWallet.TierPremium => 100000m,
                _ => 5000m
            };
            
            var monthlyLimit = dailyLimit * 30;

            var wallet = MobileWallet.Create(
                memberId: member.Id,
                phoneNumber: member.PhoneNumber ?? $"+639{170000000 + random.Next(10000000):D7}",
                provider: provider,
                dailyLimit: dailyLimit,
                monthlyLimit: monthlyLimit);

            // Activate most wallets
            if (random.NextDouble() > 0.1)
            {
                wallet.Activate();
                wallet.UpgradeTier(tier, dailyLimit, monthlyLimit);
                
                // Add some balance
                var balance = Math.Round((decimal)(random.NextDouble() * 5000), 2);
                wallet.Credit(balance, $"INITIAL-{Guid.NewGuid():N}");
            }

            await context.MobileWallets.AddAsync(wallet, cancellationToken).ConfigureAwait(false);
            walletCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} mobile wallets ({Providers})", tenant, walletCount, string.Join(", ", providers));
    }
}
