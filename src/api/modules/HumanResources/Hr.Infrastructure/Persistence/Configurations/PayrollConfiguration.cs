using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

internal sealed class PayrollConfiguration : IEntityTypeConfiguration<Payroll>
{
    public void Configure(EntityTypeBuilder<Payroll> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(p => p.Id);

        builder.Property(p => p.PayFrequency)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.TotalGrossPay)
            .HasPrecision(12, 2);

        builder.Property(p => p.TotalTaxes)
            .HasPrecision(12, 2);

        builder.Property(p => p.TotalDeductions)
            .HasPrecision(12, 2);

        builder.Property(p => p.TotalNetPay)
            .HasPrecision(12, 2);

        builder.Property(p => p.JournalEntryId)
            .HasMaxLength(50);

        builder.Property(p => p.Notes)
            .HasMaxLength(500);

        builder.HasMany(p => p.Lines)
            .WithOne(l => l.Payroll)
            .HasForeignKey(l => l.PayrollId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.StartDate)
            .HasDatabaseName("IX_Payroll_StartDate");

        builder.HasIndex(p => new { p.StartDate, p.EndDate })
            .HasDatabaseName("IX_Payroll_DateRange")
            .IsUnique();

        builder.HasIndex(p => p.Status)
            .HasDatabaseName("IX_Payroll_Status");

        builder.HasIndex(p => p.IsLocked)
            .HasDatabaseName("IX_Payroll_IsLocked");

        // Optimized for locked record filtering
        builder.HasIndex(p => new { p.IsLocked, p.Status })
            .HasDatabaseName("IX_Payroll_Locked_Status");

        // Period-based queries optimization
        builder.HasIndex(p => new { p.StartDate, p.EndDate, p.Status })
            .HasDatabaseName("IX_Payroll_Period_Status");

        // Active payroll lookup (not locked)
        builder.HasIndex(p => new { p.IsLocked, p.StartDate })
            .HasDatabaseName("IX_Payroll_Active_Period");
    }
}

