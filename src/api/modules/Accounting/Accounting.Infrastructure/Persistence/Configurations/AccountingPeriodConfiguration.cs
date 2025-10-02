using Accounting.Domain.Entities;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class AccountingPeriodConfiguration : IEntityTypeConfiguration<AccountingPeriod>
{
    public void Configure(EntityTypeBuilder<AccountingPeriod> builder)
    {
        builder.ToTable("AccountingPeriods", schema: SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.FiscalYear, x.PeriodType }).IsUnique();

        // Name, Description, Notes - align lengths with domain/AuditableEntity
        builder.Property(x => x.Name)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);

        builder.Property(x => x.FiscalYear)
            .IsRequired();

        builder.Property(x => x.PeriodType)
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.EndDate)
            .IsRequired();

        builder.Property(x => x.IsClosed)
            .IsRequired();

        builder.Property(x => x.IsAdjustmentPeriod)
            .IsRequired();
    }
}
