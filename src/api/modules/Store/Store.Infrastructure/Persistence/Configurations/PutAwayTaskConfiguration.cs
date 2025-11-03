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

        // Ensure EF uses the backing field for change tracking
        var navigation = builder.Metadata.FindNavigation(nameof(PutAwayTask.Items));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.WarehouseId)
            .HasDatabaseName("IX_PutAwayTasks_WarehouseId");

        builder.HasIndex(x => x.GoodsReceiptId)
            .HasDatabaseName("IX_PutAwayTasks_GoodsReceiptId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_PutAwayTasks_Status");

        builder.HasIndex(x => x.PutAwayStrategy)
            .HasDatabaseName("IX_PutAwayTasks_PutAwayStrategy");

        builder.HasIndex(x => x.Priority)
            .HasDatabaseName("IX_PutAwayTasks_Priority");

        builder.HasIndex(x => x.AssignedTo)
            .HasDatabaseName("IX_PutAwayTasks_AssignedTo");

        // Composite indexes for common query patterns
        builder.HasIndex(x => new { x.WarehouseId, x.Status })
            .HasDatabaseName("IX_PutAwayTasks_Warehouse_Status");

        builder.HasIndex(x => new { x.Status, x.Priority })
            .HasDatabaseName("IX_PutAwayTasks_Status_Priority");

        builder.HasIndex(x => new { x.AssignedTo, x.Status })
            .HasDatabaseName("IX_PutAwayTasks_AssignedTo_Status");

        builder.ToTable("PutAwayTasks", SchemaNames.Store);
    }
}
