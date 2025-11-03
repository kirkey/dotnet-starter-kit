using Store.Domain.Entities;

namespace Store.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for the PutAwayTaskItem entity.
/// </summary>
public class PutAwayTaskItemConfiguration : IEntityTypeConfiguration<PutAwayTaskItem>
{
    public void Configure(EntityTypeBuilder<PutAwayTaskItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion<string>();

        // Foreign key relationships
        builder.HasOne<PutAwayTask>()
            .WithMany(p => p.Items)
            .HasForeignKey(x => x.PutAwayTaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ToBin)
            .WithMany()
            .HasForeignKey(x => x.ToBinId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.LotNumber)
            .WithMany()
            .HasForeignKey(x => x.LotNumberId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.SerialNumber)
            .WithMany()
            .HasForeignKey(x => x.SerialNumberId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Indexes for foreign keys and query optimization
        builder.HasIndex(x => x.PutAwayTaskId)
            .HasDatabaseName("IX_PutAwayTaskItems_PutAwayTaskId");

        builder.HasIndex(x => x.ItemId)
            .HasDatabaseName("IX_PutAwayTaskItems_ItemId");

        builder.HasIndex(x => x.ToBinId)
            .HasDatabaseName("IX_PutAwayTaskItems_ToBinId");

        builder.HasIndex(x => x.LotNumberId)
            .HasDatabaseName("IX_PutAwayTaskItems_LotNumberId");

        builder.HasIndex(x => x.SerialNumberId)
            .HasDatabaseName("IX_PutAwayTaskItems_SerialNumberId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_PutAwayTaskItems_Status");

        // Composite index for task+item reporting
        builder.HasIndex(x => new { x.PutAwayTaskId, x.ItemId })
            .HasDatabaseName("IX_PutAwayTaskItems_Task_Item");

        // Composite index for bin location queries
        builder.HasIndex(x => new { x.ToBinId, x.Status })
            .HasDatabaseName("IX_PutAwayTaskItems_ToBin_Status");

        builder.ToTable("PutAwayTaskItems", SchemaNames.Store);
    }
}
