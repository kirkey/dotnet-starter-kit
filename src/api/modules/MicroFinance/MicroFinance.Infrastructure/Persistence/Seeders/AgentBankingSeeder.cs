using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for agent banking locations.
/// Creates agent banking partners with various tiers and locations.
/// </summary>
internal static class AgentBankingSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.AgentBankings.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return;

        var branches = await context.Branches
            .Where(b => b.IsActive)
            .Take(5)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var random = new Random(42);
        int agentCode = 15001;

        var agentData = new (string Business, string Contact, string City)[]
        {
            // Sari-sari stores
            ("Aling Nena's Store", "Nena Santos", "Quezon City"),
            ("Mang Boy's Sari-Sari", "Roberto Cruz", "Makati City"),
            ("Tindahan ni Ate Gina", "Gina Reyes", "Cebu City"),
            ("Aling Rosa General Merchandise", "Rosa Mendoza", "Davao City"),
            ("Kabayan Mini Mart", "Pedro Villanueva", "Baguio City"),
            // Pawnshops
            ("Golden Star Pawnshop", "Antonio Lim", "Manila"),
            ("M. Lhuillier Agent", "Maria Garcia", "Iloilo City"),
            ("Cebuana Lhuillier Outlet", "Jose Fernandez", "Zamboanga City"),
            // Pharmacies
            ("Mercury Drug Agent", "Ana Dela Cruz", "Pasig City"),
            ("Generika Agent Point", "Luis Santos", "Taguig City"),
            // Convenience stores
            ("7-Eleven Partner", "Patricia Tan", "Mandaluyong"),
            ("Ministop Agent", "Carlo Rivera", "Parañaque"),
            // Rural areas
            ("Barangay Palengke", "Juan Bautista", "Batangas City"),
            ("Poblacion Trading", "Elena Gonzales", "Nueva Ecija"),
            ("Farmers Cooperative Store", "Miguel Torres", "Pangasinan"),
            // Hardware/remittance
            ("LBC Agent Point", "Sofia Aquino", "Bulacan"),
            ("Western Union Agent", "Ricardo Castro", "Laguna"),
            ("Hardware Plus", "Emilio Ramos", "Cavite"),
            ("JRS Express Agent", "Cristina Dizon", "Rizal"),
            ("Rural Bank Partner", "Fernando Navarro", "Pampanga"),
        };

        var tiers = new[] { AgentBanking.TierBronze, AgentBanking.TierBronze, AgentBanking.TierSilver, AgentBanking.TierGold, AgentBanking.TierPlatinum };

        foreach (var agent in agentData)
        {
            var tier = tiers[random.Next(tiers.Length)];
            var dailyLimit = tier switch
            {
                AgentBanking.TierBronze => 50000m,
                AgentBanking.TierSilver => 100000m,
                AgentBanking.TierGold => 200000m,
                AgentBanking.TierPlatinum => 500000m,
                _ => 50000m
            };
            var monthlyLimit = dailyLimit * 25;
            var commissionRate = tier switch
            {
                AgentBanking.TierBronze => 0.5m,
                AgentBanking.TierSilver => 0.75m,
                AgentBanking.TierGold => 1.0m,
                AgentBanking.TierPlatinum => 1.25m,
                _ => 0.5m
            };

            var contractStart = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-random.Next(1, 24)));
            var branchId = branches.Any() ? branches[random.Next(branches.Count)].Id : (Guid?)null;

            var agentBanking = AgentBanking.Create(
                agentCode: $"AGT-{agentCode++:D5}",
                businessName: agent.Business,
                contactName: agent.Contact,
                phoneNumber: $"+639{random.Next(100000000, 999999999)}",
                address: $"123 Sample St., Brgy. Centro, {agent.City}, Philippines",
                commissionRate: commissionRate,
                dailyTransactionLimit: dailyLimit,
                monthlyTransactionLimit: monthlyLimit,
                contractStartDate: contractStart,
                branchId: branchId);

            // Activate agents
            agentBanking.Activate();
            agentBanking.SetTier(tier);
            agentBanking.UpdateFloatBalance(dailyLimit * 0.5m); // Initial float
            agentBanking.VerifyKyc();

            await context.AgentBankings.AddAsync(agentBanking, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} agent banking locations", tenant, agentData.Length);
    }
}
