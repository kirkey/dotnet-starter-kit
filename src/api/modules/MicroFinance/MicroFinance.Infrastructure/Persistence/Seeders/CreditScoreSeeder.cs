using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for credit scores.
/// Creates credit score records for members.
/// </summary>
internal static class CreditScoreSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.CreditScores.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var members = await context.Members.Where(m => m.IsActive).Take(60).ToListAsync(cancellationToken).ConfigureAwait(false);
        if (members.Count == 0) return;

        int scoreCount = 0;
        int seed = 42;

        foreach (var member in members)
        {
            // Generate credit score based on member profile
            decimal baseScore = 500; // Starting point
            
            // Income factor
            var income = member.MonthlyIncome ?? 0;
            if (income >= 50000) baseScore += 150;
            else if (income >= 30000) baseScore += 100;
            else if (income >= 15000) baseScore += 50;
            else if (income >= 10000) baseScore += 25;
            
            // Add some deterministic variation based on member id
            seed = (seed * 1103515245 + 12345) & int.MaxValue;
            baseScore += (seed % 150) - 50;
            baseScore = Math.Clamp(baseScore, 300, 850);

            var creditScore = CreditScore.Create(
                memberId: member.Id,
                scoreType: CreditScore.TypeInternal,
                score: baseScore,
                scoreMin: 300,
                scoreMax: 850,
                scoreModel: "MFI Internal Model v1",
                source: "Internal Credit Assessment System");

            await context.CreditScores.AddAsync(creditScore, cancellationToken).ConfigureAwait(false);
            scoreCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} credit scores for members", tenant, scoreCount);
    }
}
