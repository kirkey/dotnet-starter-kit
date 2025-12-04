using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for fixed deposits.
/// </summary>
internal static class FixedDepositSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 10;
        var existingCount = await context.FixedDeposits.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Take(10).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 1) return;

        int certNumber = 4001;
        var terms = new[] { 6, 12, 18, 24, 36 };
        var rates = new[] { 7.0m, 7.5m, 8.0m, 8.5m, 9.0m };

        for (int i = 0; i < Math.Min(10, members.Count); i++)
        {
            var certNum = $"FD-{certNumber + i:D6}";
            var exists = await context.FixedDeposits.AnyAsync(fd => fd.CertificateNumber == certNum, cancellationToken).ConfigureAwait(false);
            if (exists) continue;

            var deposit = FixedDeposit.Create(
                certificateNumber: certNum,
                memberId: members[i].Id,
                principalAmount: (i + 1) * 500,
                interestRate: rates[i % rates.Length],
                termMonths: terms[i % terms.Length],
                depositDate: DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-2)),
                maturityInstruction: i % 2 == 0 ? FixedDeposit.MaturityTransferToSavings : FixedDeposit.MaturityRenewPrincipalAndInterest);

            await context.FixedDeposits.AddAsync(deposit, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded fixed deposits", tenant);
    }
}
