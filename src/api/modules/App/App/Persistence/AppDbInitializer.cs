using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.App.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.App.Persistence;

internal sealed class AppDbInitializer(
    ILogger<AppDbInitializer> logger,
    AppDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for App module",
                context.TenantInfo!.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        const string application = "Accounting";
        const string parent = "ChartOfAccounts";
        const string code = "Assets";
        const string name = "Assets";

        if (await context.Groups.FirstOrDefaultAsync(t => t.Name == name, cancellationToken)
                .ConfigureAwait(false) is null)
        {
            var group = Group.Create(application, parent, null, 0, code, name);
            await context.Groups.AddAsync(group, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeding default group data", context.TenantInfo!.Identifier);
        }
    }
}
