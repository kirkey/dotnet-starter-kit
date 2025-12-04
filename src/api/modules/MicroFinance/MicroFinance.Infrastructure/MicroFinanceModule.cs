using Carter;
using FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure;

/// <summary>
/// MicroFinance module configuration and endpoint registration.
/// Handles all microfinance CRUD operations and service registration.
/// </summary>
public static class MicroFinanceModule
{
    /// <summary>
    /// Endpoint routes for the MicroFinance module.
    /// Maps all microfinance endpoints with proper grouping and documentation.
    /// </summary>
    public class Endpoints() : CarterModule("microfinance")
    {
        /// <summary>
        /// Adds all microfinance routes to the application.
        /// </summary>
        /// <param name="app">The endpoint route builder.</param>
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            // Members endpoints
            var membersGroup = app.MapGroup("members").WithTags("members");
            membersGroup.MapGet("/", () => Results.Ok("Members endpoint - Coming soon"))
                .WithName("GetMembers")
                .WithSummary("Gets all members");

            // Loan Products endpoints
            var loanProductsGroup = app.MapGroup("loan-products").WithTags("loan-products");
            loanProductsGroup.MapGet("/", () => Results.Ok("Loan Products endpoint - Coming soon"))
                .WithName("GetLoanProducts")
                .WithSummary("Gets all loan products");

            // Loans endpoints
            var loansGroup = app.MapGroup("loans").WithTags("loans");
            loansGroup.MapGet("/", () => Results.Ok("Loans endpoint - Coming soon"))
                .WithName("GetLoans")
                .WithSummary("Gets all loans");

            // Savings Products endpoints
            var savingsProductsGroup = app.MapGroup("savings-products").WithTags("savings-products");
            savingsProductsGroup.MapGet("/", () => Results.Ok("Savings Products endpoint - Coming soon"))
                .WithName("GetSavingsProducts")
                .WithSummary("Gets all savings products");

            // Savings Accounts endpoints
            var savingsAccountsGroup = app.MapGroup("savings-accounts").WithTags("savings-accounts");
            savingsAccountsGroup.MapGet("/", () => Results.Ok("Savings Accounts endpoint - Coming soon"))
                .WithName("GetSavingsAccounts")
                .WithSummary("Gets all savings accounts");
        }
    }

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

        // Members repository
        builder.Services.AddKeyedScoped<IRepository<Member>, MicroFinanceRepository<Member>>("microfinance:members");
        builder.Services.AddKeyedScoped<IReadRepository<Member>, MicroFinanceRepository<Member>>("microfinance:members");

        // LoanProducts repository
        builder.Services.AddKeyedScoped<IRepository<LoanProduct>, MicroFinanceRepository<LoanProduct>>("microfinance:loanproducts");
        builder.Services.AddKeyedScoped<IReadRepository<LoanProduct>, MicroFinanceRepository<LoanProduct>>("microfinance:loanproducts");

        // Loans repository
        builder.Services.AddKeyedScoped<IRepository<Loan>, MicroFinanceRepository<Loan>>("microfinance:loans");
        builder.Services.AddKeyedScoped<IReadRepository<Loan>, MicroFinanceRepository<Loan>>("microfinance:loans");

        // LoanRepayments repository
        builder.Services.AddKeyedScoped<IRepository<LoanRepayment>, MicroFinanceRepository<LoanRepayment>>("microfinance:loanrepayments");
        builder.Services.AddKeyedScoped<IReadRepository<LoanRepayment>, MicroFinanceRepository<LoanRepayment>>("microfinance:loanrepayments");

        // SavingsProducts repository
        builder.Services.AddKeyedScoped<IRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>("microfinance:savingsproducts");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsProduct>, MicroFinanceRepository<SavingsProduct>>("microfinance:savingsproducts");

        // SavingsAccounts repository
        builder.Services.AddKeyedScoped<IRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>("microfinance:savingsaccounts");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsAccount>, MicroFinanceRepository<SavingsAccount>>("microfinance:savingsaccounts");

        // SavingsTransactions repository
        builder.Services.AddKeyedScoped<IRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>("microfinance:savingstransactions");
        builder.Services.AddKeyedScoped<IReadRepository<SavingsTransaction>, MicroFinanceRepository<SavingsTransaction>>("microfinance:savingstransactions");

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

