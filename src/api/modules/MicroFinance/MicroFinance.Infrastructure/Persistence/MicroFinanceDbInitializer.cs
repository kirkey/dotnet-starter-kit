using FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence.Seeders;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence;

/// <summary>
/// Database initializer for the MicroFinance module.
/// Handles migrations and orchestrates seeding of initial data.
/// Individual entity seeding is delegated to specialized seeder classes.
/// </summary>
internal sealed class MicroFinanceDbInitializer(
    ILogger<MicroFinanceDbInitializer> logger,
    MicroFinanceDbContext context) : IDbInitializer
{
    /// <inheritdoc/>
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for microfinance module", context.TenantInfo!.Identifier);
        }
    }

    /// <inheritdoc/>
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var tenant = context.TenantInfo?.Identifier ?? "default";

        // 1) Seed Product definitions (independent entities)
        await LoanProductSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await SavingsProductSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await ShareProductSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await FeeDefinitionSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 2) Seed Members (required for accounts and loans)
        await MemberSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 3) Seed Member Groups and Memberships
        await MemberGroupSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await GroupMembershipSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 4) Seed Accounts (depend on members and products)
        await SavingsAccountSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await ShareAccountSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 5) Seed Loans (depend on members and loan products)
        await LoanSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 6) Seed Fixed Deposits (depend on members)
        await FixedDepositSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] completed seeding microfinance module with comprehensive sample data", tenant);
    }
}

