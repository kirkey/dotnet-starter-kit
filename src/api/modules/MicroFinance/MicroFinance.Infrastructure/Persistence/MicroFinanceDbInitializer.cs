using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence;

/// <summary>
/// Database initializer for the MicroFinance module.
/// Handles migrations and seeding of initial data.
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
        // Seed default loan product if not exists
        const string DefaultLoanProductCode = "PERSONAL-LOAN";
        if (await context.LoanProducts.FirstOrDefaultAsync(p => p.Code == DefaultLoanProductCode, cancellationToken).ConfigureAwait(false) is null)
        {
            var loanProduct = LoanProduct.Create(
                code: DefaultLoanProductCode,
                name: "Personal Loan",
                description: "Standard personal loan product for members",
                currencyCode: "USD",
                minLoanAmount: 100,
                maxLoanAmount: 10000,
                interestRate: 12,
                interestMethod: "Flat",
                minTermMonths: 3,
                maxTermMonths: 24,
                repaymentFrequency: "Monthly");

            await context.LoanProducts.AddAsync(loanProduct, cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded default loan product", context.TenantInfo!.Identifier);
        }

        // Seed default savings product if not exists
        const string DefaultSavingsProductCode = "REGULAR-SAVINGS";
        if (await context.SavingsProducts.FirstOrDefaultAsync(p => p.Code == DefaultSavingsProductCode, cancellationToken).ConfigureAwait(false) is null)
        {
            var savingsProduct = SavingsProduct.Create(
                code: DefaultSavingsProductCode,
                name: "Regular Savings",
                description: "Standard savings account for members",
                currencyCode: "USD",
                interestRate: 3.5m,
                interestCalculation: "Daily",
                interestPostingFrequency: "Monthly",
                minOpeningBalance: 10,
                minBalanceForInterest: 100);

            await context.SavingsProducts.AddAsync(savingsProduct, cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeded default savings product", context.TenantInfo!.Identifier);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}

