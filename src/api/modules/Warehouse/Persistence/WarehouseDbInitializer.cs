using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Warehouse.Domain;
using FSH.Starter.WebApi.Warehouse.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Persistence;

internal class WarehouseDbInitializer(WarehouseDbContext context, ILogger<WarehouseDbInitializer> logger)
    : IDbInitializer
{
    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (await context.Warehouses.AnyAsync(cancellationToken))
        {
            logger.LogInformation("Warehouse data already exists. Skipping initialization.");
            return;
        }

        logger.LogInformation("Initializing warehouse data...");

        var warehouses = new List<Domain.Warehouse>
        {
            Domain.Warehouse.Create(
                "Main Warehouse",
                "WH001",
                new Address("123 Industrial Blvd", "Manufacturing City", "CA", "90210", "USA"),
                "Primary distribution center"),

            Domain.Warehouse.Create(
                "East Coast Distribution",
                "WH002",
                new Address("456 Logistics Ave", "Port City", "NY", "10001", "USA"),
                "East coast regional distribution"),

            Domain.Warehouse.Create(
                "West Coast Hub",
                "WH003",
                new Address("789 Shipping Way", "Harbor Town", "WA", "98101", "USA"),
                "West coast distribution hub")
        };

        await context.Warehouses.AddRangeAsync(warehouses, cancellationToken);

        // Add some sample inventory items
        var inventoryItems = new List<InventoryItem>
        {
            InventoryItem.Create(
                warehouses[0].Id,
                "PROD001",
                "Widget A",
                100,
                10,
                500,
                UnitOfMeasure.Piece),

            InventoryItem.Create(
                warehouses[0].Id,
                "PROD002",
                "Component B",
                250,
                25,
                1000,
                UnitOfMeasure.Piece),

            InventoryItem.Create(
                warehouses[1].Id,
                "PROD001",
                "Widget A",
                75,
                10,
                300,
                UnitOfMeasure.Piece)
        };

        await context.InventoryItems.AddRangeAsync(inventoryItems, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Warehouse data initialization completed.");
    }

    public Task MigrateAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SeedAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
