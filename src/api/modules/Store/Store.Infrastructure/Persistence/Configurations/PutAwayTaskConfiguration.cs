using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the PutAwayTask entity.
/// </summary>
public class PutAwayTaskConfiguration : IEntityTypeConfiguration<PutAwayTask>
{
    public void Configure(EntityTypeBuilder<PutAwayTask> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TaskNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.TaskNumber)
            .IsUnique();

        builder.Property(x => x.PutAwayStrategy)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

        // Foreign key relationships
        builder.HasOne(x => x.Warehouse)
            .WithMany()
            .HasForeignKey(x => x.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.GoodsReceipt)
            .WithMany()
            .HasForeignKey(x => x.GoodsReceiptId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // One-to-many relationship with PutAwayTaskItems
        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.PutAwayTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.WarehouseId);
        builder.HasIndex(x => x.GoodsReceiptId);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.PutAwayStrategy);
        builder.HasIndex(x => x.Priority);

        builder.ToTable("PutAwayTasks", SchemaNames.Store);
    }
}
