using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Accounting.Infrastructure.Persistence;

internal sealed class AccountingDbInitializer(
    ILogger<AccountingDbInitializer> logger,
    AccountingDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for accounting module", context.TenantInfo!.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        // Check if the ChartOfAccounts table is empty
        // if (!await context.ChartOfAccounts.AnyAsync(cancellationToken).ConfigureAwait(false))
        // {
        //     const string category = "Asset";
        //     const string type = "Debit";
        //     const string name = "Cash Account";
        //     const string parentCode = "1";
        //     const string accountCode = "1000";
        //     const decimal balance = 5000.00M;
        //     const string description = "Main cash account for general operations";
        //
        //     var account = ChartOfAccount.Create(category, type, parentCode, accountCode, name, balance, description);
        //     await context.ChartOfAccounts.AddAsync(account, cancellationToken).ConfigureAwait(false);
        //     await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        //     logger.LogInformation("[{Tenant}] seeding default accounting data", context.TenantInfo!.Identifier);
        // }
    }
}
