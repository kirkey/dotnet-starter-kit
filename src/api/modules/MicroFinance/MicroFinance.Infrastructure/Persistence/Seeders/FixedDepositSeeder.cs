using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for fixed deposits.
/// Creates 75 fixed deposits with varying terms and amounts for demo database.
/// </summary>
internal static class FixedDepositSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 75;
        var existingCount = await context.FixedDeposits.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Where(m => m.IsActive && m.MonthlyIncome >= 20000).Take(75).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 1) return;

        var random = new Random(42);
        int certNumber = 4001;
        var terms = new[] { 3, 6, 12, 18, 24, 36, 60 };
        var rates = new[] { 5.5m, 6.0m, 6.5m, 7.0m, 7.5m, 8.0m, 8.5m };
        var amounts = new[] { 5000m, 10000m, 15000m, 20000m, 25000m, 30000m, 50000m, 75000m, 100000m };
        var instructions = new[] { FixedDeposit.MaturityTransferToSavings, FixedDeposit.MaturityRenewPrincipalAndInterest, FixedDeposit.MaturityRenewPrincipal };

        for (int i = 0; i < Math.Min(targetCount, members.Count); i++)
        {
            var certNum = $"FD-{certNumber + i:D6}";
            var exists = await context.FixedDeposits.AnyAsync(fd => fd.CertificateNumber == certNum, cancellationToken).ConfigureAwait(false);
            if (exists) continue;

            var termIndex = random.Next(terms.Length);
            var deposit = FixedDeposit.Create(
                certificateNumber: certNum,
                memberId: members[i].Id,
                principalAmount: amounts[random.Next(amounts.Length)],
                interestRate: rates[termIndex],
                termMonths: terms[termIndex],
                depositDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-random.Next(1, 12))),
                maturityInstruction: instructions[random.Next(instructions.Length)]);

            await context.FixedDeposits.AddAsync(deposit, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} fixed deposits", tenant, targetCount);
    }
}
