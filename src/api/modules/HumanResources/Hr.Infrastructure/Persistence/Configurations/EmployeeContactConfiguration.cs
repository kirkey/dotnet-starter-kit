using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for EmployeeContact entity.
/// </summary>
public class EmployeeContactConfiguration : IEntityTypeConfiguration<EmployeeContact>
{
    public void Configure(EntityTypeBuilder<EmployeeContact> builder)
    {
        // Multi-tenant support
        builder.IsMultiTenant();
        
        // Primary key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(c => c.ContactType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Relationship)
            .HasMaxLength(100);

        builder.Property(c => c.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .HasMaxLength(256);

        builder.Property(c => c.Address)
            .HasMaxLength(500);

        builder.Property(c => c.Priority)
            .HasDefaultValue(1);

        // Foreign key relationship with Employee
        builder.HasOne(c => c.Employee)
            .WithMany(e => e.Contacts)
            .HasForeignKey(c => c.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(c => c.EmployeeId)
            .HasDatabaseName("IX_EmployeeContacts_EmployeeId");

        builder.HasIndex(c => c.ContactType)
            .HasDatabaseName("IX_EmployeeContacts_ContactType");

        builder.HasIndex(c => new { c.EmployeeId, c.ContactType })
            .HasDatabaseName("IX_EmployeeContacts_EmployeeId_ContactType");

        builder.HasIndex(c => c.IsActive)
            .HasDatabaseName("IX_EmployeeContacts_IsActive");
    }
}

