using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

internal sealed class BenefitEnrollmentConfiguration : IEntityTypeConfiguration<BenefitEnrollment>
{
    public void Configure(EntityTypeBuilder<BenefitEnrollment> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CoverageLevel)
            .HasMaxLength(50);

        builder.Property(e => e.EmployeeContributionAmount)
            .HasPrecision(10, 2);

        builder.Property(e => e.EmployerContributionAmount)
            .HasPrecision(10, 2);

        builder.Property(e => e.CoveredDependentIds)
            .HasMaxLength(1000);

        builder.HasOne(e => e.Employee)
            .WithMany(emp => emp.BenefitEnrollments)
            .HasForeignKey(e => e.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Benefit)
            .WithMany(b => b.Enrollments)
            .HasForeignKey(e => e.BenefitId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.EmployeeId)
            .HasDatabaseName("IX_BenefitEnrollment_EmployeeId");

        builder.HasIndex(e => e.BenefitId)
            .HasDatabaseName("IX_BenefitEnrollment_BenefitId");

        builder.HasIndex(e => new { e.EmployeeId, e.BenefitId, e.EffectiveDate })
            .IsUnique(false)
            .HasDatabaseName("IX_BenefitEnrollment_Effective");

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("IX_BenefitEnrollment_IsActive");

        // Optimized for active enrollment filtering
        builder.HasIndex(e => new { e.IsActive, e.EffectiveDate })
            .HasDatabaseName("IX_BenefitEnrollment_Active_Effective");

        // Employee enrollment history
        builder.HasIndex(e => new { e.EmployeeId, e.IsActive })
            .HasDatabaseName("IX_BenefitEnrollment_EmployeeActive");

        // Benefit utilization analysis
        builder.HasIndex(e => new { e.BenefitId, e.IsActive })
            .HasDatabaseName("IX_BenefitEnrollment_BenefitActive");

        // Active enrollment with date range
        builder.HasIndex(e => new { e.EmployeeId, e.EffectiveDate, e.EndDate, e.IsActive })
            .HasDatabaseName("IX_BenefitEnrollment_Period_Active");
    }
}
