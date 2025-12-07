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

        // 1) Seed Infrastructure (branches, staff, collateral types, communication templates)
        await BranchSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await StaffSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await CollateralTypeSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await CommunicationTemplateSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 2) Seed Product definitions (loan, savings, share, insurance, fee)
        await LoanProductSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await SavingsProductSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await ShareProductSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await InsuranceProductSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await FeeDefinitionSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 3) Seed Members (required for accounts and loans)
        await MemberSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 4) Seed Member Groups and Memberships
        await MemberGroupSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await GroupMembershipSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 5) Seed Accounts (depend on members and products)
        await SavingsAccountSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await ShareAccountSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 6) Seed Loans (depend on members and loan products)
        await LoanSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 7) Seed Fixed Deposits (depend on members)
        await FixedDepositSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 8) Seed Loan Supporting Data (schedules, guarantors, collateral)
        await LoanScheduleSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await LoanGuarantorSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await LoanCollateralSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        // 9) Seed Transaction History (depend on accounts and loans)
        await SavingsTransactionSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await ShareTransactionSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);
        await LoanRepaymentSeeder.SeedAsync(context, logger, tenant, cancellationToken).ConfigureAwait(false);

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] completed seeding microfinance module with comprehensive sample data", tenant);
    }
}

