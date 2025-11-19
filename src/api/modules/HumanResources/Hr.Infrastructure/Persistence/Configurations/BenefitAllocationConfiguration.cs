using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

internal sealed class BenefitAllocationConfiguration : IEntityTypeConfiguration<BenefitAllocation>
{
    public void Configure(EntityTypeBuilder<BenefitAllocation> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AllocationType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(100);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        builder.Property(x => x.AllocatedAmount)
            .HasPrecision(12, 2);

        builder.HasOne(x => x.Enrollment)
            .WithMany()
            .HasForeignKey(x => x.EnrollmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for frequent queries
        builder.HasIndex(x => x.EnrollmentId)
            .HasDatabaseName("IX_BenefitAllocation_EnrollmentId");

        builder.HasIndex(x => x.AllocationDate)
            .HasDatabaseName("IX_BenefitAllocation_AllocationDate");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_BenefitAllocation_Status");

        builder.HasIndex(x => x.ReferenceNumber)
            .HasDatabaseName("IX_BenefitAllocation_ReferenceNumber");

        builder.HasIndex(x => new { x.EnrollmentId, x.AllocationDate })
            .HasDatabaseName("IX_BenefitAllocation_Enrollment_Date");

        // Optimized for allocation status filtering
        builder.HasIndex(x => new { x.Status, x.AllocationType })
            .HasDatabaseName("IX_BenefitAllocation_Status_Type");

        // Allocation date range queries
        builder.HasIndex(x => new { x.AllocationDate, x.Status })
            .HasDatabaseName("IX_BenefitAllocation_DateRange_Status");

        // Reference number lookup with status
        builder.HasIndex(x => new { x.ReferenceNumber, x.Status })
            .HasDatabaseName("IX_BenefitAllocation_Reference_Status");
    }
}
