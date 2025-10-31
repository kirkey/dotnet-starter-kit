namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for TrialBalance entity.
/// </summary>
public class TrialBalanceConfiguration : IEntityTypeConfiguration<TrialBalance>
{
    public void Configure(EntityTypeBuilder<TrialBalance> builder)
    {
        builder.ToTable("TrialBalances", SchemaNames.Accounting);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.TrialBalanceNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(32);
        builder.Property(x => x.FinalizedBy).HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(2048);
        builder.Property(x => x.Notes).HasMaxLength(2048);
        
        builder.Property(x => x.TotalDebits).HasPrecision(18, 2);
        builder.Property(x => x.TotalCredits).HasPrecision(18, 2);
        builder.Property(x => x.TotalAssets).HasPrecision(18, 2);
        builder.Property(x => x.TotalLiabilities).HasPrecision(18, 2);
        builder.Property(x => x.TotalEquity).HasPrecision(18, 2);
        builder.Property(x => x.TotalRevenue).HasPrecision(18, 2);
        builder.Property(x => x.TotalExpenses).HasPrecision(18, 2);
        builder.Property(x => x.OutOfBalanceAmount).HasPrecision(18, 2);
        
        builder.HasIndex(x => x.TrialBalanceNumber).IsUnique();
        builder.HasIndex(x => x.PeriodId);
        builder.HasIndex(x => x.Status);
        
        builder.OwnsMany(x => x.LineItems, lineItem =>
        {
            lineItem.ToTable("TrialBalanceLineItems", SchemaNames.Accounting);
            lineItem.WithOwner().HasForeignKey("TrialBalanceId");
            lineItem.Property<int>("Id");
            lineItem.HasKey("Id");
            lineItem.Property(li => li.AccountCode).IsRequired().HasMaxLength(50);
            lineItem.Property(li => li.AccountName).IsRequired().HasMaxLength(256);
            lineItem.Property(li => li.AccountType).IsRequired().HasMaxLength(50);
            lineItem.Property(li => li.DebitBalance).HasPrecision(18, 2);
            lineItem.Property(li => li.CreditBalance).HasPrecision(18, 2);
        });
    }
}

