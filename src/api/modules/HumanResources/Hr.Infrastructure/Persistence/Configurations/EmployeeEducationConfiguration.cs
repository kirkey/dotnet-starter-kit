using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for EmployeeEducation entity.
/// </summary>
public class EmployeeEducationConfiguration : IEntityTypeConfiguration<EmployeeEducation>
{
    public void Configure(EntityTypeBuilder<EmployeeEducation> builder)
    {
        // Multi-tenant support
        builder.IsMultiTenant();

        // Primary key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.EducationLevel)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.FieldOfStudy)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.Institution)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.Degree)
            .HasMaxLength(256);

        builder.Property(e => e.CertificateNumber)
            .HasMaxLength(100);

        builder.Property(e => e.Notes)
            .HasMaxLength(1000);

        builder.Property(e => e.Gpa)
            .HasPrecision(3, 2); // 0.00 to 4.00

        builder.Property(e => e.IsVerified)
            .HasDefaultValue(false);

        builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

        // Foreign key relationship with Employee
        builder.HasOne(e => e.Employee)
            .WithMany(emp => emp.EducationRecords)
            .HasForeignKey(e => e.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => e.EmployeeId)
            .HasDatabaseName("IX_EmployeeEducations_EmployeeId");

        builder.HasIndex(e => e.EducationLevel)
            .HasDatabaseName("IX_EmployeeEducations_EducationLevel");

        builder.HasIndex(e => new { e.EmployeeId, e.EducationLevel })
            .HasDatabaseName("IX_EmployeeEducations_EmployeeId_EducationLevel");

        builder.HasIndex(e => e.IsVerified)
            .HasDatabaseName("IX_EmployeeEducations_IsVerified");

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("IX_EmployeeEducations_IsActive");

        // Optimized for verified education lookups
        builder.HasIndex(e => new { e.IsVerified, e.IsActive })
            .HasDatabaseName("IX_EmployeeEducations_Verified_Active");

        // Education level statistics
        builder.HasIndex(e => new { e.EducationLevel, e.IsActive })
            .HasDatabaseName("IX_EmployeeEducations_Level_Active");

        // Credential verification
        builder.HasIndex(e => new { e.CertificateNumber, e.IsActive })
            .HasDatabaseName("IX_EmployeeEducations_Certificate_Active");

        // Employee education summary
        builder.HasIndex(e => new { e.EmployeeId, e.EducationLevel, e.IsVerified })
            .HasDatabaseName("IX_EmployeeEducations_Summary");
    }
}

