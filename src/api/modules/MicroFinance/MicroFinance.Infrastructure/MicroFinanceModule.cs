using FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure;

/// <summary>
/// MicroFinance module configuration and service registration.
/// Handles dependency injection setup for microfinance features.
/// See MicroFinanceEndpoints for endpoint routing configuration.
/// </summary>
public static class MicroFinanceModule
{

    /// <summary>
    /// Registers all microfinance services in the dependency injection container.
    /// Configures DbContext, repositories, and database initializers.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The web application builder for chaining.</returns>
    public static WebApplicationBuilder RegisterMicroFinanceServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.BindDbContext<MicroFinanceDbContext>();
        builder.Services.AddScoped<IDbInitializer, MicroFinanceDbInitializer>();

        // ============================================
        // Member Management Repositories
        // ============================================

        // Members repository
        builder.Services.AddKeyedScoped<IRepository<Member>, MicroFinanceRepository<Member>>("microfinance:members");
        builder.Services.AddKeyedScoped<IReadRepository<Member>, MicroFinanceRepository<Member>>("microfinance:members");

        // MemberGroups repository
        builder.Services.AddKeyedScoped<IRepository<MemberGroup>, MicroFinanceRepository<MemberGroup>>("microfinance:membergroups");
        builder.Services.AddKeyedScoped<IReadRepository<MemberGroup>, MicroFinanceRepository<MemberGroup>>("microfinance:membergroups");

        // GroupMemberships repository
        builder.Services.AddKeyedScoped<IRepository<GroupMembership>, MicroFinanceRepository<GroupMembership>>("microfinance:groupmemberships");
        builder.Services.AddKeyedScoped<IReadRepository<GroupMembership>, MicroFinanceRepository<GroupMembership>>("microfinance:groupmemberships");

        // ============================================
        // Loan Management Repositories
        // ============================================

        // LoanProducts repository
        builder.Services.AddKeyedScoped<IRepository<LoanProduct>, MicroFinanceRepository<LoanProduct>>("microfinance:loanproducts");
        builder.Services.AddKeyedScoped<IReadRepository<LoanProduct>, MicroFinanceRepository<LoanProduct>>("microfinance:loanproducts");

        // Loans repository
        builder.Services.AddKeyedScoped<IRepository<Loan>, MicroFinanceRepository<Loan>>("microfinance:loans");
        builder.Services.AddKeyedScoped<IReadRepository<Loan>, MicroFinanceRepository<Loan>>("microfinance:loans");

        // LoanRepayments repository
        builder.Services.AddKeyedScoped<IRepository<LoanRepayment>, MicroFinanceRepository<LoanRepayment>>("microfinance:loanrepayments");
        builder.Services.AddKeyedScoped<IReadRepository<LoanRepayment>, MicroFinanceRepository<LoanRepayment>>("microfinance:loanrepayments");

        // LoanSchedules repository
        builder.Services.AddKeyedScoped<IRepository<LoanSchedule>, MicroFinanceRepository<LoanSchedule>>("microfinance:loanschedules");
        builder.Services.AddKeyedScoped<IReadRepository<LoanSchedule>, MicroFinanceRepository<LoanSchedule>>("microfinance:loanschedules");

        // LoanGuarantors repository
        builder.Services.AddKeyedScoped<IRepository<LoanGuarantor>, MicroFinanceRepository<LoanGuarantor>>("microfinance:loanguarantors");
        builder.Services.AddKeyedScoped<IReadRepository<LoanGuarantor>, MicroFinanceRepository<LoanGuarantor>>("microfinance:loanguarantors");

        // LoanCollaterals repository
        builder.Services.AddKeyedScoped<IRepository<LoanCollateral>, MicroFinanceRepository<LoanCollateral>>("microfinance:loancollaterals");
        builder.Services.AddKeyedScoped<IReadRepository<LoanCollateral>, MicroFinanceRepository<LoanCollateral>>("microfinance:loancollaterals");

        // ============================================
        // Savings Management Repositories
        // ============================================

        // SavingsProducts repository
        builder.Services.AddKeyedScoped<IRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>("microfinance:savingsproducts");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>("microfinance:savingsproducts");

        // SavingsAccounts repository
        builder.Services.AddKeyedScoped<IRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>("microfinance:savingsaccounts");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>("microfinance:savingsaccounts");

        // SavingsTransactions repository
        builder.Services.AddKeyedScoped<IRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>("microfinance:savingstransactions");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>("microfinance:savingstransactions");

        // FixedDeposits repository
        builder.Services.AddKeyedScoped<IRepository<FixedDeposit>, MicroFinanceRepository<FixedDeposit>>("microfinance:fixeddeposits");
        builder.Services.AddKeyedScoped<IReadRepository<FixedDeposit>, MicroFinanceRepository<FixedDeposit>>("microfinance:fixeddeposits");

        // ============================================
        // Share Management Repositories
        // ============================================

        // ShareProducts repository
        builder.Services.AddKeyedScoped<IRepository<ShareProduct>, MicroFinanceRepository<ShareProduct>>("microfinance:shareproducts");
        builder.Services.AddKeyedScoped<IReadRepository<ShareProduct>, MicroFinanceRepository<ShareProduct>>("microfinance:shareproducts");

        // ShareAccounts repository
        builder.Services.AddKeyedScoped<IRepository<ShareAccount>, MicroFinanceRepository<ShareAccount>>("microfinance:shareaccounts");
        builder.Services.AddKeyedScoped<IReadRepository<ShareAccount>, MicroFinanceRepository<ShareAccount>>("microfinance:shareaccounts");

        // ShareTransactions repository
        builder.Services.AddKeyedScoped<IRepository<ShareTransaction>, MicroFinanceRepository<ShareTransaction>>("microfinance:sharetransactions");
        builder.Services.AddKeyedScoped<IReadRepository<ShareTransaction>, MicroFinanceRepository<ShareTransaction>>("microfinance:sharetransactions");

        // ============================================
        // Fee Management Repositories
        // ============================================

        // FeeDefinitions repository
        builder.Services.AddKeyedScoped<IRepository<FeeDefinition>, MicroFinanceRepository<FeeDefinition>>("microfinance:feedefinitions");
        builder.Services.AddKeyedScoped<IReadRepository<FeeDefinition>, MicroFinanceRepository<FeeDefinition>>("microfinance:feedefinitions");

        // FeeCharges repository
        builder.Services.AddKeyedScoped<IRepository<FeeCharge>, MicroFinanceRepository<FeeCharge>>("microfinance:feecharges");
        builder.Services.AddKeyedScoped<IReadRepository<FeeCharge>, MicroFinanceRepository<FeeCharge>>("microfinance:feecharges");

        return builder;
    }

    /// <summary>
    /// Applies the microfinance module to the web application.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>The web application for chaining.</returns>
    public static WebApplication UseMicroFinanceModule(this WebApplication app)
    {
        return app;
    }
}

