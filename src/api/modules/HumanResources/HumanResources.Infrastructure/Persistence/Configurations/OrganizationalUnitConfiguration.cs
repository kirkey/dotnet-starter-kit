using Shared.Constants;

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

        builder.Property(ou => ou.CompanyId)
            .IsRequired();

        builder.Property(ou => ou.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(ou => new { ou.CompanyId, ou.Code })
            .IsUnique()
            .HasDatabaseName("IX_OrganizationalUnits_CompanyCode");

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

        // Relationships
        builder.HasOne(ou => ou.Company)
            .WithMany()
            .HasForeignKey(ou => ou.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ou => ou.Parent)
            .WithMany(ou => ou.Children)
            .HasForeignKey(ou => ou.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Audit fields
        builder.Property(ou => ou.CreatedBy)
            .HasMaxLength(256);

        builder.Property(ou => ou.LastModifiedBy)
            .HasMaxLength(256);
    }
}

