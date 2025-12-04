using Accounting.Application;
using Accounting.Application.ChartOfAccounts.Import;
using Accounting.Application.Payees.Import;
using Accounting.Infrastructure.Endpoints.AccountingPeriods;
using Accounting.Infrastructure.Endpoints.AccountReconciliations;
using Accounting.Infrastructure.Endpoints.AccountsPayableAccounts;
using Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts;
using Accounting.Infrastructure.Endpoints.Accruals;
using Accounting.Infrastructure.Endpoints.BankReconciliations;
using Accounting.Infrastructure.Endpoints.Banks;
using Accounting.Infrastructure.Endpoints.Billing;
using Accounting.Infrastructure.Endpoints.Bills;
using Accounting.Infrastructure.Endpoints.BudgetDetails;
using Accounting.Infrastructure.Endpoints.Budgets;
using Accounting.Infrastructure.Endpoints.ChartOfAccounts;
using Accounting.Infrastructure.Endpoints.Checks.v1;
using Accounting.Infrastructure.Endpoints.Consumptions;
using Accounting.Infrastructure.Endpoints.CostCenters;
using Accounting.Infrastructure.Endpoints.CreditMemos;
using Accounting.Infrastructure.Endpoints.Customers;
using Accounting.Infrastructure.Endpoints.DebitMemos;
using Accounting.Infrastructure.Endpoints.DeferredRevenues;
using Accounting.Infrastructure.Endpoints.DepreciationMethods;
using Accounting.Infrastructure.Endpoints.FinancialStatements;
using Accounting.Infrastructure.Endpoints.FiscalPeriodCloses;
using Accounting.Infrastructure.Endpoints.FixedAssets;
using Accounting.Infrastructure.Endpoints.GeneralLedger;
using Accounting.Infrastructure.Endpoints.InterCompanyTransactions;
using Accounting.Infrastructure.Endpoints.InventoryItems;
using Accounting.Infrastructure.Endpoints.Invoice;
using Accounting.Infrastructure.Endpoints.JournalEntries;
using Accounting.Infrastructure.Endpoints.JournalEntryLines;
using Accounting.Infrastructure.Endpoints.Member;
using Accounting.Infrastructure.Endpoints.Meter;
using Accounting.Infrastructure.Endpoints.Patronage;
using Accounting.Infrastructure.Endpoints.Payees;
using Accounting.Infrastructure.Endpoints.PaymentAllocations;
using Accounting.Infrastructure.Endpoints.Payments;
using Accounting.Infrastructure.Endpoints.PostingBatch;
using Accounting.Infrastructure.Endpoints.PrepaidExpenses;
using Accounting.Infrastructure.Endpoints.Projects;
using Accounting.Infrastructure.Endpoints.Projects.Costing;
using Accounting.Infrastructure.Endpoints.RecurringJournalEntries;
using Accounting.Infrastructure.Endpoints.RegulatoryReports;
using Accounting.Infrastructure.Endpoints.RetainedEarnings;
using Accounting.Infrastructure.Endpoints.TaxCodes;
using Accounting.Infrastructure.Endpoints.TrialBalance;
using Accounting.Infrastructure.Endpoints.Vendors;
using Accounting.Infrastructure.Endpoints.WriteOffs;
using Accounting.Infrastructure.Import;
using Accounting.Infrastructure.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using Mapster;
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
    /// All endpoints are auto-discovered by Carter via ICarterModule implementations.
    /// This method is kept for backward compatibility but can be empty.
    /// </summary>
    /// <param name="app">The endpoint route builder to configure.</param>
    /// <returns>The configured endpoint route builder.</returns>
    public static IEndpointRouteBuilder MapAccountingEndpoints(this IEndpointRouteBuilder app)
    {
        // All accounting endpoints are now auto-discovered by Carter via ICarterModule implementations.
        // No manual endpoint mapping is required.
        // Individual endpoint classes implement ICarterModule and are automatically registered.
        return app;
    }

    /// <summary>
    /// Registers accounting services with dependency injection container.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The configured web application builder.</returns>
    /// <remarks>
    /// This method registers all accounting repositories and services in a highly organized structure:
    /// 
    /// 1. CORE SERVICES - DbContext, initializers, business services, and import parsers
    /// 
    /// 2. NON-KEYED REPOSITORIES - Standard DI registrations (47 entities alphabetically ordered)
    ///    Used by MediatR handlers that don't use [FromKeyedServices] attribute
    /// 
    /// 3. KEYED REPOSITORIES - Keyed DI registrations with various keys:
    ///    - "accounting" - Generic key for basic handlers  
    ///    - "accounting:{entity}" - Specific keys for specialized handlers (e.g., "accounting:invoices")
    ///    
    /// KEYED SERVICE PATTERNS:
    /// Each entity typically has 2-4 registrations:
    /// - Generic: "accounting" 
    /// - Specific: "accounting:{pluralentity}" (e.g., "accounting:bills", "accounting:payments")
    /// - Some entities have additional variant keys for legacy handler compatibility
    /// 
    /// Total: ~300+ repository registrations supporting 47 accounting entities
    /// </remarks>
    public static WebApplicationBuilder RegisterAccountingServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        
        // ============================================================================
        // CORE SERVICES
        // ============================================================================
        builder.Services.BindDbContext<AccountingDbContext>();
        builder.Services.AddScoped<IDbInitializer, AccountingDbInitializer>();
        builder.Services.AddScoped<Application.Billing.IBillingService, Application.Billing.BillingService>();
        builder.Services.AddScoped<IChartOfAccountImportParser, ChartOfAccountImportParser>();
        builder.Services.AddScoped<IPayeeImportParser, PayeeImportParser>();
    
        // ============================================================================
        // NON-KEYED REPOSITORY REGISTRATIONS (for MediatR handlers without keyed services)
        // Organized alphabetically for easy lookup
        // ============================================================================
        builder.Services.AddScoped<IRepository<Accrual>, AccountingRepository<Accrual>>();
        builder.Services.AddScoped<IReadRepository<Accrual>, AccountingRepository<Accrual>>();
        builder.Services.AddScoped<IRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>();
        builder.Services.AddScoped<IReadRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>();
        builder.Services.AddScoped<IRepository<AccountsPayableAccount>, AccountingRepository<AccountsPayableAccount>>();
        builder.Services.AddScoped<IReadRepository<AccountsPayableAccount>, AccountingRepository<AccountsPayableAccount>>();
        builder.Services.AddScoped<IRepository<AccountsReceivableAccount>, AccountingRepository<AccountsReceivableAccount>>();
        builder.Services.AddScoped<IReadRepository<AccountsReceivableAccount>, AccountingRepository<AccountsReceivableAccount>>();
        builder.Services.AddScoped<IRepository<AccountReconciliation>, AccountingRepository<AccountReconciliation>>();
        builder.Services.AddScoped<IReadRepository<AccountReconciliation>, AccountingRepository<AccountReconciliation>>();
        builder.Services.AddScoped<IRepository<Bank>, AccountingRepository<Bank>>();
        builder.Services.AddScoped<IReadRepository<Bank>, AccountingRepository<Bank>>();
        builder.Services.AddScoped<IRepository<BankReconciliation>, AccountingRepository<BankReconciliation>>();
        builder.Services.AddScoped<IReadRepository<BankReconciliation>, AccountingRepository<BankReconciliation>>();
        builder.Services.AddScoped<IRepository<Bill>, AccountingRepository<Bill>>();
        builder.Services.AddScoped<IReadRepository<Bill>, AccountingRepository<Bill>>();
        builder.Services.AddScoped<IRepository<BillLineItem>, AccountingRepository<BillLineItem>>();
        builder.Services.AddScoped<IReadRepository<BillLineItem>, AccountingRepository<BillLineItem>>();
        builder.Services.AddScoped<IRepository<Budget>, AccountingRepository<Budget>>();
        builder.Services.AddScoped<IReadRepository<Budget>, AccountingRepository<Budget>>();
        builder.Services.AddScoped<IRepository<BudgetDetail>, AccountingRepository<BudgetDetail>>();
        builder.Services.AddScoped<IReadRepository<BudgetDetail>, AccountingRepository<BudgetDetail>>();
        builder.Services.AddScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>();
        builder.Services.AddScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>();
        builder.Services.AddScoped<IRepository<Check>, AccountingRepository<Check>>();
        builder.Services.AddScoped<IReadRepository<Check>, AccountingRepository<Check>>();
        builder.Services.AddScoped<IRepository<Consumption>, AccountingRepository<Consumption>>();
        builder.Services.AddScoped<IReadRepository<Consumption>, AccountingRepository<Consumption>>();
        builder.Services.AddScoped<IRepository<CostCenter>, AccountingRepository<CostCenter>>();
        builder.Services.AddScoped<IReadRepository<CostCenter>, AccountingRepository<CostCenter>>();
        builder.Services.AddScoped<IRepository<CreditMemo>, AccountingRepository<CreditMemo>>();
        builder.Services.AddScoped<IReadRepository<CreditMemo>, AccountingRepository<CreditMemo>>();
        builder.Services.AddScoped<IRepository<Customer>, AccountingRepository<Customer>>();
        builder.Services.AddScoped<IReadRepository<Customer>, AccountingRepository<Customer>>();
        builder.Services.AddScoped<IRepository<DebitMemo>, AccountingRepository<DebitMemo>>();
        builder.Services.AddScoped<IReadRepository<DebitMemo>, AccountingRepository<DebitMemo>>();
        builder.Services.AddScoped<IRepository<DeferredRevenue>, AccountingRepository<DeferredRevenue>>();
        builder.Services.AddScoped<IReadRepository<DeferredRevenue>, AccountingRepository<DeferredRevenue>>();
        builder.Services.AddScoped<IRepository<DepreciationMethod>, AccountingRepository<DepreciationMethod>>();
        builder.Services.AddScoped<IReadRepository<DepreciationMethod>, AccountingRepository<DepreciationMethod>>();
        builder.Services.AddScoped<IRepository<FiscalPeriodClose>, AccountingRepository<FiscalPeriodClose>>();
        builder.Services.AddScoped<IReadRepository<FiscalPeriodClose>, AccountingRepository<FiscalPeriodClose>>();
        builder.Services.AddScoped<IRepository<FixedAsset>, AccountingRepository<FixedAsset>>();
        builder.Services.AddScoped<IReadRepository<FixedAsset>, AccountingRepository<FixedAsset>>();
        builder.Services.AddScoped<IRepository<GeneralLedger>, AccountingRepository<GeneralLedger>>();
        builder.Services.AddScoped<IReadRepository<GeneralLedger>, AccountingRepository<GeneralLedger>>();
        builder.Services.AddScoped<IRepository<InterCompanyTransaction>, AccountingRepository<InterCompanyTransaction>>();
        builder.Services.AddScoped<IReadRepository<InterCompanyTransaction>, AccountingRepository<InterCompanyTransaction>>();
        builder.Services.AddScoped<IRepository<InventoryItem>, AccountingRepository<InventoryItem>>();
        builder.Services.AddScoped<IReadRepository<InventoryItem>, AccountingRepository<InventoryItem>>();
        builder.Services.AddScoped<IRepository<Invoice>, AccountingRepository<Invoice>>();
        builder.Services.AddScoped<IReadRepository<Invoice>, AccountingRepository<Invoice>>();
        builder.Services.AddScoped<IRepository<InvoiceLineItem>, AccountingRepository<InvoiceLineItem>>();
        builder.Services.AddScoped<IReadRepository<InvoiceLineItem>, AccountingRepository<InvoiceLineItem>>();
        builder.Services.AddScoped<IRepository<JournalEntry>, AccountingRepository<JournalEntry>>();
        builder.Services.AddScoped<IReadRepository<JournalEntry>, AccountingRepository<JournalEntry>>();
        builder.Services.AddScoped<IRepository<JournalEntryLine>, AccountingRepository<JournalEntryLine>>();
        builder.Services.AddScoped<IReadRepository<JournalEntryLine>, AccountingRepository<JournalEntryLine>>();
        builder.Services.AddScoped<IRepository<Member>, AccountingRepository<Member>>();
        builder.Services.AddScoped<IReadRepository<Member>, AccountingRepository<Member>>();
        builder.Services.AddScoped<IRepository<Meter>, AccountingRepository<Meter>>();
        builder.Services.AddScoped<IReadRepository<Meter>, AccountingRepository<Meter>>();
        builder.Services.AddScoped<IRepository<PatronageCapital>, AccountingRepository<PatronageCapital>>();
        builder.Services.AddScoped<IReadRepository<PatronageCapital>, AccountingRepository<PatronageCapital>>();
        builder.Services.AddScoped<IRepository<Payee>, AccountingRepository<Payee>>();
        builder.Services.AddScoped<IReadRepository<Payee>, AccountingRepository<Payee>>();
        builder.Services.AddScoped<IRepository<Payment>, AccountingRepository<Payment>>();
        builder.Services.AddScoped<IReadRepository<Payment>, AccountingRepository<Payment>>();
        builder.Services.AddScoped<IRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>();
        builder.Services.AddScoped<IReadRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>();
        builder.Services.AddScoped<IRepository<PostingBatch>, AccountingRepository<PostingBatch>>();
        builder.Services.AddScoped<IReadRepository<PostingBatch>, AccountingRepository<PostingBatch>>();
        builder.Services.AddScoped<IRepository<PrepaidExpense>, AccountingRepository<PrepaidExpense>>();
        builder.Services.AddScoped<IReadRepository<PrepaidExpense>, AccountingRepository<PrepaidExpense>>();
        builder.Services.AddScoped<IRepository<Project>, AccountingRepository<Project>>();
        builder.Services.AddScoped<IReadRepository<Project>, AccountingRepository<Project>>();
        builder.Services.AddScoped<IRepository<ProjectCostEntry>, AccountingRepository<ProjectCostEntry>>();
        builder.Services.AddScoped<IReadRepository<ProjectCostEntry>, AccountingRepository<ProjectCostEntry>>();
        builder.Services.AddScoped<IRepository<RateSchedule>, AccountingRepository<RateSchedule>>();
        builder.Services.AddScoped<IReadRepository<RateSchedule>, AccountingRepository<RateSchedule>>();
        builder.Services.AddScoped<IRepository<RecurringJournalEntry>, AccountingRepository<RecurringJournalEntry>>();
        builder.Services.AddScoped<IReadRepository<RecurringJournalEntry>, AccountingRepository<RecurringJournalEntry>>();
        builder.Services.AddScoped<IRepository<RegulatoryReport>, AccountingRepository<RegulatoryReport>>();
        builder.Services.AddScoped<IReadRepository<RegulatoryReport>, AccountingRepository<RegulatoryReport>>();
        builder.Services.AddScoped<IRepository<RetainedEarnings>, AccountingRepository<RetainedEarnings>>();
        builder.Services.AddScoped<IReadRepository<RetainedEarnings>, AccountingRepository<RetainedEarnings>>();
        builder.Services.AddScoped<IRepository<SecurityDeposit>, AccountingRepository<SecurityDeposit>>();
        builder.Services.AddScoped<IReadRepository<SecurityDeposit>, AccountingRepository<SecurityDeposit>>();
        builder.Services.AddScoped<IRepository<TaxCode>, AccountingRepository<TaxCode>>();
        builder.Services.AddScoped<IReadRepository<TaxCode>, AccountingRepository<TaxCode>>();
        builder.Services.AddScoped<IRepository<TrialBalance>, AccountingRepository<TrialBalance>>();
        builder.Services.AddScoped<IReadRepository<TrialBalance>, AccountingRepository<TrialBalance>>();
        builder.Services.AddScoped<IRepository<Vendor>, AccountingRepository<Vendor>>();
        builder.Services.AddScoped<IReadRepository<Vendor>, AccountingRepository<Vendor>>();
        builder.Services.AddScoped<IRepository<WriteOff>, AccountingRepository<WriteOff>>();
        builder.Services.AddScoped<IReadRepository<WriteOff>, AccountingRepository<WriteOff>>();
        
        // ============================================================================
        // KEYED REPOSITORY REGISTRATIONS - GENERIC "accounting" KEY
        // For handlers that use [FromKeyedServices("accounting")]
        // Organized alphabetically for easy lookup
        // ============================================================================
        builder.Services.AddKeyedScoped<IRepository<Accrual>, AccountingRepository<Accrual>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Accrual>, AccountingRepository<Accrual>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<AccountsPayableAccount>, AccountingRepository<AccountsPayableAccount>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<AccountsPayableAccount>, AccountingRepository<AccountsPayableAccount>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<AccountsReceivableAccount>, AccountingRepository<AccountsReceivableAccount>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<AccountsReceivableAccount>, AccountingRepository<AccountsReceivableAccount>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<AccountReconciliation>, AccountingRepository<AccountReconciliation>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<AccountReconciliation>, AccountingRepository<AccountReconciliation>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Bank>, AccountingRepository<Bank>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Bank>, AccountingRepository<Bank>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<BankReconciliation>, AccountingRepository<BankReconciliation>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<BankReconciliation>, AccountingRepository<BankReconciliation>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Bill>, AccountingRepository<Bill>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Bill>, AccountingRepository<Bill>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<BillLineItem>, AccountingRepository<BillLineItem>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<BillLineItem>, AccountingRepository<BillLineItem>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Budget>, AccountingRepository<Budget>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Budget>, AccountingRepository<Budget>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<BudgetDetail>, AccountingRepository<BudgetDetail>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<BudgetDetail>, AccountingRepository<BudgetDetail>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Check>, AccountingRepository<Check>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Check>, AccountingRepository<Check>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Consumption>, AccountingRepository<Consumption>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Consumption>, AccountingRepository<Consumption>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<CostCenter>, AccountingRepository<CostCenter>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<CostCenter>, AccountingRepository<CostCenter>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<CreditMemo>, AccountingRepository<CreditMemo>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<CreditMemo>, AccountingRepository<CreditMemo>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Customer>, AccountingRepository<Customer>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Customer>, AccountingRepository<Customer>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<DebitMemo>, AccountingRepository<DebitMemo>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<DebitMemo>, AccountingRepository<DebitMemo>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<DeferredRevenue>, AccountingRepository<DeferredRevenue>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<DeferredRevenue>, AccountingRepository<DeferredRevenue>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<DepreciationMethod>, AccountingRepository<DepreciationMethod>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<DepreciationMethod>, AccountingRepository<DepreciationMethod>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<FiscalPeriodClose>, AccountingRepository<FiscalPeriodClose>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<FiscalPeriodClose>, AccountingRepository<FiscalPeriodClose>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<FixedAsset>, AccountingRepository<FixedAsset>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<FixedAsset>, AccountingRepository<FixedAsset>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<GeneralLedger>, AccountingRepository<GeneralLedger>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<GeneralLedger>, AccountingRepository<GeneralLedger>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<InterCompanyTransaction>, AccountingRepository<InterCompanyTransaction>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<InterCompanyTransaction>, AccountingRepository<InterCompanyTransaction>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<InventoryItem>, AccountingRepository<InventoryItem>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryItem>, AccountingRepository<InventoryItem>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Invoice>, AccountingRepository<Invoice>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Invoice>, AccountingRepository<Invoice>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<InvoiceLineItem>, AccountingRepository<InvoiceLineItem>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<InvoiceLineItem>, AccountingRepository<InvoiceLineItem>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<JournalEntry>, AccountingRepository<JournalEntry>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<JournalEntry>, AccountingRepository<JournalEntry>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<JournalEntryLine>, AccountingRepository<JournalEntryLine>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<JournalEntryLine>, AccountingRepository<JournalEntryLine>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Member>, AccountingRepository<Member>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Member>, AccountingRepository<Member>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Meter>, AccountingRepository<Meter>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Meter>, AccountingRepository<Meter>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Payee>, AccountingRepository<Payee>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Payee>, AccountingRepository<Payee>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<PrepaidExpense>, AccountingRepository<PrepaidExpense>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<PrepaidExpense>, AccountingRepository<PrepaidExpense>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<PostingBatch>, AccountingRepository<PostingBatch>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<PostingBatch>, AccountingRepository<PostingBatch>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Project>, AccountingRepository<Project>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Project>, AccountingRepository<Project>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<RecurringJournalEntry>, AccountingRepository<RecurringJournalEntry>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<RecurringJournalEntry>, AccountingRepository<RecurringJournalEntry>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<RetainedEarnings>, AccountingRepository<RetainedEarnings>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<RetainedEarnings>, AccountingRepository<RetainedEarnings>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<TrialBalance>, AccountingRepository<TrialBalance>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<TrialBalance>, AccountingRepository<TrialBalance>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<TaxCode>, AccountingRepository<TaxCode>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<TaxCode>, AccountingRepository<TaxCode>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<Vendor>, AccountingRepository<Vendor>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<Vendor>, AccountingRepository<Vendor>>("accounting");
        builder.Services.AddKeyedScoped<IRepository<WriteOff>, AccountingRepository<WriteOff>>("accounting");
        builder.Services.AddKeyedScoped<IReadRepository<WriteOff>, AccountingRepository<WriteOff>>("accounting");

        // ============================================================================
        // KEYED REPOSITORY REGISTRATIONS - SPECIFIC "accounting:{entity}" KEYS
        // For handlers that use specific keyed services
        // Organized alphabetically by entity name
        // ============================================================================
        builder.Services.AddKeyedScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:accounts");
        builder.Services.AddKeyedScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:accounts");
        builder.Services.AddKeyedScoped<IRepository<Accrual>, AccountingRepository<Accrual>>("accounting:accruals");
        builder.Services.AddKeyedScoped<IReadRepository<Accrual>, AccountingRepository<Accrual>>("accounting:accruals");
        builder.Services.AddKeyedScoped<IRepository<AccountsPayableAccount>, AccountingRepository<AccountsPayableAccount>>("accounting:apaccounts");
        builder.Services.AddKeyedScoped<IReadRepository<AccountsPayableAccount>, AccountingRepository<AccountsPayableAccount>>("accounting:apaccounts");
        builder.Services.AddKeyedScoped<IRepository<AccountsReceivableAccount>, AccountingRepository<AccountsReceivableAccount>>("accounting:araccounts");
        builder.Services.AddKeyedScoped<IReadRepository<AccountsReceivableAccount>, AccountingRepository<AccountsReceivableAccount>>("accounting:araccounts");
        builder.Services.AddKeyedScoped<IRepository<AccountReconciliation>, AccountingRepository<AccountReconciliation>>("accounting:account-reconciliations");
        builder.Services.AddKeyedScoped<IReadRepository<AccountReconciliation>, AccountingRepository<AccountReconciliation>>("accounting:account-reconciliations");
        builder.Services.AddKeyedScoped<IRepository<Bank>, AccountingRepository<Bank>>("accounting:banks");
        builder.Services.AddKeyedScoped<IReadRepository<Bank>, AccountingRepository<Bank>>("accounting:banks");
        builder.Services.AddKeyedScoped<IRepository<BillLineItem>, AccountingRepository<BillLineItem>>("accounting:bill-line-items");
        builder.Services.AddKeyedScoped<IReadRepository<BillLineItem>, AccountingRepository<BillLineItem>>("accounting:bill-line-items");
        builder.Services.AddKeyedScoped<IRepository<Bill>, AccountingRepository<Bill>>("accounting:bills");
        builder.Services.AddKeyedScoped<IReadRepository<Bill>, AccountingRepository<Bill>>("accounting:bills");
        builder.Services.AddKeyedScoped<IRepository<BudgetDetail>, AccountingRepository<BudgetDetail>>("accounting:budget-details");
        builder.Services.AddKeyedScoped<IReadRepository<BudgetDetail>, AccountingRepository<BudgetDetail>>("accounting:budget-details");
        builder.Services.AddKeyedScoped<IRepository<Budget>, AccountingRepository<Budget>>("accounting:budgets");
        builder.Services.AddKeyedScoped<IReadRepository<Budget>, AccountingRepository<Budget>>("accounting:budgets");
        builder.Services.AddKeyedScoped<IRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:chart-of-accounts");
        builder.Services.AddKeyedScoped<IReadRepository<ChartOfAccount>, AccountingRepository<ChartOfAccount>>("accounting:chart-of-accounts");
        builder.Services.AddKeyedScoped<IReadRepository<Check>, AccountingRepository<Check>>("accounting:checks");
        builder.Services.AddKeyedScoped<IRepository<Consumption>, AccountingRepository<Consumption>>("accounting:consumption");
        builder.Services.AddKeyedScoped<IReadRepository<Consumption>, AccountingRepository<Consumption>>("accounting:consumption");
        builder.Services.AddKeyedScoped<IRepository<Consumption>, AccountingRepository<Consumption>>("accounting:consumptions");
        builder.Services.AddKeyedScoped<IReadRepository<Consumption>, AccountingRepository<Consumption>>("accounting:consumptions");
        builder.Services.AddKeyedScoped<IRepository<CostCenter>, AccountingRepository<CostCenter>>("accounting:costcenters");
        builder.Services.AddKeyedScoped<IReadRepository<CostCenter>, AccountingRepository<CostCenter>>("accounting:costcenters");
        builder.Services.AddKeyedScoped<IRepository<CostCenter>, AccountingRepository<CostCenter>>("accounting:costCenters");
        builder.Services.AddKeyedScoped<IReadRepository<CostCenter>, AccountingRepository<CostCenter>>("accounting:costCenters");
        builder.Services.AddKeyedScoped<IRepository<CreditMemo>, AccountingRepository<CreditMemo>>("accounting:creditmemos");
        builder.Services.AddKeyedScoped<IReadRepository<CreditMemo>, AccountingRepository<CreditMemo>>("accounting:creditmemos");
        builder.Services.AddKeyedScoped<IRepository<Customer>, AccountingRepository<Customer>>("accounting:customers");
        builder.Services.AddKeyedScoped<IReadRepository<Customer>, AccountingRepository<Customer>>("accounting:customers");
        builder.Services.AddKeyedScoped<IRepository<DebitMemo>, AccountingRepository<DebitMemo>>("accounting:debitmemos");
        builder.Services.AddKeyedScoped<IReadRepository<DebitMemo>, AccountingRepository<DebitMemo>>("accounting:debitmemos");
        builder.Services.AddKeyedScoped<IRepository<DeferredRevenue>, AccountingRepository<DeferredRevenue>>("accounting:deferred-revenues");
        builder.Services.AddKeyedScoped<IReadRepository<DeferredRevenue>, AccountingRepository<DeferredRevenue>>("accounting:deferred-revenues");
        builder.Services.AddKeyedScoped<IRepository<DepreciationMethod>, AccountingRepository<DepreciationMethod>>("accounting:depreciation-methods");
        builder.Services.AddKeyedScoped<IReadRepository<DepreciationMethod>, AccountingRepository<DepreciationMethod>>("accounting:depreciation-methods");
        builder.Services.AddKeyedScoped<IRepository<FiscalPeriodClose>, AccountingRepository<FiscalPeriodClose>>("accounting:fiscal-period-closes");
        builder.Services.AddKeyedScoped<IReadRepository<FiscalPeriodClose>, AccountingRepository<FiscalPeriodClose>>("accounting:fiscal-period-closes");
        builder.Services.AddKeyedScoped<IRepository<FixedAsset>, AccountingRepository<FixedAsset>>("accounting:fixed-assets");
        builder.Services.AddKeyedScoped<IReadRepository<FixedAsset>, AccountingRepository<FixedAsset>>("accounting:fixed-assets");
        builder.Services.AddKeyedScoped<IRepository<GeneralLedger>, AccountingRepository<GeneralLedger>>("accounting:general-ledger");
        builder.Services.AddKeyedScoped<IReadRepository<GeneralLedger>, AccountingRepository<GeneralLedger>>("accounting:general-ledger");
        builder.Services.AddKeyedScoped<IRepository<InterCompanyTransaction>, AccountingRepository<InterCompanyTransaction>>("accounting:inter-company-transactions");
        builder.Services.AddKeyedScoped<IReadRepository<InterCompanyTransaction>, AccountingRepository<InterCompanyTransaction>>("accounting:inter-company-transactions");
        builder.Services.AddKeyedScoped<IRepository<InventoryItem>, AccountingRepository<InventoryItem>>("accounting:inventory");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryItem>, AccountingRepository<InventoryItem>>("accounting:inventory");
        builder.Services.AddKeyedScoped<IRepository<InventoryItem>, AccountingRepository<InventoryItem>>("accounting:inventory-items");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryItem>, AccountingRepository<InventoryItem>>("accounting:inventory-items");
        builder.Services.AddKeyedScoped<IRepository<InvoiceLineItem>, AccountingRepository<InvoiceLineItem>>("accounting:invoice-line-items");
        builder.Services.AddKeyedScoped<IReadRepository<InvoiceLineItem>, AccountingRepository<InvoiceLineItem>>("accounting:invoice-line-items");
        builder.Services.AddKeyedScoped<IRepository<Invoice>, AccountingRepository<Invoice>>("accounting:invoices");
        builder.Services.AddKeyedScoped<IReadRepository<Invoice>, AccountingRepository<Invoice>>("accounting:invoices");
        builder.Services.AddKeyedScoped<IRepository<JournalEntryLine>, AccountingRepository<JournalEntryLine>>("accounting:journal-lines");
        builder.Services.AddKeyedScoped<IReadRepository<JournalEntryLine>, AccountingRepository<JournalEntryLine>>("accounting:journal-lines");
        builder.Services.AddKeyedScoped<IRepository<JournalEntry>, AccountingRepository<JournalEntry>>("accounting:journal-entries");
        builder.Services.AddKeyedScoped<IReadRepository<JournalEntry>, AccountingRepository<JournalEntry>>("accounting:journal-entries");
        builder.Services.AddKeyedScoped<IRepository<JournalEntry>, AccountingRepository<JournalEntry>>("accounting:journals");
        builder.Services.AddKeyedScoped<IReadRepository<JournalEntry>, AccountingRepository<JournalEntry>>("accounting:journals");
        builder.Services.AddKeyedScoped<IRepository<Member>, AccountingRepository<Member>>("accounting:members");
        builder.Services.AddKeyedScoped<IReadRepository<Member>, AccountingRepository<Member>>("accounting:members");
        builder.Services.AddKeyedScoped<IRepository<Meter>, AccountingRepository<Meter>>("accounting:meters");
        builder.Services.AddKeyedScoped<IReadRepository<Meter>, AccountingRepository<Meter>>("accounting:meters");
        builder.Services.AddKeyedScoped<IRepository<PatronageCapital>, AccountingRepository<PatronageCapital>>("accounting:patronage-capitals");
        builder.Services.AddKeyedScoped<IReadRepository<PatronageCapital>, AccountingRepository<PatronageCapital>>("accounting:patronage-capitals");
        builder.Services.AddKeyedScoped<IRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>("accounting:payment-allocations");
        builder.Services.AddKeyedScoped<IReadRepository<PaymentAllocation>, AccountingRepository<PaymentAllocation>>("accounting:payment-allocations");
        builder.Services.AddKeyedScoped<IRepository<Payment>, AccountingRepository<Payment>>("accounting:payments");
        builder.Services.AddKeyedScoped<IReadRepository<Payment>, AccountingRepository<Payment>>("accounting:payments");
        builder.Services.AddKeyedScoped<IRepository<Payee>, AccountingRepository<Payee>>("accounting:payees");
        builder.Services.AddKeyedScoped<IReadRepository<Payee>, AccountingRepository<Payee>>("accounting:payees");
        builder.Services.AddKeyedScoped<IRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>("accounting:periods");
        builder.Services.AddKeyedScoped<IReadRepository<AccountingPeriod>, AccountingRepository<AccountingPeriod>>("accounting:periods");
        builder.Services.AddKeyedScoped<IRepository<PostingBatch>, AccountingRepository<PostingBatch>>("accounting:posting-batches");
        builder.Services.AddKeyedScoped<IReadRepository<PostingBatch>, AccountingRepository<PostingBatch>>("accounting:posting-batches");
        builder.Services.AddKeyedScoped<IRepository<PostingBatch>, AccountingRepository<PostingBatch>>("accounting:posting-batches");
        builder.Services.AddKeyedScoped<IReadRepository<PostingBatch>, AccountingRepository<PostingBatch>>("accounting:posting-batches");
        builder.Services.AddKeyedScoped<IRepository<PrepaidExpense>, AccountingRepository<PrepaidExpense>>("accounting:prepaid-expenses");
        builder.Services.AddKeyedScoped<IReadRepository<PrepaidExpense>, AccountingRepository<PrepaidExpense>>("accounting:prepaid-expenses");
        builder.Services.AddKeyedScoped<IRepository<ProjectCostEntry>, AccountingRepository<ProjectCostEntry>>("accounting:project-costing");
        builder.Services.AddKeyedScoped<IReadRepository<ProjectCostEntry>, AccountingRepository<ProjectCostEntry>>("accounting:project-costing");
        builder.Services.AddKeyedScoped<IRepository<ProjectCostEntry>, AccountingRepository<ProjectCostEntry>>("accounting:project-costs");
        builder.Services.AddKeyedScoped<IReadRepository<ProjectCostEntry>, AccountingRepository<ProjectCostEntry>>("accounting:project-costs");
        builder.Services.AddKeyedScoped<IRepository<Project>, AccountingRepository<Project>>("accounting:projects");
        builder.Services.AddKeyedScoped<IReadRepository<Project>, AccountingRepository<Project>>("accounting:projects");
        builder.Services.AddKeyedScoped<IRepository<RateSchedule>, AccountingRepository<RateSchedule>>("accounting:rateschedules");
        builder.Services.AddKeyedScoped<IReadRepository<RateSchedule>, AccountingRepository<RateSchedule>>("accounting:rateschedules");
        builder.Services.AddKeyedScoped<IRepository<RegulatoryReport>, AccountingRepository<RegulatoryReport>>("accounting:regulatory-reports");
        builder.Services.AddKeyedScoped<IReadRepository<RegulatoryReport>, AccountingRepository<RegulatoryReport>>("accounting:regulatory-reports");
        builder.Services.AddKeyedScoped<IRepository<RetainedEarnings>, AccountingRepository<RetainedEarnings>>("accounting:retained-earnings");
        builder.Services.AddKeyedScoped<IReadRepository<RetainedEarnings>, AccountingRepository<RetainedEarnings>>("accounting:retained-earnings");
        builder.Services.AddKeyedScoped<IRepository<TrialBalance>, AccountingRepository<TrialBalance>>("accounting:trial-balances");
        builder.Services.AddKeyedScoped<IReadRepository<TrialBalance>, AccountingRepository<TrialBalance>>("accounting:trial-balances");
        builder.Services.AddKeyedScoped<IRepository<TrialBalance>, AccountingRepository<TrialBalance>>("accounting:trial-balance");
        builder.Services.AddKeyedScoped<IReadRepository<TrialBalance>, AccountingRepository<TrialBalance>>("accounting:trial-balance");
        builder.Services.AddKeyedScoped<IRepository<TaxCode>, AccountingRepository<TaxCode>>("accounting:tax-codes");
        builder.Services.AddKeyedScoped<IReadRepository<TaxCode>, AccountingRepository<TaxCode>>("accounting:tax-codes");
        builder.Services.AddKeyedScoped<IRepository<BankReconciliation>, AccountingRepository<BankReconciliation>>("accounting:bank-reconciliations");
        builder.Services.AddKeyedScoped<IReadRepository<BankReconciliation>, AccountingRepository<BankReconciliation>>("accounting:bank-reconciliations");
        builder.Services.AddKeyedScoped<IRepository<RecurringJournalEntry>, AccountingRepository<RecurringJournalEntry>>("accounting:recurring-journal-entries");
        builder.Services.AddKeyedScoped<IReadRepository<RecurringJournalEntry>, AccountingRepository<RecurringJournalEntry>>("accounting:recurring-journal-entries");
        builder.Services.AddKeyedScoped<IRepository<Vendor>, AccountingRepository<Vendor>>("accounting:vendors");
        builder.Services.AddKeyedScoped<IReadRepository<Vendor>, AccountingRepository<Vendor>>("accounting:vendors");
        builder.Services.AddKeyedScoped<IRepository<WriteOff>, AccountingRepository<WriteOff>>("accounting:write-offs");
        builder.Services.AddKeyedScoped<IReadRepository<WriteOff>, AccountingRepository<WriteOff>>("accounting:write-offs");
        
        // Register Mapster mappings
        TypeAdapterConfig.GlobalSettings.Scan(typeof(AccountingMetadata).Assembly);
        
        return builder;
    }

    public static WebApplication UseAccountingModule(this WebApplication app)
    {
        return app;
    }
}
