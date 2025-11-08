using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Framework.Infrastructure.Tenant;
using Microsoft.Extensions.Options;

namespace Accounting.Infrastructure.Persistence;

public sealed class AccountingDbContext(
    IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor,
    DbContextOptions<AccountingDbContext> options,
    IPublisher publisher,
    IOptions<DatabaseOptions> settings) : FshDbContext(multiTenantContextAccessor,
    options,
    publisher,
    settings)
{
    // ============================================================================
    // CHART OF ACCOUNTS & GENERAL LEDGER
    // ============================================================================
    public DbSet<ChartOfAccount> ChartOfAccounts { get; set; } = null!;
    public DbSet<GeneralLedger> GeneralLedgers { get; set; } = null!;
    public DbSet<JournalEntry> JournalEntries { get; set; } = null!;
    public DbSet<JournalEntryLine> JournalEntryLines { get; set; } = null!;
    public DbSet<RecurringJournalEntry> RecurringJournalEntries { get; set; } = null!;

    // ============================================================================
    // ACCOUNTS RECEIVABLE
    // ============================================================================
    public DbSet<AccountsReceivableAccount> AccountsReceivableAccounts { get; set; } = null!;
    public DbSet<CreditMemo> CreditMemos { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<InvoiceLineItem> InvoiceLineItems { get; set; } = null!;
    public DbSet<WriteOff> WriteOffs { get; set; } = null!;

    // ============================================================================
    // ACCOUNTS PAYABLE
    // ============================================================================
    public DbSet<AccountsPayableAccount> AccountsPayableAccounts { get; set; } = null!;
    public DbSet<Bill> Bills { get; set; } = null!;
    public DbSet<BillLineItem> BillLineItems { get; set; } = null!;
    public DbSet<DebitMemo> DebitMemos { get; set; } = null!;
    public DbSet<Vendor> Vendors { get; set; } = null!;

    // ============================================================================
    // PAYMENTS & BANKING
    // ============================================================================
    public DbSet<Bank> Banks { get; set; } = null!;
    public DbSet<BankReconciliation> BankReconciliations { get; set; } = null!;
    public DbSet<Check> Checks { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<PaymentAllocation> PaymentAllocations { get; set; } = null!;

    // ============================================================================
    // FIXED ASSETS & DEPRECIATION
    // ============================================================================
    public DbSet<DepreciationMethod> DepreciationMethods { get; set; } = null!;
    public DbSet<FixedAsset> FixedAssets { get; set; } = null!;

    // ============================================================================
    // INVENTORY
    // ============================================================================
    public DbSet<InventoryItem> InventoryItems { get; set; } = null!;

    // ============================================================================
    // DEFERRALS & ACCRUALS
    // ============================================================================
    public DbSet<Accrual> Accruals { get; set; } = null!;
    public DbSet<DeferredRevenue> DeferredRevenues { get; set; } = null!;
    public DbSet<PrepaidExpense> PrepaidExpenses { get; set; } = null!;

    // ============================================================================
    // BUDGETING & COST CENTERS
    // ============================================================================
    public DbSet<Budget> Budgets { get; set; } = null!;
    public DbSet<BudgetDetail> BudgetDetails { get; set; } = null!;
    public DbSet<CostCenter> CostCenters { get; set; } = null!;

    // ============================================================================
    // PROJECTS & JOBS
    // ============================================================================
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectCostEntry> ProjectCostEntries { get; set; } = null!;

    // ============================================================================
    // PERIOD-END & REPORTING
    // ============================================================================
    public DbSet<AccountingPeriod> AccountingPeriods { get; set; } = null!;
    public DbSet<FiscalPeriodClose> FiscalPeriodCloses { get; set; } = null!;
    public DbSet<PostingBatch> PostingBatches { get; set; } = null!;
    public DbSet<RegulatoryReport> RegulatoryReports { get; set; } = null!;
    public DbSet<RetainedEarnings> RetainedEarnings { get; set; } = null!;
    public DbSet<TrialBalance> TrialBalances { get; set; } = null!;

    // ============================================================================
    // UTILITY-SPECIFIC
    // ============================================================================
    public DbSet<Consumption> Consumption { get; set; } = null!;
    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<Meter> Meters { get; set; } = null!;
    public DbSet<PatronageCapital> PatronageCapitals { get; set; } = null!;
    public DbSet<RateSchedule> RateSchedules { get; set; } = null!;

    // ============================================================================
    // OTHER ENTITIES
    // ============================================================================
    public DbSet<InterCompanyTransaction> InterCompanyTransactions { get; set; } = null!;
    public DbSet<Payee> Payees { get; set; } = null!;
    public DbSet<SecurityDeposit> SecurityDeposits { get; set; } = null!;
    public DbSet<TaxCode> TaxCodes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountingDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Accounting);
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<decimal>().HavePrecision(16, 2);
    }
}
