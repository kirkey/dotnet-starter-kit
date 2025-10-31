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

        // Indexes for foreign keys
        builder.HasIndex(x => x.PutAwayTaskId);
        builder.HasIndex(x => x.ItemId);
        builder.HasIndex(x => x.ToBinId);
        builder.HasIndex(x => x.LotNumberId);
        builder.HasIndex(x => x.SerialNumberId);
        builder.HasIndex(x => x.Status);

        builder.ToTable("PutAwayTaskItems", SchemaNames.Store);
    }
}
