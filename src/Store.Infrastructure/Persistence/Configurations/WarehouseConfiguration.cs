using Shared.Constants;

namespace Store.Infrastructure.Persistence.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.State)
            .HasMaxLength(100);

        builder.Property(x => x.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.PostalCode)
            .HasMaxLength(20);

        builder.Property(x => x.ManagerName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ManagerEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.ManagerPhone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.TotalCapacity)
            .HasColumnType("decimal(18,3)")
            .IsRequired();

        builder.Property(x => x.UsedCapacity)
            .HasColumnType("decimal(18,3)")
            .IsRequired()
            .HasDefaultValue(0m);

        builder.Property(x => x.CapacityUnit)
            .IsRequired()
            .HasMaxLength(20);

        // Table-level constraints
        builder.ToTable("Warehouses", SchemaNames.Store, tb =>
        {
            tb.HasCheckConstraint("CK_Warehouses_TotalCapacity_Positive", "TotalCapacity > 0");
            tb.HasCheckConstraint("CK_Warehouses_UsedCapacity_Range", "UsedCapacity >= 0 AND UsedCapacity <= TotalCapacity");
        });
    }
}
