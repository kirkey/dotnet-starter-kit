using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for branch targets.
/// Creates performance targets for branches.
/// </summary>
internal static class BranchTargetSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.BranchTargets.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var branches = await context.Branches
            .Where(b => b.IsActive)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!branches.Any()) return;

        var currentYear = DateTime.UtcNow.Year;
        int targetCount = 0;

        foreach (var branch in branches)
        {
            // Create quarterly targets for current year
            for (int quarter = 1; quarter <= 4; quarter++)
            {
                var startMonth = (quarter - 1) * 3 + 1;
                var startDate = new DateOnly(currentYear, startMonth, 1);
                var endDate = startDate.AddMonths(3).AddDays(-1);

                // Base targets scaled by branch type
                var loanTarget = branch.BranchType switch
                {
                    "Headquarters" => 5000000m,
                    "Regional" => 3000000m,
                    "Branch" => 1500000m,
                    "SubBranch" => 500000m,
                    _ => 1000000m
                };

                var savingsTarget = loanTarget * 0.8m;
                var memberTarget = (int)(loanTarget / 10000);

                var target = BranchTarget.Create(
                    branchId: branch.Id,
                    targetPeriod: $"Q{quarter}-{currentYear}",
                    startDate: startDate,
                    endDate: endDate,
                    loanDisbursementTarget: loanTarget,
                    savingsCollectionTarget: savingsTarget,
                    newMemberTarget: memberTarget,
                    loanCollectionTarget: loanTarget * 0.95m,
                    parTarget: 5.0m);

                await context.BranchTargets.AddAsync(target, cancellationToken).ConfigureAwait(false);
                targetCount++;
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} branch targets", tenant, targetCount);
    }
}
