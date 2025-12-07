using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for investment accounts.
/// Creates investment portfolios for members with various risk profiles.
/// </summary>
internal static class InvestmentAccountSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.InvestmentAccounts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var members = await context.Members
            .Where(m => m.IsActive && m.MonthlyIncome >= 30000) // Members with higher income
            .Take(25)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var products = await context.InvestmentProducts
            .Where(p => p.Status == InvestmentProduct.StatusActive)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!members.Any() || !products.Any()) return;

        var random = new Random(42);
        int accountNumber = 7001;
        int accountCount = 0;

        var riskProfiles = new[] 
        { 
            InvestmentAccount.ProfileConservative, 
            InvestmentAccount.ProfileModerate, 
            InvestmentAccount.ProfileBalanced, 
            InvestmentAccount.ProfileAggressive 
        };

        var investmentGoals = new[]
        {
            "Retirement Fund",
            "Education Fund para sa mga Anak",
            "Emergency Fund",
            "Pangkabuhayan Capital",
            "House and Lot Down Payment",
            "Travel Fund",
            "Wedding Fund",
            "Business Expansion"
        };

        foreach (var member in members)
        {
            var riskProfile = riskProfiles[random.Next(riskProfiles.Length)];
            var goal = investmentGoals[random.Next(investmentGoals.Length)];

            var account = InvestmentAccount.Create(
                memberId: member.Id,
                accountNumber: $"INV-{accountNumber++:D6}",
                riskProfile: riskProfile,
                investmentGoal: goal);

            // Make initial investment based on income level
            var initialInvestment = Math.Round((member.MonthlyIncome ?? 30000) * random.Next(2, 6) / 1000) * 1000;
            account.Invest(initialInvestment);

            // Add some growth for older accounts
            var monthsOld = random.Next(1, 18);
            var growthRate = riskProfile switch
            {
                InvestmentAccount.ProfileConservative => random.NextDouble() * 0.05 + 0.02,
                InvestmentAccount.ProfileModerate => random.NextDouble() * 0.10 + 0.03,
                InvestmentAccount.ProfileBalanced => random.NextDouble() * 0.12 + 0.04,
                InvestmentAccount.ProfileAggressive => random.NextDouble() * 0.20 - 0.05,
                _ => 0.05
            };

            var currentValue = initialInvestment * (1 + (decimal)(growthRate * monthsOld / 12));
            account.UpdateValuation(Math.Round(currentValue, 2));

            await context.InvestmentAccounts.AddAsync(account, cancellationToken).ConfigureAwait(false);
            accountCount++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} investment accounts with various risk profiles", tenant, accountCount);
    }
}
