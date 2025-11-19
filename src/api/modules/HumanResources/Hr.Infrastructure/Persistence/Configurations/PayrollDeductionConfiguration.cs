using FSH.Starter.WebApi.HumanResources.Domain.Entities;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Persistence.Configurations;

internal sealed class PayrollDeductionConfiguration : IEntityTypeConfiguration<PayrollDeduction>
{
    public void Configure(EntityTypeBuilder<PayrollDeduction> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DeductionType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.DeductionAmount)
            .HasPrecision(10, 2);

        builder.Property(x => x.DeductionPercentage)
            .HasPrecision(5, 2);

        builder.Property(x => x.StartDate)
            .IsRequired();

        builder.Property(x => x.MaxDeductionLimit)
            .HasPrecision(10, 2);

        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(100);

        builder.Property(x => x.Remarks)
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(x => x.PayComponent)
            .WithMany()
            .HasForeignKey(x => x.PayComponentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasOne(x => x.OrganizationalUnit)
            .WithMany()
            .HasForeignKey(x => x.OrganizationalUnitId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        // Indexes
        builder.HasIndex(x => x.PayComponentId)
            .HasDatabaseName("IX_PayrollDeduction_PayComponentId");

        builder.HasIndex(x => x.EmployeeId)
            .HasDatabaseName("IX_PayrollDeduction_EmployeeId");

        builder.HasIndex(x => x.OrganizationalUnitId)
            .HasDatabaseName("IX_PayrollDeduction_OrganizationalUnitId");

        builder.HasIndex(x => new { x.StartDate, x.EndDate })
            .HasDatabaseName("IX_PayrollDeduction_DateRange");

        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_PayrollDeduction_IsActive");

        builder.HasIndex(x => x.ReferenceNumber)
            .HasDatabaseName("IX_PayrollDeduction_ReferenceNumber");

        // Optimized for active deduction filtering
        builder.HasIndex(x => new { x.IsActive, x.EmployeeId })
            .HasDatabaseName("IX_PayrollDeduction_Active_Employee");

        // Active period queries
        builder.HasIndex(x => new { x.EmployeeId, x.StartDate, x.EndDate, x.IsActive })
            .HasDatabaseName("IX_PayrollDeduction_Period_Active");

        // Component-based queries
        builder.HasIndex(x => new { x.PayComponentId, x.IsActive })
            .HasDatabaseName("IX_PayrollDeduction_Component_Active");

        // Unit deductions for reports
        builder.HasIndex(x => new { x.OrganizationalUnitId, x.IsActive })
            .HasDatabaseName("IX_PayrollDeduction_OrgUnit_Active");

        // Reference number with status
        builder.HasIndex(x => new { x.ReferenceNumber, x.IsActive })
            .HasDatabaseName("IX_PayrollDeduction_Reference_Active");
    }
}

