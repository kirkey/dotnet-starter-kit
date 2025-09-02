using Accounting.Domain;
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Infrastructure.Persistence;
using FSH.Framework.Infrastructure.Tenant;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Constants;

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
    public DbSet<ChartOfAccount> ChartOfAccounts { get; set; } = null!;
    public DbSet<ConsumptionData> ConsumptionData { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<DepreciationMethod> DepreciationMethods { get; set; } = null!;
    public DbSet<FixedAsset> FixedAssets { get; set; } = null!;
    public DbSet<FuelConsumption> FuelConsumptions { get; set; } = null!;
    public DbSet<GeneralLedger> GeneralLedgers { get; set; } = null!;
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<JournalEntry> JournalEntries { get; set; } = null!;
    public DbSet<Member> Members { get; set; } = null!;
    public DbSet<Meter> Meters { get; set; } = null!;
    public DbSet<Payee> Payees { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Vendor> Vendors { get; set; } = null!;
    public DbSet<Accrual> Accruals { get; set; } = null!;
    public DbSet<DeferredRevenue> DeferredRevenues { get; set; } = null!;
    public DbSet<PostingBatch> PostingBatches { get; set; } = null!;
    public DbSet<RegulatoryReport> RegulatoryReports { get; set; } = null!;

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
        configurationBuilder.Properties<double>().HavePrecision(8, 2);
        // configurationBuilder.Properties<string>().HaveMaxLength(8192);
    }
}
