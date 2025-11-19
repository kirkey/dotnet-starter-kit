using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for EmployeeDependent entity.
/// </summary>
public class EmployeeDependentConfiguration : IEntityTypeConfiguration<EmployeeDependent>
{
    public void Configure(EntityTypeBuilder<EmployeeDependent> builder)
    {
        // Multi-tenant support
        builder.IsMultiTenant();
        
        // Primary key
        builder.HasKey(d => d.Id);

        // Properties
        builder.Property(d => d.FirstName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(d => d.LastName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(d => d.DependentType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.DateOfBirth)
            .IsRequired(false);

        builder.Property(d => d.Relationship)
            .HasMaxLength(100);

        builder.Property(d => d.Ssn)
            .HasMaxLength(11);

        builder.Property(d => d.Email)
            .HasMaxLength(256);

        builder.Property(d => d.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(d => d.IsBeneficiary)
            .HasDefaultValue(false);

        builder.Property(d => d.IsClaimableDependent)
            .HasDefaultValue(true);

        // Foreign key relationship with Employee
        builder.HasOne(d => d.Employee)
            .WithMany(e => e.Dependents)
            .HasForeignKey(d => d.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(d => d.EmployeeId)
            .HasDatabaseName("IX_EmployeeDependents_EmployeeId");

        builder.HasIndex(d => d.DependentType)
            .HasDatabaseName("IX_EmployeeDependents_DependentType");

        builder.HasIndex(d => new { d.EmployeeId, d.DependentType })
            .HasDatabaseName("IX_EmployeeDependents_EmployeeId_DependentType");

        builder.HasIndex(d => d.IsBeneficiary)
            .HasDatabaseName("IX_EmployeeDependents_IsBeneficiary");

        builder.HasIndex(d => d.IsClaimableDependent)
            .HasDatabaseName("IX_EmployeeDependents_IsClaimableDependent");

        builder.HasIndex(d => d.IsActive)
            .HasDatabaseName("IX_EmployeeDependents_IsActive");

        // Optimized for beneficiary filtering
        builder.HasIndex(d => new { d.IsBeneficiary, d.IsActive })
            .HasDatabaseName("IX_EmployeeDependents_Beneficiary_Active");

        // Claimable dependent tracking
        builder.HasIndex(d => new { d.IsClaimableDependent, d.IsActive })
            .HasDatabaseName("IX_EmployeeDependents_Claimable_Active");

        // Employee dependent type enumeration
        builder.HasIndex(d => new { d.EmployeeId, d.DependentType, d.IsActive })
            .HasDatabaseName("IX_EmployeeDependents_Employee_Type_Active");
    }
}

