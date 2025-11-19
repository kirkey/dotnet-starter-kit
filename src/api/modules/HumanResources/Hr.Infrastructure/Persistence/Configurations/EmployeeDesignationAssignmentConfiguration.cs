using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework Core configuration for DesignationAssignment entity.
/// </summary>
public class DesignationAssignmentConfiguration : IEntityTypeConfiguration<DesignationAssignment>
{
    public void Configure(EntityTypeBuilder<DesignationAssignment> builder)
    {
        // Primary key
        builder.HasKey(a => a.Id);

        // Properties
        builder.Property(a => a.Reason)
            .HasMaxLength(500);

        builder.Property(a => a.AdjustedSalary)
            .HasPrecision(16, 2);

        // Foreign key relationships
        builder.HasOne(a => a.Employee)
            .WithMany(e => e.DesignationAssignments)
            .HasForeignKey(a => a.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Designation)
            .WithMany()
            .HasForeignKey(a => a.DesignationId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for temporal queries
        builder.HasIndex(a => new { a.EmployeeId, a.EffectiveDate, a.EndDate })
            .HasDatabaseName("IX_EDA_PointInTime");

        builder.HasIndex(a => new { a.EmployeeId, a.EffectiveDate })
            .HasDatabaseName("IX_EDA_EmployeeHistory");

        builder.HasIndex(a => new { a.DesignationId, a.EffectiveDate, a.EndDate })
            .HasDatabaseName("IX_EDA_Designations");

        builder.HasIndex(a => new { a.EffectiveDate, a.EndDate })
            .HasDatabaseName("IX_EDA_Active");

        builder.HasIndex(a => new { a.EffectiveDate, a.EndDate })
            .HasDatabaseName("IX_EDA_PayrollPeriod");
    }
}

