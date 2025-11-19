using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for OrganizationalUnit.
/// </summary>
public class OrganizationalUnitConfiguration : IEntityTypeConfiguration<OrganizationalUnit>
{
    public void Configure(EntityTypeBuilder<OrganizationalUnit> builder)
    {
        builder.IsMultiTenant();
        builder.ToTable("OrganizationalUnits", SchemaNames.HumanResources);

        builder.HasKey(ou => ou.Id);

        builder.Property(ou => ou.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(ou => ou.Code)
            .IsUnique()
            .HasDatabaseName("IX_OrganizationalUnits_Code");

        builder.Property(ou => ou.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(ou => ou.Level)
            .IsRequired();

        builder.Property(ou => ou.HierarchyPath)
            .HasMaxLength(500);

        builder.HasIndex(ou => ou.HierarchyPath)
            .HasDatabaseName("IX_OrganizationalUnits_HierarchyPath");

        builder.Property(ou => ou.CostCenter)
            .HasMaxLength(50);

        builder.Property(ou => ou.Location)
            .HasMaxLength(200);

        builder.Property(ou => ou.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(ou => ou.IsActive)
            .HasDatabaseName("IX_OrganizationalUnits_IsActive");

        builder.HasIndex(ou => ou.ParentId)
            .HasDatabaseName("IX_OrganizationalUnits_ParentId");

        builder.HasIndex(ou => ou.Type)
            .HasDatabaseName("IX_OrganizationalUnits_Type");

        // Optimized for hierarchy traversal (parent-child)
        builder.HasIndex(ou => new { ou.ParentId, ou.IsActive })
            .HasDatabaseName("IX_OrganizationalUnits_Parent_Active");

        // Filtered index for active organizational units
        builder.HasIndex(ou => new { ou.Type, ou.IsActive })
            .HasDatabaseName("IX_OrganizationalUnits_Type_Active");

        // Hierarchical path queries optimization
        builder.HasIndex(ou => new { ou.HierarchyPath, ou.IsActive })
            .HasDatabaseName("IX_OrganizationalUnits_Path_Active");

        // Cost center lookups
        builder.HasIndex(ou => new { ou.CostCenter, ou.IsActive })
            .HasDatabaseName("IX_OrganizationalUnits_CostCenter_Active");

        // Location-based queries
        builder.HasIndex(ou => new { ou.Location, ou.Type, ou.IsActive })
            .HasDatabaseName("IX_OrganizationalUnits_Location_Type_Active");

        builder.HasOne(ou => ou.Parent)
            .WithMany(ou => ou.Children)
            .HasForeignKey(ou => ou.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
