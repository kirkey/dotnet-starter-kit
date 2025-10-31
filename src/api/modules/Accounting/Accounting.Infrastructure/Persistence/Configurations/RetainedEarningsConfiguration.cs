namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for RetainedEarnings entity.
/// </summary>
public class RetainedEarningsConfiguration : IEntityTypeConfiguration<RetainedEarnings>
{
    public void Configure(EntityTypeBuilder<RetainedEarnings> builder)
    {
        builder.ToTable("RetainedEarnings", SchemaNames.Accounting);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(32);
        builder.Property(x => x.ClosedBy).HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(2048);
        builder.Property(x => x.Notes).HasMaxLength(2048);
        
        builder.Property(x => x.OpeningBalance).HasPrecision(18, 2);
        builder.Property(x => x.NetIncome).HasPrecision(18, 2);
        builder.Property(x => x.Distributions).HasPrecision(18, 2);
        builder.Property(x => x.CapitalContributions).HasPrecision(18, 2);
        builder.Property(x => x.OtherEquityChanges).HasPrecision(18, 2);
        builder.Property(x => x.ClosingBalance).HasPrecision(18, 2);
        builder.Property(x => x.ApproprietedAmount).HasPrecision(18, 2);
        builder.Property(x => x.UnappropriatedAmount).HasPrecision(18, 2);
        
        builder.HasIndex(x => x.FiscalYear).IsUnique();
        builder.HasIndex(x => x.Status);
    }
}

