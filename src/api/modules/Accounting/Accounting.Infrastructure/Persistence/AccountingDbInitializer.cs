using FSH.Framework.Core.Persistence;
using Accounting.Domain;
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
        const string category = "Asset";
        const string type = "Debit";
        const string name = "Cash Account";
        const string parentCode = "1";
        const string code = "1000";
        const decimal balance = 5000.00M;
        const string description = "Main cash account for general operations";
        
        if (await context.Accounts.FirstOrDefaultAsync(a => a.Code == code, cancellationToken).ConfigureAwait(false) is null)
        {
            var account = Account.Create(category, type, parentCode, code, name, balance, description);
            await context.Accounts.AddAsync(account, cancellationToken).ConfigureAwait(false);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeding default accounting data", context.TenantInfo!.Identifier);
        }
    }
}
