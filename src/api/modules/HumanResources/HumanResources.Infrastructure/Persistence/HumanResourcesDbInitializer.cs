using Microsoft.Extensions.Logging;
using Shared.Constants;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence;

/// <summary>
/// Database initializer for HumanResources module.
/// </summary>
public class HumanResourcesDbInitializer(
    ILogger<HumanResourcesDbInitializer> logger,
    HumanResourcesDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            logger.LogInformation("Applying HumanResources module migrations...");
            await context.Database.MigrateAsync(cancellationToken);
            logger.LogInformation("HumanResources module migrations applied successfully.");
        }
    }

    public Task SeedAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("HumanResources module seeding completed (no seed data configured).");
        return Task.CompletedTask;
    }
}
