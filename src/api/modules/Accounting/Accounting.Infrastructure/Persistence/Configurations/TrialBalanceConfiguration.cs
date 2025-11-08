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
        
        builder.HasIndex(x => x.TrialBalanceNumber)
            .IsUnique()
            .HasDatabaseName("IX_TrialBalances_TrialBalanceNumber");

        builder.HasIndex(x => x.PeriodId)
            .HasDatabaseName("IX_TrialBalances_PeriodId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_TrialBalances_Status");

        builder.HasIndex(x => x.PeriodStartDate)
            .HasDatabaseName("IX_TrialBalances_PeriodStartDate");

        builder.HasIndex(x => x.PeriodEndDate)
            .HasDatabaseName("IX_TrialBalances_PeriodEndDate");

        builder.HasIndex(x => x.FinalizedDate)
            .HasDatabaseName("IX_TrialBalances_FinalizedDate");

        builder.HasIndex(x => x.FinalizedBy)
            .HasDatabaseName("IX_TrialBalances_FinalizedBy");

        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.Status, x.PeriodEndDate })
            .HasDatabaseName("IX_TrialBalances_Status_PeriodEnd");

        builder.HasIndex(x => new { x.PeriodId, x.Status })
            .HasDatabaseName("IX_TrialBalances_Period_Status");

        builder.HasIndex(x => new { x.PeriodStartDate, x.PeriodEndDate })
            .HasDatabaseName("IX_TrialBalances_PeriodRange");

        builder.HasIndex(x => new { x.Status, x.FinalizedDate })
            .HasDatabaseName("IX_TrialBalances_Status_FinalizedDate");
        
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

            // Indexes for line items
            lineItem.HasIndex("TrialBalanceId")
                .HasDatabaseName("IX_TrialBalanceLineItems_TrialBalanceId");

            lineItem.HasIndex(li => li.AccountCode)
                .HasDatabaseName("IX_TrialBalanceLineItems_AccountCode");

            lineItem.HasIndex(li => li.AccountType)
                .HasDatabaseName("IX_TrialBalanceLineItems_AccountType");
        });
    }
}

