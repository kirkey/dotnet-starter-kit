using Accounting.Infrastructure.Endpoints.AccountingPeriods;
using Accounting.Infrastructure.Endpoints.Accruals;
using Accounting.Infrastructure.Endpoints.Billing;
using Accounting.Infrastructure.Endpoints.BudgetDetails;
using Accounting.Infrastructure.Endpoints.Budgets;
using Accounting.Infrastructure.Endpoints.ChartOfAccounts;
using Accounting.Infrastructure.Endpoints.Consumptions;
using Accounting.Infrastructure.Endpoints.DeferredRevenue;
using Accounting.Infrastructure.Endpoints.DepreciationMethods;
using Accounting.Infrastructure.Endpoints.FinancialStatements;
using Accounting.Infrastructure.Endpoints.GeneralLedger;
using Accounting.Infrastructure.Endpoints.Inventory;
using Accounting.Infrastructure.Endpoints.Invoice;
using Accounting.Infrastructure.Endpoints.Member;
using Accounting.Infrastructure.Endpoints.Meter;
using Accounting.Infrastructure.Endpoints.Patronage;
using Accounting.Infrastructure.Endpoints.Payees;
using Accounting.Infrastructure.Endpoints.PaymentAllocations;
using Accounting.Infrastructure.Endpoints.Payments;
using Accounting.Infrastructure.Endpoints.PostingBatch;
using Accounting.Infrastructure.Endpoints.Projects;
using Accounting.Infrastructure.Endpoints.Projects.Costing;
using Accounting.Infrastructure.Endpoints.TrialBalance;
using Accounting.Infrastructure.Persistence;
using Accounting.Infrastructure.Import;
using Accounting.Application.ChartOfAccounts.Import;
using Accounting.Application.Payees.Import;
using FSH.Framework.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Infrastructure;

/// <summary>
/// Main module configuration for the Accounting system.
/// Registers all accounting endpoints and services.
/// </summary>
public static class AccountingModule
{
    /// <summary>
    /// Registers all accounting endpoints with the application.
    /// </summary>
    /// <param name="app">The endpoint route builder to configure.</param>
    /// <returns>The configured endpoint route builder.</returns>
    public static IEndpointRouteBuilder MapAccountingEndpoints(this IEndpointRouteBuilder app)
    {
        var accountingGroup = app.MapGroup("/accounting")
            .WithTags("Accounting")
            .WithDescription("Comprehensive accounting module endpoints");

        // Map all functional area endpoints
        accountingGroup.MapAccountingPeriodsEndpoints();
        accountingGroup.MapAccrualsEndpoints();
        accountingGroup.MapBillingEndpoints();
        accountingGroup.MapBudgetDetailsEndpoints();
        accountingGroup.MapBudgetsEndpoints();
        accountingGroup.MapChartOfAccountsEndpoints();
        accountingGroup.MapConsumptionsEndpoints();
        accountingGroup.MapDeferredRevenueEndpoints();
        accountingGroup.MapDepreciationMethodsEndpoints();
        accountingGroup.MapFinancialStatementsEndpoints();
        accountingGroup.MapGeneralLedgerEndpoints();
        accountingGroup.MapInventoryEndpoints();
        accountingGroup.MapInvoiceEndpoints();
        accountingGroup.MapMemberEndpoints();
        accountingGroup.MapMeterEndpoints();
        accountingGroup.MapPatronageEndpoints();
        accountingGroup.MapPayeesEndpoints();
        accountingGroup.MapPaymentAllocationsEndpoints();
        accountingGroup.MapPaymentsEndpoints();
        accountingGroup.MapPostingBatchEndpoints();
        accountingGroup.MapProjectsEndpoints();
        accountingGroup.MapProjectsCostingEndpoints();
        accountingGroup.MapTrialBalanceEndpoints();

        return app;
    }

    /// <summary>
    /// Registers accounting services with dependency injection container.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The configured web application builder.</returns>

    public static WebApplicationBuilder RegisterAccountingServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<AccountingDbContext>();
        builder.Services.AddScoped<IDbInitializer, AccountingDbInitializer>();
    
        // Register without keys (for MediatR handlers that don't use keyed services)
        builder.Services.AddScoped<IRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>();
        builder.Services.AddScoped<IReadRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>();
        builder.Services.AddScoped<IRepository<Budget>, AccountingRepository<Budget>>();
        builder.Services.AddScoped<IReadRepository<Budget>, AccountingRepository<Budget>>();
        builder.Services.AddScoped<IRepository<BudgetDetail>, AccountingRepository<BudgetDetail>>();
        builder.Services.AddScoped<IReadRepository<BudgetDetail>, AccountingRepository<BudgetDetail>>();
        builder.Services.AddScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>();
        builder.Services.AddScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>();
        builder.Services.AddScoped<IRepository<Consumption>, AccountingRepository<Consumption>>();
        builder.Services.AddScoped<IReadRepository<Consumption>, AccountingRepository<Consumption>>();
        builder.Services.AddScoped<IRepository<DepreciationMethod>, AccountingRepository<DepreciationMethod>>();
        builder.Services.AddScoped<IReadRepository<DepreciationMethod>, AccountingRepository<DepreciationMethod>>();
        builder.Services.AddScoped<IRepository<FixedAsset>, AccountingRepository<FixedAsset>>();
        builder.Services.AddScoped<IReadRepository<FixedAsset>, AccountingRepository<FixedAsset>>();
        builder.Services.AddScoped<IRepository<GeneralLedger>, AccountingRepository<GeneralLedger>>();
        builder.Services.AddScoped<IReadRepository<GeneralLedger>, AccountingRepository<GeneralLedger>>();
        builder.Services.AddScoped<IRepository<Invoice>, AccountingRepository<Invoice>>();
        builder.Services.AddScoped<IReadRepository<Invoice>, AccountingRepository<Invoice>>();
        builder.Services.AddScoped<IRepository<JournalEntry>, AccountingRepository<JournalEntry>>();
        builder.Services.AddScoped<IReadRepository<JournalEntry>, AccountingRepository<JournalEntry>>();
        builder.Services.AddScoped<IRepository<Member>, AccountingRepository<Member>>();
        builder.Services.AddScoped<IReadRepository<Member>, AccountingRepository<Member>>();
        builder.Services.AddScoped<IRepository<Meter>, AccountingRepository<Meter>>();
        builder.Services.AddScoped<IReadRepository<Meter>, AccountingRepository<Meter>>();
        builder.Services.AddScoped<IRepository<Payee>, AccountingRepository<Payee>>();
        builder.Services.AddScoped<IReadRepository<Payee>, AccountingRepository<Payee>>();
        builder.Services.AddScoped<IRepository<Project>, AccountingRepository<Project>>();
        builder.Services.AddScoped<IReadRepository<Project>, AccountingRepository<Project>>();
        // Register repository for project cost entries (non-keyed) so handlers without keyed services can resolve it
        builder.Services.AddScoped<IRepository<ProjectCostEntry>, AccountingRepository<ProjectCostEntry>>();
        builder.Services.AddScoped<IReadRepository<ProjectCostEntry>, AccountingRepository<ProjectCostEntry>>();
        builder.Services.AddScoped<IRepository<Vendor>, AccountingRepository<Vendor>>();
        builder.Services.AddScoped<IReadRepository<Vendor>, AccountingRepository<Vendor>>();
        // Ensure DeferredRevenue and Accrual repositories are registered for non-keyed resolution
        builder.Services.AddScoped<IRepository<DeferredRevenue>, AccountingRepository<DeferredRevenue>>();
        builder.Services.AddScoped<IReadRepository<DeferredRevenue>, AccountingRepository<DeferredRevenue>>();
        builder.Services.AddScoped<IRepository<Accrual>, AccountingRepository<Accrual>>();
        builder.Services.AddScoped<IReadRepository<Accrual>, AccountingRepository<Accrual>>();
        // Added missing repository registrations for PostingBatch, DeferredRevenue, and Accrual
        builder.Services.AddScoped<IRepository<PostingBatch>, AccountingRepository<PostingBatch>>();
        builder.Services.AddScoped<IReadRepository<PostingBatch>, AccountingRepository<PostingBatch>>();
        // New domain repositories for electric cooperative features
        builder.Services.AddScoped<IRepository<RateSchedule>, AccountingRepository<RateSchedule>>();
        builder.Services.AddScoped<IReadRepository<RateSchedule>, AccountingRepository<RateSchedule>>();
        builder.Services.AddScoped<IRepository<PatronageCapital>, AccountingRepository<PatronageCapital>>();
        builder.Services.AddScoped<IReadRepository<PatronageCapital>, AccountingRepository<PatronageCapital>>();
        builder.Services.AddScoped<IRepository<SecurityDeposit>, AccountingRepository<SecurityDeposit>>();
        builder.Services.AddScoped<IReadRepository<SecurityDeposit>, AccountingRepository<SecurityDeposit>>();
        builder.Services.AddScoped<IRepository<Payment>, AccountingRepository<Payment>>();
        builder.Services.AddScoped<IReadRepository<Payment>, AccountingRepository<Payment>>();
        builder.Services.AddScoped<IRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>();
        builder.Services.AddScoped<IReadRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>();
        // Billing service
        builder.Services.AddScoped<Application.Billing.IBillingService, Application.Billing.BillingService>();

        // Register import parsers for Chart of Accounts and Payees
        builder.Services.AddScoped<IChartOfAccountImportParser, ChartOfAccountImportParser>();
        builder.Services.AddScoped<IPayeeImportParser, PayeeImportParser>();

        // Register with the "accounting" key (for handlers that use [FromKeyedServices("accounting")])
        builder.Services.AddKeyedScoped<IRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Budget>, AccountingRepository<Budget>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Budget>, AccountingRepository<Budget>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<BudgetDetail>, AccountingRepository<BudgetDetail>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<BudgetDetail>, AccountingRepository<BudgetDetail>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Consumption>, AccountingRepository<Consumption>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Consumption>, AccountingRepository<Consumption>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<DepreciationMethod>, AccountingRepository<DepreciationMethod>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<DepreciationMethod>, AccountingRepository<DepreciationMethod>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<FixedAsset>, AccountingRepository<FixedAsset>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<FixedAsset>, AccountingRepository<FixedAsset>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<GeneralLedger>, AccountingRepository<GeneralLedger>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<GeneralLedger>, AccountingRepository<GeneralLedger>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Invoice>, AccountingRepository<Invoice>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Invoice>, AccountingRepository<Invoice>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<JournalEntry>, AccountingRepository<JournalEntry>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<JournalEntry>, AccountingRepository<JournalEntry>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Member>, AccountingRepository<Member>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Member>, AccountingRepository<Member>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Meter>, AccountingRepository<Meter>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Meter>, AccountingRepository<Meter>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Payee>, AccountingRepository<Payee>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Payee>, AccountingRepository<Payee>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Project>, AccountingRepository<Project>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<PostingBatch>, AccountingRepository<PostingBatch>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Vendor>, AccountingRepository<Vendor>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Vendor>, AccountingRepository<Vendor>>("accounting");
        // Removed keyed registrations for ProjectCostEntry
        // Also register a dedicated key for rate schedules used by billing handlers
        builder.Services.AddKeyedScoped<IRepository<RateSchedule>, AccountingRepository<RateSchedule>>("accounting:rateschedules");
        builder.Services.AddKeyedScoped<IReadRepository<RateSchedule>, AccountingRepository<RateSchedule>>("accounting:rateschedules");
        // Register keyed repositories matching handler expectations
        builder.Services.AddKeyedScoped<IRepository<PatronageCapital>, AccountingRepository<PatronageCapital>>("accounting:patronagecapitals");
        builder.Services.AddKeyedScoped<IReadRepository<PatronageCapital>, AccountingRepository<PatronageCapital>>("accounting:patronagecapitals");
        // Handlers use the key "accounting:consumption" (singular) in some places — register it as well
        builder.Services.AddKeyedScoped<IRepository<Consumption>, AccountingRepository<Consumption>>("accounting:consumption");
        builder.Services.AddKeyedScoped<IReadRepository<Consumption>, AccountingRepository<Consumption>>("accounting:consumption");
        builder.Services.AddKeyedScoped<IRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>("accounting");
        // Register with specific keys (for handlers that use specific keys like "accounting:budgets", "accounting:accounts", etc.)
        builder.Services.AddKeyedScoped<IRepository<Accrual>, AccountingRepository<Accrual>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Accrual>, AccountingRepository<Accrual>>("accounting");

        // Register with specific keys (for handlers that use specific keys like "accounting:budgets", "accounting:accounts", etc.)
        builder.Services.AddKeyedScoped<IRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>("accounting:periods");
        builder.Services.AddKeyedScoped<IReadRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>("accounting:periods");
        builder.Services.AddKeyedScoped<IRepository<Budget>, AccountingRepository<Budget>>("accounting:budgets");
        builder.Services.AddKeyedScoped<IReadRepository<Budget>, AccountingRepository<Budget>>("accounting:budgets");
        builder.Services.AddKeyedScoped<IRepository<BudgetDetail>, AccountingRepository<BudgetDetail>>("accounting:budgetdetails");
        builder.Services.AddKeyedScoped<IReadRepository<BudgetDetail>, AccountingRepository<BudgetDetail>>("accounting:budgetdetails");
        builder.Services.AddKeyedScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:accounts");
        builder.Services.AddKeyedScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:accounts");
        // Add missing registration for the key expected by import/export handlers
        builder.Services.AddKeyedScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:chartofaccounts");
        builder.Services.AddKeyedScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:chartofaccounts");
        builder.Services.AddKeyedScoped<IRepository<Consumption>, AccountingRepository<Consumption>>("accounting:Consumption");
        builder.Services.AddKeyedScoped<IReadRepository<Consumption>, AccountingRepository<Consumption>>("accounting:Consumption");
        builder.Services.AddKeyedScoped<IRepository<DepreciationMethod>, AccountingRepository<DepreciationMethod>>("accounting:depreciationmethods");
        builder.Services.AddKeyedScoped<IReadRepository<DepreciationMethod>, AccountingRepository<DepreciationMethod>>("accounting:depreciationmethods");
        builder.Services.AddKeyedScoped<IRepository<FixedAsset>, AccountingRepository<FixedAsset>>("accounting:fixedassets");
        builder.Services.AddKeyedScoped<IReadRepository<FixedAsset>, AccountingRepository<FixedAsset>>("accounting:fixedassets");
        builder.Services.AddKeyedScoped<IRepository<GeneralLedger>, AccountingRepository<GeneralLedger>>("accounting:generalledger");
        builder.Services.AddKeyedScoped<IReadRepository<GeneralLedger>, AccountingRepository<GeneralLedger>>("accounting:generalledger");
        builder.Services.AddKeyedScoped<IRepository<Invoice>, AccountingRepository<Invoice>>("accounting:invoices");
        builder.Services.AddKeyedScoped<IReadRepository<Invoice>, AccountingRepository<Invoice>>("accounting:invoices");
        builder.Services.AddKeyedScoped<IRepository<JournalEntry>, AccountingRepository<JournalEntry>>("accounting:journals");
        builder.Services.AddKeyedScoped<IReadRepository<JournalEntry>, AccountingRepository<JournalEntry>>("accounting:journals");
        builder.Services.AddKeyedScoped<IRepository<Member>, AccountingRepository<Member>>("accounting:members");
        builder.Services.AddKeyedScoped<IReadRepository<Member>, AccountingRepository<Member>>("accounting:members");
        builder.Services.AddKeyedScoped<IRepository<Meter>, AccountingRepository<Meter>>("accounting:meters");
        builder.Services.AddKeyedScoped<IReadRepository<Meter>, AccountingRepository<Meter>>("accounting:meters");
        builder.Services.AddKeyedScoped<IRepository<Payee>, AccountingRepository<Payee>>("accounting:payees");
        builder.Services.AddKeyedScoped<IReadRepository<Payee>, AccountingRepository<Payee>>("accounting:payees");
        builder.Services.AddKeyedScoped<IRepository<Project>, AccountingRepository<Project>>("accounting:projects");
        builder.Services.AddKeyedScoped<IReadRepository<Project>, AccountingRepository<Project>>("accounting:projects");
        builder.Services.AddKeyedScoped<IRepository<ProjectCostEntry>, AccountingRepository<ProjectCostEntry>>("accounting:projectcosts");
        builder.Services.AddKeyedScoped<IReadRepository<ProjectCostEntry>, AccountingRepository<ProjectCostEntry>>("accounting:projectcosts");
        // Also register with the key expected by new ProjectCosting handlers
        builder.Services.AddKeyedScoped<IRepository<ProjectCostEntry>, AccountingRepository<ProjectCostEntry>>("accounting:projectcosting");
        builder.Services.AddKeyedScoped<IReadRepository<ProjectCostEntry>, AccountingRepository<ProjectCostEntry>>("accounting:projectcosting");
        builder.Services.AddKeyedScoped<IRepository<Vendor>, AccountingRepository<Vendor>>("accounting:vendors");
        builder.Services.AddKeyedScoped<IReadRepository<Vendor>, AccountingRepository<Vendor>>("accounting:vendors");
        builder.Services.AddKeyedScoped<IRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>("accounting:paymentallocations");
        builder.Services.AddKeyedScoped<IReadRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>("accounting:paymentallocations");
        // Added missing specific-key registrations for PostingBatch, DeferredRevenue, and Accrual
        builder.Services.AddKeyedScoped<IRepository<PostingBatch>, AccountingRepository<PostingBatch>>("accounting:postingbatches");
        builder.Services.AddKeyedScoped<IReadRepository<PostingBatch>, AccountingRepository<PostingBatch>>("accounting:postingbatches");
        builder.Services.AddKeyedScoped<IReadRepository<DeferredRevenue>, AccountingRepository<DeferredRevenue>>("accounting:deferredrevenues");
        builder.Services.AddKeyedScoped<IRepository<Accrual>, AccountingRepository<Accrual>>("accounting:accruals");
        builder.Services.AddKeyedScoped<IReadRepository<Accrual>, AccountingRepository<Accrual>>("accounting:accruals");
        builder.Services.AddKeyedScoped<IRepository<RegulatoryReport>, AccountingRepository<RegulatoryReport>>("accounting:regulatoryreports");
        builder.Services.AddKeyedScoped<IReadRepository<RegulatoryReport>, AccountingRepository<RegulatoryReport>>("accounting:regulatoryreports");
        // Ensure there's a keyed registration matching the handler expectation for journal entries
        builder.Services.AddKeyedScoped<IRepository<JournalEntry>, AccountingRepository<JournalEntry>>("accounting:journalentries");
        builder.Services.AddKeyedScoped<IReadRepository<JournalEntry>, AccountingRepository<JournalEntry>>("accounting:journalentries");

        return builder;
    }

    public static WebApplication UseAccountingModule(this WebApplication app)
    {
        return app;
    }
}
