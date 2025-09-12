// filepath: /Users/kirkeypsalms/Projects/dotnet-starter-kit/src/Store.Infrastructure/Persistence/StoreDbInitializer.cs

using Microsoft.Extensions.Logging;

namespace Store.Infrastructure.Persistence;

internal sealed class StoreDbInitializer(
    ILogger<StoreDbInitializer> logger,
    StoreDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for store module", context.TenantInfo!.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        // Minimal seed: ensure at least one category exists
        const string defaultCategory = "Uncategorized";
        if (await context.Categories.FirstOrDefaultAsync(c => c.Name == defaultCategory, cancellationToken).ConfigureAwait(false) is null)
        {
            var category = Category.Create(defaultCategory, null, "UNCAT");
             await context.Categories.AddAsync(category, cancellationToken).ConfigureAwait(false);
             await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
             logger.LogInformation("[{Tenant}] seeded default store data", context.TenantInfo!.Identifier);
        }
    }
}
