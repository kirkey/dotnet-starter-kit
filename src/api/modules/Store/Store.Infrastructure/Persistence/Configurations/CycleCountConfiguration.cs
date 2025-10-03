using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

public class CycleCountConfiguration : IEntityTypeConfiguration<CycleCount>
{
    public void Configure(EntityTypeBuilder<CycleCount> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CountNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.CountNumber)
            .IsUnique();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CountType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.CounterName)
            .HasMaxLength(100);

        builder.Property(x => x.SupervisorName)
            .HasMaxLength(100);

        builder.Property(x => x.AccuracyPercentage)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.WarehouseLocation)
            .WithMany()
            .HasForeignKey(x => x.WarehouseLocationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.ToTable("CycleCounts", SchemaNames.Store);
    }
}
