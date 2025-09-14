using Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace Accounting.Infrastructure.Persistence.Configurations;

public class ConsumptionDataConfiguration : IEntityTypeConfiguration<ConsumptionData>
{
    public void Configure(EntityTypeBuilder<ConsumptionData> builder)
    {
        builder.ToTable("ConsumptionData", SchemaNames.Accounting);

        builder.HasKey(x => x.Id);

        // Unique constraint: one reading per meter per date
        builder.HasIndex(x => new { x.MeterId, x.ReadingDate }).IsUnique();

        builder.Property(x => x.MeterId)
            .IsRequired();

        builder.Property(x => x.ReadingDate)
            .IsRequired();

        builder.Property(x => x.CurrentReading)
            .HasColumnType("decimal(18,3)")
            .IsRequired();

        builder.Property(x => x.PreviousReading)
            .HasColumnType("decimal(18,3)")
            .IsRequired();

        builder.Property(x => x.KWhUsed)
            .HasColumnType("decimal(18,3)")
            .IsRequired();

        builder.Property(x => x.BillingPeriod)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(x => x.ReadingType)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.Multiplier)
            .HasColumnType("decimal(10,4)");

        builder.Property(x => x.IsValidReading)
            .IsRequired();

        builder.Property(x => x.ReadingSource)
            .HasMaxLength(32);

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.Notes)
            .HasMaxLength(2048);
    }
}
