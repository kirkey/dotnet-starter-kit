using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for Employee entity.
/// </summary>
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        // Primary key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.EmployeeNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.MiddleName)
            .HasMaxLength(256);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.Email)
            .HasMaxLength(256);

        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(e => e.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.TerminationReason)
            .HasMaxLength(500);

        // Foreign key relationship with OrganizationalUnit
        builder.HasOne(e => e.OrganizationalUnit)
            .WithMany()
            .HasForeignKey(e => e.OrganizationalUnitId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with DesignationAssignments
        builder.HasMany(e => e.DesignationAssignments)
            .WithOne(a => a.Employee)
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => e.EmployeeNumber)
            .IsUnique()
            .HasDatabaseName("IX_Employees_EmployeeNumber");

        builder.HasIndex(e => e.OrganizationalUnitId)
            .HasDatabaseName("IX_Employees_OrganizationalUnitId");

        builder.HasIndex(e => e.Status)
            .HasDatabaseName("IX_Employees_Status");

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("IX_Employees_IsActive");
    }
}

