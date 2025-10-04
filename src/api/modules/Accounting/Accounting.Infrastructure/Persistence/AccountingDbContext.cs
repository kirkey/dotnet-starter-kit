using Accounting.Domain.Entities;
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
    public DbSet<AccountingPeriod> AccountingPeriods { get; set; } = null!;
    public DbSet<Budget> Budgets { get; set; } = null!;
    public DbSet<BudgetDetail> BudgetDetails { get; set; } = null!; // Re-added: BudgetDetail is now a regular entity (HasMany relationship)
    public DbSet<ChartOfAccount> ChartOfAccounts { get; set; } = null!;
    public DbSet<Consumption> Consumption { get; set; } = null!;
    public DbSet<DepreciationMethod> DepreciationMethods { get; set; } = null!;
    public DbSet<FixedAsset> FixedAssets { get; set; } = null!;
    public DbSet<GeneralLedger> GeneralLedgers { get; set; } = null!;
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<JournalEntry> JournalEntries { get; set; } = null!;
    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<Meter> Meters { get; set; } = null!;
    public DbSet<Payee> Payees { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectCostEntry> ProjectCostEntries { get; set; } = null!;
    public DbSet<Vendor> Vendors { get; set; } = null!;
    public DbSet<Accrual> Accruals { get; set; } = null!;
    public DbSet<DeferredRevenue> DeferredRevenues { get; set; } = null!;
    public DbSet<PostingBatch> PostingBatches { get; set; } = null!;
    public DbSet<RegulatoryReport> RegulatoryReports { get; set; } = null!;

    // Added missing DbSets for other accounting domains
    public DbSet<InventoryItem> InventoryItems { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<PaymentAllocation> PaymentAllocations { get; set; } = null!;
    public DbSet<PatronageCapital> PatronageCapitals { get; set; } = null!;
    public DbSet<RateSchedule> RateSchedules { get; set; } = null!;
    public DbSet<SecurityDeposit> SecurityDeposits { get; set; } = null!;
    public DbSet<DebitMemo> DebitMemos { get; set; } = null!;
    public DbSet<CreditMemo> CreditMemos { get; set; } = null!;

    // Advanced accounting entities
    public DbSet<BankReconciliation> BankReconciliations { get; set; } = null!;
    public DbSet<RecurringJournalEntry> RecurringJournalEntries { get; set; } = null!;
    public DbSet<TaxCode> TaxCodes { get; set; } = null!;
    public DbSet<CostCenter> CostCenters { get; set; } = null!;
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
    public DbSet<WriteOff> WriteOffs { get; set; } = null!;

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
