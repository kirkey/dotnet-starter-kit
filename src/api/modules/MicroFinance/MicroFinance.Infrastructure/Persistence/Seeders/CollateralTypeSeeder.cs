using FSH.Starter.WebApi.MicroFinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;

/// <summary>
/// Seeder for collateral types used in loan security.
/// Defines standard collateral categories with LTV ratios and valuation rules.
/// </summary>
internal static class CollateralTypeSeeder
{
    public static async Task SeedAsync(
        MicroFinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 8;
        var existingCount = await context.CollateralTypes.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var collateralTypeData = new (string Code, string Name, string Category, string Description, decimal DefaultLtv, decimal MaxLtv, int UsefulLife, decimal DepreciationRate)[]
        {
            // Real Property
            ("COLL-RE", "Real Estate", CollateralType.CategoryRealEstate, "Land and buildings including houses, commercial properties, and vacant land", 60m, 70m, 30, 2m),
            
            // Vehicles
            ("COLL-VH", "Motor Vehicle", CollateralType.CategoryVehicle, "Cars, trucks, motorcycles, and other registered vehicles", 50m, 60m, 8, 15m),
            ("COLL-AG", "Agricultural Vehicle", CollateralType.CategoryVehicle, "Tractors, harvesters, and farm equipment vehicles", 45m, 55m, 10, 12m),
            
            // Equipment
            ("COLL-EQ", "Business Equipment", CollateralType.CategoryEquipment, "Machinery, tools, and equipment used in business operations", 40m, 50m, 7, 20m),
            ("COLL-FE", "Farm Equipment", CollateralType.CategoryEquipment, "Agricultural implements and farming machinery", 40m, 50m, 10, 15m),
            
            // Financial Assets
            ("COLL-SD", "Savings Deposit", CollateralType.CategoryCash, "Fixed deposits and savings accounts held as security", 90m, 95m, 99, 0m),
            
            // Livestock
            ("COLL-LV", "Livestock", CollateralType.CategoryOther, "Cattle, goats, sheep, and other farm animals", 30m, 40m, 5, 10m),
            
            // Inventory
            ("COLL-IN", "Business Inventory", CollateralType.CategoryInventory, "Stock, goods, and materials held for sale", 30m, 40m, 1, 25m),
        };

        foreach (var data in collateralTypeData)
        {
            if (await context.CollateralTypes.AnyAsync(c => c.Code == data.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var collateralType = CollateralType.Create(
                name: data.Name,
                code: data.Code,
                category: data.Category,
                defaultLtvPercent: data.DefaultLtv,
                maxLtvPercent: data.MaxLtv,
                defaultUsefulLifeYears: data.UsefulLife,
                annualDepreciationRate: data.DepreciationRate,
                description: data.Description);

            await context.CollateralTypes.AddAsync(collateralType, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} collateral types (real estate, vehicles, equipment, deposits, livestock, inventory)", tenant, targetCount);
    }
}
