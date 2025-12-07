using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for cash vaults.
/// Creates cash vaults for each branch including main vault and teller drawers.
/// </summary>
internal static class CashVaultSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.CashVaults.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var branches = await context.Branches
            .Where(b => b.IsActive)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!branches.Any()) return;

        var random = new Random(42);
        int vaultCount = 0;

        foreach (var branch in branches)
        {
            // Main vault for each branch
            var mainVault = CashVault.Create(
                branchId: branch.Id,
                code: $"MV-{branch.Code}",
                vaultType: CashVault.TypeMainVault,
                minimumBalance: 500000m,
                maximumBalance: 5000000m,
                location: "Main vault room");

            mainVault.UpdateBalance(2000000m + random.Next(0, 1000000));
            mainVault.Activate();
            await context.CashVaults.AddAsync(mainVault, cancellationToken).ConfigureAwait(false);
            vaultCount++;

            // Reserve vault
            var reserveVault = CashVault.Create(
                branchId: branch.Id,
                code: $"RV-{branch.Code}",
                vaultType: CashVault.TypeReserve,
                minimumBalance: 100000m,
                maximumBalance: 1000000m,
                location: "Reserve storage");

            reserveVault.UpdateBalance(500000m + random.Next(0, 200000));
            reserveVault.Activate();
            await context.CashVaults.AddAsync(reserveVault, cancellationToken).ConfigureAwait(false);
            vaultCount++;

            // Teller drawers (2-4 per branch)
            int tellerCount = 2 + random.Next(0, 3);
            for (int i = 1; i <= tellerCount; i++)
            {
                var tellerDrawer = CashVault.Create(
                    branchId: branch.Id,
                    code: $"TD-{branch.Code}-{i:D2}",
                    vaultType: CashVault.TypeTellerDrawer,
                    minimumBalance: 20000m,
                    maximumBalance: 200000m,
                    location: $"Teller Window {i}");

                tellerDrawer.UpdateBalance(50000m + random.Next(0, 50000));
                tellerDrawer.Activate();
                await context.CashVaults.AddAsync(tellerDrawer, cancellationToken).ConfigureAwait(false);
                vaultCount++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} cash vaults", tenant, vaultCount);
    }
}
