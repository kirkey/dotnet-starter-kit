using FSH.Framework.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.Warehouse.Persistence;

internal sealed class WarehouseDbInitializer(
    ILogger<WarehouseDbInitializer> logger,
    WarehouseDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for warehouse module", context.TenantInfo!.Identifier);
        }
    }

    public Task SeedAsync(CancellationToken cancellationToken)
    {
        // No default seed for now.
        return Task.CompletedTask;
    }
}
