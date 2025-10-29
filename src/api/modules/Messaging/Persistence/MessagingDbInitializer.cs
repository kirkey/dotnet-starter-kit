using FSH.Framework.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
namespace FSH.Starter.WebApi.Messaging.Persistence;
/// <summary>
/// Database initializer for the Messaging module.
/// Ensures the database schema is created and migrations are applied.
/// </summary>
public sealed class MessagingDbInitializer(
    MessagingDbContext context,
    ILogger<MessagingDbInitializer> logger) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("migrating messaging database schema...");
            await context.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("messaging database schema migrated successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "an error occurred while migrating messaging database schema");
            throw;
        }
    }
    public Task SeedAsync(CancellationToken cancellationToken)
    {
        // No seeding required for messaging module
        logger.LogInformation("no seed data required for messaging module");
        return Task.CompletedTask;
    }
}
