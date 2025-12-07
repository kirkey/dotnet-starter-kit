using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for teller sessions.
/// Creates sample teller sessions with transaction history.
/// </summary>
internal static class TellerSessionSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.TellerSessions.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var tellerDrawers = await context.CashVaults
            .Where(v => v.VaultType == CashVault.TypeTellerDrawer && v.Status == CashVault.StatusActive)
            .Take(10)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!tellerDrawers.Any()) return;

        var staff = await context.Staff
            .Where(s => s.Role == Staff.RoleTeller && s.Status == Staff.StatusActive)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!staff.Any()) return;

        var random = new Random(42);
        int sessionNumber = 10001;
        int sessionCount = 0;

        // Create sessions for the last 5 business days
        for (int dayOffset = 0; dayOffset < 5; dayOffset++)
        {
            var sessionDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-dayOffset));
            
            // Skip weekends
            var dayOfWeek = sessionDate.DayOfWeek;
            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
                continue;

            foreach (var drawer in tellerDrawers.Take(5))
            {
                var teller = staff[random.Next(staff.Count)];
                var openingBalance = 50000m + random.Next(0, 30000);
                var startTime = DateTime.UtcNow.AddDays(-dayOffset).Date.AddHours(8).AddMinutes(random.Next(0, 30));

                var session = TellerSession.Open(
                    branchId: drawer.BranchId,
                    cashVaultId: drawer.Id,
                    sessionNumber: $"TS-{sessionNumber++:D6}",
                    tellerUserId: teller.Id,
                    tellerName: $"{teller.FirstName} {teller.LastName}",
                    openingBalance: openingBalance);

                // Add some transaction activity
                var cashIn = random.Next(50000, 200000);
                var cashOut = random.Next(30000, 150000);
                session.RecordCashIn(cashIn);
                session.RecordCashOut(cashOut);

                // Close past sessions
                if (dayOffset > 0)
                {
                    var closingBalance = openingBalance + cashIn - cashOut;
                    session.Close(closingBalance);
                }

                await context.TellerSessions.AddAsync(session, cancellationToken).ConfigureAwait(false);
                sessionCount++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} teller sessions", tenant, sessionCount);
    }
}
