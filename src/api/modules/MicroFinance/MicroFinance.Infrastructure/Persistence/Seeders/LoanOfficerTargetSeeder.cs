using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for loan officer targets.
/// Creates performance targets for loan officers.
/// </summary>
internal static class LoanOfficerTargetSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.LoanOfficerTargets.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var loanOfficers = await context.Staff
            .Where(s => s.Role == Staff.RoleLoanOfficer && s.Status == Staff.StatusActive)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!loanOfficers.Any()) return;

        var currentYear = DateTime.UtcNow.Year;
        var currentMonth = DateTime.UtcNow.Month;
        var random = new Random(42);
        int targetCount = 0;

        foreach (var officer in loanOfficers)
        {
            // Create monthly targets for last 6 months
            for (int i = 0; i < 6; i++)
            {
                var targetMonth = currentMonth - i;
                var targetYear = currentYear;
                if (targetMonth < 1)
                {
                    targetMonth += 12;
                    targetYear--;
                }

                var startDate = new DateOnly(targetYear, targetMonth, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var loanTarget = 200000m + random.Next(0, 100000);
                var clientTarget = 10 + random.Next(0, 10);
                var collectionTarget = loanTarget * 0.95m;

                var target = LoanOfficerTarget.Create(
                    staffId: officer.Id,
                    targetType: LoanOfficerTarget.TypeLoanDisbursement,
                    period: LoanOfficerTarget.PeriodMonthly,
                    periodStart: startDate,
                    periodEnd: endDate,
                    targetValue: loanTarget,
                    description: $"{targetYear}-{targetMonth:D2} Loan Disbursement Target");

                // Add some actual performance for past months
                if (i > 0)
                {
                    var achievementRate = 0.7m + (decimal)random.NextDouble() * 0.5m; // 70-120%
                    target.RecordProgress(loanTarget * achievementRate);
                }

                await context.LoanOfficerTargets.AddAsync(target, cancellationToken).ConfigureAwait(false);
                targetCount++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} loan officer targets", tenant, targetCount);
    }
}
