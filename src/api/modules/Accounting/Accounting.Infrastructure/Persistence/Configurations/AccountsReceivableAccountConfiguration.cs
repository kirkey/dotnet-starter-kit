namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for AccountsReceivableAccount entity.
/// </summary>
public class AccountsReceivableAccountConfiguration : IEntityTypeConfiguration<AccountsReceivableAccount>
{
    public void Configure(EntityTypeBuilder<AccountsReceivableAccount> builder)
    {
        builder.ToTable("AccountsReceivableAccounts", SchemaNames.Accounting);
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
        builder.Property(x => x.AllowanceForDoubtfulAccounts).HasPrecision(18, 2);
        builder.Property(x => x.NetReceivables).HasPrecision(18, 2);
        builder.Property(x => x.DaysSalesOutstanding).HasPrecision(18, 2);
        builder.Property(x => x.BadDebtPercentage).HasPrecision(5, 4);
        builder.Property(x => x.ReconciliationVariance).HasPrecision(18, 2);
        builder.Property(x => x.YearToDateWriteOffs).HasPrecision(18, 2);
        builder.Property(x => x.YearToDateCollections).HasPrecision(18, 2);
        
        builder.HasIndex(x => x.AccountNumber).IsUnique();
        builder.HasIndex(x => x.IsActive);
        builder.HasIndex(x => x.GeneralLedgerAccountId);
        builder.HasIndex(x => x.PeriodId);
        builder.HasIndex(x => x.IsReconciled);
    }
}

