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
        builder.Property(x => x.Current0to30).HasPrecision(18, 2);
        builder.Property(x => x.Days31to60).HasPrecision(18, 2);
        builder.Property(x => x.Days61to90).HasPrecision(18, 2);
        builder.Property(x => x.Over90Days).HasPrecision(18, 2);
        builder.Property(x => x.DaysPayableOutstanding).HasPrecision(18, 2);
        builder.Property(x => x.ReconciliationVariance).HasPrecision(18, 2);
        builder.Property(x => x.YearToDatePayments).HasPrecision(18, 2);
        builder.Property(x => x.YearToDateDiscountsTaken).HasPrecision(18, 2);
        builder.Property(x => x.YearToDateDiscountsLost).HasPrecision(18, 2);
        
        builder.HasIndex(x => x.AccountNumber).IsUnique();
        builder.HasIndex(x => x.IsActive);
    }
}

