using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Infrastructure.Tenant;
using Microsoft.Extensions.Options;
using Shared.Constants;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence;

/// <summary>
/// Database context for the MicroFinance module.
/// Manages all microfinance-related entities including members, loans, savings, shares, and fees.
/// </summary>
public sealed class MicroFinanceDbContext(
    IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor,
    DbContextOptions<MicroFinanceDbContext> options,
    IPublisher publisher,
    IOptions<DatabaseOptions> settings) : FshDbContext(multiTenantContextAccessor, options, publisher, settings)
{
    // ============================================
    // Member Management
    // ============================================

    /// <summary>Gets or sets the Members DbSet.</summary>
    public DbSet<Member> Members { get; set; } = null!;

    /// <summary>Gets or sets the MemberGroups DbSet.</summary>
    public DbSet<MemberGroup> MemberGroups { get; set; } = null!;

    /// <summary>Gets or sets the GroupMemberships DbSet.</summary>
    public DbSet<GroupMembership> GroupMemberships { get; set; } = null!;

    // ============================================
    // Loan Management
    // ============================================

    /// <summary>Gets or sets the LoanProducts DbSet.</summary>
    public DbSet<LoanProduct> LoanProducts { get; set; } = null!;

    /// <summary>Gets or sets the Loans DbSet.</summary>
    public DbSet<Loan> Loans { get; set; } = null!;

    /// <summary>Gets or sets the LoanRepayments DbSet.</summary>
    public DbSet<LoanRepayment> LoanRepayments { get; set; } = null!;

    /// <summary>Gets or sets the LoanSchedules DbSet.</summary>
    public DbSet<LoanSchedule> LoanSchedules { get; set; } = null!;

    /// <summary>Gets or sets the LoanGuarantors DbSet.</summary>
    public DbSet<LoanGuarantor> LoanGuarantors { get; set; } = null!;

    /// <summary>Gets or sets the LoanCollaterals DbSet.</summary>
    public DbSet<LoanCollateral> LoanCollaterals { get; set; } = null!;

    // ============================================
    // Savings Management
    // ============================================

    /// <summary>Gets or sets the SavingsProducts DbSet.</summary>
    public DbSet<SavingsProduct> SavingsProducts { get; set; } = null!;

    /// <summary>Gets or sets the SavingsAccounts DbSet.</summary>
    public DbSet<SavingsAccount> SavingsAccounts { get; set; } = null!;

    /// <summary>Gets or sets the SavingsTransactions DbSet.</summary>
    public DbSet<SavingsTransaction> SavingsTransactions { get; set; } = null!;

    /// <summary>Gets or sets the FixedDeposits DbSet.</summary>
    public DbSet<FixedDeposit> FixedDeposits { get; set; } = null!;

    // ============================================
    // Share Management
    // ============================================

    /// <summary>Gets or sets the ShareProducts DbSet.</summary>
    public DbSet<ShareProduct> ShareProducts { get; set; } = null!;

    /// <summary>Gets or sets the ShareAccounts DbSet.</summary>
    public DbSet<ShareAccount> ShareAccounts { get; set; } = null!;

    /// <summary>Gets or sets the ShareTransactions DbSet.</summary>
    public DbSet<ShareTransaction> ShareTransactions { get; set; } = null!;

    // ============================================
    // Fee Management
    // ============================================

    /// <summary>Gets or sets the FeeDefinitions DbSet.</summary>
    public DbSet<FeeDefinition> FeeDefinitions { get; set; } = null!;

    /// <summary>Gets or sets the FeeCharges DbSet.</summary>
    public DbSet<FeeCharge> FeeCharges { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MicroFinanceDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.MicroFinance);
    }

    /// <inheritdoc/>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }
}

