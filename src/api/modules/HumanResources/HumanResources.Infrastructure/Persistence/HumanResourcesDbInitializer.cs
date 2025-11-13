using Microsoft.Extensions.Logging;
using Shared.Constants;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence;

/// <summary>
/// Database initializer for HumanResources module.
/// </summary>
internal sealed class HumanResourcesDbInitializer(
    ILogger<HumanResourcesDbInitializer> logger,
    HumanResourcesDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for humanresources module", context.TenantInfo!.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        // Seed default company if none exists
        const string companyCode = "DEFAULT";
        const string name = "Default Company";
        
        if (await context.Companies.FirstOrDefaultAsync(c => c.CompanyCode == companyCode, cancellationToken).ConfigureAwait(false) is null)
        {
            var company = Company.Create(companyCode, name);
            await context.Companies.AddAsync(company, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeding default human resource data", context.TenantInfo!.Identifier);
        }
    }
}
