using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for loan collateral items.
/// Assigns collateral to larger loans to test secured lending workflows.
/// </summary>
internal static class LoanCollateralSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        var existingCount = await context.LoanCollaterals.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount > 0) return; // Already seeded

        // Get larger loans that would typically require collateral
        var loans = await context.Loans
            .Where(l => (l.Status == Loan.StatusApproved || l.Status == Loan.StatusDisbursed) && l.PrincipalAmount >= 5000)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (!loans.Any()) return;

        int collateralCount = 0;
        var random = new Random(42);

        var collateralItems = new (string Type, string Description, decimal Value)[]
        {
            // Savings Deposits
            ("SavingsDeposit", "Fixed Deposit Certificate #FD-10234", 3000),
            ("SavingsDeposit", "Savings Account Lien - Account #SA-5678", 2500),
            
            // Motor Vehicles
            ("Vehicle", "2019 Toyota Hilux - License ABC-1234", 18000),
            ("Vehicle", "2020 Honda Motorcycle - License XYZ-999", 3500),
            ("Vehicle", "2018 Nissan Pickup - License DEF-5678", 12000),
            
            // Real Estate
            ("RealEstate", "Residential Plot - 0.5 acres at Block 12", 25000),
            ("RealEstate", "Commercial Building - Main Street Shop", 45000),
            
            // Farm Equipment
            ("Equipment", "Massey Ferguson Tractor - Model 165", 15000),
            ("Equipment", "Irrigation Pump System", 4000),
            ("Equipment", "Grain Storage Silo - 20 ton capacity", 8000),
            
            // Livestock
            ("Livestock", "Dairy Cattle - 10 head (Holstein)", 12000),
            ("Livestock", "Beef Cattle - 15 head (Brahman)", 9000),
            ("Livestock", "Goat Herd - 25 head", 3500),
            
            // Business Equipment
            ("Equipment", "Commercial Bakery Equipment Set", 8500),
            ("Equipment", "Industrial Sewing Machines - 5 units", 6000),
            ("Equipment", "Restaurant Kitchen Equipment", 12000),
            
            // Inventory
            ("Inventory", "Retail Store Inventory - Electronics", 15000),
            ("Inventory", "Wholesale Grocery Stock", 8000),
        };

        int itemIndex = 0;
        foreach (var loan in loans)
        {
            if (itemIndex >= collateralItems.Length) break;
            
            // Skip loans that already have collateral
            if (await context.LoanCollaterals.AnyAsync(c => c.LoanId == loan.Id, cancellationToken).ConfigureAwait(false))
                continue;

            var item = collateralItems[itemIndex];
            
            var collateral = LoanCollateral.Create(
                loanId: loan.Id,
                collateralType: item.Type,
                description: item.Description,
                estimatedValue: item.Value,
                forcedSaleValue: item.Value * 0.7m,
                valuationDate: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-random.Next(10, 60))),
                location: "On-site",
                documentReference: $"DOC-{1000 + itemIndex}");

            // Verify and pledge collateral for disbursed loans
            if (loan.Status == Loan.StatusDisbursed)
            {
                collateral.Verify();
                collateral.Pledge();
            }
            else if (random.NextDouble() > 0.5) // 50% chance verified for approved loans
            {
                collateral.Verify();
            }

            await context.LoanCollaterals.AddAsync(collateral, cancellationToken).ConfigureAwait(false);
            collateralCount++;
            itemIndex++;
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} loan collateral items for secured loans", tenant, collateralCount);
    }
}
