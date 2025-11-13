using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using Shared.Constants;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for Position.
/// </summary>
public class PositionConfiguration : IEntityTypeConfiguration<Designation>
{
    public void Configure(EntityTypeBuilder<Designation> builder)
    {
        builder.IsMultiTenant();
        builder.ToTable("Positions", SchemaNames.HumanResources);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => new { p.OrganizationalUnitId, p.Code })
            .IsUnique()
            .HasDatabaseName("IX_Positions_OrgUnitCode");

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(p => p.Description)
            .HasMaxLength(2000);

        builder.Property(p => p.MinSalary)
            .HasPrecision(16, 2);

        builder.Property(p => p.MaxSalary)
            .HasPrecision(16, 2);

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Positions_IsActive");

        builder.HasIndex(p => p.OrganizationalUnitId)
            .HasDatabaseName("IX_Positions_OrganizationalUnitId");

        // Relationships
        builder.HasOne(p => p.OrganizationalUnit)
            .WithMany()
            .HasForeignKey(p => p.OrganizationalUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        // Audit fields
        builder.Property(p => p.CreatedBy)
            .HasMaxLength(256);

        builder.Property(p => p.LastModifiedBy)
            .HasMaxLength(256);
    }
}

