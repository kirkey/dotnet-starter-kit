namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for AccountsPayableAccount entity.
/// </summary>
public class AccountsPayableAccountConfiguration : IEntityTypeConfiguration<AccountsPayableAccount>
{
    public void Configure(EntityTypeBuilder<AccountsPayableAccount> builder)
    {
        builder.ToTable("AccountsPayableAccounts", SchemaNames.Accounting);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.AccountNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.AccountName).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(2048);
        builder.Property(x => x.Notes).HasMaxLength(2048);
        
        builder.Property(x => x.CurrentBalance).HasPrecision(18, 2);
        builder.Property(x => x.Current0To30).HasPrecision(18, 2);
        builder.Property(x => x.Days31To60).HasPrecision(18, 2);
        builder.Property(x => x.Days61To90).HasPrecision(18, 2);
        builder.Property(x => x.Over90Days).HasPrecision(18, 2);
        builder.Property(x => x.DaysPayableOutstanding).HasPrecision(18, 2);
        builder.Property(x => x.ReconciliationVariance).HasPrecision(18, 2);
        builder.Property(x => x.YearToDatePayments).HasPrecision(18, 2);
        builder.Property(x => x.YearToDateDiscountsTaken).HasPrecision(18, 2);
        builder.Property(x => x.YearToDateDiscountsLost).HasPrecision(18, 2);
        
        // Single column indexes
        builder.HasIndex(x => x.AccountNumber)
            .IsUnique()
            .HasDatabaseName("IX_AccountsPayableAccounts_AccountNumber");
        
        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_AccountsPayableAccounts_IsActive");
        
        builder.HasIndex(x => x.GeneralLedgerAccountId)
            .HasDatabaseName("IX_AccountsPayableAccounts_GeneralLedgerAccountId");
        
        builder.HasIndex(x => x.PeriodId)
            .HasDatabaseName("IX_AccountsPayableAccounts_PeriodId");
        
        builder.HasIndex(x => x.IsReconciled)
            .HasDatabaseName("IX_AccountsPayableAccounts_IsReconciled");
        
        builder.HasIndex(x => x.LastReconciliationDate)
            .HasDatabaseName("IX_AccountsPayableAccounts_LastReconciliationDate");
        
        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.IsActive, x.PeriodId })
            .HasDatabaseName("IX_AccountsPayableAccounts_IsActive_PeriodId");
        
        builder.HasIndex(x => new { x.PeriodId, x.IsReconciled })
            .HasDatabaseName("IX_AccountsPayableAccounts_Period_IsReconciled");
        
        builder.HasIndex(x => new { x.IsActive, x.CurrentBalance })
            .HasDatabaseName("IX_AccountsPayableAccounts_IsActive_Balance");
        
        // Composite index for aging report queries
        builder.HasIndex(x => new { x.PeriodId, x.IsActive, x.CurrentBalance })
            .HasDatabaseName("IX_AccountsPayableAccounts_Period_Active_Balance");
    }
}

