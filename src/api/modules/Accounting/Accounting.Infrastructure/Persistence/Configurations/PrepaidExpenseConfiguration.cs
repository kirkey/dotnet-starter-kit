namespace Accounting.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for PrepaidExpense entity.
/// </summary>
public class PrepaidExpenseConfiguration : IEntityTypeConfiguration<PrepaidExpense>
{
    public void Configure(EntityTypeBuilder<PrepaidExpense> builder)
    {
        builder.ToTable("PrepaidExpenses", SchemaNames.Accounting);
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.PrepaidNumber).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.Property(x => x.AmortizationSchedule).IsRequired().HasMaxLength(32);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(32);
        builder.Property(x => x.VendorName).HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(2048);
        builder.Property(x => x.Notes).HasMaxLength(2048);
        
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        builder.Property(x => x.AmortizedAmount).HasPrecision(18, 2);
        builder.Property(x => x.RemainingAmount).HasPrecision(18, 2);
        
        builder.HasIndex(x => x.PrepaidNumber).IsUnique();
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.NextAmortizationDate);
        
        builder.OwnsMany(x => x.AmortizationHistory, entry =>
        {
            entry.ToTable("PrepaidAmortizationEntries", SchemaNames.Accounting);
            entry.WithOwner().HasForeignKey("PrepaidExpenseId");
            entry.Property<int>("Id");
            entry.HasKey("Id");
            entry.Property(e => e.AmortizationAmount).HasPrecision(18, 2);
            entry.Property(e => e.RemainingBalance).HasPrecision(18, 2);
        });
    }
}
