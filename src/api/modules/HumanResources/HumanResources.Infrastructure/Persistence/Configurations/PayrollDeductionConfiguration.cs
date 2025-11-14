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
            .OnDelete(DeleteBehavior.Restrict);

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
    }
}

