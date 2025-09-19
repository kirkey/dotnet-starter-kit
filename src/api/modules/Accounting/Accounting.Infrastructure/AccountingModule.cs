using Accounting.Infrastructure.Endpoints.AccountingPeriods.v1;
using Accounting.Infrastructure.Endpoints.AccountReconciliation.v1;
using Accounting.Infrastructure.Endpoints.Billing.v1;
using Accounting.Infrastructure.Endpoints.Budgets.v1;
using Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;
using Accounting.Infrastructure.Endpoints.Customers.v1;
using Accounting.Infrastructure.Endpoints.FinancialStatements.v1;
using Accounting.Infrastructure.Endpoints.FixedAssets.v1;
using Accounting.Infrastructure.Endpoints.Inventory.v1;
using Accounting.Infrastructure.Endpoints.JournalEntries.v1;
using Accounting.Infrastructure.Endpoints.Patronage.v1;
using Accounting.Infrastructure.Endpoints.Payees.v1;
using Accounting.Infrastructure.Endpoints.PaymentAllocations.v1;
using Accounting.Infrastructure.Endpoints.Payments.v1;
using Accounting.Infrastructure.Endpoints.Projects.v1;
using Accounting.Infrastructure.Endpoints.Vendors.v1;
using Accounting.Infrastructure.Persistence;
using Accounting.Infrastructure.Persistence.Configurations;
using Carter;
using FSH.Framework.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Infrastructure;

public static class AccountingModule
{
    // Restore proper CarterModule subclass pattern and constructor
    public class Endpoints() : CarterModule("accounting")
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var account = app.MapGroup("accounts").WithTags("accounts");
            account.MapChartOfAccountCreateEndpoint();
            account.MapChartOfAccountGetEndpoint();
            account.MapChartOfAccountSearchEndpoint();
            account.MapChartOfAccountUpdateEndpoint();
            account.MapChartOfAccountDeleteEndpoint();
            
            var payee = app.MapGroup("payees").WithTags("payees");
            payee.MapPayeeCreateEndpoint();
            payee.MapPayeeGetEndpoint();
            payee.MapPayeeSearchEndpoint();
            payee.MapPayeeUpdateEndpoint();
            payee.MapPayeeDeleteEndpoint();
            
            var vendor = app.MapGroup("vendors").WithTags("vendors");
            vendor.MapVendorCreateEndpoint();
            vendor.MapVendorGetEndpoint();
            vendor.MapVendorSearchEndpoint();
            vendor.MapVendorUpdateEndpoint();
            vendor.MapVendorDeleteEndpoint();

            var customer = app.MapGroup("customers").WithTags("customers");
            customer.MapCustomerCreateEndpoint();
            customer.MapCustomerGetEndpoint();
            customer.MapCustomerSearchEndpoint();
            customer.MapCustomerUpdateEndpoint();
            customer.MapCustomerDeleteEndpoint();

            var projects = app.MapGroup("projects").WithTags("projects");
            projects.MapProjectCreateEndpoint();
            projects.MapProjectGetEndpoint();
            projects.MapProjectSearchEndpoint();
            projects.MapProjectUpdateEndpoint();
            projects.MapProjectDeleteEndpoint();

            var periods = app.MapGroup("periods").WithTags("periods");
            periods.MapAccountingPeriodSearchEndpoint();
            periods.MapAccountingPeriodCreateEndpoint();
            periods.MapAccountingPeriodUpdateEndpoint();
            periods.MapAccountingPeriodDeleteEndpoint();
            periods.MapAccountingPeriodGetEndpoint();

            var budgets = app.MapGroup("budgets").WithTags("budgets");
            budgets.MapBudgetSearchEndpoint();
            budgets.MapBudgetCreateEndpoint();
            budgets.MapBudgetGetEndpoint();
            budgets.MapBudgetUpdateEndpoint();
            budgets.MapBudgetDeleteEndpoint();

            var fixedAssets = app.MapGroup("fixedassets").WithTags("fixedassets");
            fixedAssets.MapFixedAssetSearchEndpoint();
            fixedAssets.MapFixedAssetGetEndpoint();
            fixedAssets.MapFixedAssetCreateEndpoint();
            fixedAssets.MapFixedAssetUpdateEndpoint();
            fixedAssets.MapFixedAssetDeleteEndpoint();

            // New route groups for domains previously missing endpoints
            var reconciliations = app.MapGroup("reconciliations").WithTags("reconciliations");
            reconciliations.MapReconcileAccountEndpoint();

            var billing = app.MapGroup("billing").WithTags("billing");
            billing.MapCreateInvoiceFromConsumptionEndpoint();

            var financial = app.MapGroup("financialstatements").WithTags("financialstatements");
            financial.MapGenerateBalanceSheetEndpoint();
            financial.MapGenerateIncomeStatementEndpoint();
            financial.MapGenerateCashFlowStatementEndpoint();

            var inventory = app.MapGroup("inventory").WithTags("inventory");
            inventory.MapCreateInventoryItemEndpoint();

            var patronage = app.MapGroup("patronage").WithTags("patronage");
            patronage.MapRetirePatronageEndpoint();

            var payments = app.MapGroup("payments").WithTags("payments");
            payments.MapAllocatePaymentEndpoint();

            var paymentAllocations = app.MapGroup("payment-allocations").WithTags("payment-allocations");
            paymentAllocations.MapPaymentAllocationSearchEndpoint();
            paymentAllocations.MapPaymentAllocationGetEndpoint();
            paymentAllocations.MapPaymentAllocationDeleteEndpoint();

            var journals = app.MapGroup("journals").WithTags("journals");
            journals.MapJournalEntrySearchEndpoint();
        }
    }

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
        builder.Services.AddScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>();
        builder.Services.AddScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>();
        builder.Services.AddScoped<IRepository<Consumption>, AccountingRepository<Consumption>>();
        builder.Services.AddScoped<IReadRepository<Consumption>, AccountingRepository<Consumption>>();
        builder.Services.AddScoped<IRepository<Customer>, AccountingRepository<Customer>>();
        builder.Services.AddScoped<IReadRepository<Customer>, AccountingRepository<Customer>>();
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

        // Register with the "accounting" key (for handlers that use [FromKeyedServices("accounting")])
        builder.Services.AddKeyedScoped<IRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Budget>, AccountingRepository<Budget>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Budget>, AccountingRepository<Budget>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Consumption>, AccountingRepository<Consumption>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Consumption>, AccountingRepository<Consumption>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Customer>, AccountingRepository<Customer>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Customer>, AccountingRepository<Customer>>("accounting");
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
        // Added missing repository registrations for PostingBatch, DeferredRevenue, and Accrual
        builder.Services.AddKeyedScoped<IRepository<PostingBatch>, AccountingRepository<PostingBatch>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<PostingBatch>, AccountingRepository<PostingBatch>>("accounting");
        // New keyed registrations for added domain types
        builder.Services.AddKeyedScoped<IRepository<RateSchedule>, AccountingRepository<RateSchedule>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<RateSchedule>, AccountingRepository<RateSchedule>>("accounting");
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
        builder.Services.AddKeyedScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:accounts");
        builder.Services.AddKeyedScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:accounts");
        builder.Services.AddKeyedScoped<IRepository<Consumption>, AccountingRepository<Consumption>>("accounting:Consumption");
        builder.Services.AddKeyedScoped<IReadRepository<Consumption>, AccountingRepository<Consumption>>("accounting:Consumption");
        builder.Services.AddKeyedScoped<IRepository<Customer>, AccountingRepository<Customer>>("accounting:customers");
        builder.Services.AddKeyedScoped<IReadRepository<Customer>, AccountingRepository<Customer>>("accounting:customers");
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
        builder.Services.AddKeyedScoped<IRepository<Vendor>, AccountingRepository<Vendor>>("accounting:vendors");
        builder.Services.AddKeyedScoped<IReadRepository<Vendor>, AccountingRepository<Vendor>>("accounting:vendors");
        builder.Services.AddKeyedScoped<IRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>("accounting:paymentallocations");
        builder.Services.AddKeyedScoped<IReadRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>("accounting:paymentallocations");
        // Added missing specific-key registrations for PostingBatch, DeferredRevenue, and Accrual
        builder.Services.AddKeyedScoped<IRepository<PostingBatch>, AccountingRepository<PostingBatch>>("accounting:postingbatches");
        builder.Services.AddKeyedScoped<IReadRepository<PostingBatch>, AccountingRepository<PostingBatch>>("accounting:postingbatches");
        builder.Services.AddKeyedScoped<IReadRepository<DeferredRevenue>, AccountingRepository<DeferredRevenue>>("accounting:deferredrevenues");
        builder.Services.AddKeyedScoped<IRepository<Accrual>, AccountingRepository<Accrual>>("accounting:accruals");
        builder.Services.AddKeyedScoped<IRepository<RegulatoryReport>, AccountingRepository<RegulatoryReport>>("accounting:regulatoryreports");
        builder.Services.AddKeyedScoped<IReadRepository<RegulatoryReport>, AccountingRepository<RegulatoryReport>>("accounting:regulatoryreports");
        builder.Services.AddKeyedScoped<IReadRepository<Accrual>, AccountingRepository<Accrual>>("accounting:accruals");
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
