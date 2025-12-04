using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Infrastructure.Tenant;
using Microsoft.Extensions.Options;
using Shared.Constants;

namespace FSH.Starter.WebApi.MicroFinance.Infrastructure.Persistence;

/// <summary>
/// Database context for the MicroFinance module.
/// Manages all microfinance-related entities including members, loans, and savings.
/// </summary>
public sealed class MicroFinanceDbContext(
    IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor,
    DbContextOptions<MicroFinanceDbContext> options,
    IPublisher publisher,
    IOptions<DatabaseOptions> settings) : FshDbContext(multiTenantContextAccessor, options, publisher, settings)
{
    /// <summary>Gets or sets the Members DbSet.</summary>
    public DbSet<Member> Members { get; set; } = null!;

    /// <summary>Gets or sets the LoanProducts DbSet.</summary>
    public DbSet<LoanProduct> LoanProducts { get; set; } = null!;

    /// <summary>Gets or sets the Loans DbSet.</summary>
    public DbSet<Loan> Loans { get; set; } = null!;

    /// <summary>Gets or sets the LoanRepayments DbSet.</summary>
    public DbSet<LoanRepayment> LoanRepayments { get; set; } = null!;

    /// <summary>Gets or sets the SavingsProducts DbSet.</summary>
    public DbSet<SavingsProduct> SavingsProducts { get; set; } = null!;

    /// <summary>Gets or sets the SavingsAccounts DbSet.</summary>
    public DbSet<SavingsAccount> SavingsAccounts { get; set; } = null!;

    /// <summary>Gets or sets the SavingsTransactions DbSet.</summary>
    public DbSet<SavingsTransaction> SavingsTransactions { get; set; } = null!;

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

